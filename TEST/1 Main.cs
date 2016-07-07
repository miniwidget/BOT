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
    class InfinityBase
    {
        internal TReturn Call<TReturn>(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }
        internal TReturn Call<TReturn>(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }

        internal void Call(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        internal void Call(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        internal void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
    }

    public partial class test : BaseScript
    {
        internal static Entity ADMIN;
        Random rnd = new Random();
        Marker marker;
        FX fx;
        Vehicle vehicle;
        Sound sound;
        Table table;
        Tank tank;
        //SentryGun SG;
        Ospray osp;
        AC130 ac;

        public test()
        {
            Call(42, "scr_game_playerwaittime", 1);
            Call(42, "scr_game_matchstarttime", 1);
            Call(42, "testClients_watchKillcam", 0);
            Call(42, "testClients_doReload", 0);
            Call(42, "testClients_doCrouch", 1);
            Call(42, "testClients_doMove", 0);
            Call(42, "testClients_doAttack", 0);
            Utilities.ExecuteCommand("seta g_password \"0\"");
            Utilities.ExecuteCommand("sv_hostname TEST");

            //KickBOTsAll("");

        }


        void ShowHealth()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;
                Print(ent.Health);
            }
        }

        #region ++
        void AC130()
        {
            if (ac == null) ac = new AC130();
        }
        void Ospray()
        {
            if (osp == null) osp = new Ospray();
        }
        void Marker(string type)
        {
            if (marker == null) marker = new Marker();
            if (type == "selector")
            {
                marker._beginLocationSelection(ADMIN, "mobile_mortar", "map_artillery_selector", false, 500);
            }
            else if (type == "ti")
            {
                marker.TI();
            }
            else if (type == "adm")
            {
                marker.airdropMarker(ADMIN);
            }
            else if (type == "usm")
            {
                marker.uavStrikerMarker(ADMIN);
            }
        }
        void Fx(string type, string value)
        {
            if (fx == null) fx = new FX();
            if (type == "tfx") fx.triggerfx(value);
            else fx.PlayFX(null);
        }
        void Tank()
        {
            if (tank == null) tank = new TEST.Tank();
            tank.SetTank();
        }
        void Sound_(string type, string value)
        {
            if (sound == null) sound = new TEST.Sound();
            if (type == "so") sound.PlayLocalSound(value);
            else sound.PlaySound(value);
        }
        void Table_(string type, string value)
        {
            if (table == null) table = new Table();
            if (type == "model") table.GetModel();
            else if (type == "qq") table.tableValue(value);
            else if (type == "tn") table.GetTeamName(value);

        }
        void Vehicle_(string type, string value)
        {
            if (vehicle == null) vehicle = new Vehicle();
            if (type == "sp") vehicle.spawn(value);
            else if (type == "plane") vehicle.spawnPlane();
            else if (type == "rm") vehicle.remoteTestModel(value);
            else if (type == "turret") vehicle.spawnTurrent();
            else if (type == "heli") { }
            else if (type == "rmharr") vehicle.remoteHarrir();
            else if (type == "rmheli") vehicle.remoteHeli();
            else if (type == "end") vehicle.EndRemoteControl();
            else if (type == "start") vehicle.StartRemoteControl();
            else if (type == "uav") vehicle.StartRemoteUAV(ADMIN);
        }
        void Vision(string type, string value)
        {
            if (type == "v0")
            {
                /*
                    cobra_sunset3
                */
                Call("visionsetnaked", value, true);
            }
            else if (type == "v1")
            {
                /*
                cobra_sunset3
                ac130_thermal_mp 
                default_night_mp  

                thermal_snowlevel_mp thermal_mp 

                coup_sunblind
                mpnuke
                aftermath
                end_game
                */
                if (value == null) return;
                ADMIN.Call("visionsetnakedforplayer", value, 1f);
            }
            else if (type == "v2")
            {

                /*
                    cobra_sunset3 ac130_thermal_mp default_night_mp aftermath coup_sunblind
                    thermal_snowlevel_mp thermal_mp
                */
                if (value == null) return;
                ADMIN.Call("visionsetnightforplayer", value, 1f);
            }
            else if (type == "v3")
            {
                /*
                    //near_death_mp
                */

                if (value == null) return;
                ADMIN.Call("visionsetpainforplayer", value, 1f);
            }
            else if (type == "v4")
            {
                if (value == null) return;
                ADMIN.Call("VisionSetMissilecamForPlayer", value, 1f);
            }
        }

        string GetSO(string idx, Vector3 o)
        {
            int x = (int)o.X;
            int y = (int)o.Y;
            int z = (int)o.Z;

            return idx + "(" + x + "," + y + "," + z + ") ";
        }
        string GetSO2(string idx, Vector3 o)
        {
            int x = (int)o.X;
            int y = (int)o.Y;
            int xx = (int)ADMIN.Origin.X;
            int yy = (int)ADMIN.Origin.Y;
            xx = Math.Abs(x - xx);
            yy = Math.Abs(y - yy);
            return idx + "(" + x + "," + y + ")[x:" + xx + ", y:" + yy + "] ";
        }
        void SetSentryBotPos()
        {
            Vector3 angle = ADMIN.Call<Vector3>("getplayerangles");
            int ang = (int)angle.Y;
            if (ang > 0)
            {
                if (ang < 90)//2사분면
                {
                    angle.X = -50;
                    angle.Y = -50;
                }
                else//3사분면
                {
                    angle.X = +50;
                    angle.Y = -50;
                }
            }
            else
            {
                if (ang < -90)//3사분면
                {
                    angle.X = 50;
                    angle.Y = 50;
                }
                else//4사분면
                {
                    angle.X = -50;
                    angle.Y = +50;
                }
            }
            angle.Z = 0;
            Vector3 repos = ADMIN.Origin + angle;
            ADMIN.Call("setorigin", repos);
        }
        #endregion
        List<Vector3> SpawnPoints = new List<Vector3>();
        Vector3 tvector = new Vector3();
        string GetSimplePos(Vector3 v)
        {
            tvector.X = (int)v.X;
            tvector.Y = (int)v.Y;
            tvector.Z = (int)v.Z;
            return tvector.ToString();
        }
        Entity[] GetEntArray(string key, string value)
        {
            Entity[] EA = { };
            int j = 0;
            for (int i = 0; i < 1024; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;
                if (ent.GetField<string>(key) == value)
                {
                    EA[j] = ent;
                    j++;
                }
            }
            if (EA.Length > 0) return EA;
            return null;
        }
        void spawnPos()
        {

            if (SB == null) SB = new StringBuilder();
            SB.Length = 0;

            //SpawnPoints.Clear();
            SB.AppendLine("dom\n");
            for (int i = 17; i < 1024; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) break;
                if (ent.GetField<string>("classname") == "mp_dom_spawn")//"mp_dom_spawn")
                {
                    SB.AppendLine(GetSimplePos(ent.Origin));
                }
            }

            SB.AppendLine("inf\n");
            for (int i = 17; i < 1024; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) break;
                if (ent.GetField<string>("classname") == "mp_tdm_spawn")//"mp_dom_spawn")
                {
                    SB.AppendLine(GetSimplePos(ent.Origin));
                }
            }
            File.WriteAllText("z:\\mw3.txt", SB.ToString(), Encoding.Default);

        }
        public override void OnSay(Entity player, string name, string text)
        {
            if (ADMIN == null) GetADMIN();

            string[] texts = text.Split(' '); string value = null, txt = null;
            if (texts.Length > 1) { value = texts[1]; txt = texts[0]; } else txt = text;

            switch (txt)
            {
                case "spos": spawnPos(); break;
                case "s": SpawnModel(texts[1], texts[2]); break;
                case "wm": WriteModel(); break;
                case "health": ShowHealth(); break;
                case "move0": Call(42, "testClients_doMove", 0); break;
                case "crouch0": Call(42, "testClients_doCrouch", 0); break;
                case "move1": Call(42, "testClients_doMove", 1); break;
                case "crouch1": Call(42, "testClients_doCrouch", 1); break;

                //case "bp": ADMIN.TakeAllWeapons(); testBot(); break;
                //case "ag": SentryAngle(); break;
                //case "soff": SentryOffline(); break;
                //case "sron": SentryRemoteControl(true); break;
                //case "sroff": SentryRemoteControl(false); break;

                case "130": AC130(); break;
                case "osp": Ospray(); break;

                case "selector":
                case "ti":
                case "adm":
                case "usm": Marker(value); break;

                case "tank": Tank(); break;

                case "v0":
                case "v1":
                case "v2":
                case "v3":
                case "v4": Vision(txt, value); break;

                case "tfx":
                case "pfx": Fx(txt, null); break;

                case "st":
                case "so": Sound_(txt, value); break;

                case "sp":
                case "plane":
                case "rm":
                case "turret":
                case "heli":
                case "rmharr":
                case "rmheli":
                case "end":
                case "start":
                case "uav": Vehicle_(txt, value); break;

                case "model":
                case "qq":
                case "tn":
                case "qm": Table_(txt, value); break;

                //
                case "bc": Call(42, "testClients_doCrouch", 0); break;
                case "3rd": Viewchange(ADMIN); break;
                case "p": Print(GetSO("ORIGIN", ADMIN.Origin)); break;
                case "oo": ADMIN.Call("setorigin", new Vector3(0, 0, ADMIN.Origin.Z)); break;
                case "dow": ADMIN.Call("disableoffhandweapons"); break;
                case "bot": DeployBot(value); break;
                case "kb": KickBOTsAll(value); break;

                case "ulinf": Script("unloadscript inf.dll", true); break;
                case "linf": Script("loadscript inf.dll", true); break;
            }


        }

        #region QueryModel

        Entity SOLID_MODEL, BRUSH_MODEL;
        Entity GetBrushModel(string bm)
        {
            return Call<Entity>("getent", bm, "targetname");
        }
        Vector3 _30 = new Vector3(30, 30, 0);
        void SpawnModel(string brush_model, string solid_model)
        {
            if (SOLID_MODEL != null) SOLID_MODEL.Call("delete");
            if (BRUSH_MODEL != null) BRUSH_MODEL.Call("delete");
            BRUSH_MODEL = GetBrushModel(brush_model);

            SOLID_MODEL = Call<Entity>("spawn", "script_model", ADMIN.Origin + _30);
            SOLID_MODEL.Call("setmodel", solid_model);
            SOLID_MODEL.Call(33353, BRUSH_MODEL);
        }

        StringBuilder SB;
        void WriteModel()
        {
            if (SB == null) SB = new StringBuilder();
            SB.Length = 0;

            SB.AppendLine("# " + Call<string>(47, "mapname") + " script_model");

            for (int i = 0; i < 1024; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;
                string script_model = ent.GetField<string>("classname");
                if (script_model == "script_model")
                {
                    SB.AppendLine(ent.GetField<string>("model"));
                }
            }

            SB.AppendLine("# " + Call<string>(47, "mapname") + " script_brushmodel");
            for (int i = 0; i < 1024; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;
                string script_model = ent.GetField<string>("classname");
                if (script_model == "script_brushmodel")
                {
                    SB.AppendLine(ent.GetField<string>("targetname"));
                }
            }

            File.WriteAllText("z:\\mw3txt.txt", SB.ToString(), Encoding.Default);
        }

        #endregion

        bool third;
        void Viewchange(Entity player)
        {
            if (!third)
            {
                player.SetClientDvar("camera_thirdPerson", "1");
                player.SetClientDvar("camera_thirdPersonOffset", "-200");//default -120커지면확대 0-좌+우 14커지면 위에서, 작아지면 밑에서 봄
                player.SetField("3rd", true);
            }
            else
            {
                player.SetClientDvar("camera_thirdPerson", "0");
                player.SetField("3rd", false);
            }
            third = !third;
        }
        void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
        bool GetADMIN()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity p = Entity.GetEntity(i);

                if (p == null) continue;
                if (p.Name == "kwnav")
                {
                    test.ADMIN = p;
                    ADMIN.SpawnedPlayer += delegate
                    {
                        Print(GetSimplePos(ADMIN.Origin));
                    };
                    break;
                }
            }

            return true;
        }
        void Script(string command, bool restart)
        {
            AfterDelay(1000, () =>
            {
                Utilities.ExecuteCommand(command);
                if (restart)
                {
                    Utilities.ExecuteCommand("fast_restart");
                }
            });

        }
        void DeployBot(string value)
        {
            int num = 0;
            if (value == null) num = 0;
            else
            {
                int.TryParse(value, out num);
            }
            if (num == -1) return;

            for (int i = 0; i < num; i++)
            {
                Entity bot = Utilities.AddTestClient();
                if (bot == null) return;

                bot.SpawnedPlayer += () =>
                {
                    bot.Call("setorigin", test.ADMIN.Origin);
                };
            }
        }

        void KickBOTsAll(string value)
        {
            int num = 0;
            int.TryParse(value, out num);
            if (num == -1) num = 19;

            for (int i = 0; i < 18; i++)
            {
                if (i == num) continue;
                Entity ent = Entity.GetEntity(i);

                if (ent == null) continue;
                if (ent.Call<string>("getguid").Contains("bot"))
                {
                    Call("kick", i);
                }
            }
        }

    }
}
