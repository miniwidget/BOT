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
                if(text[0]=='/') if (!AdminCommand(text.Remove(0,1))) return;
            }

            if (GAME_ENDED_) return;

            string[] texts = text.ToLower().Split(' ');
            string text0 = texts[0];

            bool Axis = H_FIELD[player.EntRef].AXIS;

            int length = texts.Length;

            if (length == 1)
            {
                #region Public Say
                switch (text0)
                {
                    case "infoa": INFO.MessageInfoA(player, Axis); return;
                    case "infow": INFO.MessageInfoW(player, Axis); return;

                    case "sc":
                        if (H_FIELD[player.EntRef].AXIS) H_FIELD[player.EntRef].AX_WEP = 2;//자살로 죽음
                        AfterDelay(100, () => player.Call(33341));//"suicide"
                        return;

                    case "riot": WP.GiveWeaponTo(player, "riotshield_mp"); return;
                    case "javelin": WP.GiveWeaponTo(player, "javelin_mp"); return;
                    case "stinger": WP.GiveWeaponTo(player, "stinger_mp"); return;

                    case "loc": Relocation(player,false); return;
                    case "reloc": Relocation(player, true);return;
                        // case "ti": SetLocationByTI(player);return;
                }
                #endregion

                if (Axis) return;

                #region Allies Say

                switch (text0)
                {
                    case "ap": WP.GiveWeaponTo(player, 0); return;
                    case "ag": WP.GiveWeaponTo(player, 1); return;
                    case "ar": WP.GiveWeaponTo(player, 2); return;
                    case "sm": WP.GiveWeaponTo(player, 3); return;
                    case "lm": WP.GiveWeaponTo(player, 4); return;
                    case "sg": WP.GiveWeaponTo(player, 5); return;
                    case "sn": WP.GiveWeaponTo(player, 6); return;

                    case "rpg": WP.GiveWeaponTo(player, "rpg_mp"); return;
                    case "smaw": WP.GiveWeaponTo(player, "iw5_smaw_mp"); return;
                    case "m320": WP.GiveWeaponTo(player, "m320_mp"); return;
                    case "xm25": WP.GiveWeaponTo(player, "xm25_mp"); return;

                }
                #endregion

            }
            else if (length == 2)
            {
                if (Axis) return;

                int i;
                if (!int.TryParse(texts[1], out i)) return;

                switch (text0)
                {
                    case "ap": WP.GiveWeaponTo(player, 0,i); return;
                    case "ag": WP.GiveWeaponTo(player, 1, i); return;
                    case "ar": WP.GiveWeaponTo(player, 2, i); return;
                    case "sm": WP.GiveWeaponTo(player, 3, i); return;
                    case "lm": WP.GiveWeaponTo(player, 4, i); return;
                    case "sg": WP.GiveWeaponTo(player, 5, i); return;
                    case "sn": WP.GiveWeaponTo(player, 6, i); return;
                }

            }

        }

    }
}
