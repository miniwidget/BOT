﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Tdm
{
    public partial class Tdm
    {
        #region field
        string
            SERVER_NAME, ADMIN_NAME,
            NEXT_MAP, TEAMNAME_ALLIES, TEAMNAME_AXIS,
            WELLCOME_MESSAGE;

        bool
            USE_ADMIN_SAFE_, DEPLAY_BOT_, 
            GAME_ENDED_, HUMAN_CONNECTED_;

        float
            INFECTED_TIMELIMIT, PLAYERWAIT_TIME, MATCHSTART_TIME;

        int
            t0 = 100, t1 = 1000, t2 = 2000, t3 = 3000,
            SEARCH_TIME, FIRE_TIME, BOT_DELAY_TIME, BOT_SETTING_NUM, FIRE_DIST;

        bool isAllies(Entity player) { return player.GetField<string>("sessionteam") == "allies"; }

        Random rnd = new Random();
        Entity ADMIN;
        #endregion

        #region server side
        /// <summary>
        /// server side dvar
        /// </summary>
        void Server_SetDvar()
        {
            Call("setdvar", "g_TeamName_Allies", TEAMNAME_ALLIES);
            Call("setdvar", "g_TeamName_Axis", TEAMNAME_AXIS);
            Call("setdvar", "scr_game_playerwaittime", PLAYERWAIT_TIME);
            Call("setdvar", "scr_game_matchstarttime", MATCHSTART_TIME);

            Call("setdvar", "testClients_watchKillcam", 0);
            Call("setdvar", "testClients_doReload", 0);
            //Call("setdvar", "testClients_doCrouch", 0);
            //Call("setdvar", "testClients_doMove", 1);
            //Call("setdvar", "testClients_doAttack", 1);

            readMAP();

            //if (SERVER_NAME == "^2BOT ^7TDM SERVER" || SERVER_NAME == "^2BOT ^7TDM test server") SERVER_NAME += " " + rnd.Next(1000);
            Utilities.ExecuteCommand("sv_hostname " + SERVER_NAME);
            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(new B_SET());
                H_FIELD.Add(new H_SET());
            }

        }
        //MAP_INDEX=40
        //mp_village,bot_infected,1
        void readMAP()
        {
            string currentMAP = Call<string>("getdvar", "mapname");
            string ENTIRE_MAPLIST = "mp_aground_ss|mp_alpha|mp_boardwalk|mp_exchange|mp_burn_ss|mp_bootleg|mp_bravo|mp_courtyard_ss|mp_carbon|mp_cement|mp_interchange|mp_hillside_ss|mp_crosswalk_ss|mp_dome|mp_nola|mp_hardhat|mp_italy|mp_moab|mp_restrepo_ss|mp_lambeth|mp_meteora|mp_six_ss|mp_mogadishu|mp_overwatch|mp_aground_ss|mp_paris|mp_plaza2|mp_morningwood|mp_burn_ss|mp_qadeem|mp_radar|mp_courtyard_ss|mp_roughneck|mp_seatown|mp_hillside_ss|mp_shipbreaker|mp_terminal_cls|mp_nola|mp_underground|mp_village|mp_park";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            string path = @"admin\TDM.dspl"; if (TEST_) path = @"admin\test\TDM.dspl";

            int index = 0;
            var fileContents = File.ReadAllLines(path);
            if (fileContents.Length == 2)
            {
                string firstLine = fileContents[0];
                if (!int.TryParse(firstLine.Split('=')[1], out index))
                {
                    index = map_list.IndexOf(currentMAP);
                }
            }
            else
            {
                index = map_list.IndexOf(currentMAP);
            }


            string map_name = map_list[index];
            if (map_name != currentMAP)
            {
                int current_map_idx = map_list.IndexOf(currentMAP);
                if (current_map_idx < index)
                {
                    while (index > 0)
                    {
                        if (map_list[index] == currentMAP) break;
                        index--;
                    }
                }
                else
                {
                    while (index < max)
                    {
                        if (map_list[index] == currentMAP) break;
                        index++;
                    }
                }
                map_name = map_list[index];
            }


            int[] smallMap = { 0, 4, 7, 11, 14, 18, 21, 24, 28, 31, 34, 37 };
            int[] largeMap = { 3, 10, 17, 27, 40 };

            if (smallMap.Contains(index))
            {
                FIRE_DIST = 600;
            }
            else if (largeMap.Contains(index))
            {
                FIRE_DIST = 850;
            }
            else
            {
                FIRE_DIST = 750;
            }
            index++;
            if (index > max) index = 0;

            NEXT_MAP = map_list[index];
            Call("setdvar", "sv_nextmap", NEXT_MAP);

            if (TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");
                writrMAP(index, path);
            }

        }
        void writrMAP(int idx, string path)
        {
            string content = "//MAP_INDEX=" + idx + "\n" + NEXT_MAP + ",TDM,1";

            File.WriteAllText(path, content);
        }
        #endregion

        #region Bots side

        /// <summary>
        /// BOT SET class for custom fields set
        /// </summary>
        class B_SET
        {
            public Entity target { get; set; }
            public int death { get; set; }
            public bool fire { get; set; }
            public bool temp_fire { get; set; }
            public string wep { get; set; }
        }
        List<B_SET> B_FIELD = new List<B_SET>(18);
        //Dictionary<int, int> BOT_ID = new Dictionary<int, int>();
        List<Entity> BOTs_List = new List<Entity>();
        #endregion

        #region Human side
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
            public bool USE_TANK { get; set; }
            public string TEAM { get; set; }

        }
        List<H_SET> H_FIELD = new List<H_SET>(18);
        //Dictionary<int, int> H_ID = new Dictionary<int, int>();
        List<Entity> human_List = new List<Entity>();

        List<Entity> H_AXIS_LIST = new List<Entity>();
        List<Entity> H_ALLIES_LIST = new List<Entity>();
        #endregion



        #region client side

        /// <summary>
        /// client side dvar & set notifycommand & give weapon & set HUD & change class
        /// </summary>
        void Client_init_GAME_SET(Entity player)
        {

            #region set
            human_List.Add(player);

            H_SET H = H_FIELD[player.EntRef];
            H.PERK = 2;
            H.TEAM = null;
            H.USE_TANK = false;
            #endregion

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

            #region notifyonplayercommand

            player.Call("notifyonplayercommand", "+TAB", "+scores");
            player.Call("notifyonplayercommand", "-TAB", "-scores");
            player.Call("notifyonplayercommand", "HOLD_STRAFE", "+strafe");
            player.Call("notifyonplayercommand", "HOLD_CROUCH", "+movedown");
            player.Call("notifyonplayercommand", "HOLD_PRONE", "+prone");
            player.Call("notifyonplayercommand", "HOLD_STANCE", "+stance");
            //+strafe

            player.OnNotify("HOLD_STRAFE", ent =>
            {
                var weapon = player.CurrentWeapon;
                if (weapon.Length > 3 && weapon[2] == '5')
                {
                    player.Call("givemaxammo", weapon);
                }
            });

            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                giveAttachScope(player);
            });

            player.OnNotify("HOLD_PRONE", ent =>//attachment silencer heartbeat,
            {
                giveAttachHeartbeat(player);
            });

            string offhand = "";
            switch (rnd.Next(4))
            {
                case 0: offhand = "frag_grenade_mp"; break;
                case 1: offhand = "semtex_mp"; break;//OK
                case 2: offhand = "bouncingbetty_mp"; break;//OK
                case 3: offhand = "claymore_mp"; break;//OK
            }

            player.OnNotify("HOLD_STANCE", ent =>//offhand weapon
            {
                giveOffhandWeapon(player, offhand);
            });
            
            player.OnNotify("menuresponse", (p, Menu, Response) =>
            {
                string menu = Menu.ToString();
                string resp = Response.ToString();

                if (menu=="changeclass" && resp=="axis_recipe1")
                {
                    AfterDelay(100, () =>
                    {
                        p.Notify("menuresponse", "changeclass", "back");
                        AfterDelay(100, () =>
                        {
                            p.Notify("menuresponse", "changeclass", "axis_recipe4");
                            p.Call("suicide");
                        });
                    });
                }
            });
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
                }
            });
            player.OnNotify("end_remote", (Entity ent) =>
            {
                if (H.USE_TANK)
                {
                    H.USE_TANK = false;
                    human_List.Remove(TANK);
                }
            });
            #endregion

            #endregion

            #region AlliesHud
            AlliesHud(player, offhand.Replace("_mp", "").ToUpper());
            #endregion

            player.SpawnedPlayer += () => human_spawned(player);
        }
        #endregion

    }
}
