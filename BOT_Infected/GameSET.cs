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
        #region field
        string
            SERVER_NAME, ADMIN_NAME,
            TEAMNAME_ALLIES, TEAMNAME_AXIS,
            NEXT_MAP,
            WELLCOME_MESSAGE;

        bool
            USE_ADMIN_SAFE_, Disable_Melee_, HELI_MAP,
            DEPLAY_BOT_, SUICIDE_BOT_, OVERFLOW_BOT_,

           GAME_ENDED_, HUMAN_CONNECTED_;


        float
            INFECTED_TIMELIMIT, PLAYERWAIT_TIME, MATCHSTART_TIME;

        int
            t0 = 100, t1 = 1000, t2 = 2000, t3 = 3000,
            SEARCH_TIME, FIRE_TIME, BOT_DELAY_TIME, BOT_SETTING_NUM, FIRE_DIST,
            PLAYER_LIFE;

        bool isSurvivor(Entity player) { return player.GetField<string>("sessionteam") == "allies"; }

        Random rnd = new Random();
        Entity ADMIN;
        string[] BOTs_CLASS = { "axis_recipe1", "axis_recipe2", "axis_recipe3", "class0", "class1", "class2", "class4", "class5", "class6", "class6" };
        #endregion
        enum MAP
        {
            mp_plaza2,
            mp_mogadishu,
            mp_bootleg,
            mp_carbon,
            mp_dome,
            mp_exchange,
            mp_lambeth,
            mp_hardhat,
            mp_interchange,
            mp_alpha,
            mp_bravo,
            mp_radar,
            mp_paris,
            mp_seatown,
            mp_underground,
            mp_village,
            mp_morningwood,
            mp_park,
            mp_overwatch,
            mp_italy,
            mp_cement,
            mp_qadeem,
            mp_meteora,
            mp_hillside_ss,
            mp_restrepo_ss,
            mp_aground_ss,
            mp_courtyard_ss,
            mp_terminal_cls,
            mp_burn_ss,
            mp_nola,
            mp_six_ss,
            mp_moab
        }


        void readMAP()
        {

            string currentMAP = Call<string>("getdvar", "mapname");
            string ENTIRE_MAPLIST = "mp_plaza2|mp_mogadishu|mp_bootleg|mp_carbon|mp_dome|mp_exchange|mp_lambeth|mp_hardhat|mp_interchange|mp_alpha|mp_bravo|mp_radar|mp_paris|mp_seatown|mp_underground|mp_village|mp_morningwood|mp_park|mp_overwatch|mp_italy|mp_cement|mp_qadeem|mp_meteora|mp_hillside_ss|mp_restrepo_ss|mp_aground_ss|mp_courtyard_ss|mp_terminal_cls|mp_burn_ss|mp_nola|mp_six_ss|mp_moab";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            int index = map_list.IndexOf(currentMAP);

            List<MAP> theList = Enum.GetValues(typeof(MAP)).Cast<MAP>().ToList();
            MAP m = theList[index];
            theList.Clear();

            Enum[] largemap = { MAP.mp_exchange, MAP.mp_interchange, MAP.mp_morningwood, MAP.mp_park, MAP.mp_moab };
            Enum[] smallmap = { MAP.mp_hillside_ss, MAP.mp_restrepo_ss, MAP.mp_aground_ss, MAP.mp_courtyard_ss, MAP.mp_burn_ss, MAP.mp_nola };

            /*set player life */
            if (PLAYER_LIFE == 0) PLAYER_LIFE = 2;

            /* set bot's fire distance */
            if (largemap.Contains(m))
            {
                FIRE_DIST = 850;
            }
            else if (smallmap.Contains(m))
            {
                PLAYER_LIFE += 1;
                FIRE_DIST = 600;
            }
            else
            {
                FIRE_DIST = 750;
            }

            if (PLAYER_LIFE == 0) PLAYER_LIFE = 2;

            Enum[] helimap = { MAP.mp_interchange, MAP.mp_hardhat, MAP.mp_lambeth, MAP.mp_morningwood, MAP.mp_paris, MAP.mp_radar, MAP.mp_restrepo_ss, MAP.mp_qadeem, MAP.mp_moab, MAP.mp_meteora };
            if (helimap.Contains(m))
            {
                HELI_MAP = true;
                switch (m)
                {
                    case MAP.mp_interchange: HELI_WAY_POINT = new Vector3(2535, -573, 100); break;
                    case MAP.mp_hardhat: HELI_WAY_POINT = new Vector3(1903, -1220, 380); break;
                    case MAP.mp_lambeth: HELI_WAY_POINT = new Vector3(1262, 2668, -272); break;
                    case MAP.mp_morningwood: HELI_WAY_POINT = new Vector3(-1601, -1848, 620); break;
                    case MAP.mp_paris: HELI_WAY_POINT = new Vector3(662, -952, 112); break;
                    case MAP.mp_radar: HELI_WAY_POINT = new Vector3(-3441, -660, 1162); break;
                    case MAP.mp_restrepo_ss: HELI_WAY_POINT = new Vector3(165, 1364, 1800); break;
                    case MAP.mp_qadeem: HELI_WAY_POINT = new Vector3(2362, 1607, 220); break;
                    case MAP.mp_moab: HELI_WAY_POINT = new Vector3(-1472, 1101, 323); break;
                    case MAP.mp_meteora: HELI_WAY_POINT = new Vector3(1880, -469, 1574); break;
                }

                Call("precachemodel", "prop_flag_neutral");
                Call("precacheVehicle", "attack_littlebird_mp");
                SetHelipoint(true);
            }
            HELI_WAY_POINT.Z += 120;
            //set next map
            if (index >= max || index < 0) index = 0; else index++;
            NEXT_MAP = map_list[index];

            Call("setdvar", "sv_nextmap", NEXT_MAP);

            //TEST_ = true;

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
