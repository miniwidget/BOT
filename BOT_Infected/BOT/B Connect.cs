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
            HCT.SetHeliPort();

            #region remove Bot
            List<int> tempStrList = null;
            int botCount = 0;
            foreach (Entity p in Players)
            {
                if (p == null || p.Name == "")
                {
                    continue;
                }
                else if (p.Name.StartsWith("bot"))
                {
                    var sessionteam = p.GetField<string>("sessionteam");
                    if (sessionteam != "spectator")
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
                    Call("kick", tempStrList[i]);
                }
                tempStrList.Clear();
            }
            #endregion

            #region deploy bot
            i = BOTs_List.Count;
            if (botCount < BOT_SETTING_NUM || i < BOT_SETTING_NUM)
            {
                i = BOT_SETTING_NUM - i;
                //print(i + "의 봇이 모자랍니다.");

                int fail_count = 0, max = BOT_SETTING_NUM - 1;
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
        int BOT_RPG_ENTREF,BOT_RIOT_ENTREF, BOT_JUGG_ENTREF;
        byte BOT_CLASS_NUM = 3;
        readonly string[] BOTs_CLASS = { "axis_recipe1", "axis_recipe2", "axis_recipe3", "class0", "class1", "class2", "class4", "class5", "class6", "class6" };

        private void Bot_Connected(Entity bot)
        {

            var i = BOTs_List.Count;

            if (i > BOT_SETTING_NUM)
            {
                Call(286, bot.EntRef);//kick
                return;
            }

            if (i == 0) BOT_JUGG_ENTREF = bot.EntRef;
            else if (i == 1) BOT_RPG_ENTREF = bot.EntRef;
            else if(i==2)BOT_RIOT_ENTREF = bot.EntRef;
                
            if (i == BOT_SETTING_NUM - 1) BotWaitOnFirstInfected();

            if (i > 9)
            {
                if (BOT_CLASS_NUM > 9) BOT_CLASS_NUM = 3;
                i = BOT_CLASS_NUM;
                BOT_CLASS_NUM++;
            }
            bot.Notify("menuresponse", "changeclass", BOTs_CLASS[i]);
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
                        TK.SetTank(BOTs_List[i]);
                        return GetTeamState(fi);
                    }

                    BotSet(BOTs_List[i], false);
                    i++;
                    return true;
                });

                return;
            }

            int fidx = BOTs_List.IndexOf(fi);
            int lucky_bot_idx = max;
            if (fidx == max) lucky_bot_idx -= 1;

            TK.SetTank(BOTs_List[lucky_bot_idx]);

            OnInterval(250, () =>
            {
                if (i == max)
                {
                    BotSet(fi, true);
                    return GetTeamState(fi);
                }
                if (i != lucky_bot_idx)
                {
                    BotSet(BOTs_List[i], false);
                }

                i++;
                return true;
            });
        }

        void BotSet(Entity bot, bool change)
        {
            bot.SpawnedPlayer += () => BotSpawned(bot);

            bot.Call(33341);//suicide
            if (change) SetTeamName();
        }

        bool GetTeamState(Entity fi)
        {
            int alive = 0, max = BOTs_List.Count;

            foreach (Entity bot in BOTs_List)
            {
                if (IsSurvivor(bot)) alive++;
            }

            Print("■ BOTs:" + max + " AXIS:" + (max - alive) + " ALLIES:" + alive + " Inf: " + fi.Name + " ■ MAP: "+ my.MAP_IDX);
            HUD.ServerHud();

            Call(42, "testClients_doReload", 0);//setdvar
            Call(42, "testClients_doCrouch", 0);
            Call(42, "testClients_doMove", 1);
            Call(42, "testClients_doAttack", 1);

            return false;
        }
    }
}
