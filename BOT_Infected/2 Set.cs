﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    /// <summary>
    /// BOT SET class for custom fields set
    /// </summary>
    class B_SET
    {
        internal Entity target { get; set; }
        internal int death { get; set; }
        internal string wep;
        internal int killer = -1;
        internal bool not_fire;

        //internal bool temp_fire { get; set; }
        //internal bool fire { get; set; }
        //internal bool wait { get; set; }
    }

    class H_SET
    {
        public H_SET(int life)
        {
            this.LIFE = life;
        }
        /// <summary>
        /// player life chances before being infected
        /// </summary>
        internal int LIFE;
        internal bool RESPAWN;

        /// <summary>
        /// perk count
        /// </summary>
        internal int PERK = 2;
        internal HudElem HUD_PERK_COUNT, HUD_TOP_INFO, HUD_RIGHT_INFO, HUD_SERVER;
        internal string PERK_TXT = "PRDT **";

        /// <summary>
        /// 0: Allies under 10kill IN ALLIES /
        /// 1: ready to call heli /
        /// </summary>
        internal bool CAN_USE_HELI;

        /// <summary>
        /// 0 not using remote /
        /// 1 remote helicopter /
        /// 2 remote tank /
        /// </summary>
        internal byte REMOTE_STATE;
        /// <summary>
        /// when roop massage, if on_message state, it blocks reapeted roop
        /// </summary>
        internal bool ON_MESSAGE;

        /// <summary>
        /// related to relocation
        /// </summary>
        internal bool LOC_NOTIFIED;
        internal bool LOC_DO;
        internal Vector3 RELOC;

        /// <summary>
        /// test ac130
        /// </summary>
        internal bool AC130_NOTIFIED;
        internal bool AC130_ON_USE;

        /// <summary>
        /// is Axis
        /// </summary>
        internal bool AXIS;
        /// <summary>
        /// Axis weapon state count
        /// </summary>
        internal int AX_WEP;

        internal bool USE_PREDATOR;
        internal bool PREDATOR_NOTIFIED;
    }

    class Set
    {
        internal readonly int BOT_SETTING_NUM;
        internal static bool TURRET_MAP;
        internal bool DEPLAY_BOT_, TEST_, USE_ADMIN_SAFE_, ATTACK_;
        internal int PLAYER_LIFE;
        internal byte BOT_CLASS_NUM = 3;
        bool MAP_ROTATE_;

        public Set()
        {

#region Load Custom Setting from a set.txt file

            string setFile = "admin\\INF.txt";

            if (File.Exists(setFile))
            {
                using (StreamReader set = new StreamReader(setFile))
                {
                    bool b;
                    int num;
                    while (!set.EndOfStream)
                    {
                        string line = set.ReadLine();
                        if (line.StartsWith("//") || line.Equals(string.Empty)) continue;

                        string[] split = line.Split('=');
                        if (split.Length < 1) continue;

                        string name = split[0];
                        string value = split[1];
                        var comment = value.IndexOf("//");
                        if (comment != -1) value = value.Substring(0, comment);

                        switch (name)
                        {
                            case "SERVER_NAME": Hud.SERVER_NAME_ = value; break;
                            case "ADMIN_NAME": Infected.ADMIN_NAME = value; break;

                            case "BOT_SETTING_NUM": if (int.TryParse(value, out num)) BOT_SETTING_NUM = num; break;
                            case "PLAYER_LIFE": if (int.TryParse(value, out num)) PLAYER_LIFE = num; break;

                            case "DEPLAY_BOT_": if (bool.TryParse(value, out b)) DEPLAY_BOT_ = b; break;
                            case "USE_ADMIN_SAFE_": if (bool.TryParse(value, out b)) USE_ADMIN_SAFE_ = b; break;
                            case "MAP_ROTATE_": if (bool.TryParse(value, out b)) MAP_ROTATE_ = b; break;
                            case "TEST_": if (bool.TryParse(value, out b)) TEST_ = b; break;
                            case "ATTACK_": if (bool.TryParse(value, out b)) ATTACK_ = b; break;
                        }
                    }
               }
            }


#endregion

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (assembly.Location.Contains("test"))
            {
                TEST_ = true;
            }

            if (TEST_) Utilities.ExecuteCommand("sv_hostname TEST");
            else Utilities.ExecuteCommand("sv_hostname " + Hud.SERVER_NAME_);

            ReadMAP();
        }
        internal byte MAP_IDX;
        void ReadMAP()
        {
            Function.SetEntRef(-1);
            string map = Function.Call<string>(47, "mapname");//"getdvar"
            string ENTIRE_MAPLIST = "mp_plaza2|mp_mogadishu|mp_bootleg|mp_carbon|mp_dome|mp_exchange|mp_lambeth|mp_hardhat|mp_interchange|mp_alpha|mp_bravo|mp_radar|mp_paris|mp_seatown|mp_underground|mp_village|mp_morningwood|mp_park|mp_overwatch|mp_italy|mp_cement|mp_qadeem|mp_meteora|mp_hillside_ss|mp_restrepo_ss|mp_aground_ss|mp_courtyard_ss|mp_terminal_cls|mp_burn_ss|mp_nola|mp_six_ss|mp_moab|mp_boardwalk|mp_crosswalk_ss|mp_roughneck|mp_shipbreaker";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            MAP_IDX = (byte)map_list.IndexOf(map);
            float[] fff = null;

            switch (MAP_IDX)
            {
                case 00: fff = new float[3] { -338.2646f, 2086.079f, 780.125f }; break;
                case 01: fff = new float[3] { -528.8417f, 3310.055f, 96.125f }; TURRET_MAP = true; break;
                case 02: fff = new float[3] { 990.2207f, 1219.364f, -87.875f }; break;
                case 03: fff = new float[3] { -1587.473f, -4330.379f, 3862.001f }; break;
                case 04: fff = new float[3] { 574.9448f, -562.5687f, -348.6788f }; break;
                case 05: fff = new float[3] { 2458.22f, 1552.072f, 148.4162f }; break;
                case 06: fff = new float[3] { 1262, 2668, -272 }; break;
                case 07: fff = new float[3] { 1903, -1220, 380 }; break;
                case 08: fff = new float[3] { 2535, -573, 100 }; break;
                case 09: fff = new float[3] { 839.8029f, -397.5345f, -5.537515f }; break;
                case 10: fff = new float[3] { -1826.247f, 637.1963f, 1049.175f }; break;
                case 11: fff = new float[3] { -3441, -660, 1162 }; break;
                case 12: fff = new float[3] { 662, -952, 112 }; break;
                case 13: fff = new float[3] { -1515.302f, 1060.371f, 50.338187f }; break;
                case 14: fff = new float[3] { -45.68791f, -926.9956f, 0.1250008f }; break;
                case 15: fff = new float[3] { -253.9192f, -1614.135f, 352.125f }; break;
                case 16: fff = new float[3] { -389.8866f, -1498.549f, 686.7453f }; break;
                case 17: fff = new float[3] { 970.2472f, -2889.921f, 124.2467f }; TURRET_MAP = true; break;
                case 18: fff = new float[3] { 806.4158f, 2471.344f, 12752.63f }; break;
                case 19: fff = new float[3] { -175.7144f, -0.04654822f, 907.125f }; break;
                case 20: fff = new float[3] { 1434.789f, -348.4124f, 337.4833f }; break;
                case 21: fff = new float[3] { 2362, 1607, 220 }; break;
                case 22: fff = new float[3] { 1880, -469, 1574 }; break;
                case 23: fff = new float[3] { 53.63142f, 639.6542f, 2171.125f }; break;
                case 24: fff = new float[3] { 131.7994f, 1357.19f, 1800.125f }; TURRET_MAP = true; break;
                case 25: fff = new float[3] { 404.5297f, 725.2506f, 452.8782f }; break;
                case 26: fff = new float[3] { 1012.743f, 398.3938f, 168.125f }; break;
                case 27: fff = new float[3] { 1362.443f, 3537.729f, 112.125f }; break;
                case 28: fff = new float[3] { -1523.346f, -276.3622f, 166.625f }; break;
                case 29: fff = new float[3] { -4.174267f, -142.9193f, 34.59882f }; break;
                case 30: fff = new float[3] { 724.9612f, -1579.811f, 186.125f }; break;
                case 31: fff = new float[3] { -1808.454f, 619.3239f, 240.2601f }; break;
                case 32: fff = new float[3] { -1542.883f, -626.9022f, 117.1221f };  break;
                case 33: fff = new float[3] { -1528.624f, 298.8828f, 1392.125f }; break;
                case 34: fff = new float[3] { -838.4569f, -1980.241f, 198.395f }; break;
                case 35: fff = new float[3] { -448.7873f, -275.8554f, 820.5182f }; break;
            }

            Helicopter.HELI_WAY_POINT = new Vector3(fff[0], fff[1], fff[2] + 150);
           
            if (new byte[] { 23, 24, 25, 26, 28, 29, 30 }.Contains(MAP_IDX))//small map
            {
                Infected.FIRE_DIST = 600;
                PLAYER_LIFE += 1;
            }
            else if (new byte[] { 8, 16, 17, 31 }.Contains(MAP_IDX))//large map
            {
                Infected.FIRE_DIST = 850;
            }
            else//medium
            {
                Infected.FIRE_DIST = 750;
            }

            if (MAP_IDX >= max || MAP_IDX < 0)
            {
                map = map_list[0];
            }
            else
            {
                MAP_IDX++;
                map = map_list[MAP_IDX];
                MAP_IDX --;
            }
            

            if (TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");
            }
            if (!MAP_ROTATE_) return;
            
            if (Directory.Exists("INF_dspl")) Utilities.ExecuteCommand("sv_maprotation INF_dspl\\" + map );
            else
            {
                string content = map + ",INF,1";
                File.WriteAllText("admin\\INF.dspl", content);
            }
        }
        internal void SetADMIN(Entity player)
        {
            player.Call(33445, "SPECT", "centerview");
            bool spect = false;
            player.OnNotify("SPECT", a =>
            {
                if (!spect)
                {
                    player.Call(33349, "freelook", true);
                    player.SetField("sessionstate", "spectator");
                }
                else
                {
                    player.Call(33349, "freelook", false);
                    player.SetField("sessionstate", "playing");
                }
                spect = !spect;
            });

            player.SpawnedPlayer += delegate
            {
                if (USE_ADMIN_SAFE_) player.Health = 9999;
                if (!ATTACK_)
                {
                    Function.SetEntRef(-1); Function.Call(42, "testClients_doCrouch", 1);
                    Function.SetEntRef(-1); Function.Call(42, "testClients_doMove", 0);
                    Function.SetEntRef(-1); Function.Call(42, "testClients_doAttack", 0);
                }
            };
        }

        internal bool StringToBool(string s)
        {
            if (s == "1") return true;
            else return false;
        }

        internal readonly string[] SOUND_ALERTS =
        {
            "AF_1mc_losing_fight", "AF_1mc_lead_lost", "PC_1mc_losing_fight", "PC_1mc_take_positions", "PC_1mc_positions_lock"
        };

        internal readonly string[] BOTs_CLASS = {
            "axis_recipe1",//jugg 0
            "axis_recipe2",//rpg 1
            "axis_recipe3",//riot 2
            "axis_recipe3",//heli 3
            "axis_recipe3",//riot 4

            "class0",//AR g36c 5
            "class1",//SMG ump45 6
            "class2",//LMG mk46 7
            "class4",//SG striker 8
            "class5",//AR m4 9
            "class6",//SMG mp5 10
            "class3",//sniper 11 Jugg Allies
        };


    }

    class Inf
    {
        protected TReturn Call<TReturn>(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }
        protected TReturn Call<TReturn>(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }

        protected void Call(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        protected void Call(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        protected void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
    }

    class Common
    {
        internal static Vector3 ZERO = new Vector3();
        //internal static Vector3 
        //internal static Vector3 AC130_WAY_POS;

        internal static void StartOrEndThermal(Entity player, bool start)
        {
            player.Call(33436, "", 0);//VisionSetNakedForPlayer
            bool Axis = Infected.H_FIELD[player.EntRef].AXIS;

            if (start)
            {
                if (!Axis)
                {
                    player.Call(32936);//thermalvisionfofoverlayon
                    player.Health = 300;
                }

                return;
            }
            if (!Axis) player.Call(32937);//thermalvisionfofoverlayoff

            player.Health = 100;
            player.Call(33531, ZERO);


        }
        static Vector3 tempV = new Vector3();
        internal static Vector3 GetVector(float x, float y, float z)
        {
            tempV.X = x;
            tempV.Y = y;
            tempV.Z = z;
            return tempV;
        }

    }

}
