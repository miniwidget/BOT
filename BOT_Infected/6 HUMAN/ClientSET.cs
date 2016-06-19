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
        #region Human side field
        /// <summary>
        /// HUMAN PLAYER SET class for custom fields set
        /// </summary>
        class H_SET
        {
            int att = 0;
            public int SIRENCERorHB
            {
                get
                {
                    this.att++;
                    if (this.att > 2) this.att = 0;
                    return this.att;
                }
            }
            public int PERK { get; set; }
            public int AX_WEP { get; set; }
            public bool BY_SUICIDE { get; set; }
            public int LIFE { get; set; }
            public bool RESPAWN { get; set; }
            //public bool CLASS_CHANGED { get; set; }
            //public HudElem WEAPONINFO { get; set; }
            public bool USE_TANK { get; set; }
            public byte USE_HELI { get; set; }//0 1 2
            public bool ON_MESSAGE { get; set; }


        }
        List<H_SET> H_FIELD = new List<H_SET>(18);
        List<Entity> human_List = new List<Entity>();
        List<Entity> HUMAN_AXIS_LIST = new List<Entity>();


        string[] HELI_MESSAGE_KEY_INFO = { "HELI INFO", "^2[^7 [{+breath_sprint}] ^2] MOVE DOWN", "^2[^7 [{+gostand}] ^2] MOVE UP" };
        string[] HELI_MESSAGE_ACTIVATE = { "^2[ ^7HELI TURRET ^2] ENABLED", "GO TO THE HELI AREA  AND", "^2PRESS [^7 [{+activate}] ^2] AT THE HELI TURRET" };
        string[] HELI_MESSAGE_ALERT = { "YOU ARE NOT IN THE HELI AREA", "GO TO HELI AREA AND", "^2PRESS [^7 [{+activate}] ^2] AT THE HELI TURRET" };
        #endregion

        /// <summary>
        /// client side dvar & set notifycommand & give weapon & set HUD & change class
        /// </summary>
        void Client_init_GAME_SET(Entity player)
        {
            /* change class */
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            /* set */
            human_List.Add(player);
            H_SET H = H_FIELD[player.EntRef];
            H.LIFE = PLAYER_LIFE;
            H.PERK = 2;
            H.AX_WEP = 0;
            H.BY_SUICIDE = false;
            H.USE_TANK = false;

            #region SetClientDvar

            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");
            player.SetClientDvar("cg_drawBreathHint", "0");
            player.SetClientDvar("cg_scoreboardpingtext", "1");
            player.SetClientDvar("cg_brass", "1");
            player.SetClientDvar("waypointIconHeight", "13");
            player.SetClientDvar("waypointIconWidth", "13");

            //player.SetClientDvar("cg_fov", "75");
            //player.SetClientDvar("clientsideeffects", "1");
            //player.SetClientDvar("cl_maxpackets", "60");
            //player.SetClientDvar("com_maxfps", "91");
            //player.SetClientDvar("r_fog", "1");
            //player.SetClientDvar("r_distortion", "1");
            //player.SetClientDvar("r_dlightlimit", "4");
            //player.SetClientDvar("fx_drawclouds", "1");
            //player.SetClientDvar("snaps", "20");

            #endregion

            #region helicopter

            player.Call("notifyonplayercommand", "ACTIVATE", "+activate");
            player.OnNotify("ACTIVATE", ent =>
            {
                if (H.USE_HELI == 4) return;//Axis

                if (H.USE_HELI == 0)//Allise & under 10 kill
                {
                    if (H.ON_MESSAGE) return;

                    if (!IsHeliArea(player))// not in the area
                    {
                        if (HELI_GUNNER == player) HELI_GUNNER = null;
                    }
                    else
                    {
                        if (H.PERK < 10) RM(player, 0, new[] { "^2" + (11 - H.PERK) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI", "IF ANOTHER PLAYER ONBOARD" });
                        else RM(player, 0, new[] { "YOU CAN RIDE HELLI", "IF ANOTHER PLAYER ONBOARD" });

                        if (HELI_GUNNER == null || HELI_GUNNER != player) HELI_GUNNER = player;
                    }

                    return;
                }
                else if (H.USE_HELI == 1) // above 10 kill
                {
                    if (HELI == null)
                    {
                        H.USE_HELI = 2;
                        CallHeli(player);
                        return;
                    }
                    else if (HELI_ON_USE_)
                    {
                        showMessage(player, "^2HELICOPTER IS OCCUPIED BY ^7" + HELI_OWNER_NAME);
                        return;
                    }
                    else
                    {
                        H.USE_HELI = 2;
                    }
                }

                if (H.USE_HELI == 2)
                {
                    if (!IsHeliArea(player))
                    {
                        RM(player, 0, HELI_MESSAGE_ALERT);
                    }
                    else
                    {
                        H.USE_HELI = 3;
                        StartHeli(player);
                    }
                }
                else if (H.USE_HELI == 3)
                {
                    H.USE_HELI = 0;
                    EndUseHeli(player, true);
                    return;
                }
            });

            #endregion

            #region ammo
            player.Call("notifyonplayercommand", "HOLD_STRAFE", "+strafe");
            player.OnNotify("HOLD_STRAFE", ent =>
            {
                if (H.AX_WEP != 0) return;

                var weapon = player.CurrentWeapon;
                if (weapon.Length > 3 && weapon[2] == '5')
                {
                    player.Call("givemaxammo", weapon);
                }
            });
            #endregion

            #region weapon attachment
            player.Call("notifyonplayercommand", "HOLD_CROUCH", "+movedown");
            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                if (H.AX_WEP != 0)
                {
                    giveOffhandWeapon(player, "throwingknife");
                    return;
                }

                giveAttachScope(player);

            });

            player.Call("notifyonplayercommand", "HOLD_PRONE", "+prone");
            player.OnNotify("HOLD_PRONE", ent =>//attachment silencer heartbeat,
            {
                if (H.AX_WEP != 0)
                {
                    giveOffhandWeapon(player, "bouncingbetty_mp");
                    return;
                }
                giveAttachHeartbeat(player);
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
                if (H.AX_WEP != 0)
                {
                    giveOffhandWeapon(player, "claymore_mp");
                    return;
                }
                giveOffhandWeapon(player, offhand);
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

                                //print("들어왔다 "+TANK.Name);
                                human_List.Add(TANK);
                                player.Call(32936);//thermalvisionfofoverlayon

                            }
                        });
            player.OnNotify("end_remote", (Entity ent) =>
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
            AlliesHud(player, offhand.Replace("_mp", "").ToUpper());
            #endregion

            #region giveweapon
            giveWeaponTo(player, getRandomWeapon());
            player.AfterDelay(500, x => giveOffhandWeapon(player, offhand));
            #endregion
        }

    }
}
