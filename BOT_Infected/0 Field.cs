using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    public partial class Infected
    {
        internal static Random rnd;
        internal Entity ADMIN;
        internal static int FIRE_DIST, PLAYER_LIFE = 2;
        internal static bool USE_ADMIN_SAFE_;
        internal static string ADMIN_NAME;

        bool[] IsBOT = new bool[18];

        void Print(object s)
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
            public H_SET(int life)
            {
                this.LIFE = life;
            }
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
            internal int LIFE;
            internal bool RESPAWN;
            internal int PERK = 2;

            internal bool AXIS;
            internal int AX_WEP;
            internal bool BY_SUICIDE;

            internal int USE_HELI;

            internal bool LOC_NOTIFIED;
            internal bool LOC_DO;
            internal Vector3 RELOC;

            internal bool AC130_NOTIFIED;
            internal bool AC130_ON_USE;

            internal byte TURRET_STATE;
        }
        internal static List<H_SET> H_FIELD = new List<H_SET>(18);
        internal static List<Entity> human_List = new List<Entity>();

        bool
            GET_TEAMSTATE_FINISHED,
            BOT_SERCH_ON_LUCKY_FINISHED,
            HUMAN_CONNECTED_, HUMAN_DIED_ALL,
            IS_FIRST_INFECTD_HUMAN_FINISHED, Human_FIRST_INFECTED_, GAME_ENDED_;
        DateTime GRACE_TIME;
    }
}
