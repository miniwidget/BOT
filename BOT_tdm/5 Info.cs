using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tdm
{
     class Info
    {
        readonly string[] MESSAGES_ALLIES_INFO_A =
        {
             "ATTACHMENT INFORMATION",
             "^7BIND FOLLOWING KEYS  IF SHOW ^2UNBOUND",
             "Press ^2ESC ^7and Goto ^2OPTIONS",
             "Goto ^2CONTROLS ^7 -> ^2MOVEMENT",
             "Bind follwing Keys",

             "1. ^2HOLD STRAFE ^7to any key for ^2AMMO",
             "2. ^2HOLD CROUCH ^7to any key for ^2VIEWSCOPE",
             "3. ^2CHANGE STANCE ^7to any key for ^OFFHANDS"
        };
        readonly string[] MESSAGES_ALLIES_INFO_W =
        {
            "WEAPON  INFORMATION",
            "^7TYPE ^2[ ^7FOLLOWING ^2] ^7TO GET WEAPONS",
            "^2[ ^7RIOT ^2] TO GET RIOTSHIELD",
            "^2[ ^7STINGER ^2] TO GET STINGER",
            "^2[ ^7JAVELIN ^2] TO GET JAVELIN",
            "^2[ ^7AP ^2] TO GET AKIMBO PISTOL",
            "^2[ ^7AG ^2] TO GET AKIMBO GUN",
            "^2[ ^7AR ^2] TO GET ASSAULT RIFFLE",
            "^2[ ^7SM ^2] TO GET SUB MACHINE GUN",
            "^2[ ^7LM ^2] TO GET LIGHT MACHINE GUN",
            "^2[ ^7SG ^2] TO GET SHOT GUN",
            "^2[ ^7SN ^2] TO GET SNIPE GUN",
        };
        internal void MessageInfoA(Entity ent)
        {
            MessageRoop(ent, 0, MESSAGES_ALLIES_INFO_A);
        }
        internal void MessageInfoW(Entity ent)
        {
            MessageRoop(ent, 0, MESSAGES_ALLIES_INFO_W);
        }

        internal static void MessageRoop(Entity e, int i, string[] lists)
        {
            var H = Tdm.H_FIELD[e.EntRef];
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
