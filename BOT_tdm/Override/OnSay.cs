using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {

        public override void OnSay(Entity player, string name, string text)
        {

            if (player.Name == ADMIN_NAME)
            {
                if (!AdminCommand(text)) return;
            }
            if (GAME_ENDED_) return;


            var texts = text.ToLower().Split(' ');
            var length = texts.Length;

            if (length == 1)
            {
                switch (texts[0])
                {
                    case "infoa": INFO.MessageInfoA(player); return;
                    case "infow": INFO.MessageInfoW (player); return;

                    case "sc": AfterDelay(100, () => player.Call("suicide")); return;

                    case "riot": giveWeaponTo(player, "riotshield_mp"); return;
                    case "javelin": giveWeaponTo(player, "javelin_mp"); return;
                    case "stinger": giveWeaponTo(player, "stinger_mp"); return;

                    case "ap": giveWeaponTo(player, AP()); return;
                    case "ag": giveWeaponTo(player, AG()); return;
                    case "ar": giveWeaponTo(player, AR()); return;
                    case "sm": giveWeaponTo(player, SM()); return;
                    case "lm": giveWeaponTo(player, LM()); return;
                    case "sn": giveWeaponTo(player, SN()); return;
                    case "sg": giveWeaponTo(player, SG()); return;

                    case "rpg": giveWeaponTo(player, "rpg_mp"); return;
                    case "smaw": giveWeaponTo(player, "iw5_smaw_mp"); return;
                    case "m320": giveWeaponTo(player, "m320_mp"); return;
                    case "xm25": giveWeaponTo(player, "xm25_mp"); return;

                }

            }
            else if (length == 2)
            {
                var tx = texts[1];
                int i = 0;
                if (!int.TryParse(tx, out i)) i = 0;

                switch (texts[0])
                {
                    case "ap": giveWeaponTo(player, AP(i)); return;
                    case "ag": giveWeaponTo(player, AG(i)); return;
                    case "ar": giveWeaponTo(player, AR(i)); return;
                    case "sm": giveWeaponTo(player, SM(i)); return;
                    case "lm": giveWeaponTo(player, LM(i)); return;
                    case "sn": giveWeaponTo(player, SN(i)); return;
                    case "sg": giveWeaponTo(player, SG(i)); return;
                }

            }

        }

    }
}
