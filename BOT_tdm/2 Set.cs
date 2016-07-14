using System;
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

        internal static bool TURRET_MAP, USE_PREDATOR;

        bool USE_ADMIN_SAFE_, DEPLAY_BOT_, GAME_ENDED_, HUMAN_CONNECTED_;

        string SERVER_NAME, ADMIN_NAME, NEXT_MAP, WELLCOME_MESSAGE;

        float PLAYERWAIT_TIME, MATCHSTART_TIME;

        int SEARCH_TIME, FIRE_TIME, BOT_DELAY_TIME, BOT_SETTING_NUM, FIRE_DIST;

        bool[] IsBOT = new bool[18];

        void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }

        Random rnd = new Random();
        Entity ADMIN;
        #endregion

        #region server side
        /// <summary>
        /// server side dvar
        /// </summary>
        void Server_SetDvar()
        {

            Call("setdvar", "scr_game_playerwaittime", PLAYERWAIT_TIME);
            Call("setdvar", "scr_game_matchstarttime", MATCHSTART_TIME);

            Call("setdvar", "testClients_watchKillcam", 0);
            Call("setdvar", "testClients_doReload", 0);

            BotDoAttack(false);

            if (SERVER_NAME == "^2BOT ^7TDM SERVER") SERVER_NAME += " " + rnd.Next(1000);
            Utilities.ExecuteCommand("sv_hostname " + SERVER_NAME);
            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(new B_SET());
                H_FIELD.Add(new H_SET());
            }

        }

        byte MAP_IDX;
        bool MAP_ROTATE_;

        void readMAP()
        {
            string map = Call<string>(47, "mapname");//"getdvar"
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
                case 32: fff = new float[3] { -1542.8f, -626.9f, 117.1f }; break;
                case 33: fff = new float[3] { -918, -261, 966 }; break;
                case 34: fff = new float[3] { -838.4f, -1980.2f, 198.3f }; break;
                case 35: fff = new float[3] { 1209, -563, 707 }; break;
            }

            Helicopter.HELI_WAY_POINT = new Vector3(fff[0], fff[1], fff[2] + 150);

            if (new byte[] { 23, 24, 25, 26, 28, 29, 30 }.Contains(MAP_IDX))//small map
            {
                FIRE_DIST = 600;
            }
            else if (new byte[] { 8, 16, 17, 31 }.Contains(MAP_IDX))//large map
            {
                FIRE_DIST = 850;
            }
            else//medium
            {
                FIRE_DIST = 750;
            }

            if (MAP_IDX >= max || MAP_IDX < 0)
            {
                map = map_list[0];
            }
            else
            {
                MAP_IDX++;
                map = map_list[MAP_IDX];
                MAP_IDX--;
            }

            if (TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
                Utilities.ExecuteCommand("sv_hostname TEST");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");

            }
            if (!MAP_ROTATE_) return;

            if (Directory.Exists("TDM_dspl")) Utilities.ExecuteCommand("sv_maprotation TDM_dspl\\" + map);
            else
            {
                string content = NEXT_MAP + ",TDM,1";
                File.WriteAllText("admin\\TDM.dspl", content);
            }


        }

        #endregion

        #region Bots side

        /// <summary>
        /// BOT SET class for custom fields set
        /// </summary>
        class B_SET
        {
            internal Entity target { get; set; }
            internal string weapon;
            internal int killer = -1;
            internal int ammoClip;
            internal string alert;
            internal bool wait;

            internal bool Axis;
        }
        List<B_SET> B_FIELD = new List<B_SET>(18);
        List<Entity> BOTs_List = new List<Entity>();
        #endregion

        #region Human side
        /// <summary>
        /// HUMAN PLAYER SET class for custom fields set
        /// </summary>
        internal class H_SET
        {
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
            /// <summary>
            /// perk count
            /// </summary>
            internal int PERK = 2;
            internal string PERK_TXT = "PRDT **";
            internal HudElem HUD_PERK_COUNT, HUD_TOP_INFO, HUD_RIGHT_INFO, HUD_SERVER, HUD_BULLET_INFO, HUD_KEY_INFO;

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
        internal static List<H_SET> H_FIELD = new List<H_SET>(18);
        //Dictionary<int, int> H_ID = new Dictionary<int, int>();
        internal static List<Entity> human_List = new List<Entity>();

        List<Entity> H_AXIS_LIST = new List<Entity>();
        List<Entity> H_ALLIES_LIST = new List<Entity>();
        #endregion

    }
}
