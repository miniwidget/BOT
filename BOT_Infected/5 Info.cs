using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    internal class Info
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
            "^2AP ^7| ^2AG ^7| ^2AR ^7| ^2SM ^7| ^2LM ^7| ^2SG ^7| ^2SN",
            "^2[ ^7AP ^2] TO GET AKIMBO PISTOL",
            "^2[ ^7AG ^2] TO GET AKIMBO GUN",
            "^2[ ^7AR ^2] TO GET ASSAULT RIFFLE",
            "^2[ ^7SM ^2] TO GET SUB MACHINE GUN",
            "^2[ ^7LM ^2] TO GET LIGHT MACHINE GUN",
            "^2[ ^7SG ^2] TO GET SHOT GUN",
            "^2[ ^7SN ^2] TO GET SNIPE GUN",
      };
        readonly string[] MESSAGES_AXIS_INFO_W =
        {
            "^7TYPE ^2[ ^7FOLLOWING ^2] ^7TO GET WEAPONS",
            "^2[ ^7RIOT ^2] TO GET RIOTSHIELD",
            "^2[ ^7STINGER ^2] TO GET STINGER",
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
            if (Infected.H_FIELD[e.EntRef].ON_MESSAGE) return;

            if(i==0) Infected.H_FIELD[e.EntRef].ON_MESSAGE = true;

            e.Call(33344, lists[i]);
            i++;

            if (i == lists.Length)
            {
                Infected.H_FIELD[e.EntRef].ON_MESSAGE = false;
                return;
            }
            e.AfterDelay(4000, e1 =>
            {
                MessageRoop(e, i, lists);
            });
        }
    }
}

