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
        readonly string[] MESSAGES_ALLIES_INFO_A =
        {
             "ATTACHMENT INFORMATION",
             "BIND FOLLOWING KEYS  IF SHOW *UNBOUND",
             "Press *ESC ^7and Goto *OPTIONS",
             "Goto *CONTROLS ^7 -> *MOVEMENT",
             "Bind follwing Keys",

             "1. *HOLD STRAFE ^7to any key for *AMMO",
             "2. *HOLD CROUCH ^7to any key for *VIEWSCOPE",
             "3. *CHANGE STANCE ^7to any key for *OFFHANDS"
        };
        readonly string[] MESSAGES_ALLIES_INFO_W =
        {
            "WEAPON  INFORMATION",
            "^7TYPE *[ ^7FOLLOWING *] ^7TO GET WEAPONS",
            "*AP ^7| *AG ^7| *AR ^7| *SM ^7| *LM ^7| *SG ^7| *SN",
            "*[ ^7AP *] TO GET AKIMBO PISTOL",
            "*[ ^7AG *] TO GET AKIMBO GUN",
            "*[ ^7AR *] TO GET ASSAULT RIFFLE",
            "*[ ^7SM *] TO GET SUB MACHINE GUN",
            "*[ ^7LM *] TO GET LIGHT MACHINE GUN",
            "*[ ^7SG *] TO GET SHOT GUN",
            "*[ ^7SN *] TO GET SNIPE GUN",
      };
        readonly string[] MESSAGES_AXIS_INFO_W =
        {
            "^7TYPE *[ ^7FOLLOWING *] ^7TO GET WEAPONS",
            "*[ ^7RIOT *] TO GET RIOTSHIELD",
            "*[ ^7STINGER *] TO GET STINGER",
        };

        internal void MessageInfoA(Entity ent, bool Axis)
        {
            if (!Axis)
                MessageRoop(ent, 0, MESSAGES_ALLIES_INFO_A);
            else
                ent.Call(33344, "NO FUNCTION. BYE");
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
                if(H.ON_MESSAGE) return;
                H.ON_MESSAGE = true;
            }

            e.Call(33344, GetStr( lists[i],H.AXIS));
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

