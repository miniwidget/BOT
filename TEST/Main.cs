using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;
using System.Reflection;

namespace TEST
{
    public partial class test : BaseScript
    {
        public test()
        {
            CheckTEST();

            Call("setdvar", "scr_game_playerwaittime", 1);
            Call("setdvar", "scr_game_matchstarttime", 1);
            Call("setdvar", "scr_game_allowkillcam", "0");
            Call("setdvar", "scr_infect_timelimit", 10);

            OnServerCommand("/lsc", () =>
            {
                string com = "loadscript test\\sc.dll";
                if (!TEST_) com = com.Replace("test\\", "");
                Utilities.ExecuteCommand(com);
                Utilities.ExecuteCommand("fast_restart");
            });

            OnServerCommand("!", (string[] txts) =>
            {
                int lenth = txts.Length;
                if (lenth == 2)// / command
                {
                    CommandsOne(txts[1]);
                }
                else if (lenth == 3)// / command var
                {
                    CommandsTwo(txts);
                }else
                {
                }
            });
            

        }
        bool TEST_;
        void CheckTEST()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (assembly.Location.Contains("test"))
            {
                TEST_ = true;
                print("■ " + assembly.GetName().Name + ".dll & TEST MODE");
            }
        }
        bool getADMIN()
        {
            foreach(Entity p in Players)
            {
                if (p == null) continue;
                if(p.Name == "kwnav")
                {
                    ADMIN = p;
                    break;
                }
            }
            return true;
        }
        void playLocalSound(string s)
        {
            ADMIN.Call("playlocalsound", s);
        }
        void getTeamName(string teamRef)
        {
            //pmc_africa

            string value = Call<string>("tableLookup", "mp/factionTable.csv", 0, teamRef, 7);
            print("value: " + value);
        }
        void viewModel(string teamRef)
        {
            foreach (Entity p in Players)
            {
                var s = p.GetField<string>("model");
                print(s);
            }
        }

        void playSound(string soundname)
        {
            //string voicePrefix = Call<string>("tableLookup", "mp/factionTable.csv", 0, "allies", 7) + "0_";//US_0_
            //string bcSoounds = "rpg_incoming";
            //string soundAlias = voicePrefix + bcSoounds;//US_0_rpg_incoming
            //AF_1mc_losing_fight
            ADMIN.Call("playsoundtoteam", soundname, "allies");
        }
        void CommandsOne(string text)
        {
            if (ADMIN == null) getADMIN();
            switch (text)
            {
                
                case "sb": spawndBot(); break;
                case "fc": fc(); break;
                case "selector": _beginLocationSelection(ADMIN, "mobile_mortar", "map_artillery_selector", false, 500); break;
                case "ti": TI(); break;
                case "marker": airdropMarker(); break;
                case "ww": uavStrikerMarker(); break;
                case "p": print(ADMIN.Origin); break;
                case "loc": ADMIN.Call("setorigin", new Vector3(-2943, 453, 527)); break;
                case "h": testHealth(); break;
                case "turret": spawnTurrent(); break;
                case "heli": break;

                case "ds": ADMIN.Call(33503); break;
                //remote    
                case "rmharr": remoteHarrir(); break;
                case "rmheli": remoteHeli(); break;
                case "end": EndRemoteControl(); break;
                case "start": StartRemoteControl(); break;
            }

        }
        void CommandsTwo(string[] texts)
        {
            if (ADMIN == null) getADMIN();
            switch (texts[1])
            {
                case "st": playSound(texts[2]); break;
                case "tn": getTeamName(texts[2]); break;
                case "so": playLocalSound(texts[2]);  break;
                case "fx": LoadFX(texts[2]); break;
                case "qq": tableValue(texts[2]); break;
                case "sp": spawn(texts[2]); break;
                case "plane": spawnPlane(); break;
                case "rm": remoteTestModel(texts[2]); break;
            }
        }
    }
}
