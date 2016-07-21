using System;
using InfinityScript;

/*

    Not yet fully tested enough
    and I'm still making this code.

    Until BOT_INFECTD work finish, I can't mod or edit BOT_TDM codes.

*/

namespace Tdm
{
    public partial class Tdm : BaseScript
    {
        internal static Gun GUN;
        internal static Predator PRDT;
        internal static Tank TK;
        internal static Helicopter HCT;

        Set SET;
        Perk PK;
        Hud HUD;
        Info INFO;

        Vehicle VHC;
        CarePackage CP;


        public Tdm()
        {
            SET = new Set();
            rnd = new Random();
            GUN = new Gun();
            PK = new Perk();
            HUD = new Hud();
            INFO = new Info();

            HCT = new Helicopter();
            TK = new Tank();
            VHC = new Vehicle();
            CP = new CarePackage();

            Call(42, "scr_game_playerwaittime", 2);//Before prematch, times for kicking bots when map rotate or restart, not fast_restart
            Call(42, "scr_game_matchstarttime", 5);//After prematch_done, times for starting match, in this priod, bot will be deployed
            Call(42, "testClients_watchKillcam", 0);
            Call(42, "testClients_doReload", 0);

            BotDoAttack(false);

            //Utilities.SetDropItemEnabled(false);

            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(null);
                H_FIELD.Add(null);
                KILLER_ENTREF.Add(i, -1);
            }

            PlayerConnecting += player =>
            {
                if (!player.Name.StartsWith("bot")) return;
                if (!BOTs_List.Contains(player)) Call(286, player.EntRef);//"kick" //in case of a bot, he spawn PlayerConnectd and later PlayerConnecting. It differ from Human player's sequence
            };

            PlayerConnected += player =>
            {
                string name = player.Name;

                if (name.StartsWith("bot"))
                {
                    BOTs_List.Add(player);

                    if (BOTs_List.Count == SET.BOT_SETTING_NUM)
                    {
                        int max = SET.BOT_SETTING_NUM - 1;
                        string team = "axis";

                        for (int i = 0; i < BOTs_List.Count; i++)
                        {
                            Entity bot = BOTs_List[i];

                            if (team=="axis") team = "allies"; else team = "axis";

                            Bot_Connected(bot, SET.BOTs_CLASS[i], team);

                            if (i == max) AfterDelay(1000, () => GetTeamState());
                        }

                    }
                }
                else
                {
                    Human_Connected(player, name);
                }


            };

            PlayerDisconnected += player =>
            {
                human_List.Remove(player);// 봇 타겟리스트에서 접속 끊은 사람 제거

                if (Allies_List.Contains(player)) Allies_List.Remove(player); else if (Axis_List.Contains(player)) Axis_List.Remove(player);

                int hlc = human_List.Count;

                if (hlc == 0) BotDoAttack(false);
                else if (hlc == 1) BotTeamBalanceEnable(false, false, null, hlc);
                else HumanTeamBalance();
            };

            OnNotify("prematch_done", () => BotDeplay());

            OnNotify("game_ended", (level) =>
            {
                GAME_ENDED_ = true;
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
                Print("GAME_ENDED");

                if (BOT_HELI != null) BOT_HELI.Call(32928);//delete ?? for freezing ??
                AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));//go to next map if server state is freezing
            });

            if (!TEST_) return;

            Print("테스트 모드");

#if DEBUG
            OnServerCommand("/", (string[] texts) =>
            {
                if (texts.Length == 1) return;
                string key = texts[1].ToLower();

                Print(key);

                switch (key)
                {
                    case "11":
                        {
                            Print(JUGG_BOT_ALLIES.Name + " " + JUGG_BOT_AXIS.Name);
                        }
                        break;
                    case "change1":
                        {
                            Entity bot = BOTs_List[0];

                            bot.Notify("menuresponse", "team_marinesopfor", texts[2]);/*important. match team and team class such as 'axis & axis_recipe', not 'axis & allies_recipe  */
                            bot.AfterDelay(100, b =>//important
                            {
                                bot.Notify("menuresponse", "changeclass", texts[3]);//important. If don't do change class, bot doesn't have a gun
                                AfterDelay(500, () => Print(bot.CurrentWeapon + " " + GetPlayerTeam(bot)));
                            });
                        }
                        break;
                    case "toaxis":
                        {
                            int fail_count = 0;

                            OnInterval(300, () =>
                            {
                                if (Axis_List.Count <= 4) return false;

                                if (fail_count == 18) return false;

                                Entity bot = GetBotByTeam(false);

                                BotChangeTeamToAxis(bot, true);
                                fail_count++;
                                return true;
                            });
                        }
                        break;
                    case "toallies":
                        {
                            int fail_count = 0;

                            OnInterval(300, () =>
                            {
                                if (Allies_List.Count <= 4) return false;

                                if (fail_count == 18) return false;

                                Entity bot = GetBotByTeam(true);

                                BotChangeTeamToAxis(bot, false);
                                fail_count++;
                                return true;
                            });
                        }
                        break;
                    case "balance":
                        {
                            Allies_List.Clear(); Axis_List.Clear();

                            bool toAxis = true;
                            int max = BOTs_List.Count - 1;
                            for (int i = 0; i < BOTs_List.Count; i++)
                            {
                                Entity bot = BOTs_List[i];
                                BotChangeTeamToAxis(bot, toAxis = !toAxis);
                            }
                        }
                        break;
                    case "state":
                        {
                            foreach (Entity ent in Players)
                            {
                                Print(ent.Name + " " + GetPlayerTeam(ent) + " " + ent.CurrentWeapon);
                            }
                        }
                        break;
                }


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

