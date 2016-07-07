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
        void addMoreBot(int bc)
        {
            int num = 0;
            foreach(Entity bot in BOTs_List)
            {
                if (bot.GetField<string>("sessionteam") == "spectator")
                {
                    Call("kick", bot.EntRef);
                    num++;
                }
            }
            for (int i = 0; i < 18; i++)
            {
                Entity p = Entity.GetEntity(i);

                if (p == null)
                {
                    Players.Remove(p);
                    BOTs_List.Remove(p);
                    continue;
                }
                if (p.Name == "")
                {
                    Players.Remove(p);
                    BOTs_List.Remove(p);
                }
            }

            while(num != 0)
            {
                addBot();
                num--;
            }
        }
        void deplayBOTs()
        {
            int i = BOTs_List.Count;
            if (i < BOT_SETTING_NUM)
            {
                i = BOT_SETTING_NUM - i;
                int fail_count = 0, max = BOT_SETTING_NUM - 1;
                OnInterval(250, () =>
                {
                    i = BOTs_List.Count;
                    if (i > max || Players.Count == 18)
                    {
                        Print("총 불러온 봇 수는 " + i + " 입니다.");
                        addMoreBot(i);
                        return false;
                    }
                    if (fail_count > 20)
                    {
                        Print("봇 추가로 불러오기 실패");
                        return false;
                    }

                    Entity bot = addBot();
                    if (bot == null) fail_count++;

                    return true;
                });
            }
        }

        //string[] BOTs_CLASS = { "axis_recipe1", "axis_recipe2", "axis_recipe3", "axis_recipe4", "axis_recipe5", "allies_recipe1", "allies_recipe2", "allies_recipe3", "allies_recipe4", "allies_recipe5" };
        //string[] BOTs_CLASS = { "axis_recipe1", "axis_recipe2", "axis_recipe3", "class0", "class1", "class2", "class4", "class5", "class6", "class6" };

        string[] BOTs_CLASS = { "class0", "axis_recipe2", "class1", "class2", "class4", "class5", "class1", "class2", "class6", "class6" };
        int BOT_AXIS_IDX, BOT_ALLIES_IDX;
        string getClass
        {
            get
            {
                if (ADDING_BOT_COUNT % 2 ==0)
                {
                    BOT_AXIS_IDX++;
                    return BOTs_CLASS[BOT_AXIS_IDX - 1];
                }
                else
                {
                    BOT_ALLIES_IDX++;
                    return BOTs_CLASS[BOT_ALLIES_IDX - 1];
                }
            }
        }
        string getTeam
        {
            get
            {
                if (ADDING_BOT_COUNT % 2 == 0)
                {
                    return "axis";
                }
                else
                {
                    return "allies";
                }
            }
        }
        int ADDING_BOT_COUNT;
        Entity addBot()
        {
            Entity bot = Utilities.AddTestClient();
            if (bot == null)
            {
                Print("null");
                Players.Remove(bot);
                return Utilities.AddTestClient();
            }
            else
            {
                string team = getTeam;
                string cls = getClass;
                bot.OnNotify("joined_spectators", tc =>
                {
                    tc.Notify("menuresponse", "team_marinesopfor", team);
                });
                bot.OnNotify("joined_team", b =>
                {
                    b.Notify("menuresponse", "changeclass", cls);
                });

                bot.SpawnedPlayer += () => SpawnBot(bot);

                ADDING_BOT_COUNT++;

            }
            return bot;
        }

        private void Bot_Connected(Entity bot)
        {

            var i = BOTs_List.Count;

            if (i > BOT_SETTING_NUM)
            {
                Call("kick", bot.EntRef);
                return;
            }
            if (i == -1)
            {
                Print("■ ■ IMPORTANT Bot_Connected -1 " + bot.Name);
                Call("kick", bot.EntRef);
                return;
            }

            BOTs_List.Add(bot);

        }
    }
}
