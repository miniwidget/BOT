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
            "1. IF  ^1[ PRDT ]  ^7APPEARS LEFT BOTTOM,",
            "PRESS  *[  [{+activate}]  ]  ^7TO GET RIDE PREDATOR",
            "2. IF  ^1[ HELI ]  ^7APPEARS LEFT BOTTOM,",
            "PRESS  *[  [{+activate}]  ]  ^7TO AT THE HELICOPTER AREA",
            "3. IF  ^2[  KEY CODE  ]  ^7MESSAGE APPEARS,",
            "TYPE KEY CODE IMMEDIATLY TO GET RIDE VEHICLES"
        };
        readonly string[] MESSAGES_ALLIES_INFO_W =
        {
            "WEAPON  INFORMATION",
            "*TYPE [  ^7FOLLOWING  *] 7TO GET attachment",
            "*[  ^7VS  *]  TO GET VIEW SCOPE",
            "*[  ^7AT  *]  TO GET ATTACHMENT",
            "*[  ^7SL  *]  TO GET SIRENCER",
            "*TYPE [  ^7FOLLOWING  *] 7TO GET WEAPONS",
            "*[  ^7AP  *]  TO GET AKIMBO PISTOL",
            "*[  ^7AG  *]  TO GET AKIMBO GUN",
            "*[  ^7AR  *]  TO GET ASSAULT RIFFLE",
            "*[  ^7SM  *]  TO GET SUB MACHINE GUN",
            "*[  ^7LM  *]  TO GET LIGHT MACHINE GUN",
            "*[  ^7SG  *]  TO GET SHOT GUN",
            "*[  ^7SN  *]  TO GET SNIPE GUN",
      };
        readonly string[] MESSAGES_AXIS_INFO_W =
        {
            "*TYPE [  ^7FOLLOWING  *] TO GET WEAPONS",
            "*[  ^7RIOT  *] TO GET RIOTSHIELD",
            "*[  ^7STINGER  *] TO GET STINGER",
            "*[  ^7JAVELIN  *] TO GET JAVELIN",
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

