using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    internal class Set
    {
        public Set()
        {

            #region Load Custom Setting from a set.txt file

            string setFile = "admin\\INF.txt";

            if (File.Exists(setFile))
            {
                using (StreamReader set = new StreamReader(setFile))
                {
                    bool b;

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
                            case "SERVER_NAME": Hud.SERVER_NAME_= value; break;
                            case "ADMIN_NAME": Infected.ADMIN_NAME = value; break;

                            case "TEST_": if ( bool.TryParse(value, out b)) Infected.TEST_ = b; break;
                            case "DEPLAY_BOT_": if (bool.TryParse(value, out b)) Infected.DEPLAY_BOT_ = b; break;
                            case "USE_ADMIN_SAFE_": if (bool.TryParse(value, out b)) Infected.USE_ADMIN_SAFE_ = b; break;

                        }
                    }

                    //if (TEST_) SERVER_NAME = "^2BOT ^7TEST";
                }
            }


            #endregion

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (assembly.Location.Contains("test"))
            {
                Infected.TEST_ = true;
                print("■ " + assembly.GetName().Name + ".dll & TEST MODE");
            }

            if (Infected.TEST_) Utilities.ExecuteCommand("sv_hostname TEST");
            else Utilities.ExecuteCommand("sv_hostname " + Hud.SERVER_NAME_);

            ReadMAP();
        }

        void ReadMAP()
        {
            Function.SetEntRef(-1);
            string map = Function.Call<string>("getdvar", "mapname");
            string ENTIRE_MAPLIST = "mp_plaza2|mp_mogadishu|mp_bootleg|mp_carbon|mp_dome|mp_exchange|mp_lambeth|mp_hardhat|mp_interchange|mp_alpha|mp_bravo|mp_radar|mp_paris|mp_seatown|mp_underground|mp_village|mp_morningwood|mp_park|mp_overwatch|mp_italy|mp_cement|mp_qadeem|mp_meteora|mp_hillside_ss|mp_restrepo_ss|mp_aground_ss|mp_courtyard_ss|mp_terminal_cls|mp_burn_ss|mp_nola|mp_six_ss|mp_moab";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            byte map_index = (byte)map_list.IndexOf(map);
            float[] fff = null;

            switch (map_index)
            {
                case 00: fff = new float[3] { -338.2646f, 2086.079f, 780.125f }; break;
                case 01: fff = new float[3] { -528.8417f, 3310.055f, 96.125f }; break;
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
                case 16: fff = new float[3] { -1601, -1848, 620 }; break;
                case 17: fff = new float[3] { 3342.936f, -2147.835f, 201.125f }; break;
                case 18: fff = new float[3] { 687.1823f, 2715.979f, 12670.13f }; break;
                case 19: fff = new float[3] { 1073.359f, -1203.881f, 704.125f }; break;
                case 20: fff = new float[3] { 1434.789f, -348.4124f, 337.4833f }; break;
                case 21: fff = new float[3] { 2362, 1607, 220 }; break;
                case 22: fff = new float[3] { 1880, -469, 1574 }; break;
                case 23: fff = new float[3] { 53.63142f, 639.6542f, 2171.125f }; break;
                case 24: fff = new float[3] { 131.7994f, 1357.19f, 1800.125f }; break;
                case 25: fff = new float[3] { 404.5297f, 725.2506f, 452.8782f }; break;
                case 26: fff = new float[3] { 1012.743f, 398.3938f, 168.125f }; break;
                case 27: fff = new float[3] { 2481.313f, 2897.811f, 40.125f }; break;
                case 28: fff = new float[3] { -1523.346f, -276.3622f, 166.625f }; break;
                case 29: fff = new float[3] { -4.174267f, -142.9193f, 34.59882f }; break;
                case 30: fff = new float[3] { 724.9612f, -1579.811f, 186.125f }; break;
                case 31: fff = new float[3] { -1456.08f, 1086.234f, 323.125f }; break;
            }

            Helicopter.HELI_WAY_POINT = new Vector3(fff[0], fff[1], fff[2] + 150);

            if (new byte[] { 23, 24, 25, 26, 28, 29, 30 }.Contains(map_index))//small map
            {
                Infected.FIRE_DIST = 600;
            }
            else if (new byte[] { 8, 16, 17, 31 }.Contains(map_index))//large map
            {
                Infected.FIRE_DIST = 850;
            }
            else//medium
            {
                Infected.FIRE_DIST = 750;
            }

            //Call("precachemodel", "prop_flag_neutral");
            //Call("precacheVehicle", "attack_littlebird_mp");


            print("map: " + map_index + "/" + max);

            if (map_index >= max || map_index < 0) map_index = 0; else map_index++;
            map = map_list[map_index];

            if (Infected.TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");
                string content = map + ",INF,1";
                File.WriteAllText("admin\\INF.dspl", content);
            }
        }

        internal void print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
    }
}
