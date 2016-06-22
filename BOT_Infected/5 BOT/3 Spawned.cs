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
        void AddSearchRoop(int entref)
        {
            Field F = FL[entref];
            Entity bot = F.player;
            int htidx = -1;

            string alertSound = null;
            int fire_time = FIRE_TIME;
            int bullet = 5;
            if (entref == JUGG_BOT_ENTREF)
            {
                alertSound = "AF_victory_music";
            }
            else if (entref == RPG_BOT_ENTREF)
            {
                alertSound = "missile_incoming"; fire_time = 1500;
                bullet = 2;
            }
            else if (entref == RIOT_BOT_ENTREF)
            {
                return;
            }

            F.weapon = bot.CurrentWeapon;
            string weapon = F.weapon;
            Entity target = null;

            OnInterval(SEARCH_TIME, () =>
            {
                if (F.wait) return true;/*죽었을 때*/

                Vector3 bo = bot.Origin;

                htidx = F.human_target_idx;
                if (htidx != -1)/*이미 타겟을 찾은 경우*/
                {
                    if (htidx < HUMAN_LIST.Count)
                    {

                        var POD = HUMAN_LIST[htidx].Origin.DistanceTo(bo);
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

                    F.human_target_idx = -1;
                    bot.Call(33468, weapon, 0);//setweaponammoclip
                }

                for (int i = 0; i < HUMAN_LIST.Count; i++)
                {
                    Entity human = HUMAN_LIST[i];
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        F.human_target_idx = i;
                        if (alertSound != null) human.Call(33466, alertSound);
                        return true;
                    }
                }
                return true;
            });

            OnInterval(fire_time, () =>
            {
                var idx = F.human_target_idx;
                if (idx == -1 || idx >= HUMAN_LIST.Count) return true;
                if (target == null) return true;

                var ho = HUMAN_LIST[idx].Origin - z50 - bot.Origin;
                
                bot.Call(33531, Call<Vector3>(247, ho));//SetPlayerAngles
                bot.Call(33468, weapon, bullet);//setweaponammoclip
                return true;
            });
        }

        void StartAllyBotSearch()
        {

            START_LAST_BOT_SEARCH = true;

            Entity bot = BOTs_List[LAST_ALLY_BOT_IDX];
            if (!isSurvivor(bot)) return;
            bot.Call(33220, 1.5f);

            Field F = FL[LAST_ALLY_BOT_IDX];
            F.human_target_idx = -1;

            string weapon = bot.CurrentWeapon;
            bot.Call(33468, weapon, 0);//setweaponammoclip
            bot.Call(33469, weapon, 0);//setweaponammostock

            Vector3 bo;
            OnInterval(SEARCH_TIME, () =>
            {
                if (F.wait) return true;/*죽었을 때*/

                bo = bot.Origin;

                var tNum = F.human_target_idx;
                if (tNum != -1)/*이미 타겟을 찾은 경우*/
                {
                    if (tNum < HUMAN_AXIS_LIST.Count)
                    {
                        var POD = HUMAN_AXIS_LIST[tNum].Origin.DistanceTo(bo);
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

                    F.human_target_idx = -1;
                    bot.Call(33468, weapon, 0);//setweaponammoclip
                }
                for (int i = 0; i < HUMAN_AXIS_LIST.Count; i++)
                {
                    Entity human = HUMAN_AXIS_LIST[i];
                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        F.human_target_idx = i;
                        return true;
                    }
                }

                return true;
            });

            OnInterval(200, () =>
            {
                var idx = F.human_target_idx;
                if (idx == -1|| idx >= HUMAN_LIST.Count) return true;
                var ho = HUMAN_AXIS_LIST[idx].Origin - z50 - bot.Origin;

                bot.Call(33531, Call<Vector3>(247, ho));//SetPlayerAngles
                bot.Call(33468, weapon, 5);//setweaponammoclip
                return true;
            });

        }


    }
}
