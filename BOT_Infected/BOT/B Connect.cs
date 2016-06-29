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
        #region deplay
        void BotDeplay()
        {
            #region remove Bot
            List<int> tempStrList = null;
            int botCount = 0;
            foreach (Entity p in Players)
            {
                if (p == null || p.Name == "")   continue;
                
                if (p.Name.StartsWith("bot"))
                {
                    if (p.GetField<string>("sessionteam") != "spectator")
                    {
                        botCount++;
                    }
                    else
                    {
                        if (tempStrList == null) tempStrList = new List<int>();
                        tempStrList.Add(p.EntRef);
                    }
                }
            }
            int i = 0;
            if (tempStrList != null)
            {
                for (i = 0; i < tempStrList.Count; i++)
                {
                    Call(286, tempStrList[i]);//"kick"
                }
                tempStrList.Clear();
            }
            #endregion

            #region deploy bot
            i = BOTs_List.Count;
            if (botCount < SET.BOT_SETTING_NUM || i < SET.BOT_SETTING_NUM)
            {
                i = SET.BOT_SETTING_NUM - i;
                //print(i + "의 봇이 모자랍니다.");

                int fail_count = 0, max = SET.BOT_SETTING_NUM - 1;
                OnInterval(250, () =>
                {
                    if (BOTs_List.Count > max || Players.Count == 18)
                    {
                        //print("총 불러온 봇 수는 " + getBOTCount + " 입니다.");
                        return false;
                    }
                    if (fail_count > 20)
                    {
                        //print("봇 추가로 불러오기 실패");
                        return false;
                    }

                    Entity bot = Utilities.AddTestClient();
                    if (bot == null) fail_count++;

                    return true;
                });
            }
            #endregion

        }
        #endregion

        #region Bot_Connected

        int BOT_RPG_ENTREF, BOT_RIOT_ENTREF, BOT_JUGG_ENTREF;

        private void Bot_Connected(Entity bot)
        {

            var i = BOTs_List.Count;

            if (i > SET.BOT_SETTING_NUM)
            {
                Call(286, bot.EntRef);//kick
                return;
            }

            if (i == 0) BOT_JUGG_ENTREF = bot.EntRef;
            else if (i == 1) BOT_RPG_ENTREF = bot.EntRef;
            else if (i == 2) BOT_RIOT_ENTREF = bot.EntRef;

            if (i == SET.BOT_SETTING_NUM - 1) BotWaitOnFirstInfected();

            if (i > 9)
            {
                if (SET.BOT_CLASS_NUM > 9) SET.BOT_CLASS_NUM = 3;
                i = SET.BOT_CLASS_NUM;
                SET.BOT_CLASS_NUM++;
            }
            bot.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[i]);
            BOTs_List.Add(bot);
            IsBOT[bot.EntRef] = true;

        }
        #endregion

        void BotWaitOnFirstInfected()
        {
            //my.print("■ waitOnFirstInfected");

            int failCount = 0;
            OnInterval(2000, () =>
            {
                if (failCount == 6)//in case, if over 12 sec, in a state that no one got infected. ※ Infected time is 8 sec.
                {
                    Utilities.ExecuteCommand("map_restart");
                    return false;
                }

                foreach (Entity player in Players)
                {
                    if (player == null) continue;
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
            SET.LUCKY_BOT_IDX = max;
           
            if (!human_infected)
            {
                if (BOTs_List.IndexOf(fi) == max) SET.LUCKY_BOT_IDX -= 1;
            }

            int i = 0;
            OnInterval(250, () =>
            {
                Entity bot;

                if (i == max)
                {
                    if (!human_infected)
                    {
                        fi.SpawnedPlayer += () => BotSpawned(fi);
                        fi.Call(33341);//suicide
                        SetTeamName();
                    }
                    
                    return GetTeamState(fi.Name);
                }
                if (i != SET.LUCKY_BOT_IDX)
                {
                    bot = BOTs_List[i];
                    bot.SpawnedPlayer += () => BotSpawned(bot);
                    bot.Call(33341);//suicide
                }

                i++;
                return true;
            });
        }

        bool GetTeamState(string first_inf_name)
        {
            int alive = 0, max = BOTs_List.Count;

            foreach (Entity bot in BOTs_List)
            {
                if (bot.GetField<string>("sessionteam") == "allies") alive++;
            }

            Log.Write(LogLevel.None, "■ BOTs:{0} AXIS:{1} ALLIES:{2} INF:{3} ■ MAP:{4}", max, (max - alive), alive, first_inf_name, SET.MAP_IDX);

            HUD.ServerHud();
            HCT.SetHeliPort();
            TK.SetTank(BOTs_List[SET.LUCKY_BOT_IDX]);
            BotDoAttack(true);

            return false;
        }
        DateTime GRACE_TIME;
        void BotDoAttack(bool attack)
        {
            if (attack)
            {
                Call(42, "testClients_doCrouch", 0);

                Call(42, "testClients_doMove", 1);

                Call(42, "testClients_doAttack", 1);

                Call(42, "scr_infect_timelimit", "12");
                GRACE_TIME = DateTime.Now.AddSeconds(166);
            }
            else
            {
                Call(42, "testClients_doCrouch", 1);
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
            }
        }
    }
}
