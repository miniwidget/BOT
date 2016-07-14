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

        //AC130 ac130;
        Helicopter HCT;
        Predator PRDT;
        Tank TK;

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

            BotDoAttack(false);

            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(new B_SET());
                H_FIELD.Add(new H_SET());
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
                string name = player.Name;

                if (name.StartsWith("bot"))  Bot_Connected(player);
                
                else Human_Connected(player,name);
                
            };

            OnNotify("prematch_done", () =>
            {

                if (SET.DEPLAY_BOT_) BotDeplay();

                PlayerDisconnected += player =>
                {
                    if (human_List.Contains(player)) human_List.Remove(player);// 봇 타겟리스트에서 접속 끊은 사람 제거

                    else if (HumanAxis_LIST.Contains(player)) HumanAxis_LIST.Remove(player);
                    
                    if (human_List.Count == 0)
                    {
                        HUMAN_DIED_ALL_ = true;
                        BotDoAttack(false);
                    }
                };

                OnNotify("game_ended", (level) =>
                {
                    GAME_ENDED_ = true;
                    Call(42, "testClients_doMove", 0);
                    Call(42, "testClients_doAttack", 0);
                    Print("GAME_ENDED");
                });
            });

        }
    }


}

