﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    class Field
    {
        internal Entity player;
        internal bool BOT;
        internal int killerIdx=-1;
        bool _wait;
        internal bool wait
        {
            get
            {
                return this._wait;
            }
            set
            {
                if(player ==null) return;
                
                if(value == true)
                {
                    _hti = -1;
                    this.player.Health = -1;
                    this.player.Call(32848);//hide
                    this.player.Call(33220, 0f);//setmovescale
                    this.player.Call(33468, weapon, 0);//setweaponammoclip
                    this.player.Call(33469, weapon, 0);//setweaponammostock
                }
                else
                {
                    this.player.Health = 150;
                    this.player.Call(33220, 1f);
                    this.player.Call(32847);//show
                }
                this._wait = value;
            }
        }
        int _hti;
        internal int human_target_idx
        {
            get
            {
                return _hti;
            }
            set
            {
                if(value == -1)
                {
                    damaged = false;
                }
                _hti = value;
            }
        }

        internal bool damaged;
        internal string weapon;

        int att = 0;
        internal int SIRENCERorHB
        {
            get
            {
                this.att++;
                if (this.att > 2) this.att = 0;
                return this.att;
            }
        }
        internal bool RESPAWN;
        internal int LIFE;
        internal int PERK;

        internal bool USE_TANK;
        internal byte USE_HELI;
        internal bool ON_MESSAGE;
        //internal bool SHOTGUN;

        internal bool AXIS;
        internal byte AX_WEP;
        internal bool BY_SUICIDE;

    }

    public partial class Infected
    {

        #region field
        string SERVER_NAME, ADMIN_NAME, TEAMNAME_ALLIES, TEAMNAME_AXIS, NEXT_MAP, WELLCOME_MESSAGE;

        bool DEPLAY_BOT_, SUICIDE_BOT_, GAME_ENDED_, HELI_ON_USE_;
        
        float INFECTED_TIMELIMIT, PLAYERWAIT_TIME, MATCHSTART_TIME;
        
        int
            SEARCH_TIME, FIRE_TIME, BOT_DELAY_TIME, FIRE_DIST,
            BOT_SETTING_NUM, PLAYER_LIFE, MAP_INDEX;

        static Random rnd = new Random();
        Vector3 HELI_WAY_POINT;
        Entity ADMIN, HELI, TL, TR, HELI_OWNER, HELI_GUNNER;

        bool isSurvivor(Entity ent) { return ent.GetField<string>("sessionteam") == "allies"; }


        readonly int t1 = 1000, t2 = 2000, t3 = 3000;
        readonly Vector3 ZERO = new Vector3(0, 0, 0), z50 = new Vector3(0, 0, 50);


        List<Field> FL = new List<Field>(18);
        List<Entity> BOTs_List = new List<Entity>();
        List<Entity> HUMAN_LIST = new List<Entity>();
        List<Entity> HUMAN_AXIS_LIST = new List<Entity>();

        PerkList CPL = new PerkList();
        MessageText MT = new MessageText();
        Weapon WP = new Weapon( ref rnd);
        #endregion

        void ServerSetDvar()
        {
            //setdvar
            Call(42, "scr_game_playerwaittime", PLAYERWAIT_TIME);
            Call(42, "scr_game_matchstarttime", MATCHSTART_TIME);
            Call(42, "scr_game_allowkillcam", "0");
            Call(42, "scr_infect_timelimit", INFECTED_TIMELIMIT);

            Call(42, "testClients_watchKillcam", 0);
            Call(42, "testClients_doReload", 0);
            Call(42, "testClients_doCrouch", 1);
            Call(42, "testClients_doMove", 0);
            Call(42, "testClients_doAttack", 0);

            //connect 175.114.149.215:27015
            Utilities.ExecuteCommand("sv_hostname ^2BOT ^7INF CRASH TEST");
            //Utilities.ExecuteCommand("sv_hostname " + SERVER_NAME);
            for (int i = 0; i < 18; i++)
            {
                FL.Add(new Field());
            }

        }

        void readMAP()
        {
            string currentMAP = Call<string>("getdvar", "mapname");
            string ENTIRE_MAPLIST = "mp_plaza2|mp_mogadishu|mp_bootleg|mp_carbon|mp_dome|mp_exchange|mp_lambeth|mp_hardhat|mp_interchange|mp_alpha|mp_bravo|mp_radar|mp_paris|mp_seatown|mp_underground|mp_village|mp_morningwood|mp_park|mp_overwatch|mp_italy|mp_cement|mp_qadeem|mp_meteora|mp_hillside_ss|mp_restrepo_ss|mp_aground_ss|mp_courtyard_ss|mp_terminal_cls|mp_burn_ss|mp_nola|mp_six_ss|mp_moab";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            MAP_INDEX = map_list.IndexOf(currentMAP);
            
            if (PLAYER_LIFE == 0) PLAYER_LIFE = 2;/*set player life */

            switch (MAP_INDEX)
            {
                case 00: HELI_WAY_POINT = new Vector3(-338.2646f, 2086.079f, 780.125f); break;
                case 01: HELI_WAY_POINT = new Vector3(-528.8417f, 3310.055f, 96.125f); break;
                case 02: HELI_WAY_POINT = new Vector3(990.2207f, 1219.364f, -87.875f); break;
                case 03: HELI_WAY_POINT = new Vector3(-1587.473f, -4330.379f, 3862.001f); break;
                case 04: HELI_WAY_POINT = new Vector3(574.9448f, -562.5687f, -348.6788f); break;
                case 05: HELI_WAY_POINT = new Vector3(2458.22f, 1552.072f, 148.4162f); break;
                case 06: HELI_WAY_POINT = new Vector3(1262, 2668, -272); break;
                case 07: HELI_WAY_POINT = new Vector3(1903, -1220, 380); break;
                case 08: HELI_WAY_POINT = new Vector3(2535, -573, 100); break;
                case 09: HELI_WAY_POINT = new Vector3(839.8029f, -397.5345f, -5.537515f); break;
                case 10: HELI_WAY_POINT = new Vector3(-1826.247f, 637.1963f, 1049.175f); break;
                case 11: HELI_WAY_POINT = new Vector3(-3441, -660, 1162); break;
                case 12: HELI_WAY_POINT = new Vector3(662, -952, 112); break;
                case 13: HELI_WAY_POINT = new Vector3(-1515.302f, 1060.371f, 50.338187f); break;
                case 14: HELI_WAY_POINT = new Vector3(-45.68791f, -926.9956f, 0.1250008f); break;
                case 15: HELI_WAY_POINT = new Vector3(-253.9192f, -1614.135f, 352.125f); break;
                case 16: HELI_WAY_POINT = new Vector3(-1601, -1848, 620); break;
                case 17: HELI_WAY_POINT = new Vector3(3342.936f, -2147.835f, 201.125f); break;
                case 18: HELI_WAY_POINT = new Vector3(687.1823f, 2715.979f, 12670.13f); break;
                case 19: HELI_WAY_POINT = new Vector3(1073.359f, -1203.881f, 704.125f); break;
                case 20: HELI_WAY_POINT = new Vector3(1434.789f, -348.4124f, 337.4833f); break;
                case 21: HELI_WAY_POINT = new Vector3(2362, 1607, 220); break;
                case 22: HELI_WAY_POINT = new Vector3(1880, -469, 1574); break;
                case 23: HELI_WAY_POINT = new Vector3(53.63142f, 639.6542f, 2171.125f); break;
                case 24: HELI_WAY_POINT = new Vector3(131.7994f, 1357.19f, 1800.125f); break;
                case 25: HELI_WAY_POINT = new Vector3(404.5297f, 725.2506f, 452.8782f); break;
                case 26: HELI_WAY_POINT = new Vector3(1012.743f, 398.3938f, 168.125f); break;
                case 27: HELI_WAY_POINT = new Vector3(2481.313f, 2897.811f, 40.125f); break;
                case 28: HELI_WAY_POINT = new Vector3(-1523.346f, -276.3622f, 166.625f); break;
                case 29: HELI_WAY_POINT = new Vector3(-4.174267f, -142.9193f, 34.59882f); break;
                case 30: HELI_WAY_POINT = new Vector3(724.9612f, -1579.811f, 186.125f); break;
                case 31: HELI_WAY_POINT = new Vector3(-1456.08f, 1086.234f, 323.125f); break;
            }

            if (new int[] { 23, 24, 25, 26, 28, 29, 30 }.Contains(MAP_INDEX))//small map
            {
                PLAYER_LIFE += 1;
                FIRE_DIST = 600;
            }
            else if (new int[] { 8, 16, 17, 31 }.Contains(MAP_INDEX))//large map
            {
                FIRE_DIST = 850;
            }
            else//medium
            {
                FIRE_DIST = 750;
            }

            Call("precachemodel", "prop_flag_neutral");
            Call("precacheVehicle", "attack_littlebird_mp");
            HELI_WAY_POINT.Z += 150;

            /* mini map point */
            Call(431, 17, "active"); // objective_add
            Call(435, 17, HELI_WAY_POINT); // objective_position
            Call(434, 17, "compass_objpoint_ac130_friendly"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

            print("map: " + MAP_INDEX + "/" + max);

            //set next map
            if (MAP_INDEX >= max || MAP_INDEX < 0) MAP_INDEX = 0; else MAP_INDEX++;
            NEXT_MAP = map_list[MAP_INDEX];

            Call("setdvar", "sv_nextmap", NEXT_MAP);

#if DEBUG
            //TEST_ = true;
#endif
            if (TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");
                string content = NEXT_MAP + ",INF,1";
                File.WriteAllText(GetPath("admin\\INF.dspl"), content);
            }
        }

        void setTeamName()
        {
            Call("setdvar", "g_TeamName_Allies", TEAMNAME_ALLIES);
            Call("setdvar", "g_TeamName_Axis", TEAMNAME_AXIS);
        }

        string GetPath(string path)
        {
            if (TEST_)
            {
                path = path.Replace("admin\\", "admin\\test\\");
            }
            return path;
        }

        int getBOTCount
        {
            get
            {
                int botCount = 0;
                foreach (Entity p in Players)
                {

                    if (p == null)
                    {
                        continue;
                    }
                    else if (p.Name.StartsWith("bot"))
                    {
                        botCount++;
                    }
                }
                return botCount;
            }
        }

        //IMPORTANT
        bool TEST_;
        void CheckTEST()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (assembly.Location.Contains("test"))
            {
                TEST_ = true;
                print("■ " + assembly.GetName().Name + ".dll & TEST MODE");
            }
        }
    }
}
