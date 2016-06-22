using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{

    class MessageText
    {
        internal readonly string[] MESSAGES_ALLIES_INFO_A =
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
        internal readonly string[] MESSAGES_ALLIES_INFO_W =
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
        internal readonly string[] MESSAGES_AXIS_INFO_W =
        {
                "^7TYPE ^2[ ^7FOLLOWING ^2] ^7TO GET WEAPONS",
                "^2[ ^7RIOT ^2] TO GET RIOTSHIELD",
                "^2[ ^7STINGER ^2] TO GET STINGER",
            };

        internal readonly string[] BOTs_CLASS = { "axis_recipe1", "axis_recipe2", "axis_recipe3", "class0", "class1", "class2", "class4", "class5", "class6", "class6" };
        internal readonly string[] HELI_MESSAGE_KEY_INFO = { "HELI INFO", "^2[ [{+breath_sprint}] ] ^7MOVE DOWN", "^2[ [{+gostand}] ] ^7MOVE UP" };
        internal readonly string[] HELI_MESSAGE_ACTIVATE = { "PRESS ^2[ [{+activate}] ] ^7AT THE HELI TURRET AREA", "YOU CAN RIDE IN HELICOPTER" };
        internal readonly string[] HELI_MESSAGE_ALERT = { "YOU ARE NOT IN THE HELI AREA", "GO TO HELI AREA AND", "PRESS ^2[ [{+activate}] ] ^7AT THE HELI AREA" };
        internal readonly string[] HELI_MESSAGE_WAIT_PLAYER = { "YOU CAN RIDE HELLI", "IF ANOTHER PLAYER ONBOARD" };
        internal readonly string[] soundAlert = { "AF_1mc_losing_fight", "AF_1mc_lead_lost", "PC_1mc_losing_fight", "PC_1mc_take_positions", "PC_1mc_positions_lock" };

    }
}
