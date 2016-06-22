using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    public partial class Infected
    {

        /// <summary>
        /// client side dvar & set notifycommand & give weapon & set HUD & change class
        /// </summary>
        void Client_init_GAME_SET(Entity player)
        {
            /* change class */
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            /* set */
            human_List.Add(player);
            Field H = FL[player.EntRef];
            H.LIFE = PLAYER_LIFE;
            H.PERK = 2;

            H.USE_TANK = false;
            H.USE_HELI =0;
            H.ON_MESSAGE = false;

            H.AXIS = false;
            H.AX_WEP = 0;
            H.BY_SUICIDE = false;

            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");

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

                if (player.CurrentWeapon.Contains("kill"))return;

                player.AfterDelay(500, x =>
                {
                    if (!isUsingTurret(ref player))//heli 생성
                    {
                        if (HELI == null)
                        {
                            if (H.USE_HELI == 1) { HeliCall(player); H.USE_HELI = 2; }
                        }
                        else
                        {
                            if (H.USE_HELI == 3 && HELI_OWNER == player)
                            {
                                HeliEndUse(player, true); H.USE_HELI = 0;
                            }
                            else if (H.USE_HELI == 1 && !isHeliArea(ref player))
                            {
                                RM(player, 0, MT.HELI_MESSAGE_ALERT);
                            }
                        }
                    }
                    else
                    {
                        if (HELI == null || HELI != null && !isHeliArea(ref player)) return;/*다른 튜렛을 사용 중인 경우*/

                        if (H.USE_HELI == 2)//튜렛을 붙잡은 경우 remote control 시작
                        {
                            HeliStart(player); H.USE_HELI = 3;
                        }
                        else if (H.USE_HELI == 0)//owner가 도착하지 않은 경우
                        {
                            HELI_GUNNER = player;

                            if (H.PERK < 10) RM(player, 0, new[] { "^2" + (11 - H.PERK) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI", "IF ANOTHER PLAYER ONBOARD" });
                            else RM(player, 0, MT.HELI_MESSAGE_WAIT_PLAYER);
                        }
                        else if (H.USE_HELI == 1)
                        {
                            if (!HELI_ON_USE_)
                            {
                                HeliStart(player); H.USE_HELI = 3;
                            }
                        }

                    }
                });

            });

            #endregion

            #region ammo
            player.Call(33445, "HOLD_STRAFE", "+strafe");
            player.OnNotify("HOLD_STRAFE", ent =>
            {
                if (H.AXIS) return;

                var weapon = player.CurrentWeapon;
                if (weapon.Length > 3 && weapon[2] == '5')
                {
                    player.Call("givemaxammo", weapon);
                }
            });
            #endregion

            #region weapon attachment
            player.Call(33445, "HOLD_CROUCH", "+movedown");
            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                if (H.AXIS) 
                {
                    WP.giveOffhandWeapon(ref player, "throwingknife");
                    return;
                }

                WP.giveAttachScope(ref player);

            });

            player.Call(33445, "HOLD_PRONE", "+prone");
            player.OnNotify("HOLD_PRONE", ent =>//attachment silencer heartbeat,
            {
                if (H.AXIS) 
                {
                    WP.giveOffhandWeapon(ref player, "bouncingbetty_mp");
                    return;
                }
                WP.giveAttachHeartbeat(ref player);
            });
            #endregion

            #region offhand weapon
            string offhand = "";

            switch (rnd.Next(4))
            {
                case 0: offhand = "frag_grenade_mp"; break;
                case 1: offhand = "semtex_mp"; break;//OK
                case 2: offhand = "bouncingbetty_mp"; break;//OK
                case 3: offhand = "claymore_mp"; break;//OK
            }

            player.Call("notifyonplayercommand", "HOLD_STANCE", "+stance");
            player.OnNotify("HOLD_STANCE", ent =>//offhand weapon
            {
                if (H.AXIS)
                {
                    WP.giveOffhandWeapon(ref player, "claymore_mp");
                    return;
                }
                WP.giveOffhandWeapon(ref player, offhand);
            });
            #endregion

            #region TANK

            Entity TANK = null;
            player.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                if (H.USE_TANK) return;

                string weap = newWeap.ToString();
                //print(weap);

                if (weap == "killstreak_remote_tank_remote_mp")
                {
                    H.USE_TANK = true;
                    TANK = null;

                    bool found = false;
                    for (int i = 0; i < 2048; i++)
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
                    player.Call(32936);//thermalvisionfofoverlayon
                }
            });
            player.OnNotify("end_remote", e =>
            {
                if (H.USE_TANK)
                {
                    H.USE_TANK = false;
                    human_List.Remove(TANK);
                    player.Call(32937);//thermalvisionfofoverlayoff
                }
            });

            #endregion

            #region AlliesHud
            HudAllies(player, offhand.Replace("_mp", "").ToUpper());
            #endregion

            #region giveweapon
            WP.giveWeaponTo(ref player, WP.getRandomWeapon());
            player.AfterDelay(500, x => WP.giveOffhandWeapon(ref player, offhand));
            #endregion
        }

    }
}
