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
        void Relocation(Entity player)
        {
            player.Call(33344, "NOT YET...ON TEST. SORRY");

            return;

            int count = human_List.Count;
            if (count== 1)
            {

            }else
            {
                if (HCT.IsUsingTurret(player))
                {
                    if (TK.IfTankOwnerEnd(player))
                    {
                        player.AfterDelay(500, p =>
                        {
                            player.Call("setorigin", human_List[rnd.Next(count)].Origin);
                        });
                        return;
                    }
                }
                player.Call("setorigin", human_List[rnd.Next(count)].Origin);
            }
        }
        public override void OnSay(Entity player, string name, string text)
        {
            if (name == "kwnav")
            {
                if(text[0]=='/') if (!AdminCommand(text.Remove(0,1))) return;
               // if (!AdminCommand(text)) return;
            }

            if (GAME_ENDED_) return;

            string[] texts = text.ToLower().Split(' ');
            string text0 = texts[0];

            bool Axis = IsAXIS[player.EntRef];

            int length = texts.Length;

            if (length == 1)
            {
                #region Public Say
                switch (text0)
                {
                    case "infoa": info.MessageInfoA(player, Axis); return;
                    case "infow": info.MessageInfoW(player, Axis); return;
                    case "sc": AfterDelay(100, () => player.Call("suicide")); return;
                    case "riot": WP.GiveWeaponTo(player, "riotshield_mp"); return;
                    case "javelin": WP.GiveWeaponTo(player, "javelin_mp"); return;
                    case "stinger": WP.GiveWeaponTo(player, "stinger_mp"); return;
                }
                #endregion

                if (Axis) return;

                #region Allies Say

                switch (text0)
                {
                    case "loc": return;
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
