using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {

        void BotDeplay()
        {
            if (!SET.DEPLAY_BOT_) return;

            int setNum = SET.BOT_SETTING_NUM;

            int diff = setNum - BOTs_List.Count;

            int fail_count = 0;

            if (diff == 0) return;

            else if (diff > 0)
            {
                OnInterval(250, () =>
                {
                    if (BOTs_List.Count == setNum || fail_count >= 18) return false;

                    Entity bot = Utilities.AddTestClient();
                    fail_count++;
                    return true;
                });
            }
            else
            {
                while (true)
                {
                    int i = BOTs_List.Count - 1;
                    Entity bot = BOTs_List[i];
                    BOTs_List.Remove(bot);
                    if (Axis_List.Contains(bot)) Axis_List.Remove(bot);
                    else if (Allies_List.Contains(bot)) Allies_List.Remove(bot);

                    Call("kick", bot.EntRef);

                    i = BOTs_List.Count;
                    if (i <= setNum) return;
                    if (fail_count >= 18) return;
                    fail_count++;
                }
            }
        }

        private void Bot_Connected(Entity bot, string cls,string team)
        {
            bot.Notify("menuresponse", "team_marinesopfor", team);
            bot.AfterDelay(100, b =>
            {
                bot.Notify("menuresponse", "changeclass", cls);
            });
            
            //Print("c: "+bot.Name + " " + cls );
            bot.AfterDelay(200, b =>
            {
                int be = bot.EntRef;

                string wp = bot.CurrentWeapon;
                string sound = null;

                if (wp == "rpg_mp") sound = "missile_incoming";
                else if (wp.StartsWith("iw5_m60"))
                {
                    if (team == "axis") JUGG_BOT_AXIS = bot; else JUGG_BOT_ALLIES = bot;
                    sound = "AF_victory_music";
                }

                bot.Call(33523, wp);//giveMaxaAmmo

                IsBOT[be] = true;
                B_FIELD[be] = new B_SET(bot, team == "axis", wp, sound, bot.Call<int>(33460, wp), cls);



                bot.SpawnedPlayer += delegate
                {
                    if (GAME_ENDED_) return;
                    bot.Call(33469, wp, 0);//setweaponammostock
                    bot.Call(33468, wp, 0);//setweaponammoclip
                    bot.Call(33427);//disableweaponpickup

                    GivePerkToHumanKiller(be);

                    B_FIELD[be].WAIT = false;
                };

            });
            
        }

        void GetTeamState()
        {
            TK.SetTank(BOTs_List[rnd.Next(3, 4)]);

            Log.Write(LogLevel.None, "■ BOTs:{0} ■ MAP:{1}", BOTs_List.Count, SET.MAP_IDX);

            Call(42, "scr_infect_timelimit", "20");

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

            for (int i = 0; i < DIALOG_ALLIES.Length; i++)
            {
                DIALOG_AXIS[i] = DIALOG_ALLIES[i].Insert(0, axisCharSet);
                DIALOG_ALLIES[i] = DIALOG_ALLIES[i].Insert(0, alliesCharSet);
            }

            int hlc = human_List.Count;

            if (hlc== 1)
            {
                Entity human = human_List[0];
                CheckTeamState(human, IsAxis[human.EntRef]);
            }
            if (hlc != 0)
            {
                foreach (H_SET H in H_FIELD)
                {
                    if (H == null) continue;
                    H.HUD_SERVER.Alpha = 0.7f;
                    H.HUD_RIGHT_INFO.Alpha = 0.7f;
                }
            }

            GET_TEAMSTATE_FINISHED = true;
        }

        bool BotDoAttack(bool attack)
        {
            if (attack)
            {
                if (HUMAN_ZERO_) HUMAN_ZERO_ = false;
                if (!BOT_ADD_WATCH_FINISHED) BotAddWatch();

                Call(42, "testClients_doCrouch", 0);
                Call(42, "testClients_doMove", 1);
                Call(42, "testClients_doAttack", 1);
            }
            else
            {
                if (!HUMAN_ZERO_) HUMAN_ZERO_ = true;
                Call(42, "testClients_doCrouch", 1);
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
            }
            return false;
        }

    }

}
