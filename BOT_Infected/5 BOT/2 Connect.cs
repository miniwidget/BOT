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

        #region Bot_Connected
        int RPG_BOT_ENTREF,RIOT_BOT_ENTREF, JUGG_BOT_ENTREF;

        int BOT_CLASS_NUM=3;
        private void Bot_Connected(Entity bot)
        {

            var i = BOTs_List.Count;

            if (i > BOT_SETTING_NUM)
            {
                OVERFLOW_BOT_ = true;
                Call("kick", bot.EntRef);
                return;
            }
            if (i == -1)
            {
                print("■ ■ IMPORTANT Bot_Connected -1 " + bot.Name);
                Call("kick", bot.EntRef);
                return;
            }

            if (i == 0) JUGG_BOT_ENTREF = bot.EntRef;
            else if (i == 1) RPG_BOT_ENTREF = bot.EntRef;
            else if(i==2)RIOT_BOT_ENTREF = bot.EntRef;
                
            //print(i + "//" + (BOT_SETTING_NUM - 1));
            if (i == BOT_SETTING_NUM - 1) waitOnFirstInfected();

            if (i > 9)
            {
                if (BOT_CLASS_NUM > 9) BOT_CLASS_NUM = 3;
                i = BOT_CLASS_NUM;
                BOT_CLASS_NUM++;
            }
            bot.Notify("menuresponse", "changeclass", BOTs_CLASS[i]);
            BOTs_List.Add(bot);

        }
        #endregion


        void waitOnFirstInfected()
        {
            print("■ waitOnFirstInfected");

            //string message = "If 1 user is on game and got infected, ^2RESTART  in 5 seconds";
            int failCount = 0;
            OnInterval(t2, () =>
            {
                if (failCount == 6)//in case, if over 12 sec, in a state that no one got infected. ※ Infected time is 8 sec.
                {
                    FIRST_INF_ENTREF = BOTs_List[0].EntRef;

                    if (HUMAN_CONNECTED_)//사람이 감염된 경우
                    {
                         BotToAxisExceptLast();
                    }
                    else
                    {
                        BotToAxisExceptOne();
                    }

                    Server_Hud();

                    return false;
                }

                foreach (Entity player in Players)
                {
                    if (player == null || !player.IsPlayer) continue;
                    if (player.GetField<string>("sessionteam") == "axis")//감염 시작
                    {
                        FIRST_INF_ENTREF = player.EntRef;

                        if (player.Name.StartsWith("bot"))//봇이 감염된 경우
                        {
                            BotToAxisExceptOne();
                        }
                        else//사람이 감염된 경우
                        {
                             BotToAxisExceptLast();
                        }

                        Server_Hud();

                        return false;
                    }

                }

                failCount++;
                return true;
            });

        }
        /// <summary>
        /// 봇이 처음 감염된 경우
        /// </summary>
        /// 
        int LAST_ALLY_BOT_IDX;
        void BotToAxisExceptOne()
        {

            var max = BOTs_List.Count - 1;
            int i = 0;
            int fidx = BOTs_List.IndexOf(Players[FIRST_INF_ENTREF]);
            LAST_ALLY_BOT_IDX = max;

            if (fidx == max) LAST_ALLY_BOT_IDX -= 1;

            OnInterval(250, () =>
            {
                if (i == max)
                {
                    
                    changeBotClass(Players[FIRST_INF_ENTREF], fidx, true);
                    return getTeamState();
                }
                if (i!= LAST_ALLY_BOT_IDX)
                {
                    changeBotClass(BOTs_List[i], i, false);
                }else
                {
                   
                }

                i++;
                return true;
            });
        }

        /// <summary>
        /// 사람이 처음으로 감염된 경우
        /// </summary>
        void BotToAxisExceptLast()
        {
            var max = BOTs_List.Count - 1;

            int i = 0;
            OnInterval(250, () =>
            {
                if (i == max)
                {
                    LAST_BOT_SEARCH_ON = true;
                    return getTeamState();
                }

                changeBotClass(BOTs_List[i], i, false);
                i++;
                return true;
            });
        }


        /// <summary>
        /// 봇이 처음 감염된 경우 & 사람이 아무도 접속하지 않은 경우 = 대기상태를 만들기 위해 봇 1 마리를 살려 놓음
        /// </summary>
        void changeBotClass(Entity bot, int i, bool change)
        {

            if (i == 1 || i == 2)//1=RPG BOT 2=RIOT
            {
                bot.SpawnedPlayer += () => spawned_bot_slower(bot);
            }
            else
            {
                bot.SpawnedPlayer += () => spawned_bot(bot);
            }

            if (!SUICIDE_BOT_) return;

            bot.Call("suicide");
            if (change) setTeamName();
        }

        bool getTeamState()
        {
            int alive = 0, max = BOTs_List.Count;
            //string test = null;
            foreach (Entity bot in BOTs_List)
            {
                if (isSurvivor(bot)) alive++;

                //test += " " + bot.EntRef ;
            }
            print("■ BOTs:" + max + " AXIS:" + (max - alive) + " ALLIES:" + alive + " Inf: " + Players[FIRST_INF_ENTREF].Name );
            //print("■ HUMANs: " +human_List.Count);

            if (HELI_MAP && human_List.Count>0)
            {
                //SetupHelicopter(human_List[0]);
            }
            return false;
        }

    }
}
