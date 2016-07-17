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
        
        public Tdm()
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
                if (player.Name.StartsWith("bot"))
                {
                    if(!BOTs_List.Contains(player)) Call("kick", player.EntRef);//in case of a bot, he spawn PlayerConnectd and later PlayerConnecting. It differ from Human player's sequence
                }
            };

            string team = "axis";
            int botNum = 0;
            PlayerConnected += player =>
            {
                string name = player.Name;

                if (name.StartsWith("bot"))
                {
                    BOTs_List.Add(player);

                    if (team == "axis") team = "allies"; else team = "axis";
                    player.Notify("menuresponse", "team_marinesopfor", team);
                    player.AfterDelay(250, p =>
                    {
                        player.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[BOTs_List.Count - 1]);
                        
                        player.AfterDelay(250,pp=> Bot_Connected(player,++botNum));
                    });

                }
                else
                {
                    Human_Connected(player, name);
                }
                

            };

            PlayerDisconnected += player =>
            {
                human_List.Remove(player);// 봇 타겟리스트에서 접속 끊은 사람 제거

                if (Allies_List.Contains(player)) Allies_List.Remove(player);

                else if (Axis_List.Contains(player)) Axis_List.Remove(player);

                if (human_List.Count == 0)
                {
                    HUMAN_ZERO_ = true;
                    BotDoAttack(false);
                }
            };

            OnNotify("prematch_done", () => BotDeplay());

            OnNotify("game_ended", (level) =>
            {
                GAME_ENDED_ = true;
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
                Print("GAME_ENDED");

                if(BOT_HELI!=null) BOT_HELI.Call(32928);//delete ?? for freezing ??
                AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));//go to next map if server state is freezing
            });
            
            if (!TEST_) return;

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

