using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {
        void BotCheckPerk(int k)
        {
            if (human_List.Count > k)
            {
                Entity killer = human_List[k];
                if (killer.EntRef > 17) return;

                H_SET H = H_FIELD[killer.EntRef];
                if (H.PERK > 34) return;

                if (H.PERK_TXT.Length != 12) H.PERK_COUNT_HUD.SetText(H.PERK_TXT += "*");

                var i = (H.PERK += 1);

                if (i > 2 && i % 3 == 0)
                {
                    i = i / 3; if (i > 10) return;
                    PK.Perk_Hud(killer, i);
                    killer.Call(33466, "mp_killstreak_radar");
                }
                else if (i == 11)
                {
                    H.PERK_COUNT_HUD.SetText(H.PERK_TXT = "^1**********");

                    H.CAN_USE_HELI = true;
                    HCT.HeliAttachFlagTag(killer);
                }
            }
        }

        private void SpawnBot(Entity bot)
        {
            if (GAME_ENDED_) return;

            int num = bot.EntRef;
            B_SET B = B_FIELD[num];
            int k = B.killer;
            if (k != -1)
            {
                BotCheckPerk(k);
                B.killer = -1;
            }

            if (B.wep == null) B.wep = bot.CurrentWeapon;
            if (B.wep == "riotshild_mp")
            {
                bot.Call(33220, 2f);//setmovescale
                return;
            }

            bot.Call(32848);//hide
            bot.Call(33220, 0f);//setmovescale
            bot.Health = -1;

            bot.Call(33468, B.wep, 0);//setweaponammoclip
            bot.Call(33469, B.wep, 0);//setweaponammostock

            int delay_time = 6100;

            if (B.wep[2] != '5')
            {
                delay_time = 10000;
            }
            #region check perk to killer

            #endregion

            bot.AfterDelay(delay_time, x =>
            {
                if (GAME_ENDED_) return;
                
                B.fire = true;
                bot.Call(32847);//show
                bot.Call(33220, 1f);

                bot.Health = 120;

                if (B.Axis)
                {
                    BotSearchAxis(bot, B);
                }
                else
                {
                    BotSearchAllies(bot, B);
                }
            });
        }

        void BotSearchAllies(Entity bot, B_SET B)
        {

        }

        void BotSearchAxis(Entity bot, B_SET B)
        {

        }

    }
}
