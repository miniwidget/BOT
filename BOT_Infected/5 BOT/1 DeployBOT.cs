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
        void deplayBOTs()
        {
            try
            {

            
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
            catch(Exception ex)
            {
                writeErrorLoc(ref ex);
            }

        }

        #region Bot_Connected
        int RPG_BOT_ENTREF, RIOT_BOT_ENTREF, JUGG_BOT_ENTREF, LAST_ALLY_BOT_IDX;
        int BOT_CLASS_NUM = 3;

        private void Bot_Connected(ref Entity bot)
        {
#if DEBUG
            if (HEAVY_TEST_)
            {
                //HeavyBotConnected(bot);
                return;
            }
#endif
            var i = BOTs_List.Count;

            if (i > BOT_SETTING_NUM)
            {
                Call("kick", bot.EntRef);
                return;
            }

            if (i == 0) JUGG_BOT_ENTREF = bot.EntRef;
            else if (i == 1) RPG_BOT_ENTREF = bot.EntRef;
            else if (i == 2) RIOT_BOT_ENTREF = bot.EntRef;

            if (i == BOT_SETTING_NUM - 1) waitOnFirstInfected();

            if (i > 9)
            {
                if (BOT_CLASS_NUM > 9) BOT_CLASS_NUM = 3;
                i = BOT_CLASS_NUM;
                BOT_CLASS_NUM++;
            }
            bot.Notify("menuresponse", "changeclass", MT.BOTs_CLASS[i]);
            BOTs_List.Add(bot);
            FL[bot.EntRef].BOT = true;
        }
        #endregion

    }
}
