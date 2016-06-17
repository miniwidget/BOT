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
        #region Bots side

        /// <summary>
        /// BOT SET class for custom fields set
        /// </summary>
        class B_SET
        {
            public Entity target { get; set; }
            public int death { get; set; }
            public bool fire { get; set; }
            public bool temp_fire { get; set; }
            public string wep { get; set; }
        }
        List<B_SET> B_FIELD = new List<B_SET>(18);
        //Dictionary<int, int> BOT_ID = new Dictionary<int, int>();
        List<Entity> BOTs_List = new List<Entity>();
        #endregion

        #region 게임 시작 후, 봇 불러오기 

        void deplayBOTs()
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
                    if (BOTs_List.Count > max || OVERFLOW_BOT_ || Players.Count == 18)
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

    }
}
