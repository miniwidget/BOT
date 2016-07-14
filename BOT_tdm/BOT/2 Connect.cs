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
        int BOT_CLASS_NUM;
        private void Bot_Connected(Entity bot)
        {
            var i = BOTs_List.Count;
            int be = bot.EntRef;
            if (be == -1 || i > BOT_SETTING_NUM)
            {
                Call(286, be);//kick
                return;
            }

            B_SET B = B_FIELD[be];
            IsBOT[be] = true;

            bot.AfterDelay(250, b =>
            {
                B.weapon = bot.CurrentWeapon;
                bot.Call(33523, B.weapon);//giveMaxaAmmo
                B.ammoClip = bot.Call<int>(33460, B.weapon);//getcurrentweaponclipammo
                bot.Call(33469, B.weapon, 0);//setweaponammostock
                bot.Call(33468, B.weapon, 0);//setweaponammoclip

                if (B.weapon =="rpg_mp")
                {
                    bot.OnNotify("weapon_fired", (p, wp) =>
                    {
                        if (B.target == null) return;
                        BotAngle(bot, B.target.Origin);
                    });
                    return;
                }

                bool fire = true;
                bot.OnNotify("weapon_fired", (p, wp) =>
                {
                    if (!fire) { fire = true; return; }
                    fire = false;

                    if (B.target == null) return;
                    BotAngle(bot, B.target.Origin);
                });

            });

            BOTs_List.Add(bot);
            bot.SpawnedPlayer += delegate
            {
                if (GAME_ENDED_) return;

                bot.Call(33469, B.weapon, 0);//setweaponammostock
                bot.Call(33468, B.weapon, 0);//setweaponammoclip

                #region check perk to killer

                int k = B.killer;
                if (k != -1)
                {
                    BotCheckPerk(k);
                    B.killer = -1;
                }

                #endregion

                bot.Health = 120;
                B.wait = false;

            };

        }
        void BotAngle(Entity bot, Vector3 TO)
        {
            var BO = bot.Origin;

            float dx = TO.X - BO.X;
            float dy = TO.Y - BO.Y;
            float dz = BO.Z - TO.Z + 50;

            int dist = (int)Math.Sqrt(dx * dx + dy * dy);
            BO.X = (float)Math.Atan2(dz, dist) * 57.3f;
            BO.Y = -5 + (float)Math.Atan2(dy, dx) * 57.3f;
            BO.Z = 0;

            bot.Call(33531, BO);//SetPlayerAngles
        }

        void BotCheckPerk(int k)
        {
            if (human_List.Count > k)
            {
                Entity killer = human_List[k];
                if (killer.EntRef > 17) return;

                H_SET H = H_FIELD[killer.EntRef];
                if (H.PERK > 34) return;

                if (H.PERK_TXT.Length != 12) H.HUD_PERK_COUNT.SetText(H.PERK_TXT += "*");

                var i = (H.PERK += 1);

                if (i > 2 && i % 3 == 0)
                {
                    i = i / 3; if (i > 10) return;
                    PK.Perk_Hud(killer, i);
                    killer.Call(33466, "mp_killstreak_radar");
                }
                else if (i == 11)
                {
                    H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "^1**********");

                    H.CAN_USE_HELI = true;
                    HCT.HeliAttachFlagTag(killer);
                }
            }
        }
        void BotAddWatch()
        {
            OnInterval(2000, () =>
            {
                if (GAME_ENDED_) return false;

                foreach (Entity bot in BOTs_List)
                {
                    B_SET B = B_FIELD[bot.EntRef];
                    if (B.wait) continue;
                    if (B.Axis) BotSearchAllies(bot, B);
                    else BotSearchAxis(bot, B);
                }
                return true;
            });
        }

        void BotSearchAllies(Entity bot, B_SET B)
        {
            Vector3 bo = bot.Origin;

            if (B.target != null)//이미 타겟을 찾은 경우
            {
                if (H_ALLIES_LIST.Contains(B.target))
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

            foreach (Entity human in H_ALLIES_LIST)
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

        void BotSearchAxis(Entity bot, B_SET B)
        {
            Vector3 bo = bot.Origin;

            if (B.target != null)//이미 타겟을 찾은 경우
            {
                if (H_AXIS_LIST.Contains(B.target))
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

            foreach (Entity human in H_AXIS_LIST)
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

    }
}
