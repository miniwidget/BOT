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
        Perk PK;
        Info INFO;
        Tank TK;
        Helicopter HCT;

        bool PREMATCH_DONE, TEST_;

        public Tdm()
        {
            #region Load Custom Setting from a set.txt file

            string setFile = "admin\\TDM.txt";

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
                            case "WELLCOME_MESSAGE": WELLCOME_MESSAGE = value; break;

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

            PK = new Perk();
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

            Server_SetDvar();

            PlayerConnecting += player =>
            {
                if (PREMATCH_DONE) return;

                if (player.Name.StartsWith("bot"))
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


    }
    class Inf
    {
        protected TReturn Call<TReturn>(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }
        protected TReturn Call<TReturn>(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }

        protected void Call(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        protected void Call(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        protected void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
    }
    class Common
    {
        internal static Vector3 ZERO = new Vector3();
        //internal static Vector3 AC130_WAY_POS;

        internal static void StartOrEndThermal(Entity player, bool start)
        {
            player.Call(33436, "", 0);//VisionSetNakedForPlayer
            if (start)
            {
                player.Call(32936);//thermalvisionfofoverlayon
                player.Health = 300;

                return;
            }
            player.Call(32937);//thermalvisionfofoverlayoff
            player.Health = 100;
            player.Call(33531, ZERO);

        }
        static Vector3 tempV = new Vector3();
        internal static Vector3 GetVector(float x, float y, float z)
        {
            tempV.X = x;
            tempV.Y = y;
            tempV.Z = z;
            return tempV;
        }

    }

}

