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


        public Infected()
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

            Call(42, "scr_game_playerwaittime", 1);
            Call(42, "scr_game_matchstarttime", 1);
            Call(42, "testClients_watchKillcam", 0);
            Call(42, "testClients_doReload", 0);

            BotDoAttack(false);

            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(null);
                H_FIELD.Add(null);
            }

            PlayerConnecting += player =>
            {
                string name = player.Name;
                if (!name.StartsWith("bot")) return;
                if (player.GetField<string>("sessionteam") == "spectator")
                {
                    Call(286, player.EntRef);//kick

                    Utilities.AddTestClient();
                }
            };


            PlayerConnected += player =>
            {
                string name = player.Name;

                if (name.StartsWith("bot")) Bot_Connected(player);

                else Human_Connected(player, name);
            };

            PlayerDisconnected += player =>
            {
                human_List.Remove(player);// 봇 타겟리스트에서 접속 끊은 사람 제거

                if (Allies_List.Contains(player)) Allies_List.Remove(player); else if (Axis_List.Contains(player)) Axis_List.Remove(player);

                if (human_List.Count == 0) BotDoAttack(false);
            };

            OnNotify("prematch_done", () =>
            {
                //Call(42, "scr_infect_timelimit", "40");
                BotDeplay();
            });

            OnNotify("game_ended", (level) =>
            {
                GAME_ENDED_ = true;
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
                Print("GAME_ENDED");

                if (BotHeli.BOT_HELI != null) BotHeli.BOT_HELI.Call(32928);//delete ?? for freezing ??
                AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));//go to next map if server state is freezing
            });

            if (!TEST_) return;

            Print("테스트 모드");
          

            //OnServerCommand("/", (string[] texts) =>
            //{
            //    if (texts.Length == 1) return;
            //    string key = texts[1].ToLower();

            //    //Print("TYPED: "+key);
            //    switch (key)
            //    {
            //        case "w":
            //            {
            //                ADMIN.TakeWeapon(ADMIN.CurrentWeapon);
            //                ADMIN.GiveWeapon(texts[2]);
            //                ADMIN.SwitchToWeaponImmediate(texts[2]);
            //                AfterDelay(1000, () =>
            //                {
            //                    Print("RESULT: "+ADMIN.CurrentWeapon);
            //                });
            //            }
            //            break;
            //    }
            //});
        }
    }


}

