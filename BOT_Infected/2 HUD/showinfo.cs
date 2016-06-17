using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    public partial class Infected
    {
        string[] MESSAGES_ALLIES_INFO_A =
        {
             "ATTACHMENT INFORMATION",
             "^7BIND FOLLOWING KEYS  IF SHOW ^2UNBOUND",
             "press ^2ESC ^7and goto ^2OPTIONS",
             "goto ^2CONTROLS ^7 -> ^2MOVEMENT",
             "bind follwing keys",

             "1. ^2HOLD STRAFE ^7to any key",
             "2. ^2HOLD CROUCH ^7to any key",
             "3. ^2HOLD PRONE ^7to any key",
             "4. ^2CHANGE STANCE ^7to any key"
        };
        string[] MESSAGES_ALLIES_INFO_W =
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
        string[] MESSAGES_AXIS_INFO_W =
        {
            "^7TYPE ^2[ ^7FOLLOWING ^2] ^7TO GET WEAPONS",
            "^2[ ^7RIOT ^2] TO GET RIOTSHIELD",
            "^2[ ^7STINGER ^2] TO GET STINGER",
        };

        void ShowInfoA(Entity ent)
        {
            if (!isSurvivor(ent))
            {
                ent.Call("iPrintlnBold", "NO FUNCTION. BYE");
                return;
            }

            roopMessage(ent, 0, MESSAGES_ALLIES_INFO_A);
        }
        void ShowInfoW(Entity ent)
        {
            if (isSurvivor(ent))

                roopMessage(ent, 0, MESSAGES_ALLIES_INFO_W);
            else
                roopMessage(ent, 0, MESSAGES_AXIS_INFO_W);
        }

        void roopMessage(Entity e, int i, string[] lists)
        {
            if (e == null) return;
            if (i == 0)
            {
                if (H_FIELD[e.EntRef].ON_MESSAGE) return;
            }
            e.Call("iPrintlnBold", lists[i]);
            i++;
            if (i == lists.Length)
            {
                H_FIELD[e.EntRef].ON_MESSAGE = false;
                return;
            }
            AfterDelay(4000, () =>
            {
                roopMessage(e, i, lists);
            });
        }
        void RM(Entity e, int i, string[] lists)
        {
            if (e == null) return;
            if (i == 0)
            {
                if (H_FIELD[e.EntRef].ON_MESSAGE) return;
            }
            e.Call("iPrintlnBold", lists[i]);
            i++;
            if (i == lists.Length)
            {
                H_FIELD[e.EntRef].ON_MESSAGE = false;
                return;
            }
            AfterDelay(3000, () =>
            {
                roopMessage(e, i, lists);
            });
        }

        void showMessage(Entity e, string message)
        {
            if (H_FIELD[e.EntRef].ON_MESSAGE) return;
            e.Call("iPrintlnBold", message);
        }
    }
}

