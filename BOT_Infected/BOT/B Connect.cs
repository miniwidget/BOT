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
#if DEBUG
        Entity CARE_PACKAGE;
#endif
        int BOT_RPG_ENTREF, BOT_RIOT_ENTREF, BOT_JUGG_ENTREF, BOT_LUCKY_IDX, BOT_HELI_ENTREF;
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
            if (i > SET.BOT_SETTING_NUM)
            {
                Call(286, be);//kick
                return;
            }
            if (be == -1) return;

            if (i == SET.BOT_SETTING_NUM - 1)
            {
                AfterDelay(4000,()=>BotWaitOnFirstInfected());
            }

            /*

                    internal readonly string[] BOTs_CLASS = {
                        "axis_recipe1",//jugg
                        "axis_recipe2",//rpg
                        "axis_recipe3",//riot
                        "axis_recipe3",//heli
                        "class0",//AR g36c
                        "class1",//SMG ump45
                        "class2",//LMG mk46
                        //"class3",//sniper
                        "class4",//SG striker
                        "class5",//AR m4
                        "class6",//SMG mp5
                        "class6"// riotshield
                    };    
            */
            if (i == 0) BOT_JUGG_ENTREF = be;
            else if (i == 1) BOT_RPG_ENTREF = be;
            else if (i == 2) BOT_RIOT_ENTREF = be;
            else if (i == 3) BOT_HELI_ENTREF = be;

            if (i > 10)
            {
                if (SET.BOT_CLASS_NUM > 10) SET.BOT_CLASS_NUM = 4;
                i = SET.BOT_CLASS_NUM;
                SET.BOT_CLASS_NUM++;
            }

            bot.Notify("menuresponse", "changeclass", SET.BOTs_CLASS[i]);
            BOTs_List.Add(bot);
            IsBOT[be] = true;
            H_FIELD[be] = null;
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
                            if (i == 3) bot.SpawnedPlayer += () => BotHeliSpawned(bot);
                            else bot.SpawnedPlayer += () => BotSpawned(bot);

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
                    TK.SetTank(bot);
                    string wep = bot.CurrentWeapon;
                    B_FIELD[bot.EntRef].wep = wep;
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

#if DEBUG
            CarePackage();
#endif
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
#if DEBUG
        void CarePackage()
        {
            Entity ent = Call<Entity>("getent", "mp_dom_spawn", "classname"); if (ent == null) return;

            Vector3 origin = ent.Origin; ent = null;

            Entity brushmodel = Call<Entity>("getent", "pf1_auto1", "targetname");

            if (brushmodel == null) brushmodel = Call<Entity>("getent", "pf3_auto1", "targetname");
            if (brushmodel != null)
            {

                CARE_PACKAGE = Call<Entity>("spawn", "script_model", origin);
                CARE_PACKAGE.Call("setmodel", "com_plasticcase_friendly");//com_plasticcase_friendly
                CARE_PACKAGE.Call(33353, brushmodel);

                Call(431, 20, "active"); // objective_add
                Call(435, 20, origin); // objective_position
                Call(434, 20, "compass_waypoint_bomb"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

                return;
            }

            for (int i = 18; i < 1024; i++)
            {
                brushmodel = Entity.GetEntity(i);
                if (brushmodel == null) continue;
                if (brushmodel.GetField<string>("classname") == "script_brushmodel")
                {

                    string targetName = brushmodel.GetField<string>("targetname");

                    if (targetName == null) continue;
                    Print(targetName + " entref " + i);
                    CARE_PACKAGE = Call<Entity>("spawn", "script_model", origin);
                    CARE_PACKAGE.Call("setmodel", "com_plasticcase_friendly");//com_plasticcase_friendly
                    CARE_PACKAGE.Call(33353, brushmodel);

                    Call(431, 20, "active"); // objective_add
                    Call(435, 20, origin); // objective_position
                    Call(434, 20, "compass_waypoint_bomb"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

                    break;
                }
            }

        }
#endif
    }

}
