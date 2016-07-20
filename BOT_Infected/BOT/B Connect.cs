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
            if (botCount < SET.BOT_SETTING_NUM || BOTs_List.Count < SET.BOT_SETTING_NUM)
            {
                int fail_count = 0, max = SET.BOT_SETTING_NUM;
                OnInterval(250, () =>
                {
                    if (BOTs_List.Count == max || Players.Count == 18 || fail_count > 20)
                    {
                        //Print("BLC:"+BOTs_List.Count + " PC:" + Players.Count + " FC:" + fail_count);
                        return false;
                    }

                    Entity bot = Utilities.AddTestClient(); if (bot == null) fail_count++;

                    return true;
                });
            }
            #endregion

        }
        #endregion

        #region Bot_Connected
        private void Bot_Connected(Entity bot)
        {
            BOTs_List.Add(bot);

            if (BOT_TO_AXIS_COMP) return;//If deploy bot after game in the middle time, deny bot setting

            int blc = BOTs_List.Count;
            if (blc == SET.BOT_SETTING_NUM)
            {
                if (blc < 2) return;

                int bot1_num, bot2_num;
                if (!int.TryParse(BOTs_List[0].Name.Substring(3), out bot1_num)) return;
                if (!int.TryParse(BOTs_List[1].Name.Substring(3), out bot2_num)) return;

                if (bot1_num > bot2_num) BOTs_List.Reverse();//when fast_restart executed, Players are reversed. So, it need to be reversed again due to change class properly.

                int i = 0;

                OnInterval(150, () =>
                {
                    if (i == blc) return false;
                    Entity BOT = BOTs_List[i];
                    BOT.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[i]);
                    int num = i;
                    bot.AfterDelay(100, x => Bot_Connected2(BOT, num));//important. must be delayed due to get changed class weapon
                    i++;
                    return true;
                });
                    
            }
        }
        private void Bot_Connected2(Entity bot, int i)
        {
            int be = bot.EntRef;
            bool block = false;
            string weapon = bot.CurrentWeapon;
            string alert_sound = null;

            if (i == 0) alert_sound = "AF_victory_music";//jugg bot
            else if (i == 1) alert_sound = "missile_incoming";//rpg bot
            else if (i == 4) { block = true; BOT_HELIRIDER_IDX = i; }//heli bot
            else if (weapon == "riotshield_mp") { block = true; bot.SpawnedPlayer += () => bot.Call(33220, 2f); }//riot bot

            IsAxis[be] = true;

            //Print(i + weapon + " " + alert_sound + " " + SET.BOT_SETTING_NUM);

            if (block) return;

            bot.Call(33523, weapon);//giveMaxaAmmo

            B_FIELD[be] = new B_SET(bot, alert_sound)
            {
                WEAPON = weapon,
                AMMO_CLIP = bot.Call<int>(33460, weapon)
            };

            bot.Call(33469, weapon, 0);//setweaponammostock
            bot.Call(33468, weapon, 0);//setweaponammoclip
            IsBOT[be] = true;
            B_SET B = B_FIELD[be];

            bot.SpawnedPlayer += delegate
            {
                if (GAME_ENDED_) return;
                bot.Call(33469, weapon, 0);//setweaponammostock
                bot.Call(33468, weapon, 0);//setweaponammoclip

                int k = B.KILLER;
                if (k != -1)
                {
                    BotGivePerkToKiller(k); B.KILLER = -1;
                }

                B.WAIT = false;
            };

            if (i == SET.BOT_SETTING_NUM - 1) BotWaitOnFirstInfected();
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
                    BOT_LUCKY_IDX = max;//11

                    if (BOTs_List.IndexOf(bot) == max) BOT_LUCKY_IDX -= 1;//10

                    Entity lucky_bot = BOTs_List[BOT_LUCKY_IDX];
                    lucky_bot.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[0]);//Allies bot change to Jugg class
                    lucky_bot.AfterDelay(100, lb =>
                    {
                        TK.SetTank(lucky_bot);
                        int lbe = lucky_bot.EntRef;
                        B_SET B = B_FIELD[lbe];//
                        if (B != null)
                        {
                            IsAxis[lbe] = B.AXIS = false;
                            B.WAIT = true;
                            B.ALERT_SOUND = "AF_victory_music";
                            B.WEAPON = lucky_bot.CurrentWeapon;
                            B.AMMO_CLIP = 100;
                            lucky_bot.Call(33469, B.WEAPON, 0);//setweaponammostock
                            lucky_bot.Call(33468, B.WEAPON, 0);//setweaponammoclip
                            lucky_bot.Call(33220, 0f);//setmovespeedscale

                            lucky_bot.SpawnedPlayer += delegate
                            {
                                if (!B.AXIS)
                                {
                                    B.AXIS = true;
                                    IsAxis[lbe] = true;
                                }
                            };
                        }
                        else
                        {
                            Print("B_SET 눌");
                        }

                    });

                    int i = 0;
                    OnInterval(250, () =>
                    {
                        if (i != BOT_LUCKY_IDX) BOTs_List[i].Call(33341);//suicide
                        if (i == max) return GetTeamState(bot.Name);

                        i++; return true;
                    });

                    return false;
                }

                return true;
            });
        }

        bool GetTeamState(string first_inf_name)
        {

            Log.Write(LogLevel.None, "■ BOTs:{0} INF:{1} ■ MAP:{2}", BOTs_List.Count, first_inf_name, SET.MAP_IDX);

            Call(42, "scr_infect_timelimit", "12");

            GRACE_TIME = DateTime.Now.AddSeconds(166);

            foreach (H_SET H in H_FIELD)
            {
                if (H == null) continue;
                H.HUD_SERVER.Alpha = 0.7f;
                H.HUD_RIGHT_INFO.Alpha = 0.7f;
            }

            GET_TEAMSTATE_FINISHED = true;

            if (!HUMAN_DIED_ALL_) BotDoAttack(true);

            HumanCheckInf(null);

            string alliesCharSet = Call<string>(221, "allieschar");//getMapCustom
            if (alliesCharSet == null) alliesCharSet = "sas_urban";

            string axisCharSet = Call<string>(221, "axischar");//getMapCustom
            if (axisCharSet == null) axisCharSet = "opforce_henchmen";

            switch (alliesCharSet)
            {
                case "delta_multicam": alliesCharSet = "US"; break;
                case "sas_urban": alliesCharSet = "UK"; break;
                case "gign_paris": alliesCharSet = "FR"; break;
                case "pmc_africa": alliesCharSet = "PC"; break;
                default: alliesCharSet = "PC"; break;
            }
            switch (axisCharSet)
            {
                case "opforce_air":
                case "opforce_snow":
                case "opforce_urban":
                case "opforce_woodland": axisCharSet = "RU"; break;
                case "opforce_africa": axisCharSet = "AF"; break;
                case "opforce_henchmen": axisCharSet = "IC"; break;
                default: axisCharSet = "RU"; break;
            }

            DIALOG_AXIS = new string[DIALOG_ALLIES.Length];
            int sal = SET.SOUND_ALERTS.Length;
            for (int i = 0; i < DIALOG_ALLIES.Length; i++)
            {
                DIALOG_AXIS[i] = DIALOG_ALLIES[i].Insert(0, axisCharSet);
                DIALOG_ALLIES[i] = DIALOG_ALLIES[i].Insert(0, alliesCharSet);
                if (i < sal) SET.SOUND_ALERTS[i] = SET.SOUND_ALERTS[i].Insert(0, alliesCharSet);
            }
            //AfterDelay(2000, () =>
            //{
            //    foreach (Entity bot in BOTs_List)
            //    {
            //        B_SET B = B_FIELD[bot.EntRef];
            //        if (B == null)
            //        {
            //            Print("NA: " + bot.Name + " " + bot.CurrentWeapon);
            //        }
            //        else
            //        {
            //            Print("AP: " + bot.Name + " gun:" + B.WEAPON + " isAxis:" + B.AXIS + " sound:" + B.ALERT_SOUND + " wait:" + B.WAIT);
            //        }
            //    }
            //});

            return false;
        }

        bool BotDoAttack(bool attack)
        {
            HUMAN_DIED_ALL_ = !attack;

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

        void BotAddWatch()
        {
            BOT_ADD_WATCHED = true;

            List<B_SET> bots_fire = new List<B_SET>();
            foreach (B_SET B in B_FIELD)
            {
                if (B == null) continue;
                bots_fire.Add(B);
            }

            BotHeli BH = new BotHeli(BOTs_List[BOT_HELIRIDER_IDX], Players.Select(ent => ent.Origin).ToArray());

            OnInterval(2500, () =>//deny riot bot
            {
                if (GAME_ENDED_) return false;

                foreach (B_SET B in bots_fire) B.Search();

                BH.HeliBotSearch();//HELI BOT

                return true;
            });
        }

        /// <summary>
        /// Survivor bot starts searching Infected humans
        /// </summary>
        private void BotSerchOn_lucky()
        {
            BOT_SERCH_ON_LUCKY_FINISHED = true;

            Entity lucky_bot = BOTs_List[BOT_LUCKY_IDX];

            if (lucky_bot.GetField<string>("sessionteam") == "axis") return;
            B_FIELD[lucky_bot.EntRef].WAIT = false;
            lucky_bot.Call(33220, 1f);//setmovespeedscale
            //IsBOT[lbe] = "1";
        }
    }

}
