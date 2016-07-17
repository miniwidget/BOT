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
        Entity ADMIN;
        List<B_SET> B_FIELD = new List<B_SET>();

        internal static List<Entity> BOTs_List = new List<Entity>();
        internal static List<Entity> Axis_List = new List<Entity>();
        internal static List<Entity> Allies_List = new List<Entity>();
        internal static List<Entity> human_List = new List<Entity>();
        internal static List<H_SET> H_FIELD = new List<H_SET>();

        internal static Random rnd;
        internal static string ADMIN_NAME, VEHICLE_CODE;
        internal static int BOT_HELI_HEIGHT = 1500, FIRE_DIST;
        internal static bool GAME_ENDED_, USE_PREDATOR, TEST_, USE_ADMIN_SAFE_;
        bool [] IsBOT = new bool [18];

        bool
            GET_TEAMSTATE_FINISHED,
            BOT_ADD_WATCH_FINISHED,

            HUMAN_ZERO_ = true;

        internal static void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }

        internal static Vector3 VectorAddZ(Vector3 origin, float add)
        {
            origin.Z += add;
            return origin;
        }

        class B_SET
        {

            Entity bot;
            internal bool AXIS;
            public B_SET(Entity bot_)
            {
                bot = bot_;
            }
            /// <summary>
            /// 0 None /
            /// 1 Rider start /
            /// 2 Rider end /
            /// </summary>
            internal byte RiderState;
            internal void Search()
            {
                if (wait) return;
                if (AXIS) BotSearchAllies();
                else BotSearchAxis();
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
                            SetBullet();
                            return;
                        }
                    }
                }

                foreach (Entity human in Allies_List)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        target = human;
                        if (alertSound != null) if (human.Name != null) human.Call(33466, alertSound);//"playlocalsound" //deny remote tank !important if not deny, server cause crash
                        return;
                    }
                }

                if (_target != null) target = null;
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
                            SetBullet();
                            return;
                        }
                    }
                }

                foreach (Entity human in Axis_List)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        target = human;
                        return;
                    }
                }

                if (_target != null) target = null;
            }

            internal void SetBullet()
            {
                bot.Call(33468, weapon, ammoClip);
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

            bool _wait;
            internal bool wait
            {
                get
                {
                    return _wait;
                }
                set
                {
                    if (value == true) target = null;

                    _wait = value;
                }
            }
            Entity _target;
            internal Entity target
            {
                get
                {
                    return _target;
                }
                set
                {
                    if (value == null) bot.Call(33468, weapon, 0);
                    else bot.Call(33468, weapon, ammoClip);//setweaponammoclip

                    _target = value;

                }
            }
            internal string weapon, alertSound;
            internal int ammoClip, killer = -1;
        }

    }

    class H_SET
    {
        /// <summary>
        /// perk count
        /// </summary>
        internal int PERK = 2;
        internal string PERK_TXT = "PRDT **";
        internal HudElem HUD_PERK_COUNT, HUD_TOP_INFO, HUD_RIGHT_INFO, HUD_SERVER, HUD_BULLET_INFO, HUD_KEY_INFO;

        /// <summary>
        /// 0 not using remote /
        /// 1 remote helicopter /
        /// 2 remote turret tank /
        /// 3 killstreak remote tank /
        /// 4 vehicle turret /
        /// 5 remote mortar /
        /// 6 ride predator /
        /// </summary>
        internal byte REMOTE_STATE;

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
        /// 0 NONE /
        /// 1 PREDATOR /
        /// 2 HELICOPTER /
        /// </summary>
        internal byte MARKER_TYPE;

        /// <summary>
        /// reapeated notify block
        /// </summary>
        internal bool PREDATOR_FIRE_NOTIFIED;
        internal bool VEHICLE_FIRE_NOTIFIED;
        internal bool MARKER_NOTIFIED;

        /// <summary>
        /// if player's life consumed all, change to Axis
        /// </summary>
        internal bool AXIS;

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

            string setFile = "admin\\TDM.txt";

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
                            case "ADMIN_NAME": Tdm.ADMIN_NAME = value; break;
                            case "SERVER_NAME": if (value == "^2BOT ^7TDM SERVER") value += DateTime.Now.Second; Hud.SERVER_NAME_ = value; break;

                            case "BOT_SETTING_NUM": SetValue(value, ref BOT_SETTING_NUM); break;

                            case "DEPLAY_BOT_": SetValue(value, ref DEPLAY_BOT_); break;
                            case "USE_ADMIN_SAFE_": SetValue(value, ref Tdm.USE_ADMIN_SAFE_); break;
                            case "MAP_ROTATE_": SetValue(value, ref MAP_ROTATE_); break;
                            case "TEST_": SetValue(value, ref Tdm.TEST_); break;
                        }
                    }
                }
            }


            #endregion

            if (Tdm.TEST_) Utilities.ExecuteCommand("sv_hostname TDM TEST");
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
                case 5: Tdm.BOT_HELI_HEIGHT += 1000;break;
            }

            if (new byte[] { 23, 24, 25, 26, 28, 29, 30 }.Contains(MAP_IDX))//small map
            {
                Tdm.FIRE_DIST = 600;
                PLAYER_LIFE += 1;
            }
            else if (new byte[] { 8, 16, 17, 31 }.Contains(MAP_IDX))//large map
            {
                Tdm.FIRE_DIST = 850;
            }
            else//medium
            {
                Tdm.FIRE_DIST = 750;
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


            if (Tdm.TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");
            }
            if (!MAP_ROTATE_) return;

            if (Directory.Exists("TDM_dspl")) Utilities.ExecuteCommand("sv_maprotation TDM_dspl\\" + map);
            else
            {
                File.WriteAllText("admin\\TDM.dspl", map + ",TDM,1");//mapname, dsr, 가중치
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

        //internal bool StringToBool(string s)
        //{
        //    if (s == "1") return true;
        //    else return false;
        //}

        internal readonly string[] SOUND_ALERTS =
        {
            "AF_1mc_losing_fight", "AF_1mc_lead_lost", "PC_1mc_losing_fight", "PC_1mc_take_positions", "PC_1mc_positions_lock"
        };

        internal readonly string[] BOTs_CLASS = {
            "axis_recipe5",//JUGG
            "axis_recipe5",//JUGG

            "allies_recipe5",//RPG
            "allies_recipe5",//RPG

            "class0",//AR g36c
            "class0",//AR g36c

            "class2",//LMG mk46
            "class2",//LMG mk46

            "class4",//SG striker
            "class4",//SG striker

            "class6",//SMG mp5
            "class6",//SMG mp5

            /* */

            "class1",//SMG ump45
            "class1",//SMG ump45

            "class3",//SN sniper
            "class3",//SN sniper

            "class5",//AR m4
            "class5",//AR m4

        };
        /*            "allies_recipe5",//jugg allies
            "axis_recipe5",//jugg axis

            "allies_recipe4",//rpg allies
            "axis_recipe4",//rpg axis

*/

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
    }

    class Common
    {
        internal static Vector3 ZERO = new Vector3();

        internal static void StartOrEndThermal(Entity player, bool start)
        {
            player.Call(33436, "", 0);//VisionSetNakedForPlayer

            if (start)
            {
                player.Call(32936);//thermalvisionfofoverlayon
                player.Health = 300;

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
            H.HUD_KEY_INFO.SetText("^2PRESS [ ^7[{+frag}] ^2]");

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
