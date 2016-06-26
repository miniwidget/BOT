using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Infected
{

    public partial class Infected : BaseScript
    {
        #region class
        Set my;
        Weapon WP;
        Perk PK;
        Info info;
        Hud HUD;
        Helicopter HCT;
        Tank TK;
        #endregion

        public Infected()
        {
            my = new Set();
            rnd = new Random();
            WP = new Weapon();
            PK = new Perk();
            HUD = new Hud();
            info = new Info();
            HCT = new Helicopter();
            TK = new Tank();

            Call(42, "scr_game_playerwaittime", 1);
            Call(42, "scr_game_matchstarttime", 1);
            Call(42, "scr_game_allowkillcam", "0");
            Call(42, "scr_infect_timelimit", 8);

            Call(42, "testClients_watchKillcam", 0);
            Call(42, "testClients_doReload", 0);
            Call(42, "testClients_doCrouch", 1);
            Call(42, "testClients_doMove", 0);
            Call(42, "testClients_doAttack", 0);

            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(new B_SET());
                H_FIELD.Add(new H_SET());
            }

            PlayerConnecting += player =>
            {
                string name = player.Name;
                if (name.StartsWith("bot"))
                {
                    string state = player.GetField<string>("sessionteam");
                    if (state == "spectator")
                    {
                        Call(286, player.EntRef);//kick
                        Entity b = Utilities.AddTestClient();
                    }
                }
            };

            PlayerConnected += player =>
            {
                if (player.Name.StartsWith("bot"))
                {
                    Bot_Connected(player);
                }
                else
                {
                    Human_Connected(player);
                }
            };

            OnNotify("prematch_done", () =>
            {
                if(DEPLAY_BOT_) BotDeplay();

                PlayerDisconnected += player =>
                {
                    if (human_List.Contains(player))// 봇 타겟리스트에서 접속 끊은 사람 제거
                    {
                        human_List.Remove(player);
                    }
                };

                OnNotify("game_ended", (level) =>
                {
                    Call(42, "testClients_doMove", 0);
                    Call(42, "testClients_doAttack", 0);

                    GAME_ENDED_ = true;
                    HUD.SERVER.Call(32897);

                    foreach (var v in B_FIELD)
                    {
                        v.fire = false;
                        v.target = null;
                        v.death += 1;
                    }
                    AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));
                });
            });


        }

        void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }

        #region field

        internal static Random rnd;
        internal static Entity ADMIN;
        internal static Vector3 ZERO = new Vector3();

        readonly int BOT_SETTING_NUM = 12;

        internal static int FIRE_DIST;
        internal static bool TEST_, DEPLAY_BOT_, USE_ADMIN_SAFE_;
        internal static string ADMIN_NAME;

        bool GAME_ENDED_;
        bool IsSurvivor(Entity player) { return player.GetField<string>("sessionteam") == "allies"; }
        bool[] IsBOT = new bool[18];
        bool[] IsAXIS = new bool[18];
        int[] IsPERK = new int[18];
        readonly string[] SOUND_ALERTS =
        {
            "AF_1mc_losing_fight", "AF_1mc_lead_lost", "PC_1mc_losing_fight", "PC_1mc_take_positions", "PC_1mc_positions_lock" , "PC_1mc_enemy_take_a" , "PC_1mc_enemy_take_b", "PC_1mc_enemy_take_c"
        };


        /// <summary>
        /// BOT SET class for custom fields set
        /// </summary>
        class B_SET
        {
            internal Entity target { get; set; }
            internal int death { get; set; }
            internal bool fire { get; set; }
            internal bool temp_fire { get; set; }
            internal string wep { get; set; }
            internal int killer = -1;
        }
        List<B_SET> B_FIELD = new List<B_SET>(18);
        List<Entity> BOTs_List = new List<Entity>();

        /// <summary>
        /// HUMAN PLAYER SET class for custom fields set
        /// </summary>
        internal class H_SET
        {
            //int att = 0;
            //internal int SIRENCERorHB
            //{
            //    get
            //    {
            //        this.att++;
            //        if (this.att > 2) this.att = 0;
            //        return this.att;
            //    }
            //}
            internal int LIFE { get; set; }
            internal bool RESPAWN { get; set; }

            internal int AX_WEP { get; set; }
            internal bool BY_SUICIDE { get; set; }

            internal bool USE_TANK { get; set; }
            internal int USE_HELI { get; set; }

            internal bool TI_NOTIFIED;
            internal bool TI_DO;

            internal bool LOC_NOTIFIED;
            internal bool LOC_DO;
            internal float[] LOC = null;

            internal void reset (bool Axis)
            {
                BY_SUICIDE = false;
                USE_TANK = false;
                TI_NOTIFIED = false;
                TI_DO = false;
                LOC_NOTIFIED = false;
                LOC_DO = false;
                
                if (!Axis)
                {
                    LIFE = 2;
                    USE_HELI = 0;
                    AX_WEP = 0;
                }else
                {
                    LIFE = -2;
                    USE_HELI = 4;
                    AX_WEP = 1;
                }
            }
        }
        internal static List<H_SET> H_FIELD = new List<H_SET>(18);
        internal static List<Entity> human_List = new List<Entity>();
        #endregion

    }
}

