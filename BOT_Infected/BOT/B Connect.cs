using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Timers;

namespace Infected
{
    public partial class Infected
    {

        #region deplay
        void BotDeplay()
        {
            if (!SET.DEPLAY_BOT_) return;

            #region remove Bot
            List<int> tempStrList = null;
            int botCount = 0;
            foreach (Entity p in Players)
            {
                if (p == null || p.Name == "") continue;

                if (p.Name.StartsWith("bot"))
                {
                    if (p.GetField<string>("sessionteam") != "spectator")
                    {
                        botCount++;
                    }
                    else
                    {
                        if (tempStrList == null) tempStrList = new List<int>();
                        tempStrList.Add(p.EntRef);
                    }
                }
            }
            int i = 0;
            if (tempStrList != null)
            {
                for (i = 0; i < tempStrList.Count; i++)
                {
                    Call(286, tempStrList[i]);//"kick"
                }
                tempStrList.Clear();
            }
            #endregion

            #region deploy bot
            i = BOTs_List.Count;
            if (botCount < SET.BOT_SETTING_NUM || i < SET.BOT_SETTING_NUM)
            {
                i = SET.BOT_SETTING_NUM - i;

                int fail_count = 0, max = SET.BOT_SETTING_NUM - 1;
                OnInterval(250, () =>
                {
                    if (BOTs_List.Count > max || Players.Count == 18)
                    {
                        return false;
                    }
                    if (fail_count > 20)
                    {
                        return false;
                    }

                    Entity bot = Utilities.AddTestClient();
                    if (bot == null) fail_count++;

                    return true;
                });
            }
            #endregion

        }
        #endregion

        #region Bot_Connected

        private void Bot_Connected(Entity bot)
        {
            if (BOT_TO_AXIS_COMP)//If deploy bot after game in the middle time, deny bot setting
            {
                //bot.Call(33341);//"suicide"
                return;
            }

            var i = BOTs_List.Count;
            int be = bot.EntRef;
            if (be == -1 || i > SET.BOT_SETTING_NUM)
            {
                Call(286, be);//kick
                return;
            }

            bool block = false;

            if (i == 2 || i == 3)
            {
                block = true;
                bot.SpawnedPlayer += () => bot.Call(33220, 2f);
            }
            else if (i == 4)
            {
                block = true;
                BOT_HELIRIDER_IDX = 4;
            }

            if (i > 12)
            {
                if (SET.BOT_CLASS_NUM > 12) SET.BOT_CLASS_NUM = 5;
                i = SET.BOT_CLASS_NUM;
                SET.BOT_CLASS_NUM++;
            }
            bot.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[i]);

            BOTs_List.Add(bot);
            if (block) return;

            B_FIELD[be] = new B_SET(bot);
            IsBOT[be] = "1";

            B_SET B = B_FIELD[be];

            if (i == 0) B.alertSound = "AF_victory_music";
            if (i == 1) B.alertSound = "missile_incoming";

            bot.AfterDelay(250, b =>
            {
                B.weapon = bot.CurrentWeapon;
                bot.Call(33523, B.weapon);//giveMaxaAmmo
                B.ammoClip = bot.Call<int>(33460, B.weapon);//getcurrentweaponclipammo
                bot.Call(33469, B.weapon, 0);//setweaponammostock
                bot.Call(33468, B.weapon, 0);//setweaponammoclip
            });

            if (i == SET.BOT_SETTING_NUM - 1) BotWaitOnFirstInfected();

            bot.SpawnedPlayer += delegate
            {
                if (GAME_ENDED_) return;
                bot.Call(33469, B.weapon, 0);//setweaponammostock
                bot.Call(33468, B.weapon, 0);//setweaponammoclip

                int k = B.killer;
                if (k != -1)
                {
                    BotCheckPerk(k);
                    B.killer = -1;
                }

                B.wait = false;
            };

            if (B.alertSound == "missile_incoming")
            {
                bot.OnNotify("weapon_fired", (p, wp) =>
                {
                    B.SetAngle();
                });
                return;
            }

            byte fire = 1;
            bot.OnNotify("weapon_fired", (p, wp) =>
            {
                if (fire == 1) fire = 2;
                else
                {
                    if (fire == 3) fire = 0;
                    fire++;
                    return;
                }
                B.SetAngle();

            });
        }

        #endregion

        void BotWaitOnFirstInfected()
        {

            OnInterval(1000, () =>
            {
                foreach (Entity bot in BOTs_List)
                {
                    if (bot.GetField<string>("sessionteam") != "axis") continue;

                    //if (!BOT_TO_AXIS_COMP) Utilities.ExecuteCommand("fast_restart");//Check first human infected to Allise
                    BOT_TO_AXIS_COMP = true;

                    var max = BOTs_List.Count - 1;//11
                    int bot_lucky_idx = max;//11

                    if (BOTs_List.IndexOf(bot) == max) bot_lucky_idx -= 1;//10
                    LUCKY_BOT = BOTs_List[bot_lucky_idx];
                    LUCKY_BOT.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[0]);//Allies bot change to Jugg class

                    int i = 0;
                    OnInterval(250, () =>
                    {
                        if (i != bot_lucky_idx) BOTs_List[i].Call(33341);//suicide
                        if (i == max)
                        {
                            return GetTeamState(bot.Name, bot_lucky_idx);
                        }
                        i++;
                        return true;
                    });

                    return false;
                }

                return true;
            });
        }

        bool GetTeamState(string first_inf_name, int bot_lucky_idx)
        {
            TK.SetTank(LUCKY_BOT);
            int lbe = LUCKY_BOT.EntRef;
            B_FIELD[lbe] = null;
            IsBOT[lbe] = null;
            string weapon = LUCKY_BOT.CurrentWeapon;
            LUCKY_BOT.Call(33469, weapon, 0);//setweaponammostock
            LUCKY_BOT.Call(33468, weapon, 0);//setweaponammoclip
            LUCKY_BOT.Call(33220, 0f);//setmovespeedscale

            Log.Write(LogLevel.None, "■ BOTs:{0} INF:{1} ■ MAP:{2}", BOTs_List.Count, first_inf_name, SET.MAP_IDX);

            Call(42, "scr_infect_timelimit", "12");

            GET_TEAMSTATE_FINISHED = true;

            GRACE_TIME = DateTime.Now.AddSeconds(166);

            foreach (H_SET H in H_FIELD)
            {
                if (H == null) continue;
                H.HUD_SERVER.Alpha = 0.7f;
                H.HUD_RIGHT_INFO.Alpha = 0.7f;
            }

            if (!HUMAN_DIED_ALL_) BotDoAttack(true);

            HumanCheckInf(null);

            return false;
        }

        bool BotDoAttack(bool attack)
        {
            if (attack)
            {
                if (!BOT_ADD_WATCHED)
                {
                    SetTeamName();
                    BotAddWatch();
                }

                Call(42, "testClients_doCrouch", 0);
                Call(42, "testClients_doMove", 1);
                Call(42, "testClients_doAttack", 1);
            }
            else
            {
                Call(42, "testClients_doCrouch", 1);
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
            }
            return false;
        }


    }

}
