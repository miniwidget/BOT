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
                my.print(name + " connected ♥");
                SetPlayer(player);
            }
            else
            {
                my.print("AXIS connected ☜");
                SetAxis(player.EntRef);
                player.SetField("sessionteam", "axis");
                human_List.Remove(player);
                player.AfterDelay(100, p =>
                {
                    player.Call(33341);//"suicide"
                    player.Notify("menuresponse", "changeclass", "axis_recipe4");
                });
            }

        }
        void SetPlayer(Entity player)
        {
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            human_List.Add(player);
            int pe = player.EntRef;
            SetAlly(pe);

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
                                int ti = TK.IsTankOwner(player);
                                if (ti != 5)
                                {
                                    TK.TankEnd(player, ti);
                                }
                                else
                                {
                                    Info.MessageRoop(player, 0, HCT.HELI_MESSAGE_ALERT);
                                }   
                                return;
                            }
                        }
                        int i = TK.IsTankOwner(player);
                        if (i != 5)
                        {
                            TK.TankEnd(player, i);
                            return;
                        }
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
        void SetAxis(int pe)
        {
            H_SET H = H_FIELD[pe];
            H.LIFE = -2;
            H.AX_WEP = 1;
            H.BY_SUICIDE = false;
            IsAXIS[pe] = true;
            H.USE_HELI = 4;

        }
        void SetAlly(int pe)
        {
            H_SET H = H_FIELD[pe];
            H.LIFE = 2;
            H.AX_WEP = 0;
            H.BY_SUICIDE = false;
            H.USE_TANK = false;
            H.USE_HELI = 0;
            IsAXIS[pe] = false;
            IsPERK[pe] = 2;
        }

    }
}
