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

            if (player.Name == ADMIN_NAME)
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
                player.SpawnedPlayer += () => human_spawned(player);
                player.Call(33341);//"suicide"
            }

        }
        void Set_hset(H_SET H, bool Axis, bool init)
        {
            H.BY_SUICIDE = false;
            H.USE_TANK = false;

            if (init)
            {
                H.LOC_NOTIFIED = false;
            }
            H.LOC_DO = false;
            H.AXIS = Axis;

            if (Axis)
            {
                H.LIFE = -2;
                H.USE_HELI = 4;
                H.AX_WEP = 1;
            }
            else
            {
                H.USE_HELI = 0;
                H.AX_WEP = 0;
                H.PERK = 2;
                H.LIFE -= 1;
            }
        }

        void SetPlayer(Entity player)
        {
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            human_List.Add(player);
            int pe = player.EntRef;
            H_SET H = H_FIELD[pe];
            Set_hset(H,false, true);
            H.LIFE = PLAYER_LIFE;

            #region SetClientDvar

            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");

            #endregion

            #region notifyonplayercommand

            player.Call(33445, "HOLD_STRAFE", "+strafe");//notifyonplayercommand
            player.OnNotify("HOLD_STRAFE", ent =>
            {
                if (H.AXIS) return;
                var weapon = player.CurrentWeapon;
                if (weapon[2] == '5')
                {
                    player.Call(33523, weapon);//"givemaxammo"
                }
            });

            player.Call(33445, "HOLD_CROUCH", "+movedown");
            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                if (H.AXIS) return;
                WP.GiveAttachScope(player);
            });

            //player.Call(33445, "HOLD_STANCE", "+stance");
            //player.OnNotify("HOLD_STANCE", ent =>//offhand weapon
            //{
            //    if (IsAXIS[pe]) return;
            //    WP.GiveRandomOffhandWeapon(player);
            //});

            #endregion

            #region TANK

            Entity TANK = null;
            player.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                if (H.USE_TANK) return;

                string weap = newWeap.ToString();
                if (weap != "killstreak_remote_tank_remote_mp") return;
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
            });
            player.OnNotify("end_remote", (Entity ent) =>
            {
                if (!H.USE_TANK) return;
                H.USE_TANK = false;
                human_List.Remove(TANK);
                human_List.Add(player);
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
                if (H.AXIS || H.USE_TANK) return;//Axis or OnMessage or HELI null

                if (player.CurrentWeapon[2] != '5') return;

                player.AfterDelay(500, x =>
                {
                    if (player.Call<int>(33533) == 1) return;//usebuttonpressed
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

                            if (H.PERK < 10) Info.MessageRoop(player, 0, new[] { "^2" + (11 - H.PERK) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI", "IF ANOTHER PLAYER ONBOARD" });
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

            HUD.AlliesHud(player);

            WP.GiveRandomWeaponTo(player);
            WP.GiveRandomOffhandWeapon(player);

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
        }
        void Relocation(Entity player, bool getback)
        {
            H_SET H = H_FIELD[player.EntRef];

            if (getback)
            {
                if (!H.LOC_NOTIFIED) return;
                player.Call(33529, H.RELOC);
                return;
            }
            player.Call(33344, "^2THROW MARKER ^7to Relocate Your Position");

            string marker = "airdrop_sentry_marker_mp";
            player.GiveWeapon(marker);
            player.SwitchToWeaponImmediate(marker);
            
            H.LOC_DO = true;
            H.RELOC = player.Origin;

            if (!H.LOC_NOTIFIED)
            {
                H.LOC_NOTIFIED = true;
                
                player.OnNotify("grenade_fire", (Entity owner, Parameter mk, Parameter weaponName) =>
                {
                    if (!H.LOC_DO) return;
                    if (weaponName.ToString() != "airdrop_sentry_marker_mp") return;

                    H.LOC_DO = false;
                    Entity e = mk.As<Entity>();
                    if (e == null) return;

                    Vector3 mk_pos = e.GetField<Vector3>("angles");
                    if (mk_pos.Z != 0)
                    {
                        mk_pos.Z = 0;
                        e.SetField("angles", mk_pos);
                        player.Call(33531, mk_pos);
                    }
                    player.Call(32841, e);//linkto
                    player.AfterDelay(3000, p =>
                    {
                        player.Call(32843);//unlink
                        e.Call(32928);//delete
                        p.AfterDelay(200, x =>
                        {
                            int ground = player.Call<int>(33538);
                            if (ground == 0)
                            {
                                player.Call(33344, "TYPE ^2RELOC ^7IF GET BACK LOCATION");
                            }
                        });
                    });
                });
            }
        }
        void SetTeamName()
        {
            Call(42, "g_TeamName_Allies", "ALIVE");//setdvar
            Call(42, "g_TeamName_Axis", "BOTs");//setdvar
        }

        /*
        void SetLocationByTI(Entity player)
        {
            player.Call(33344, "^2PRESS [{+attack}] ^7TO SAVE YOUR POSITION");

            string flare_mp = "flare_mp";
            player.GiveWeapon(flare_mp);
            player.SwitchToWeaponImmediate(flare_mp);

            int pe = player.EntRef;
            H_SET H = H_FIELD[pe];
            H.TI_DO = true;

            if (!H.TI_NOTIFIED)
            {
                H.TI_NOTIFIED = true;

                player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
                {
                    if (!H.TI_DO) return;
                    if (H.AXIS) return;
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
        */
    }
}
