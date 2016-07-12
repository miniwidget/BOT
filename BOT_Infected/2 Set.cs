using System;
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
        internal string PERK_TXT = "PRDT **";
        internal HudElem HUD_PERK_COUNT, HUD_TOP_INFO, HUD_RIGHT_INFO, HUD_SERVER,HUD_BULLET_INFO,HUD_KEY_INFO;

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


        ///// <summary>
        ///// test ac130
        ///// </summary>
        //internal bool AC130_NOTIFIED;
        //internal bool AC130_ON_USE;

        /// <summary>
        /// if sessionteam Axis
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
                case 00: fff = new float[3] { -338.2f, 2086.0f, 780.1f }; break;
                case 01: fff = new float[3] { -528.8f, 3310.0f, 96.1f }; TURRET_MAP = true; break;
                case 02: fff = new float[3] { 990.2f, 1219.3f, -87.8f }; break;
                case 03: fff = new float[3] { -1587.4f, -4330.3f, 3862.0f }; break;
                case 04: fff = new float[3] { 574.9f, -562.5f, -348.6f }; break;
                case 05: fff = new float[3] { 2458.2f, 1552.0f, 148.4f }; break;
                case 06: fff = new float[3] { 1262, 2668, -272 }; break;
                case 07: fff = new float[3] { 1903, -1220, 380 }; break;
                case 08: fff = new float[3] { 2535, -573, 100 }; break;
                case 09: fff = new float[3] { 839.8f, -397.5f, -5.5f }; break;
                case 10: fff = new float[3] { -1826.2f, 637.1f, 1049.1f }; break;
                case 11: fff = new float[3] { -3441, -660, 1162 }; break;
                case 12: fff = new float[3] { 662, -952, 112 }; break;
                case 13: fff = new float[3] { -1515.3f, 1060.3f, 50.3f }; break;
                case 14: fff = new float[3] { -45.6f, -926.9f, 0.1f }; break;
                case 15: fff = new float[3] { -253.9f, -1614.1f, 352.1f }; break;
                case 16: fff = new float[3] { -389.8f, -1498.5f, 686.7f }; break;
                case 17: fff = new float[3] { 970.2f, -2889.9f, 124.2f }; TURRET_MAP = true; break;
                case 18: fff = new float[3] { 806.4f, 2471.3f, 12752.6f }; break;
                case 19: fff = new float[3] { -175.7f, -0.0f, 907.1f }; break;
                case 20: fff = new float[3] { 1434.7f, -348.4f, 337.4f }; break;
                case 21: fff = new float[3] { 2362, 1607, 220 }; break;
                case 22: fff = new float[3] { 1880, -469, 1574 }; break;
                case 23: fff = new float[3] { 53.6f, 639.6f, 2171.1f }; break;
                case 24: fff = new float[3] { 131.7f, 1357.1f, 1800.1f }; TURRET_MAP = true; break;
                case 25: fff = new float[3] { 404.5f, 725.2f, 452.8f }; break;
                case 26: fff = new float[3] { 1012.7f, 398.3f, 168.1f }; break;
                case 27: fff = new float[3] { 1362.4f, 3537.7f, 112.1f }; break;
                case 28: fff = new float[3] { -1523.3f, -276.3f, 166.6f }; break;
                case 29: fff = new float[3] { -4.1f, -142.9f, 34.5f }; break;
                case 30: fff = new float[3] { 724.9f, -1579.8f, 186.1f }; break;
                case 31: fff = new float[3] { -1808.4f, 619.3f, 240.2f }; break;
                case 32: fff = new float[3] { -1542.8f, -626.9f, 117.1f };  break;
                case 33: fff = new float[3] { -1528.6f, 298.8f, 1392.1f }; break;
                case 34: fff = new float[3] { -838.4f, -1980.2f, 198.3f }; break;
                case 35: fff = new float[3] { -448.7f, -275.8f, 820.5f }; break;
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
