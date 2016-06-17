﻿using System;
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

        public Infected()
        {
            CheckTEST();

            #region Load Custom Setting from a set.txt file

            string setFile = GetPath("admin\\INF.txt");

            int i;

            if (File.Exists(setFile))
            {
                using (StreamReader set = new StreamReader(setFile))
                {
                    bool b;
                    float f;

                    while (!set.EndOfStream)
                    {
                        string line = set.ReadLine();
                        if (line.StartsWith("//") || line.Equals(string.Empty)) continue;

                        string[] split = line.Split('=');
                        if (split.Length < 1) continue;

                        string name = split[0];
                        string value = split[1];
                        var comment = value.IndexOf("//");
                        if (comment != -1) value = value.Substring(0, comment);

                        switch (name)
                        {
                            case "SERVER_NAME": SERVER_NAME = value; break;
                            case "ADMIN_NAME": ADMIN_NAME = value; break;
                            case "TEAMNAME_ALLIES": TEAMNAME_ALLIES = value; break;
                            case "TEAMNAME_AXIS": TEAMNAME_AXIS = value; break;
                            case "WELLCOME_MESSAGE": WELLCOME_MESSAGE = value; break;

                            case "INFECTED_TIMELIMIT": if (float.TryParse(value, out f)) INFECTED_TIMELIMIT = f; break;
                            case "PLAYERWAIT_TIME": if (float.TryParse(value, out f)) PLAYERWAIT_TIME = f; break;
                            case "MATCHSTART_TIME": if (float.TryParse(value, out f)) MATCHSTART_TIME = f; break;
                            case "SEARCH_TIME": if (int.TryParse(value, out i)) SEARCH_TIME = i; break;
                            case "FIRE_TIME": if (int.TryParse(value, out i)) FIRE_TIME = i; break;
                            case "BOT_DELAY_TIME": if (int.TryParse(value, out i)) BOT_DELAY_TIME = i; break;
                            case "BOT_SETTING_NUM": if (int.TryParse(value, out i)) BOT_SETTING_NUM = i; break;
                            case "PLAYER_LIFE": if (int.TryParse(value, out i)) PLAYER_LIFE = i; break;

                            case "TEST_": if (!TEST_ && bool.TryParse(value, out b)) TEST_ = b; break;
                            case "DEPLAY_BOT_": if (bool.TryParse(value, out b)) DEPLAY_BOT_ = b; break;
                            case "USE_ADMIN_SAFE_": if (bool.TryParse(value, out b)) USE_ADMIN_SAFE_ = b; break;
                            case "SUICIDE_BOT_": if (bool.TryParse(value, out b)) SUICIDE_BOT_ = b; break;
                            case "Disable_Melee_": if (bool.TryParse(value, out b)) Disable_Melee_ = b; break;

                        }
                    }

                    //if (TEST_) SERVER_NAME = "^2BOT ^7TEST";
                }
            }


            #endregion

            ServerSetDvar();

            PlayerConnecting += player =>
            {
                string name = player.Name;
                if (name.StartsWith("bot"))
                {
                    string state = player.GetField<string>("sessionteam");
                    if (state == "spectator")
                    {
                        Call("kick", player.EntRef);
                        if (DEPLAY_BOT_)
                        {
                            Entity b = Utilities.AddTestClient();
                        }
                    }
                }
            };

            PlayerConnected += player =>
            {
                if (player.Name.StartsWith("bot"))
                {
                    if (getBOTCount > BOT_SETTING_NUM) Call("kick", player.EntRef);
                    else Bot_Connected(player);
                }
                else
                {
                    Human_Connected(player);
                }
            };

            OnNotify("prematch_done", () =>
            {
                PREMATCH_DONE = true;
                if (DEPLAY_BOT_) deplayBOTs();

                PlayerDisconnected += Inf_PlayerDisConnected;

                readMAP();

                OnNotify("game_ended", (level) =>
                {
                    GAME_ENDED_ = true;
                    SERVER_HUD.Call(32897);
                    foreach (var v in B_FIELD)
                    {
                        v.fire = false;
                        v.target = null;
                        v.death += 1;
                    }
                    foreach (Entity bot in BOTs_List)
                    {
                        bot.Call("setmovespeedscale", 0f);
                    }
                    AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));
                });
            });
        }

        void ServerSetDvar()
        {
            Call("setdvar", "scr_game_playerwaittime", PLAYERWAIT_TIME);
            Call("setdvar", "scr_game_matchstarttime", MATCHSTART_TIME);
            Call("setdvar", "scr_game_allowkillcam", "0");
            Call("setdvar", "scr_infect_timelimit", INFECTED_TIMELIMIT);

            Call("setdvar", "testClients_watchKillcam", 0);
            Call("setdvar", "testClients_doReload", 0);
            Call("setdvar", "testClients_doCrouch", 0);
            Call("setdvar", "testClients_doMove", 1);
            Call("setdvar", "testClients_doAttack", 1);

            Utilities.ExecuteCommand("sv_hostname " + SERVER_NAME);
            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(new B_SET());
                H_FIELD.Add(new H_SET());
            }

        }

    }
}

