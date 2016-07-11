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
            if (human_List.Count > k)
            {
                Entity killer = human_List[k];
                if (killer.EntRef > 17) return;
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
                    else killer.Call(33344, "^2PRESS [^7 [{+activate}] ^2] AT THE CARE PACKAGE");
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
        }

        private void BotSearchOn(Entity bot, B_SET B, string alert, string weapon, int ammo)
        {

            int death = B.death;

            bot.OnInterval(2000, b =>
            {
                if (death != B.death) return false;
                if (HUMAN_DIED_ALL_) return true;

                Vector3 bo = b.Origin;

                if (B.target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(B.target))
                    {
                        var TOD = B.target.Origin.DistanceTo(bo);
                        if (TOD < FIRE_DIST)
                        {
                            b.Call(33468, weapon, ammo);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo
                            return true;
                        }
                    }

                    B.target = null;
                    b.Call(33469, weapon, 0);//setweaponammostock
                    b.Call(33468, weapon, 0);//setweaponammoclip

                }

                foreach (Entity human in human_List)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        B.target = human;

                        b.Call(33468, weapon, ammo);//setweaponammoclip
                        b.Call(33523, weapon);//givemaxammo

                        if (alert != null) if (human.Name != null) human.Call(33466, alert);//"playlocalsound" //deny remote tank !important if not deny, server cause crash

                        return true;
                    }

                }

                return true;

            });

        }

        /// <summary>
        /// Survivor bot starts searching Infected humans
        /// </summary>
        private void BotSerchOn_lucky(Entity bot)
        {
            BOT_SERCH_ON_LUCKY_FINISHED = true;
            if (bot.GetField<string>("sessionteam") == "axis") return;

            bot.Call(33220, 1f);//setmovespeedscale

            B_SET B = B_FIELD[bot.EntRef];
            int death = B.death;

            string weapon = bot.CurrentWeapon;
            bot.Call(33469, weapon, 0);//setweaponammostock
            bot.Call(33468, weapon, 0);//setweaponammoclip

            List<Entity> HumanAxis = new List<Entity>();

            foreach (Entity player in Players)
            {
                if (player == null || !player.IsPlayer) continue;
                if (player.Name.StartsWith("bot")) continue;

                HumanAxis.Add(player);
            }

            bot.OnInterval(2000, b =>
            {
                if (death != B.death) return false;

                Vector3 bo = b.Origin;

                if (B.target != null)//이미 타겟을 찾은 경우
                {
                    if (HumanAxis.Contains(B.target))
                    {
                        if (B.target.Origin.DistanceTo(bo) < FIRE_DIST)
                        {
                            b.Call(33468, weapon, 500);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo
                            return true;
                        }
                    }

                    B.target = null;
                    b.Call(33469, weapon, 0);//setweaponammostock
                    b.Call(33468, weapon, 0);//setweaponammoclip
                }

                foreach (Entity human in HumanAxis)
                {
                    var HOD = human.Origin.DistanceTo(bo);

                    if (HOD < FIRE_DIST)
                    {
                        B.target = human;

                        b.Call(33468, weapon, 500);//setweaponammoclip
                        b.Call(33523, weapon);//givemaxammo

                        return true;
                    }
                }
                return true;

            });
        }
    }
}
