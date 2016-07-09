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
        Set SET;
        Weapon WP;
        Perk PK;
        Hud HUD;
        Info INFO;

        Helicopter HCT;
        Tank TK;
        //AC130 ac130;

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

            Call(42, "scr_game_playerwaittime", 1); 
            Call(42, "scr_game_matchstarttime", 1);
            Call(42, "testClients_watchKillcam", 0);
            Call(42, "testClients_doReload", 0);
            Call(42, "testClients_doCrouch", 1);
            Call(42, "testClients_doMove", 0);
            Call(42, "testClients_doAttack", 0);
      
            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(new B_SET());
                H_FIELD.Add(new H_SET(0));
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
                        Entity b = Utilities.AddTestClient();
                    }
                }
            };

            PlayerConnected += player =>
            {
                if (player.Name.StartsWith("bot"))
                {
                    Bot_Connected(player);
                }
                else
                {
                    Human_Connected(player);
                }
            };

            OnNotify("prematch_done", () =>
            {

                if (SET.DEPLAY_BOT_) BotDeplay();

                PlayerDisconnected += player =>
                {
                    if (human_List.Contains(player))// 봇 타겟리스트에서 접속 끊은 사람 제거
                    {
                        human_List.Remove(player);
                    }
                    if (human_List.Count == 0)
                    {
                        HUMAN_DIED_ALL_ = true;
                        BotDoAttack(false);
                    }
                };

                OnNotify("game_ended", (level) =>
                {
                    Call(42, "testClients_doMove", 0);
                    Call(42, "testClients_doAttack", 0);

                    GAME_ENDED_ = true;

                    foreach (var v in B_FIELD)
                    {
                        if (v == null) continue;
                        v.death += 1;
                        v.fire = false;
                    }
                    //AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));
                });
            });

        }
    }


}

