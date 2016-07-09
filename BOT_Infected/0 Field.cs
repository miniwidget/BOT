using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{       

    public partial class Infected
    {
        Entity ADMIN;
        List<Entity> BOTs_List = new List<Entity>(18);
        internal static List<Entity> human_List = new List<Entity>(18);

        internal static Random rnd;
        internal static int FIRE_DIST = 3;
        internal static string ADMIN_NAME;

        bool[] IsBOT = new bool[18];
        bool GET_TEAMSTATE_FINISHED;
        bool
            BOT_SERCH_ON_LUCKY_FINISHED, HUMAN_DIED_ALL_ = true,
            GAME_ENDED_;

        List<B_SET> B_FIELD = new List<B_SET>(18);
        internal static List<H_SET> H_FIELD = new List<H_SET>(18);

        DateTime GRACE_TIME;

        void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }

        bool BotDoAttack(bool attack)
        {
            if (attack)
            {
                Call(42, "testClients_doCrouch", 0);
                Call(42, "testClients_doMove", 1);
                Call(42, "testClients_doAttack", 1);
            }
            else
            {
                Call(42, "testClients_doCrouch", 1);
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
            }
            return false;
        }

    }

}
