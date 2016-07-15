using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Timers;

namespace Infected
{

    public partial class Infected
    {

        void BotCheckPerk(int k)
        {
            if (human_List.Count <= k) return;

            Entity killer = human_List[k];
            if (killer.EntRef > 17) return;//deny tank

            H_SET H = H_FIELD[killer.EntRef];
            if (H.AXIS) return;
            if (H.PERK > 34) return;

            var i = (H.PERK += 1);
            if (i == 9) H.PERK_TXT = H.PERK_TXT.Replace("^1PRDT", "HELI");

            if (H.PERK_TXT.Length != 17) H.HUD_PERK_COUNT.SetText(H.PERK_TXT += "*");

            if (i > 2 && i % 3 == 0)
            {
                i = i / 3; if (i > 10) return;
                PK.Perk_Hud(killer, i);
                killer.Call(33466, "mp_killstreak_radar");
            }
            else if (i == 8)
            {
                H.CAN_USE_PREDATOR = true;

                if (CARE_PACKAGE != null) killer.Call(33344, "PRESS ^2[ [{+activate}] ] ^7AT THE CARE PACKAGE");
                else killer.Call(33344, "PRESS ^2[ [{+activate}] ] ^7TO CALL PREDATOR");

                string txt = H.PERK_TXT;
                H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "^1" + txt);
            }
            else if (i == 11)
            {
                H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "^1HELI **********");

                H.CAN_USE_HELI = true;
                HCT.HeliAttachFlagTag(killer);
            }
        }

        void BotAddWatch()
        {
            BOT_ADD_WATCHED = true;

            List<B_SET> bots_fire = new List<B_SET>();
            foreach (B_SET B in B_FIELD)
            {
                if (B == null) continue;
                bots_fire.Add(B);
            }

            if (SET.MAP_IDX == 5) BOT_HELI_HEIGHT += 1000;

            BotHeli BH = new BotHeli(BOTs_List[BOT_HELIRIDER_IDX], Players.Select(ent => ent.Origin).ToArray());
          
            OnInterval(2500, () =>
            {
                if (GAME_ENDED_) return false;

                foreach (B_SET B in bots_fire)
                {
                    if (B.wait) continue;
                    B.BotSearch();
                }

                if (LUCKY_BOT_START) LUCKY_B.BotSearchAxis();

                BH.HeliBotSearch();

                return true;
            });
        }

        /// <summary>
        /// Survivor bot starts searching Infected humans
        /// </summary>
        private void BotSerchOn_lucky()
        {
            BOT_SERCH_ON_LUCKY_FINISHED = true;

            if (LUCKY_BOT.GetField<string>("sessionteam") == "axis") return;
            B_FIELD[LUCKY_BOT.EntRef] = new B_SET(LUCKY_BOT )
            {
                weapon = LUCKY_BOT.CurrentWeapon,
                ammoClip = 100,
            };
            LUCKY_B = B_FIELD[LUCKY_BOT.EntRef];
            LUCKY_BOT_START = true;
            LUCKY_BOT.Call(33220, 1f);//setmovespeedscale

            
        }
    }
}
