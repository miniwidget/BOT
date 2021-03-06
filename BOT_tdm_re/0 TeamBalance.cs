﻿using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tdm
{
    public partial class Tdm
    {

        void BotTeamBalanceEnable(bool isAxis, bool enabled, Entity player, int hlc)
        {
            if (!enabled)
            {

                #region team unbalance


                if (hlc > 1)
                {
                    BALANCE_STATE = State.balance_on; BotDoAttack(true);
                    //Print("HC return: " + hlc);
                    return;
                }
                if (player == null)
                {
                    if (hlc == 1)
                    {
                        player = human_List[0];
                        isAxis = IsAxis[player.EntRef];
                    }
                    else return;
                }

                if (BALANCE_STATE != State.balance_on) return;

                BALANCE_STATE = State.balance_off_wait; BotDoAttack(false);

                //Print("axis:" + Axis_List.Count + " allies:" + Allies_List.Count + "BOTs:" + BOTs_List.Count + " isAxis: " + isAxis);

                OnInterval(300, () =>
                {
                    if (isAxis)
                    {
                        if (Axis_List.Count <= 4 || BOT_TEAM_CHANGE_DENIED)
                        {
                            BALANCE_STATE = State.balance_off; BotDoAttack(true);
                            return BOT_TEAM_CHANGE_DENIED = false;
                        }
                    }
                    else
                    {
                        if (Allies_List.Count <= 4 || BOT_TEAM_CHANGE_DENIED)
                        {
                            BALANCE_STATE = State.balance_off; BotDoAttack(true);
                            return BOT_TEAM_CHANGE_DENIED = false;
                        }

                    }

                    BotChangeTeamToAxis(GetBotByTeam(isAxis), !isAxis);
                    return true;
                });


                return;
                #endregion
            }

            #region team balance
            if (BALANCE_STATE != State.balance_off) return;

            BALANCE_STATE = State.balance_on_wait;
            BotDoAttack(false);

            Allies_List.Clear(); Axis_List.Clear();

            bool toAxis = true;
            int max = BOTs_List.Count - 1;
            for (int i = 0; i < BOTs_List.Count; i++)
            {
                if (BOT_TEAM_CHANGE_DENIED)
                {
                    BOT_TEAM_CHANGE_DENIED = false;
                    HumanTeamBalance();
                    break;
                }

                BotChangeTeamToAxis(BOTs_List[i], toAxis = !toAxis);

                if (i == max) AfterDelay(1000, () => HumanTeamBalance());
            }

            #endregion
        }
        bool BOT_TEAM_CHANGE_DENIED;
        void BotChangeTeamToAxis(Entity bot, bool toAxis)
        {
            string Team = "allies";
            string OtherTeam = "axis";
            if (toAxis)
            {
                Team = "axis";
                OtherTeam = "allies";
            }

            bot.Notify("menuresponse", "team_marinesopfor", Team);
            bot.AfterDelay(100, x =>
            {
                B_SET B = B_FIELD[bot.EntRef];
                if (B.CLASS.Contains("recipe")) B.CLASS = B.CLASS.Replace(OtherTeam, Team);
                bot.Notify("menuresponse", "changeclass", B.CLASS);

                bot.AfterDelay(100, xx =>
                {
                    if (GetPlayerTeam(bot) == OtherTeam)
                    {
                        if (B.CLASS.Contains("recipe")) B.CLASS = B.CLASS.Replace(Team, OtherTeam);
                        bot.Notify("menuresponse", "changeclass", B.CLASS);
                        //Print("Bot denied");
                        BOT_TEAM_CHANGE_DENIED = true;
                        return;
                    }

                    B.AXIS = toAxis;

                    if (toAxis)
                    {
                        if (!Axis_List.Contains(bot)) Axis_List.Add(bot);
                        if (Allies_List.Contains(bot)) Allies_List.Remove(bot);
                    }
                    else
                    {
                        if (!Allies_List.Contains(bot)) Allies_List.Add(bot);
                        if (Axis_List.Contains(bot)) Axis_List.Remove(bot);
                    }

                    //Print(bot.Name + " " + bot.CurrentWeapon + " " + GetPlayerTeam(bot));
                });
            });
        }

        void HumanTeamBalance()
        {
            int axisCount = 0;
            int alliesCount = 0;
            foreach (Entity human in human_List)
            {
                if (human.EntRef > 17)
                {
                    if (IsHumanTankRemoterAxis()) axisCount++; else alliesCount++; continue;
                }
                if (IsAxis[human.EntRef]) axisCount++; else alliesCount++;
            }
            if (axisCount == alliesCount) return;

            int failCount = 0;
            int hlc = human_List.Count;
            int i = 0;

            int small = 0, big = 0;
            string AddTeam = "allies";
            bool axis = false;

            if (axisCount > alliesCount)
            {
                big = axisCount;
                small = alliesCount;
                axis = true;
            }
            else
            {
                big = alliesCount;
                small = axisCount;
                AddTeam = "axis";
            }

            bool stop = false;
            OnInterval(300, () =>
            {
                if (stop)
                {
                    BALANCE_STATE = State.balance_on;
                    BotDoAttack(true);
                    return false;
                }
                if (++failCount > hlc) stop = true;

                Entity human = human_List[i];
                if (human.EntRef > 17)
                {
                    if (IsHumanTankRemoterAxis())
                    {
                        if (AddTeam == "axis") small++;
                    }
                    else
                    {
                        if (AddTeam == "allies") small++;
                    }
                    i++;
                    return true;
                }
                if (IsAxis[human.EntRef] == axis)
                {
                    HumanChangeTeamToAxis(human, !axis);
                    human.AfterDelay(200, x =>
                    {
                        if (GetPlayerTeam(human) == AddTeam) big--;
                        if (big - small < 2) stop = true;
                        if (++failCount > hlc) stop = true;
                    });
                }

                i++;
                return true;
            });
        }
        void HumanChangeTeamToAxis(Entity human, bool toAxis)
        {
            string Team = "allies";
            string OtherTeam = "axis";
            if (toAxis)
            {
                Team = "axis";
                OtherTeam = "allies";
            }

            human.Notify("menuresponse", "team_marinesopfor", Team);
            human.AfterDelay(100, x =>
            {
                human.Notify("menuresponse", "changeclass", Team + "_recipe" + rnd.Next(1, 4));
                human.AfterDelay(100, xx =>
                {
                    if (GetPlayerTeam(human) == OtherTeam)
                    {
                        human.Notify("menuresponse", "changeclass", OtherTeam + "_recipe" + rnd.Next(1, 4));
                        //Print("Human denied");
                        return;
                    }

                    H_FIELD[human.EntRef].AXIS = toAxis;
                    if (toAxis)
                    {
                        if (!Axis_List.Contains(human)) Axis_List.Add(human);
                        if (Allies_List.Contains(human)) Allies_List.Remove(human);
                    }
                    else
                    {
                        if (!Allies_List.Contains(human)) Allies_List.Add(human);
                        if (Axis_List.Contains(human)) Axis_List.Remove(human);
                    }
                    HUD.ChangeHud(human, GET_TEAMSTATE_FINISHED);
                });

            });
        }

        string HumanTeamAssgin()
        {
            int hlc = human_List.Count;

            if (hlc == 0) return "autoassign";

            int axisCount = 0;
            int alliesCount = 0;
            foreach (Entity human in human_List)
            {
                if (human.EntRef > 17)
                {
                    if (IsHumanTankRemoterAxis()) axisCount++; else alliesCount++; continue;
                }
                if (IsAxis[human.EntRef]) axisCount++; else alliesCount++;
            }
            if (axisCount == alliesCount) return "autoassign";
            if (axisCount > alliesCount) return "allies";
            return "axis";
        }
        bool IsHumanTankRemoterAxis()
        {
            foreach (Entity player in Players)
            {
                if (player.Name.StartsWith("bot")) continue;
                if (human_List.IndexOf(player) == -1)
                {
                    if (GetPlayerTeam(player) == "axis") return true;
                    else return false;
                }
            }
            return false;
        }
        string GetPlayerTeam(Entity player)
        {
            return player.GetField<string>("sessionteam");
        }
        Entity GetBotByTeam(bool axis)
        {
            foreach (Entity bot in BOTs_List)
            {
                if (axis)
                {
                    if (GetPlayerTeam(bot) == "axis") return bot;
                }
                else
                {
                    if (GetPlayerTeam(bot) == "allies") return bot;
                }
            }
            return null;
        }

        void CheckTeamState(Entity player, bool isAxis)
        {
            int hlc = human_List.Count;

            if (hlc == 1)
            {
                player.OnInterval(1000, x =>
                {
                    hlc = human_List.Count;
                    if (hlc == 0 || hlc > 1) return false;
                    if (BALANCE_COUNT == 0)
                    {
                        BALANCE_COUNT = 10;
                        player.Call(33344, "Until Human Connected, Balance OFF");
                        BotTeamBalanceEnable(isAxis, false, player, hlc);
                        return false;
                    }

                    if (player != null) player.Call(33344, BALANCE_COUNT);
                    BALANCE_COUNT--;

                    return true;
                });
            }
            else if (BALANCE_STATE == State.balance_off) BotTeamBalanceEnable(isAxis, true, player, hlc);

        }

    }
}
