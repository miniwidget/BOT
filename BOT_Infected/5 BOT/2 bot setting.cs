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


        void waitOnFirstInfected()
        {
            print("■ waitOnFirstInfected");

            int failCount = 0;
            OnInterval(t2, () =>
            {
                if (failCount == 6)//in case, if over 12 sec, in a state that no one got infected. ※ Infected time is 8 sec.
                {
                    Utilities.ExecuteCommand("map_restart");
                    return false;
                }

                foreach (Entity player in Players)
                {
                    if (player == null || !player.IsPlayer) continue;
                    if (player.GetField<string>("sessionteam") == "axis")//감염 시작
                    {
                        if (player.Name.StartsWith("bot"))//봇이 감염된 경우
                        {
                            BotToAxis(player, false);
                        }
                        else//사람이 감염된 경우
                        {
                            BotToAxis(player, true);
                        }

                        return false;
                    }

                }

                failCount++;
                return true;
            });

        }

        /// <summary>
        /// 봇이 처음 감염된 경우 & 사람이 아무도 접속하지 않은 경우 = 대기상태를 만들기 위해 봇 1 마리를 살려 놓음
        /// 사람이 접속했을 지라도, 팀변경을 위해 봇 1마리를 살려 놓음
        /// </summary>
        void BotToAxis(Entity fi, bool human_infected)
        {
            var max = BOTs_List.Count - 1;
            int i = 0;
            if (human_infected)
            {
                OnInterval(250, () =>
                {
                    if (i == max)
                    {
                        return getTeamState(ref fi);
                    }

                    BotSet(BOTs_List[i], i, false);
                    i++;
                    return true;
                });

                return;
            }

            int fidx = BOTs_List.IndexOf(fi);
            LAST_ALLY_BOT_IDX = max;
            if (fidx == max) LAST_ALLY_BOT_IDX -= 1;

            OnInterval(250, () =>
            {
                if (i == max)
                {
                    BotSet(fi, fidx, true);
                    return getTeamState(ref fi);
                }
                if (i != LAST_ALLY_BOT_IDX)
                {
                    BotSet(BOTs_List[i], i, false);
                }

                i++;
                return true;
            });
#if DEBUG
            //AfterDelay(10000, () =>
            //{
            //    Players[LAST_ALLY_BOT_IDX].Call(33341);
            //});
#endif

        }

        void BotSet(Entity bot, int i, bool change)
        {
            var entref = bot.EntRef;
            Field F = FL[entref];
            F.player = bot;

            switch (i)
            {
                case 0: bot.SpawnedPlayer += ()=> PerkWait(F, BOT_DELAY_TIME); break;//Jugg
                case 1: bot.SpawnedPlayer += ()=> PerkWait(F, 10000); break;//Rpg
                case 2: bot.SpawnedPlayer += ()=> PerkWait(F, 10000); break;//Riot
                default: bot.SpawnedPlayer += ()=>PerkWait(F, BOT_DELAY_TIME); break;//Normal
            }

            if (i != LAST_ALLY_BOT_IDX ) AddSearchRoop(entref);

            if (!SUICIDE_BOT_) return;

            bot.Call(33341);//suicide
            if (change) setTeamName();
        }

        bool getTeamState(ref Entity fi)
        {
            int alive = 0, max = BOTs_List.Count;

            foreach (Entity bot in BOTs_List)
            {
                if (isSurvivor(bot)) alive++;
            }
            print("■ BOTs:" + max + " AXIS:" + (max - alive) + " ALLIES:" + alive + " Inf: " + fi.Name);

            Call(42, "testClients_doCrouch", 0);//setdvar
            Call(42, "testClients_doMove", 1);
            Call(42, "testClients_doAttack", 1);

            HudServer(); /* Server HUD */
            return false;
        }

    }
}
