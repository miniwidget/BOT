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
        //봇 스폰 시작
        private void BotSpawned(Entity bot)
        {
            #region general

            bot.Call(32848);//hide
            bot.Call(33220, 0f);//setmovescale

            int num = bot.EntRef;

            B_SET B = B_FIELD[num];
            if (B.wep == null) B.wep = bot.CurrentWeapon;

            bot.Call(33469, B.wep, 0);//setweaponammostock
            bot.Call(33468, B.wep, 0);//setweaponammoclip

            #endregion

            #region check perk to killer

            int k = B.killer;
            if (k != -1)
            {
                if (human_List.Count > k)
                {
                    Entity killer = human_List[k];
                    if (killer.EntRef > 17) return;

                    H_SET H = H_FIELD[killer.EntRef];
                    if (H.PERK > 34) return;

                    var i = (H.PERK += 1);

                    if (i > 2 && i % 3 == 0)
                    {
                        i = i / 3; if (i > 10) return;
                        PK.Perk_Hud(killer, i);
                        killer.Call(33466, "mp_killstreak_radar");
                    }
                    else if (i == 11)
                    {
                        H.USE_HELI = 1;
                        HCT.HeliAttachFlagTag(killer);
                    }
                }

                B.killer = -1;
            }

            #endregion

            int delay_time = 6100;

            if (num == BOT_RPG_ENTREF || num == BOT_RIOT_ENTREF) delay_time = 10000;
            else if (num != BOT_JUGG_ENTREF) bot.Health = -1;

            bot.AfterDelay(delay_time, bot_ =>
            {
                if (GAME_ENDED_) return;
                B.fire = true;

                bot_.Call(32847);//show

                if (num == BOT_RPG_ENTREF) BotSearchOn_slow(bot_, B);

                else if (num == BOT_RIOT_ENTREF)
                {
                    bot.Call(33220, 2f);
                    
                    return;
                }
                else if (num == BOT_JUGG_ENTREF)
                {
                    BotSearchOnJugg(bot_, B);
                    return;
                }

                BotSearchOn(bot_, B);
                

            });
        }

        //봇 목표물 찾기 루프
        private void BotSearchOn(Entity bot, B_SET B)
        {
            bot.Call(33220, 1f);//setmovescale
            bot.Health = 150;
            bool pause = false;
            int death = B.death;
            string weapon = B.wep;

            bot.OnInterval(2000, b =>
            {
                if (death != B.death) return false;

                Vector3 bo = b.Origin;

                var target = B.target;
                if (target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(target))
                    {
                        var POD = target.Origin.DistanceTo(bo);
                        if (POD < FIRE_DIST)
                        {
                            b.Call(33468, weapon, 500);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo

                            pause = false;
                            return true;
                        }
                    }

                    B.target = null;
                    B.fire = false;
                }
                b.Call(33469, weapon, 0);//setweaponammostock
                b.Call(33468, weapon, 0);//setweaponammoclip
                pause = true;

                //타겟 찾기 시작
                foreach (Entity human in human_List)
                {
                    var POD = human.Origin.DistanceTo(bo);

                    if (POD < FIRE_DIST)
                    {
                        B.target = human;
                        B.fire = true;
                        pause = false;
                        b.Call(33468, weapon, 500);//setweaponammoclip
                        b.Call(33523, weapon);//givemaxammo
                        b.OnInterval(400, bb =>
                        {
                            if (pause || !B.fire) return false;

                            var TO = human.Origin;
                            var BO = bb.Origin;

                            float dx = TO.X - BO.X;
                            float dy = TO.Y - BO.Y;
                            float dz = BO.Z - TO.Z + 50;

                            int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                            BO.X = (float)Math.Atan2(dz, dist) * 57.32f;
                            BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.32f;
                            BO.Z = 0;

                            bb.Call(33531, BO);//SetPlayerAngles
                            return true;
                        });

                        return true;
                    }

                }
                return true;

            });

        }
        private void BotSearchOnJugg(Entity bot, B_SET B)
        {
            bot.Call(33220, 1f);//setmovescale
            bool pause = false;
            int death = B.death;
            string weapon = B.wep;

            bot.OnInterval(2000, b =>
            {
                if (death != B.death) return false;

                Vector3 bo = bot.Origin;

                var target = B.target;
                if (target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(target))
                    {
                        if (target.Origin.DistanceTo(bo) < FIRE_DIST)
                        {
                            b.Call(33468, weapon, 500);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo
                            pause = false;
                            return true;
                        }
                    }

                    B.target = null;
                    B.fire = false;
                }
                b.Call(33469, weapon, 0);//setweaponammostock
                b.Call(33468, weapon, 0);//setweaponammoclip
                pause = true;

                //타겟 찾기 시작
                bool found = false;
                foreach (Entity human in human_List)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        if (!found)
                        {
                            found = true;
                            target = B.target = human;
                            B.fire = true;
                            pause = false;
                        }
                        if (human.Name != null) human.Call(33466, "AF_victory_music");//"playlocalsound"
                    }
                }

                if (target != null)
                {
                    b.Call(33468, weapon, 500);//setweaponammoclip
                    b.Call(33523, weapon);//givemaxammo
                    b.OnInterval(300, bb =>
                    {
                        if (pause || !B.fire) return false;

                        var TO = target.Origin;
                        var BO = bb.Origin;

                        float dx = TO.X - BO.X;
                        float dy = TO.Y - BO.Y;
                        float dz = BO.Z - TO.Z + 50;

                        int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                        BO.X = (float)Math.Atan2(dz, dist) * 57.32f;
                        BO.Y =-10+ (float)Math.Atan2(dy, dx) * 57.32f;
                        BO.Z = 0;

                        bb.Call(33531, BO);//SetPlayerAngles

                        return true;
                    });
                }
                return true;

            });

        }
        private void BotSearchOn_slow(Entity bot, B_SET B)
        {
            bot.Call(33220, 0.7f);//setmovespeedscale
            bot.Health = 150;
            bool pause = false;
            int death = B.death;

            bot.OnInterval(2000, bot_ =>
            {
                if (death != B.death) return false;

                Vector3 bo = bot.Origin;
                var target = B.target;
                if (target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(target))
                    {
                        var POD = target.Origin.DistanceTo(bo);
                        if (POD < FIRE_DIST)
                        {
                            pause = false;
                            return true;
                        }
                    }

                    B.target = null; //타겟과 거리가 멀어진 경우, 타겟 제거
                    B.fire = false;
                    bot_.Call(33468, "rpg_mp", 0);//setweaponammoclip
                }

                pause = true;
                //타겟 찾기 시작
                foreach (Entity human in human_List)
                {

                    var POD = human.Origin.DistanceTo(bo);

                    if (POD < FIRE_DIST)
                    {
                        B.target = human;
                        B.fire = true;
                        pause = false;

                        if (human.Name != null) human.Call(33466, "missile_incoming");

                        bot_.OnInterval(1500, bb =>
                        {

                            if (pause || !B.fire) return false;

                            var ho = human.Origin; ho.Z -= 50;

                            Vector3 a = Call<Vector3>(247, ho - bb.Origin);//vectortoangles
                            bb.Call(33531, a);//SetPlayerAngles
                            bb.Call(33468, "rpg_mp", 2);//setweaponammoclip
                            return true;
                        });

                        return true;
                    }

                }

                return true;

            });
        }

    }
}
