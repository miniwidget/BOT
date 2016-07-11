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


        int BOT_RPG_ENTREF, BOT_LUCKY_IDX, BOT_LUCKY_ENTREF;
        bool BOT_TO_AXIS_COMP;

        #region deplay
        void BotDeplay()
        {
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
                //print(i + "의 봇이 모자랍니다.");

                int fail_count = 0, max = SET.BOT_SETTING_NUM - 1;
                OnInterval(250, () =>
                {
                    if (BOTs_List.Count > max || Players.Count == 18)
                    {
                        //print("총 불러온 봇 수는 " + getBOTCount + " 입니다.");
                        return false;
                    }
                    if (fail_count > 20)
                    {
                        //print("봇 추가로 불러오기 실패");
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

            var i = BOTs_List.Count;
            int be = bot.EntRef;
            if (be == -1 || i > SET.BOT_SETTING_NUM)
            {
                Call(286, be);//kick
                return;
            }

            if (i == SET.BOT_SETTING_NUM - 1)
            {
                 BotWaitOnFirstInfected();
            }

            B_SET B = B_FIELD[be];

            string alertSound = null;
            int delay_time = 0;

            if (i == 0)
            {
                alertSound = "AF_victory_music";
                delay_time = 6100;
            }
            else if (i == 1)
            {
                BOT_RPG_ENTREF = be;
                alertSound = "missile_incoming";
                delay_time = 10000;
            }
            else if (i == 2 || i == 3)
            {
                B.not_fire = true;
                bot.SpawnedPlayer += delegate
                {
                    bot.Call(33220, 2f);
                    int k = B.killer;
                    if (k != -1)
                    {
                        BotCheckPerk(k);
                        B.killer = -1;
                    }
                };
            }
            else if (i == 4)
            {
                B.not_fire = true;
                bot.SpawnedPlayer += () => BotHeliSpawned(bot);
            }
            else
            {
                delay_time = 6100;
            }

            if (i > 11)
            {
                if (SET.BOT_CLASS_NUM > 11) SET.BOT_CLASS_NUM = 5;
                i = SET.BOT_CLASS_NUM;
                SET.BOT_CLASS_NUM++;
            }

            bot.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[i]);
            BOTs_List.Add(bot);
            IsBOT[be] = true;
            H_FIELD[be] = null;

            if (B.not_fire) return;

            int ammo = 0;
            string weapon = null;

            bot.SpawnedPlayer += delegate
            {
                if (GAME_ENDED_) return;

                bot.Health = -1;
                bot.Call(32848);//hide
                bot.Call(33220, 0f);//setmovespeedscale

                if (weapon == null)
                {
                    weapon = bot.CurrentWeapon;
                    bot.Call(33523, weapon);
                    ammo = bot.Call<int>(33470, weapon);
                }

                bot.Call(33469, weapon, 0);//setweaponammostock
                bot.Call(33468, weapon, 0);//setweaponammoclip

                #region check perk to killer

                int k = B.killer;
                if (k != -1)
                {
                    BotCheckPerk(k);
                    B.killer = -1;
                }

                #endregion

                bot.AfterDelay(delay_time, x =>
                {
                    if (GAME_ENDED_) return;

                    if (i == 0) bot.Health = 100;
                    else bot.Health = 120;

                    bot.Call(33220, 1f);//setmovespeedscale

                    BotSearchOn(bot, B, alertSound, weapon, ammo);

                    bot.Call(32847);//show
                });

            };
            bool fire = true;
            bot.OnNotify("weapon_fired", (p, wp) =>
            {
                if (!fire)
                {
                    fire = true;
                    return;
                }
                fire = false;

                if (B.target == null) return;

                var TO = B.target.Origin;
                var BO = bot.Origin;

                float dx = TO.X - BO.X;
                float dy = TO.Y - BO.Y;
                float dz = BO.Z - TO.Z + 50;

                int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                BO.X = (float)Math.Atan2(dz, dist) * 57.3f;
                BO.Y = -5 + (float)Math.Atan2(dy, dx) * 57.3f;
                BO.Z = 0;

                bot.Call(33531, BO);//SetPlayerAngles

            });
        }

        #endregion

        void BotWaitOnFirstInfected()
        {
            OnInterval(1000, () =>
            {
                foreach (Entity ent in Players)
                {
                    if (ent == null) continue;
                    if (ent.GetField<string>("sessionteam") != "axis") continue;
                    BOT_TO_AXIS_COMP = true;

                    var max = BOTs_List.Count - 1;//11
                    BOT_LUCKY_IDX = max;//11

                    if (ent.Name.StartsWith("bot"))//봇이 감염된 경우
                    {
                        if (BOTs_List.IndexOf(ent) == max) BOT_LUCKY_IDX -= 1;//10
                    }

                    BOTs_List[BOT_LUCKY_IDX].Notify("menuresponse", "changeclass", SET.BOTs_CLASS[0]);//Allies bot change to Jugg class

                    int i = 0;
                    OnInterval(250, () =>
                    {
                        Entity bot = BOTs_List[i];

                        if (i != BOT_LUCKY_IDX)
                        {
                            bot.Call(33341);//suicide
                        }
                        if (i == max)
                        {
                            return GetTeamState(ent.Name);
                        }

                        i++;
                        return true;
                    });

                    return false;

                }

                return true;
            });
        }

        bool GetTeamState(string first_inf_name)
        {
            int alive = 0, max = BOTs_List.Count;


            for (int i = 0; i < BOTs_List.Count; i++)
            {
                Entity bot = BOTs_List[i];

                if (bot.GetField<string>("sessionteam") == "allies") alive++;
                if (i == BOT_LUCKY_IDX)
                {
                    BOT_LUCKY_ENTREF = bot.EntRef;
                    TK.SetTank(bot);
                    string wep = bot.CurrentWeapon;
                    B_FIELD[BOT_LUCKY_ENTREF].wep = wep;
                    bot.Call(33469, wep, 0);//setweaponammostock
                    bot.Call(33468, wep, 0);//setweaponammoclip
                    bot.Call(33220, 0f);
                }
            }

            Log.Write(LogLevel.None, "■ BOTs:{0} AXIS:{1} ALLIES:{2} INF:{3} ■ MAP:{4}", max, (max - alive), alive, first_inf_name, SET.MAP_IDX);

            Call(42, "scr_infect_timelimit", "12");

            GET_TEAMSTATE_FINISHED = true;

            GRACE_TIME = DateTime.Now.AddSeconds(166);

            HCT.SetHeliPort();

            foreach (Entity human in human_List)
            {
                H_FIELD[human.EntRef].HUD_SERVER.Alpha = 0.7f;
                H_FIELD[human.EntRef].HUD_RIGHT_INFO.Alpha = 0.7f;
            }

            if (!HUMAN_DIED_ALL_)
            {
                BotDoAttack(true);
            }

            SetTeamName();

            return false;
        }

    }

}
