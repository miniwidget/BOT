using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Infected
{
    public partial class Infected : BaseScript
    {
        internal static Weapon WP;
        internal static Predator PRDT;
        internal static Tank TK;
        internal static Helicopter HCT;

        Set SET;
        Perk PK;
        Hud HUD;
        Info INFO;
        
        Vehicle VHC;
        CarePackage CP;
        public Infected()
        {
            SET = new Set();
            rnd = new Random();
            WP = new Weapon();
            PK = new Perk();
            HUD = new Hud();
            INFO = new Info();

            HCT = new Helicopter();
            TK = new Tank();
            VHC = new Vehicle();
            CP = new CarePackage();

            Call(42, "scr_game_playerwaittime", 1);
            Call(42, "scr_game_matchstarttime", 1);
            Call(42, "testClients_watchKillcam", 0);
            Call(42, "testClients_doReload", 0);

            BotDoAttack(false);

            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(null);
                H_FIELD.Add(null);
                IsBOT[i] = null;
            }

            PlayerConnecting += player =>
            {
                string name = player.Name;
                if (name.StartsWith("bot"))
                {
                    string state = player.GetField<string>("sessionteam");
                    if (state == "spectator")
                    {
                        Call(286, player.EntRef);//kick
                        Utilities.AddTestClient();
                    }
                }
            };

            PlayerConnected += player =>
            {
                string name = player.Name;

                if (name.StartsWith("bot")) Bot_Connected(player);

                else Human_Connected(player, name);

            };

            OnNotify("prematch_done", () =>
            {

                if (SET.DEPLAY_BOT_) BotDeplay();

                PlayerDisconnected += player =>
                {
                    if (human_List.Contains(player)) human_List.Remove(player);// 봇 타겟리스트에서 접속 끊은 사람 제거

                    else if (HumanAxis_List.Contains(player)) HumanAxis_List.Remove(player);

                    if (human_List.Count == 0)
                    {
                        HUMAN_DIED_ALL_ = true;
                        BotDoAttack(false);
                    }
                };
              
            });

            OnNotify("game_ended", (level) =>
            {
                GAME_ENDED_ = true;
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
                Print("GAME_ENDED");

                BotHeli.BOT_HELI.Call(32928);//delete ?? for freezing ??
                AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));//go to next map if server state is freezing
            });

            if (!SET.TEST_) return;

            Print("테스트 모드");
#if DEBUG
            OnServerCommand("/", (string[] texts) =>
            {
                if (texts.Length == 1) return;
                string key = texts[1].ToLower();

                if (key == "team")
                {
                    Print(Players[int.Parse(texts[2])].GetField<string>("sessionteam"));
                }
                else if(key == "mb")
                {
                    ADMIN.Notify("menuresponse", "class", "back");
                }
                else if (key == "name")
                {
                    Print(Players[int.Parse(texts[2])].Name);
                }
                else if (key == "bot")
                {
                    Entity bot = Utilities.AddTestClient();
                }
                else if (key == "heavy_test")
                {
                    Entity me = human_List[human_List.IndexOf(ADMIN)];
                    human_List.RemoveAt(0);
                    int i = 0;
                    while (Players.Count != 18)
                    {
                        if (i > 20) break;
                        i++;
                        Entity bot = Utilities.AddTestClient();
                        if (bot == null) continue;
                        human_List.Add(bot);
                       AfterDelay(250,()=> bot.Health = -1);
                    }

                    human_List.Add(me);
                }
                else if (key == "rm")
                {
                    Entity testHuman = human_List[rnd.Next(human_List.Count)];
                    int entref = testHuman.EntRef;
                    Call("kick", entref);

                    AfterDelay(500, () =>
                    {
                        if (testHuman == null) Print("testHuman 눌");
                        else Print(testHuman.Name + "^__^");
                    });


                }
                else if (key == "status")
                {
                    string s = null;
                    foreach (Entity p in Players)
                    {
                        if (p == null)
                        {
                            Print("STATUS 눌");
                            continue;
                        }

                        string sessionteam = p.GetField<string>("sessionteam").Substring(0, 2);
                        string name = p.Name;

                        if (sessionteam == "no")
                        {
                            s += " NONE";
                        }
                        else if (name.StartsWith("bot"))
                        {
                            if (human_List.Contains(p)) s += " ◆" + name + "(" + p.EntRef + ")" + sessionteam;
                            else s += " ◎" + name + "(" + p.EntRef + ")" + sessionteam;
                        }
                        else s += " ◐" + p.EntRef + sessionteam;
                    }
                    Print(s + "\n총:" + Players.Count + "명");
                }
            });
#endif
        }
    }


}

