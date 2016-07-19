using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    class Info
    {
        readonly string[] MESSAGES_KILLSTREAKS =
        {
            "KILLSTREAKS  INFORMATION",
            "1. IF PRDT APPEARS LEFT BOTTOM,",
            "PRESS  *[  [{+activate}]  ]  ^7TO GET RIDE PREDATOR",
            "2. IF HELI APPEARS LEFT BOTTOM,",
            "PRESS  *[  [{+activate}]  ]  ^7TO AT THE HELICOPTER AREA",
            "3. IF KEY CODE MESSAGE APPEARS,",
            "TYPE KEY CODE IMMEDIATLY TO GET RIDE VEHICLES"
        };
        readonly string[] MESSAGES_ALLIES_INFO_W =
        {
            "WEAPON  INFORMATION",
            "TYPE [  *FOLLOWING  ^7] 7TO GET WEAPONS",
            "[  *AP  ^7]  TO GET AKIMBO PISTOL",
            "[  *AG  ^7]  TO GET AKIMBO GUN",
            "[  *AR  ^7]  TO GET ASSAULT RIFFLE",
            "[  *SM  ^7]  TO GET SUB MACHINE GUN",
            "[  *LM  ^7]  TO GET LIGHT MACHINE GUN",
            "[  *SG  ^7]  TO GET SHOT GUN",
            "[  *SN  ^7]  TO GET SNIPE GUN",
      };
        readonly string[] MESSAGES_AXIS_INFO_W =
        {
            "TYPE [  *FOLLOWING  ^7] TO GET WEAPONS",
            "[  *RIOT  ^7] TO GET RIOTSHIELD",
            "[  *STINGER  ^7] TO GET STINGER",
            "[  *JAVELIN  ^7] TO GET JAVELIN",
        };
        internal void MessageInfoK(Entity ent, bool Axis)
        {
            MessageRoop(ent, 0, MESSAGES_KILLSTREAKS);
        }
        internal void MessageInfoW(Entity ent, bool Axis)
        {
            if (!Axis)
                MessageRoop(ent, 0, MESSAGES_ALLIES_INFO_W);
            else
                MessageRoop(ent, 0, MESSAGES_AXIS_INFO_W);
        }

        internal static void MessageRoop(Entity e, int i, string[] lists)
        {
            var H = Infected.H_FIELD[e.EntRef];
            if (i == 0)
            {
                if (H.ON_MESSAGE) return;
                H.ON_MESSAGE = true;
            }

            e.Call(33344, GetStr(lists[i], H.AXIS));
            i++;

            if (i == lists.Length)
            {
                H.ON_MESSAGE = false;
                return;
            }
            e.AfterDelay(4000, e1 =>
            {
                MessageRoop(e, i, lists);
            });
        }
        internal static string GetStr(string value, bool axis)
        {
            if (!axis) return value.Replace("*", "^2");
            else return value.Replace("*", "^1");
        }

    }
}

