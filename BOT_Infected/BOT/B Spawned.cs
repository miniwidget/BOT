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
                if (CARE_PACKAGE == null) CarePackage(killer);
                else killer.Call(33344, "PRESS ^2[ [{+activate}] ] ^7AT THE CARE PACKAGE");
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
            List<Entity> BOTS = new List<Entity>();
            for(int i = 0; i < BOTs_List.Count; i++)
            {
                Entity bot = BOTs_List[i];
                B_SET B = B_FIELD[bot.EntRef];
                if (B.ammoClip == 0|| bot == LUCKY_BOT) continue;
                BOTS.Add(bot);
            }
            OnInterval(2000, () =>
            {
                if (GAME_ENDED_) return false;

                foreach (Entity bot in BOTS)
                {
                    B_SET B = B_FIELD[bot.EntRef];
                    if (!B.wait) BotSearch(bot, B);
                }
                return true;
            });
        }

        void BotSearch(Entity bot, B_SET B)
        {
            
            Vector3 bo = bot.Origin;
   
            if (B.target != null)//이미 타겟을 찾은 경우
            {
                if (human_List.Contains(B.target))
                {
                    if (B.target.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        bot.Call(33468, B.weapon, B.ammoClip);//setweaponammoclip
                        return;
                    }
                }

                B.target = null;
                bot.Call(33468, B.weapon, 0);//setweaponammoclip
            }

            foreach (Entity human in human_List)
            {
                if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                {
                    B.target = human;

                    bot.Call(33468, B.weapon, B.ammoClip);//setweaponammoclip

                    if (B.alert != null) if (human.Name != null) human.Call(33466, B.alert);//"playlocalsound" //deny remote tank !important if not deny, server cause crash

                    return;
                }

            }

        }

        List<Entity> HumanAxis_LIST = new List<Entity>();
        /// <summary>
        /// Survivor bot starts searching Infected humans
        /// </summary>
        private void BotSerchOn_lucky()
        {
            Entity bot = LUCKY_BOT;
            BOT_SERCH_ON_LUCKY_FINISHED = true;
            if (bot.GetField<string>("sessionteam") == "axis") return;

            bot.Call(33220, 1f);//setmovespeedscale

            B_SET B = B_FIELD[bot.EntRef];
            B.wait = false;

            string weapon = B.weapon;
            bot.Call(33469, weapon, 0);//setweaponammostock
            bot.Call(33468, weapon, 0);//setweaponammoclip

            foreach (Entity player in Players)
            {
                if (player == null || !player.IsPlayer) continue;
                if (player.Name.StartsWith("bot")) continue;

                HumanAxis_LIST.Add(player);
            }

            B.wait = false;
            bot.OnInterval(2500, b =>
            {
                if (B.wait || GAME_ENDED_) return false;

                Vector3 bo = b.Origin;

                if (B.target != null)//이미 타겟을 찾은 경우
                {
                    if (HumanAxis_LIST.Contains(B.target))
                    {
                        if (B.target.Origin.DistanceTo(bo) < FIRE_DIST)
                        {
                            b.Call(33468, weapon, B.ammoClip);//setweaponammoclip
                            return true;
                        }
                    }

                    B.target = null;
                    b.Call(33468, weapon, 0);//setweaponammoclip
                }

                foreach (Entity human in HumanAxis_LIST)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        B.target = human;

                        b.Call(33468, B.weapon, B.ammoClip);//setweaponammoclip

                        return true;
                    }
                }
                return true;

            });
        }
    }
}
