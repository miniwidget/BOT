using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Infected
{
    public partial class Infected
    {
        void Human_Connected(Entity player)
        {
            if (human_List.Count > 6)
            {
                Utilities.ExecuteCommand("dropclient " + player.EntRef + " \"MAX players count overflows\"");
                return;
            }

            string name = player.Name;

            if (player.Name == "kwnav")
            {
                ADMIN = player;
                SetADMIN();
            }

            if (IsSurvivor(player))
            {
                Print(name + " connected ♥");
                SetPlayer(player);
            }
            else
            {
                Print("AXIS connected ☜");
                H_FIELD[player.EntRef].LIFE = -1;
                player.Call(33341);//"suicide"
            }

        }
        void SetPlayer(Entity player)
        {
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            human_List.Add(player);
            int pe = player.EntRef;
            H_FIELD[pe].reset(false);
            IsAXIS[pe] = false;
            IsPERK[pe] = 2;

            #region SetClientDvar

            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");

            #endregion

            #region notifyonplayercommand

            string offhand = "";
            switch (rnd.Next(4))
            {
                case 0: offhand = "frag_grenade_mp"; break;
                case 1: offhand = "semtex_mp"; break;//OK
                case 2: offhand = "bouncingbetty_mp"; break;//OK
                case 3: offhand = "claymore_mp"; break;//OK
            }

            player.Call(33445, "HOLD_STRAFE", "+strafe");//notifyonplayercommand
            player.OnNotify("HOLD_STRAFE", ent =>
            {
                if (IsAXIS[pe]) return;
                var weapon = player.CurrentWeapon;
                if (weapon.Length > 3 && weapon[2] == '5')
                {
                    player.Call("givemaxammo", weapon);
                }
            });

            player.Call(33445, "HOLD_CROUCH", "+movedown");
            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                if (IsAXIS[pe])
                {
                    WP.GiveOffhandWeapon(player, "throwingknife");
                    return;
                }

                WP.GiveAttachScope(player);

            });


            player.Call(33445, "HOLD_STANCE", "+stance");
            player.OnNotify("HOLD_STANCE", ent =>//offhand weapon
            {
                if (IsAXIS[pe])
                {
                    WP.GiveOffhandWeapon(player, "claymore_mp");
                    return;
                }
                WP.GiveOffhandWeapon(player, offhand);
            });

            #endregion

            #region TANK

            Entity TANK = null;
            H_SET H = H_FIELD[pe];
            player.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                if (H.USE_TANK) return;

                string weap = newWeap.ToString();
                if (weap == "killstreak_remote_tank_remote_mp")
                {
                    H.USE_TANK = true;
                    TANK = null;

                    bool found = false;
                    for (int i = TK.remoteTank.EntRef + 1; i < 2048; i++)
                    {
                        TANK = Entity.GetEntity(i);
                        if (TANK == null) continue;
                        var model = TANK.GetField<string>("model");
                        if (model == "vehicle_ugv_talon_mp")
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found) return;

                    human_List.Add(TANK);
                    human_List.Remove(player);
                }
            });
            player.OnNotify("end_remote", (Entity ent) =>
            {
                if (H.USE_TANK)
                {
                    H.USE_TANK = false;
                    human_List.Remove(TANK);
                    human_List.Add(player);
                }
            });

            #endregion

            #region helicopter

            /*
             0 : Allies under 10kill
             1 : ready to call heli
             2 : start heli
             3 : end heli 
             4 : axis
            */
            player.Call(33445, "ACTIVATE", "+activate");//"notifyonplayercommand"
            player.OnNotify("ACTIVATE", ent =>
            {
                if (IsAXIS[pe] || H.USE_TANK) return;//Axis or OnMessage or HELI null

                if (player.CurrentWeapon[2] != '5') return;

                player.AfterDelay(250, x =>
                {

                    if (!HCT.IsUsingTurret(player))//heli 생성
                    {
                        if (HCT.HELI == null)
                        {
                            if (H.USE_HELI == 1)
                            {
                                HCT.HeliCall(player); H.USE_HELI = 2;
                                return;
                            }
                        }
                        else
                        {
                            if (H.USE_HELI == 3 && HCT.HELI_OWNER == player)
                            {
                                HCT.HeliEndUse(player, true); H.USE_HELI = 0;
                                return;
                            }
                            else if (H.USE_HELI == 1 && !HCT.IsHeliArea(player))
                            {
                                if (!TK.IfTankOwner_DoEnd(player))
                                {
                                    Info.MessageRoop(player, 0, HCT.HELI_MESSAGE_ALERT);
                                }
                                return;
                            }
                        }
                        TK.IfTankOwner_DoEnd(player);
                    }
                    else
                    {
                        if (HCT.HELI == null || HCT.HELI != null && !HCT.IsHeliArea(player))/*다른 튜렛을 사용 중인 경우*/
                        {
                            TK.TankStart(player);

                            return;
                        }

                        if (H.USE_HELI == 2)//튜렛을 붙잡은 경우 remote control 시작
                        {
                            HCT.HeliStart(player); H.USE_HELI = 3;
                        }
                        else if (H.USE_HELI == 0)//owner가 도착하지 않은 경우
                        {
                            HCT.HELI_GUNNER = player;

                            if (IsPERK[pe] < 10) Info.MessageRoop(player, 0, new[] { "^2" + (11 - IsPERK[pe]) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI", "IF ANOTHER PLAYER ONBOARD" });
                            else Info.MessageRoop(player, 0, HCT.HELI_MESSAGE_WAIT_PLAYER);
                        }
                        else if (H.USE_HELI == 1)
                        {
                            if (!HCT.HELI_ON_USE_)
                            {
                                HCT.HeliStart(player); H.USE_HELI = 3;
                            }
                        }

                    }
                });

            });

            #endregion


            HUD.AlliesHud(player, offhand.Replace("_mp", "").ToUpper());

            WP.GiveRandomWeaponTo(player);
            WP.GiveOffhandWeapon(player, offhand);

            player.SpawnedPlayer += () => human_spawned(player);

            player.Call(33531, ZERO);//setplayerangles
        }

        void SetADMIN()
        {
            ADMIN.Call(33445, "SPECT", "centerview");
            bool spect = false;
            ADMIN.OnNotify("SPECT", a =>
            {
                if (!spect)
                {
                    ADMIN.Call(33349, "freelook", true);
                    ADMIN.SetField("sessionstate", "spectator");
                }
                else
                {
                    ADMIN.Call(33349, "freelook", false);
                    ADMIN.SetField("sessionstate", "playing");
                }
                spect = !spect;
            });
            if (TEST_)
            {
                ADMIN.Call(32936);
                ADMIN.Call(33220, 2f);
            }
            //HeliSetup(ADMIN);

        }
        void SetLocationByTI(Entity player)
        {
            player.Call(33344, "^2PRESS [{+frag}] ^7TO SAVE YOUR POSITION");
            string flare_mp = "flare_mp";
            player.GiveWeapon(flare_mp);
            player.SwitchToWeaponImmediate(flare_mp);

            H_SET H = H_FIELD[player.EntRef];
            H.TI_DO = true;

            if (!H.TI_NOTIFIED)
            {
                H.TI_NOTIFIED = true;

                player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
                {
                    if (!H.TI_DO) return;
                    if (weaponName.ToString() != flare_mp) return;
                    H.TI_DO = false;
                    Vector3 pos = player.Origin;
                    int i = Call<int>(303, "misc/flare_ambient");//loadfx
                    if (i > 0)
                    {
                        Entity Effect = Call<Entity>(308, i, pos);//spawnFx
                        Call(309, Effect);//triggerfx
                        player.Call(33344, "^2NEXT SPAWN POS ^7SAVED TO " + (int)pos.X + "," + (int)pos.Y);
                        H.LOC = new[] { pos.X, pos.Y, pos.Z };

                        player.AfterDelay(10000, p => Effect.Call(32928));
                        
                    }
                });
            }
        }
        void Relocation(Entity player)
        {
            player.Call(33344, "^2Throw marker ^7to relocate your position");

            string marker = "airdrop_marker_mp";
            player.GiveWeapon(marker);
            player.SwitchToWeaponImmediate(marker);

            H_SET H = H_FIELD[player.EntRef];
            H.LOC_DO = true;

            if (!H.LOC_NOTIFIED)
            {
                H.LOC_NOTIFIED = false;

                player.OnNotify("grenade_fire", (Entity owner, Parameter mk, Parameter weaponName) =>
                {
                    if (!H.LOC_DO) return;
                    if (weaponName.ToString() != "airdrop_marker_mp") return;
                    H.LOC_DO = false;
                    Entity e = mk.As<Entity>();
                    if (e == null) return;
                    player.Call(32841, e);//linkto
                    player.AfterDelay(3000, p =>
                    {
                        player.Call(32843);//unlink
                        var v = player.Origin; v.Z += 10;
                        player.Call(33529, v);//setorigin
                        e.Call(32928);//delete
                    });
                   
                });
            }
        }
        void SetTeamName()
        {
            Call(42, "g_TeamName_Allies", "ALIVE");//setdvar
            Call(42, "g_TeamName_Axis", "BOTs");//setdvar
        }

    }
}
