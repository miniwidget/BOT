using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Infected
{
    public partial class Infected
    {

        public override void OnSay(Entity player, string name, string text)
        {
            if (name == ADMIN_NAME)
            {
                if(text[0]=='!') if (!AdminCommand(text.Remove(0,1))) return;
            }
            if (GAME_ENDED_) return;

            var texts = text.ToLower().Split(' ');
            var length = texts.Length;

            bool survivor = human_List.Contains(player);

            if (length == 1)
            {
                #region Public Say
                switch (texts[0])
                {
                    
                    case "infoa": ShowInfoA(ref player); return;
                    case "infow": ShowInfoW(ref player); return;

                    case "sc": player.AfterDelay(100, x => player.Call("suicide")); return;

                    case "riot": WP.giveWeaponTo(ref player, "riotshield_mp"); return;
                    case "javelin": WP.giveWeaponTo(ref player, "javelin_mp"); return;
                    case "stinger": WP.giveWeaponTo(ref player, "stinger_mp"); return;
                    case "help": Utilities.RawSayTo( player, "information is in ^7[^2github.com/miniwidget/BOT-infected^7]"); return;
                }
                #endregion

                if (!survivor) return;

                #region Allies Say

                switch (texts[0])
                {
                    case "ap": WP.giveWeaponTo(ref player, WP.AP()); return;
                    case "ag": WP.giveWeaponTo(ref player, WP.AG()); return;
                    case "ar": WP.giveWeaponTo(ref player, WP.AR()); return;
                    case "sm": WP.giveWeaponTo(ref player, WP.SM()); return;
                    case "lm": WP.giveWeaponTo(ref player, WP.LM()); return;
                    case "sn": WP.giveWeaponTo(ref player, WP.SN()); return;
                    case "sg": WP.giveWeaponTo(ref player, WP.SG()); return;

                    case "rpg": WP.giveWeaponTo(ref player, "rpg_mp"); return;
                    case "smaw": WP.giveWeaponTo(ref player, "iw5_smaw_mp"); return;
                    case "m320": WP.giveWeaponTo(ref player, "m320_mp"); return;
                    case "xm25": WP.giveWeaponTo(ref player, "xm25_mp"); return;

                    //case "heli": CallHelicopter(hli);  return;

                }
                #endregion

            }
            else if (length == 2)
            {
                if (!survivor) return;
                var tx = texts[1];
                int i = 0;
                if (!int.TryParse(tx, out i)) return;

                switch (texts[0])
                {
                    case "ap": WP.giveWeaponTo(ref player, WP.AP(i)); return;
                    case "ag": WP.giveWeaponTo(ref player, WP.AG(i)); return;
                    case "ar": WP.giveWeaponTo(ref player, WP.AR(i)); return;
                    case "sm": WP.giveWeaponTo(ref player, WP.SM(i)); return;
                    case "lm": WP.giveWeaponTo(ref player, WP.LM(i)); return;
                    case "sn": WP.giveWeaponTo(ref player, WP.SN(i)); return;
                    case "sg": WP.giveWeaponTo(ref player, WP.SG(i)); return;
                }
            }

        }

    }
}
