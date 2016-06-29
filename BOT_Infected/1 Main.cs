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
        Set SET;
        Weapon WP;
        Perk PK;
        Info INFO;
        Hud HUD;
        Helicopter HCT;
        Tank TK;
        #endregion

        public Infected()
        {
            SET = new Set();
            rnd = new Random();
            WP = new Weapon();
            PK = new Perk();
            HUD = new Hud();
            INFO = new Info();
            HCT = new Helicopter();
            TK = new Tank();

            Call(42, "scr_game_playerwaittime", 1);
            Call(42, "scr_game_matchstarttime", 1);

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

                if (SET.DEPLAY_BOT_) BotDeplay();

                PlayerDisconnected += player =>
                {
                    if (human_List.Contains(player))// 봇 타겟리스트에서 접속 끊은 사람 제거
                    {
                        human_List.Remove(player);
                    }
                    if (human_List.Count == 0)
                    {
                        HUMAN_CONNECTED_ = false;
                        BotDoAttack(false);
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


        #region field

        internal static Random rnd;
        internal static Entity ADMIN;
        internal static Vector3 ZERO = new Vector3();
        internal static int FIRE_DIST, PLAYER_LIFE = 2;
        internal static bool  USE_ADMIN_SAFE_;
        internal static string ADMIN_NAME;

        bool[] IsBOT = new bool[18];

        internal static void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }


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
            internal int PERK = 2;

            internal bool AXIS;
            internal int AX_WEP { get; set; }
            internal bool BY_SUICIDE { get; set; }

            internal int USE_HELI { get; set; }

            internal bool LOC_NOTIFIED;
            internal bool LOC_DO;
            internal float[] LOC = null;
            internal Vector3 RELOC;
        }
        internal static List<H_SET> H_FIELD = new List<H_SET>(18);
        internal static List<Entity> human_List = new List<Entity>();

        bool
            GET_TEAMSTATE_FINISHED,
            BOT_SERCH_ON_LUCKY_FINISHED, 
            HUMAN_CONNECTED_, HUMAN_DIED_ALL,  
            IS_FIRST_INFECTD_HUMAN_FINISHED, Human_FIRST_INFECTED_, GAME_ENDED_;
        DateTime GRACE_TIME;

        #endregion

    }
}

