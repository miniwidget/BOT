﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    enum State
    {
        remote_not_using, remote_helicopter, remote_turretTank, remote_assaultDrone, remote_mortar, remote_predator, remote_vehicleTurret,
        marker_none, marker_predator, marker_helicopter
    }

    public partial class Infected
    {
        Entity ADMIN;
        List<B_SET> B_FIELD = new List<B_SET>();
        Dictionary<string, int> PLAYER_STATE = new Dictionary<string, int>();

        DateTime GRACE_TIME;

        internal static List<Entity> BOTs_List = new List<Entity>();
        internal static List<Entity> Axis_List = new List<Entity>();
        internal static List<Entity> Allies_List = new List<Entity>();
        internal static List<Entity> human_List = new List<Entity>();
        internal static List<H_SET> H_FIELD = new List<H_SET>();

        internal static Random rnd;
        internal static string ADMIN_NAME, VEHICLE_CODE;
        internal static int BOT_HELI_HEIGHT = 1500, FIRE_DIST;
        internal static bool GAME_ENDED_, USE_PREDATOR, TEST_, USE_ADMIN_SAFE_;

        bool[] IsBOT = new bool[18];
        internal static bool[] IsAxis = new bool[18];

        bool
            BOT_TO_AXIS_COMP,
            GET_TEAMSTATE_FINISHED,
            BOT_ADD_WATCHED,

            BOT_SERCH_ON_LUCKY_FINISHED,


            HUMAN_DIED_ALL_ = true,
            UNLIMITED_LIEF_ = true;

        int BOT_HELIRIDER_IDX, BOT_LUCKY_IDX, F_INF_IDX = -1;

        internal static void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }

        internal static Vector3 VectorAddZ(Vector3 origin, float add)
        {
            origin.Z += add;
            return origin;
        }
        readonly string[] vehicles = { "mortar_remote_mp", "killstreak_helicopter_mp", "heli_remote_mp" };
        static string[] DIALOG_AXIS;
        static string[] DIALOG_ALLIES =
         {
            "_1mc_achieve_sleightofhand",//0
            "_1mc_achieve_quickdraw",//1
            "_1mc_achieve_extremeconditioning",//2
            "_1mc_achieve_stalker",//3
            "_1mc_achieve_scavenger",//4
            "_1mc_achieve_steadyaim",//5
            "_1mc_achieve_deadsilence",//6
            "_1mc_achieve_blindeye",//7
            "_1mc_achieve_assassin",//8
            "_1mc_achieve_blastshield",//9
            "_1mc_achieve_hardline",//10
            
            "_1mc_acheive_bomb",//11
            "_1mc_enemy_ah6guard",//12
            "_1mc_use_ah6guard",//13
            "_1mc_KS_lbd_inposition",//14
            "_1mc_achieve_hellfire",//15
            "_1mc_achieve_carepackage",//16
       };


        /// <summary>
        /// 11 acheive_bomb
        /// 12 enemy_ah6guard
        /// 13 use_ah6guard
        /// 14 KS_lbd_inposition
        /// 15 achieve_hellfire
        /// 16 achieve_carepackage
        /// </summary>
        internal static void PlayDialog(Entity player, bool axis, int idx)
        {
            if (axis) player.Call(33466, DIALOG_AXIS[idx]);//playlocalsound
            else player.Call(33466, DIALOG_ALLIES[idx]);
        }

        /// <summary>
        /// 11 acheive_bomb
        /// 12 enemy_ah6guard
        /// 13 use_ah6guard
        /// 14 KS_lbd_inposition
        /// 15 achieve_hellfire
        /// 16 achieve_carepackage
        /// </summary>
        internal static void PlayDialogToTeam(Entity player, bool axis, int idx, bool toOtherTeam)
        {
            if (toOtherTeam) axis = !axis;

            if (axis) player.Call(32771, DIALOG_AXIS[idx], "axis");//playsoundtoteam
            else player.Call(32771, DIALOG_ALLIES[idx], "allies");
        }

        class B_SET
        {

            internal string WEAPON, ALERT_SOUND;
            internal int AMMO_CLIP;
            internal int KILLER = -1;
            Entity bot;

            byte _fire = 1;

            public B_SET(Entity bot,string alert_sound)
            {
                this.bot = bot;
                AXIS = true;

                if (alert_sound == "missile_incoming")
                {
                    bot.OnNotify("weapon_fired", (p, w) => SetAngle());
                    return;
                }
               
                bot.OnNotify("weapon_fired", (p, w) =>
                {
                    if (_fire == 1) _fire = 2;
                    else
                    {
                        if (_fire == 3) _fire = 0;
                        _fire++;
                        return;
                    }
                    SetAngle();
                });
            }
            internal void Search()
            {
                if (WAIT) return;
                if (AXIS) BotSearchAllies(); else BotSearchAxis();
            }
            internal void BotSearchAllies()
            {
                Vector3 bo = bot.Origin;

                if (_target != null)//이미 타겟을 찾은 경우
                {
                    if (Allies_List.Contains(_target))
                    {
                        if (_target.Origin.DistanceTo(bo) < FIRE_DIST)
                        {
                            //Print(bot.Name + " " + bot.GetField<string>("sessionteam") + " " + WEAPON + " " + _target.Name+ " "+_target.GetField<string>("sessionteam"));
                            SetBullet();
                            return;
                        }
                    }
                }

                foreach (Entity human in Allies_List)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        TARGET = human;
                        if (ALERT_SOUND != null) if (human.EntRef < 18) human.Call(33466, ALERT_SOUND);//"playlocalsound" //deny remote tank !important if not deny, server cause crash
                        return;
                    }
                }

                if (_target != null) TARGET = null;
            }
            internal void BotSearchAxis()
            {
                Vector3 bo = bot.Origin;

                if (_target != null)//이미 타겟을 찾은 경우
                {
                    if (Axis_List.Contains(_target))
                    {
                        if (_target.Origin.DistanceTo(bo) < FIRE_DIST)
                        {
                            //Print(bot.Name + " " + bot.GetField<string>("sessionteam") + " " + WEAPON + " " + _target.Name + " " + _target.GetField<string>("sessionteam"));
                            SetBullet();
                            return;
                        }
                    }
                }

                foreach (Entity human in Axis_List)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        TARGET = human;
                        if (ALERT_SOUND != null) if (human.EntRef < 18) human.Call(33466, ALERT_SOUND);//"playlocalsound" //deny remote tank !important if not deny, server cause crash
                        return;
                    }
                }

                if (_target != null) TARGET = null;
            }
            void SetBullet()
            {
                bot.Call(33468, WEAPON, AMMO_CLIP);
            }
            internal void SetAngle()
            {
                if (_target == null) return;
                Vector3 BO = bot.Origin;
                Vector3 TO = _target.Origin;

                float dx = TO.X - BO.X;
                float dy = TO.Y - BO.Y;
                float dz = BO.Z - TO.Z + 50;

                var dist = Math.Sqrt(dx * dx + dy * dy);
                BO.X = Convert.ToSingle(Math.Atan2(dz, dist) * 57.3);
                BO.Y = -10 + Convert.ToSingle(Math.Atan2(dy, dx) * 57.3);
                BO.Z = 0;

                bot.Call(33531, BO);//SetPlayerAngles
            }

            internal bool AXIS;
            bool _wait;
            internal bool WAIT
            {
                get
                {
                    return _wait;
                }
                set
                {
                    if (value == true) TARGET = null;

                    _wait = value;
                    _fire = 1;
                }
            }
            Entity _target;
            internal Entity TARGET
            {
                get
                {
                    return _target;
                }
                set
                {
                    if (value == null) bot.Call(33468, WEAPON, 0);
                    else bot.Call(33468, WEAPON, AMMO_CLIP);//setweaponammoclip

                    _target = value;

                }
            }

        }

    }

    class H_SET
    {
        /// <summary>
        /// player life counts before being infected
        /// </summary>
        internal int LIFE;

        /// <summary>
        /// block reapted spawn
        /// </summary>
        internal bool RESPAWN;

        /// <summary>
        /// perk count
        /// </summary>
        internal int PERK = 2,ENTREF;
        internal string PERK_TXT = "PRDT **";
        internal HudElem HUD_PERK_COUNT, HUD_TOP_INFO, HUD_RIGHT_INFO, HUD_SERVER, HUD_BULLET_INFO, HUD_KEY_INFO;

        internal State REMOTE_STATE = State.remote_not_using;
        internal State MARKER_TYPE = State.marker_none;

        /// <summary>
        /// when roop massage, if on_message state, it blocks reapeted roop
        /// </summary>
        internal bool ON_MESSAGE;

        internal bool CAN_USE_PREDATOR;
        internal bool CAN_USE_HELI;

        /// <summary>
        /// block repeated activate notify key
        /// </summary>
        internal bool WAIT;

        /// <summary>
        /// predator or remote mortar missile count
        /// </summary>
        internal byte MISSILE_COUNT;

        /// <summary>
        /// board real killstreak vehicle by type VEHICLE_CODE
        /// </summary>
        internal Entity VEHICLE;

        /// <summary>
        /// reapeated notify block
        /// </summary>
        internal bool PREDATOR_FIRE_NOTIFIED;
        internal bool VEHICLE_FIRE_NOTIFIED;
        internal bool MARKER_NOTIFIED;

        /// <summary>
        /// if player's life consumed all, change to Axis
        /// </summary>
        bool _axis;
        internal bool AXIS {
            get
            {
                return _axis;
            }
            set
            {
                Infected.IsAxis[ENTREF] = _axis = value;
            }
        }
        /// <summary>
        /// Axis gun index
        /// </summary>
        internal int AX_WEP;
        internal string GUN;

    }

    class Set
    {
        internal static bool TURRET_MAP;
        internal bool DEPLAY_BOT_;
        internal int PLAYER_LIFE;
        internal byte BOT_SETTING_NUM, BOT_CLASS_NUM = 3, MAP_IDX;
        bool MAP_ROTATE_;

        void SetValue(string keyStr, ref bool result)
        {
            bool value_ = false;

            if (bool.TryParse(keyStr, out value_)) result = value_;
        }
        void SetValue(string keyStr, ref int result)
        {
            int value_ = 0;

            if (int.TryParse(keyStr, out value_)) result = value_;
        }
        void SetValue(string keyStr, ref byte result)
        {
            byte value_ = 0;

            if (byte.TryParse(keyStr, out value_)) result = value_;
        }

        public Set()
        {

            #region Load Custom Setting from a set.txt file

            string setFile = "admin\\INF.txt";

            if (File.Exists(setFile))
            {
                using (StreamReader set = new StreamReader(setFile))
                {

                    while (!set.EndOfStream)
                    {
                        string line = set.ReadLine();
                        if (line.StartsWith("//") || line.Equals(string.Empty)) continue;

                        string[] split = line.Split('=');
                        if (split.Length < 1) continue;

                        string key = split[0];
                        string value = split[1];
                        var comment = value.IndexOf("//");
                        if (comment != -1) value = value.Substring(0, comment);

                        switch (key)
                        {
                            case "ADMIN_NAME": Infected.ADMIN_NAME = value; break;
                            case "SERVER_NAME": Hud.SERVER_NAME_ = value; break;

                            case "BOT_SETTING_NUM": SetValue(value, ref BOT_SETTING_NUM); break;
                            case "PLAYER_LIFE": SetValue(value, ref PLAYER_LIFE); break;

                            case "DEPLAY_BOT_": SetValue(value, ref DEPLAY_BOT_); break;
                            case "USE_ADMIN_SAFE_": SetValue(value, ref Infected.USE_ADMIN_SAFE_); break;
                            case "MAP_ROTATE_": SetValue(value, ref MAP_ROTATE_); break;
                            case "TEST_": SetValue(value, ref Infected.TEST_); break;
                        }
                    }
                }
            }


            #endregion

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (assembly.Location.Contains("test"))
            {
                Infected.TEST_ = true;
            }

            if (Infected.TEST_) Utilities.ExecuteCommand("sv_hostname TEST INF");
            else Utilities.ExecuteCommand("sv_hostname " + Hud.SERVER_NAME_);

            ReadMAP();
        }

        void ReadMAP()
        {
            Function.SetEntRef(-1);
            string map = Function.Call<string>(47, "mapname");//"getdvar"
            string ENTIRE_MAPLIST = "mp_plaza2|mp_mogadishu|mp_bootleg|mp_carbon|mp_dome|mp_exchange|mp_lambeth|mp_hardhat|mp_interchange|mp_alpha|mp_bravo|mp_radar|mp_paris|mp_seatown|mp_underground|mp_village|mp_morningwood|mp_park|mp_overwatch|mp_italy|mp_cement|mp_qadeem|mp_meteora|mp_hillside_ss|mp_restrepo_ss|mp_aground_ss|mp_courtyard_ss|mp_terminal_cls|mp_burn_ss|mp_nola|mp_six_ss|mp_moab|mp_boardwalk|mp_crosswalk_ss|mp_roughneck|mp_shipbreaker";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            MAP_IDX = (byte)map_list.IndexOf(map);
            switch (MAP_IDX)
            {
                case 01:
                case 17:
                case 24: TURRET_MAP = true; break;
                case 5: Infected.BOT_HELI_HEIGHT += 1000; break;
            }
            //float[] fff = null;
            //switch (MAP_IDX)
            //{
            //    case 00: fff = new float[3] { -338.2f, 2086.0f, 780.1f }; break;
            //    case 01: fff = new float[3] { -528.8f, 3310.0f, 96.1f }; TURRET_MAP = true; break;
            //    case 02: fff = new float[3] { 990.2f, 1219.3f, -87.8f }; break;
            //    case 03: fff = new float[3] { -1587.4f, -4330.3f, 3862.0f }; break;
            //    case 04: fff = new float[3] { 574.9f, -562.5f, -348.6f }; break;
            //    case 05: fff = new float[3] { 2458.2f, 1552.0f, 148.4f }; break;
            //    case 06: fff = new float[3] { 1262, 2668, -272 }; break;
            //    case 07: fff = new float[3] { 1903, -1220, 380 }; break;
            //    case 08: fff = new float[3] { 2535, -573, 100 }; break;
            //    case 09: fff = new float[3] { 839.8f, -397.5f, -5.5f }; break;
            //    case 10: fff = new float[3] { -1826.2f, 637.1f, 1049.1f }; break;
            //    case 11: fff = new float[3] { -3441, -660, 1162 }; break;
            //    case 12: fff = new float[3] { 662, -952, 112 }; break;
            //    case 13: fff = new float[3] { -1515.3f, 1060.3f, 50.3f }; break;
            //    case 14: fff = new float[3] { -45.6f, -926.9f, 0.1f }; break;
            //    case 15: fff = new float[3] { -253.9f, -1614.1f, 352.1f }; break;
            //    case 16: fff = new float[3] { -389.8f, -1498.5f, 686.7f }; break;
            //    case 17: fff = new float[3] { 970.2f, -2889.9f, 124.2f }; TURRET_MAP = true; break;
            //    case 18: fff = new float[3] { 806.4f, 2471.3f, 12752.6f }; break;
            //    case 19: fff = new float[3] { -175.7f, -0.0f, 907.1f }; break;
            //    case 20: fff = new float[3] { 1434.7f, -348.4f, 337.4f }; break;
            //    case 21: fff = new float[3] { 2362, 1607, 220 }; break;
            //    case 22: fff = new float[3] { 1880, -469, 1574 }; break;
            //    case 23: fff = new float[3] { 53.6f, 639.6f, 2171.1f }; break;
            //    case 24: fff = new float[3] { 131.7f, 1357.1f, 1800.1f }; TURRET_MAP = true; break;
            //    case 25: fff = new float[3] { 404.5f, 725.2f, 452.8f }; break;
            //    case 26: fff = new float[3] { 1012.7f, 398.3f, 168.1f }; break;
            //    case 27: fff = new float[3] { 1362.4f, 3537.7f, 112.1f }; break;
            //    case 28: fff = new float[3] { -1523.3f, -276.3f, 166.6f }; break;
            //    case 29: fff = new float[3] { -4.1f, -142.9f, 34.5f }; break;
            //    case 30: fff = new float[3] { 724.9f, -1579.8f, 186.1f }; break;
            //    case 31: fff = new float[3] { -1808.4f, 619.3f, 240.2f }; break;
            //    case 32: fff = new float[3] { -1542.8f, -626.9f, 117.1f }; break;
            //    case 33: fff = new float[3] { -918, -261, 966 }; break;
            //    case 34: fff = new float[3] { -838.4f, -1980.2f, 198.3f }; break;
            //    case 35: fff = new float[3] { 1209, -563, 707 }; break;
            //}
            //Helicopter.HELI_WAY_POINT = new Vector3(fff[0], fff[1], fff[2] + 150);

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
                map = map_list[++MAP_IDX];
                MAP_IDX--;
            }


            if (Infected.TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");
            }
            if (!MAP_ROTATE_) return;

            if (Directory.Exists("INF_dspl")) Utilities.ExecuteCommand("sv_maprotation INF_dspl\\" + map);
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
        }

        internal string[] SOUND_ALERTS =
        {
            "_1mc_losing_fight", "_1mc_lead_lost", "_1mc_losing_fight", "_1mc_take_positions", "_1mc_positions_lock"
        };

        internal readonly string[] BOTs_CLASS = {
            "axis_recipe1",//jugg 0
            "axis_recipe2",//rpg 1
            "axis_recipe3",//riot 2
            "axis_recipe3",//riot 3
            "class3",//heli - sniper - 4

            "class0",//AR g36c 5
            "class1",//SMG ump45 6
            "class2",//LMG mk46 7
            "class4",//SG striker 8
            "class5",//AR m4 9
            "class6",//SMG mp5 10
            "class3",//sniper 11 - Jugg Allies

            "class0",//AR g36c 12
            "class1",//SMG ump45 13
            "class2",//LMG mk46 14
            "class4",//SG striker 15
            "class5",//AR m4 16
            "class6",//SMG mp5 17
       };


    }

    class Infinity
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
    }

    class Common
    {
        internal static Vector3 ZERO = new Vector3();

        internal static void StartOrEndThermal(Entity player, bool start)
        {
            player.Call(33436, "", 0);//VisionSetNakedForPlayer
            bool Axis = Infected.IsAxis[player.EntRef];

            if (start)
            {
                player.Call(32936);//thermalvisionfofoverlayon

                if (!Axis)
                {
                    player.Health = 300;
                }
                if (Axis) player.AfterDelay(3000, p => player.Call(32937));

                return;
            }
            player.Call(32937);//thermalvisionfofoverlayoff

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
        static float[] GMP = { 0, 0 };
        internal static float[] GetMissilePos(Vector3 angle, Vector3 origin)
        {
            var degreeToRadian = 0.01745f;// (float)Math.PI / 180;

            var dist = (float)Math.Abs(origin.Z / Math.Tan(angle.X * degreeToRadian));

            float Hor_Degree = angle.Y;
            float HD = Math.Abs(Hor_Degree);

            if (HD > 90) HD = 180 - HD;

            var rad = HD * degreeToRadian;

            var x = dist * (float)Math.Abs(Math.Cos(rad));
            var y = dist * (float)Math.Abs(Math.Sin(rad));

            // Print("(" + (int)origin.X + ", " + (int)origin.Y + ")[" + (int)angle.X + "," + (int)angle.Y + "]" + (int)x + " " + (int)y);

            if (Hor_Degree < 0)
            {
                if (Hor_Degree > -90)//4사분면
                {
                    y = y * -1;
                }
                else//3사분면
                {
                    x *= -1;
                    y *= -1;
                }
            }
            else
            {
                if (Hor_Degree > 90)
                {
                    x *= -1;
                }
            }
            GMP[0] = x; GMP[1] = y;
            return GMP;
        }
        internal static void BulletHudInfoCreate(Entity player, H_SET H, byte missile_count)
        {
            H.HUD_KEY_INFO = HudElem.CreateFontString(player, "hudbig", 0.6f);
            H.HUD_BULLET_INFO = HudElem.CreateFontString(player, "hudbig", 0.6f);

            H.HUD_KEY_INFO.HorzAlign = "center";
            H.HUD_KEY_INFO.AlignX = "center";
            H.HUD_KEY_INFO.VertAlign = "bottom";
            H.HUD_KEY_INFO.Y = 10;
            H.HUD_KEY_INFO.SetText(Info.GetStr("*PRESS  [ ^7[{+frag}]  *]",H.AXIS));

            H.HUD_BULLET_INFO.HorzAlign = "right";
            H.HUD_BULLET_INFO.AlignX = "center";
            H.HUD_BULLET_INFO.VertAlign = "bottom";
            H.HUD_BULLET_INFO.Y = 10;
            H.HUD_BULLET_INFO.SetText(missile_count.ToString());
        }
        internal static void BulletHudInfoDestroy(H_SET H)
        {
            if (H.HUD_KEY_INFO != null)
            {
                H.HUD_KEY_INFO.Call(32897);//destroy
                H.HUD_KEY_INFO = null;
            }
            if (H.HUD_BULLET_INFO != null)
            {
                H.HUD_BULLET_INFO.Call(32897);//destroy
                H.HUD_BULLET_INFO = null;
            }
        }
    }

}
