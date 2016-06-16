using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Tdm
{
    public partial class Tdm : BaseScript
    {
        bool PREMATCH_DONE;
        public Tdm()
        {
            CheckTEST();

            #region Load Custom Setting from a set.txt file

            string setFile = GetPath("admin\\TDM.txt");

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

                            case "TEST_": if (!TEST_ && bool.TryParse(value, out b)) TEST_ = b; break;
                            case "DEPLAY_BOT_": if (bool.TryParse(value, out b)) DEPLAY_BOT_ = b; break;
                            case "USE_ADMIN_SAFE_": if (bool.TryParse(value, out b)) USE_ADMIN_SAFE_ = b; break;

                        }
                    }
                  
                    //if (TEST_) SERVER_NAME = "^2BOT ^7TEST";
                }
            }


            #endregion

            Server_SetDvar();

            PlayerConnecting += player =>
            {
                if (PREMATCH_DONE) return;
                string name = player.Name;
                if (name.StartsWith("bot"))
                {
                    Call("kick", player.EntRef);
                }
            };

            PlayerConnected += player =>
            {
                if (player.Name.StartsWith("bot"))
                {
                    if (!PREMATCH_DONE)
                    {
                        Call("kick", player.EntRef);
                        return;
                    }
                    if (BOTs_List.Count > BOT_SETTING_NUM) Call("kick", player.EntRef);
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

                Server_Hud();
                readMAP();

                PlayerDisconnected += Tdm_PlayerDisConnected;

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
                    AfterDelay(20000, () => Utilities.ExecuteCommand("map_rotate"));
                });
            });


        }

        bool TEST_;
        bool CheckTEST()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (assembly.Location.Contains("test"))
            {
                return TEST_ = true;
            }
            return TEST_ = false;
        }

    }
}

