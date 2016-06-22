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
        void StartAllyBotSearch()
        {
            START_LAST_BOT_SEARCH = true;
            Entity bot = BOTs_List[LAST_ALLY_BOT_IDX];
            if (!isSurvivor(bot)) return;
            bot.Call(33220, 1.5f);

            Field F = FL[LAST_ALLY_BOT_IDX];
            F.target = null;

            string weapon = bot.CurrentWeapon;
            bot.Call(33468, weapon, 0);//setweaponammoclip
            bot.Call(33469, weapon, 0);//setweaponammostock

            bool pause = false;
            OnInterval(t1, () =>
            {
                var target = F.target;
                if (target != null)
                {
                    if (FL[target.EntRef].AXIS)
                    {
                        var POD = target.Origin.DistanceTo(bot.Origin);
                        if (POD < FIRE_DIST) return !(pause = false);
                    }

                    F.target = null;
                    bot.Call(33468, weapon, 0);//setweaponammoclip
                }
                pause = true;

                //타겟 찾기 시작
                var bo = bot.Origin;
                foreach (Entity human in HUMAN_AXIS_LIST)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        F.target = human;
                        pause = false;
                        bot.OnInterval(200, xx =>
                        {
                            if (pause || !F.damaged) return false;

                            var ho = human.Origin - z50 - bot.Origin;

                            Vector3 angle = Call<Vector3>(247, ho);//vectortoangles
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

        void AddSearchRoop(int entref)
        {
            Field F = FL[entref];
            Entity bot = F.player;

            string alertSound = null;
            int fire_time = FIRE_TIME;
            if (entref == JUGG_BOT_ENTREF)
            {
                alertSound = "AF_victory_music";
            }
            else if (entref == RPG_BOT_ENTREF)
            {
                alertSound = "missile_incoming"; fire_time = 1500;
            }
            else if(entref == RIOT_BOT_ENTREF)
            {
                return;
            }

            F.weapon = bot.CurrentWeapon;
            string weapon = F.weapon;

            Vector3 bo;
            OnInterval(SEARCH_TIME, () =>
            {
                if (F.wait) return true;/*죽었을 때*/

                bo = bot.Origin;

                var target = F.target;
                if (target != null)/*이미 타겟을 찾은 경우*/
                {
                    if (human_List.Contains(target))
                    {
                        var POD = target.Origin.DistanceTo(bo);
                        if (POD < FIRE_DIST)
                        {
                            return true;
                        }
                        else
                        {
                            if (F.damaged)
                            {
                                F.damaged = false;
                                return true;
                            }
                        }
                    }

                    F.target = null;
                    bot.Call(33468, weapon, 0);//setweaponammoclip
                }

                foreach (Entity human in human_List)
                {
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        F.target = human;
                        if (alertSound != null) human.Call(33466, alertSound);
                        return true;
                    }
                }
                return true;
            });

            OnInterval(fire_time, () =>
            {
                if (F.target == null) return true;

                var ho = F.target.Origin - z50 - bot.Origin;

                bot.Call(33531, Call<Vector3>(247, ho));//SetPlayerAngles
                bot.Call(33468, weapon, 5);//setweaponammoclip
                return true;
            });
        }
    }
}
