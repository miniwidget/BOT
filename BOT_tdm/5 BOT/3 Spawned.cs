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

        #region 일반
        //봇 스폰 시작
        private void SpawnBot(Entity bot)
        {
            int num = bot.EntRef;
            if (num == -1 || GAME_ENDED_) return;

            B_SET B = B_FIELD[num];
            bot.SetField("sessionteam", "bot");

            bot.Call(32848);//hide
            bot.Call(33220, 0f);//setmovescale
            bot.Health = -1;
            if (B.wep == null) B.wep = bot.CurrentWeapon;
            var weapon = B.wep;
            bot.Call(33468, weapon, 0);//setweaponammoclip
            bot.Call(33469, weapon, 0);//setweaponammostock

            AfterDelay(BOT_DELAY_TIME, () =>
            {
                if (GAME_ENDED_) return;
                B.fire = true;
                bot.Call(32847);//show
                bot.Call(33220, 1f);

                bot.Health = 150;
                StartBotSearch(bot, B);
            });
        }

        //봇 목표물 찾기 루프
        private void StartBotSearch(Entity bot, B_SET B)
        {
            try
            {

                bool pause = false;
                int death = B.death;
                string weapon = B.wep;

                OnInterval(SEARCH_TIME, () =>
                {
                    if (death != B.death) return false;
                    if (!HUMAN_CONNECTED_) return pause = true;

                    var target = B.target;

                    if (target != null)//이미 타겟을 찾은 경우
                    {
                        if (human_List.Contains(target))
                        {
                            //if (TEST_) return true;
                            var POD = target.Origin.DistanceTo(bot.Origin);
                            if (POD < FIRE_DIST) return !(pause = false);
                        }

                        B.target = null;
                        B.fire = false;
                        bot.Call(33468, weapon, 0);//setweaponammoclip
                    }
                    pause = true;

                    //타겟 찾기 시작
                    foreach (Entity human in human_List)
                    {
                        var POD = human.Origin.DistanceTo(bot.Origin);

                        if (POD < FIRE_DIST)
                        {
                            B.target = human;
                            B.fire = true;
                            pause = false;
                            OnInterval(FIRE_TIME, () =>
                            {
                                if (pause || !B.fire) return false;

                                var ho = human.Origin; ho.Z -= 50;

                                Vector3 angle = Call<Vector3>(247, ho - bot.Origin);//vectortoangles
                                bot.Call(33531, angle);//SetPlayerAngles
                                bot.Call(33468, weapon, 5);//setweaponammoclip
                                return true;
                            });

                            return true;
                        }

                    }
                    return true;

                });
            }
            catch
            {
                print("★ 빠른 봇 예외 발생");
            }

        }
        #endregion

        #region RPG bot

        void SpawnRpgBot(Entity bot, string team)
        {
            int num = bot.EntRef;
            if (num == -1 || GAME_ENDED_) return;

            bot.Call(33468, "rpg_mp", 0);//setweaponammoclip
            bot.Call(33469, "rpg_mp", 0);//setweaponammostock

            B_SET B = B_FIELD[num];
            if (B.wep == null) B.wep = bot.CurrentWeapon;

            bot.Call(32848);//hide
            bot.Call(33220, 0f);
            bot.Health = -1;
            AfterDelay(10000, () =>
            {
                if (GAME_ENDED_) return;

                B.fire = true;
                bot.Health = 150;
                bot.Call(32847);//show
                bot.Call(33220, 0.7f);//setmovescale

                if (team == "axis") StartRpgBotSearch_Allies(bot, B);
                else StartRpgBotSearch_Axis(bot, B);
            });
        }
        void StartRpgBotSearch_Axis(Entity bot, B_SET B)
        {
            try
            {
                bool pause = false;
                int death = B.death;

                OnInterval(SEARCH_TIME, () =>
                {
                    if (death != B.death) return false;
                    if (!HUMAN_CONNECTED_) return pause = true;

                    var target = B.target;
                    if (target != null)//이미 타겟을 찾은 경우
                    {
                        if (human_List.Contains(target))
                        {
                            //if (TEST_) return true;
                            var POD = target.Origin.DistanceTo(bot.Origin);
                            if (POD < FIRE_DIST) return !(pause = false);
                        }

                        B.target = null; //타겟과 거리가 멀어진 경우, 타겟 제거
                        B.fire = false;
                        bot.Call(33468, "rpg_mp", 0);//setweaponammoclip
                    }

                    pause = true;
                    //B.rooping = true;
                    //타겟 찾기 시작
                    foreach (Entity human in H_AXIS_LIST)
                    {

                        var POD = human.Origin.DistanceTo(bot.Origin);

                        if (POD < FIRE_DIST)
                        {
                            B.target = human;
                            B.fire = true;
                            pause = false;

                            OnInterval(1500, () =>
                            {

                                if (pause || !B.fire) return false;

                                var ho = human.Origin; ho.Z -= 50;

                                Vector3 angle = Call<Vector3>(247, ho - bot.Origin);//vectortoangles
                                bot.Call(33531, angle);//SetPlayerAngles
                                bot.Call(33468, "rpg_mp", 1);//setweaponammoclip
                                return true;
                            });

                            return true;
                        }

                    }

                    return true;

                });
            }
            catch
            {
                print("★ 느린 봇 예외 발생");
            }
        }
        void StartRpgBotSearch_Allies(Entity bot, B_SET B)
        {
            try
            {
                bool pause = false;
                int death = B.death;

                OnInterval(SEARCH_TIME, () =>
                {
                    if (death != B.death) return false;
                    if (!HUMAN_CONNECTED_) return pause = true;

                    var target = B.target;
                    if (target != null)//이미 타겟을 찾은 경우
                    {
                        if (human_List.Contains(target))
                        {
                            //if (TEST_) return true;
                            var POD = target.Origin.DistanceTo(bot.Origin);
                            if (POD < FIRE_DIST) return !(pause = false);

                        }

                        B.target = null; //타겟과 거리가 멀어진 경우, 타겟 제거
                        B.fire = false;
                        bot.Call(33468, "rpg_mp", 0);//setweaponammoclip
                    }

                    pause = true;
                    //B.rooping = true;
                    //타겟 찾기 시작
                    foreach (Entity human in H_ALLIES_LIST)
                    {

                        var POD = human.Origin.DistanceTo(bot.Origin);

                        if (POD < FIRE_DIST)
                        {
                            B.target = human;
                            B.fire = true;
                            pause = false;

                           OnInterval(1500, () =>
                            {

                                if (pause || !B.fire) return false;

                                var ho = human.Origin; ho.Z -= 50;

                                Vector3 angle = Call<Vector3>(247, ho - bot.Origin);//vectortoangles
                                bot.Call(33531, angle);//SetPlayerAngles
                                bot.Call(33468, "rpg_mp", 1);//setweaponammoclip
                                return true;
                            });

                            return true;
                        }

                    }

                    return true;

                });
            }
            catch
            {
                print("★ 느린 봇 예외 발생");
            }
        }

        #endregion

    }
}
