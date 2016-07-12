using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{       

    public partial class Infected
    {
        Entity ADMIN,LUCKY_BOT;
        List<B_SET> B_FIELD = new List<B_SET>(18);
        List<Entity> BOTs_List = new List<Entity>(18);
        
        internal static List<Entity> human_List = new List<Entity>(18);
        internal static List<H_SET> H_FIELD = new List<H_SET>(18);

        internal static Random rnd;
        internal static int FIRE_DIST;
        internal static string ADMIN_NAME;
        internal static bool USE_PREDATOR;
        
        bool[] IsBOT = new bool[18];
        bool GAME_ENDED_, GET_TEAMSTATE_FINISHED, BOT_SERCH_ON_LUCKY_FINISHED, HUMAN_DIED_ALL_ = true;

        int INITIAL_HUMAN_INF_IDX;
        DateTime GRACE_TIME;

        void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
        
    }

}
