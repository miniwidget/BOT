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

        Entity SG;
        Entity SpawnSentry()
        {
            SG = Call<Entity>(19, "misc_turret", ADMIN.Origin, "sentry_minigun_mp");
            var angle = ADMIN.Call<Vector3>("getplayerangles");
            SG.SetField("angles", new Vector3(0, angle.Y, 0));
            SG.Call(33084, 80f);//SetLeftArc
            SG.Call(33083, 80f);//SetRightArc
            SG.Call("setturretminimapvisible", true);
            SG.Health = 700;
            return SG;
        }
        bool SentryStopFire;
        bool SentryExplode()
        {
            SG.Call("setmodel", "sentry_minigun_weak_destroyed");
            SentryStopFire = true;
            int i = Call<int>("loadfx", "explosions/sentry_gun_explosion");
            SG.Call("SetTurretMinimapVisible", false);
            SG.Call("playSound", "sentry_explode");
            Call("playfxontag", i, SG, "tag_origin");
            SG.AfterDelay(3000, sg =>
            {
                SG.Call("delete");
                SG = null;
            });
            return false;
        }
        void SentryOnline()
        {
            if (SG != null)
            {
                SentryStopFire = true;
                SG.Call("delete"); SG = null;
            }
            if (SG == null) SG = SpawnSentry();
            SentryStopFire = false;
            //int count = 40;
            //SG.OnInterval(100, sg =>
            //{
            //    if (SG == null) return false;
            //    if (SG.Health < 0)
            //    {
            //        return SentryExplode();
            //    }
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (!SentryStopFire) SG.Call("shootTurret");
            //    }
            //    return true;
            //});

            SG.Call(32929, "sentry_minigun_weak");//setModel

            SG.Call(33007, false);//setsentrycarrier
            SG.Call("setCanDamage", true);
            SG.Call("makeTurretSolid");
            ADMIN.SetField("isCarrying", false);

            SG.Call(32864, "sentry");//setmode : sentry sentry_offline
            SG.Call("makeUsable");
            SG.Call("SetDefaultDropPitch", -89f);
            SG.Call("playSound", "sentry_gun_plant");
            SG.Notify("placed");
            SG.Call(33051, "axis");//setturretteam
            //SG.Call(33006, Players[3]);//setsentryowner

            
        }
        void SentryOffline()
        {
            SentryStopFire = true;
            if (SG == null) SG = SpawnSentry();
            SG.Call(32929, "sentry_minigun_weak_obj");//setModel
                                                      //
            SG.Call(33051, "allies");//setturretteam
            SG.Call(33006, ADMIN);//setsentryowner
            SG.Call(33007, ADMIN);//setsentrycarrier

            SG.Call(32864, "sentry_offline");//setmode : sentry sentry_offline
        }
        void SentryRemoteControl(bool control)
        {
            if (control) {
                ADMIN.Call("remotecontrolturret", SG);
                SG.Call(32929, "sentry_minigun_weak");//setModel
            }
            else
            {
                ADMIN.Call("remotecontrolturretoff", SG);
                SG.Call(32929, "sentry_minigun_weak_obj");//setModel
            }
        }
        void SentryAngle()
        {
            if (testbot == null) testBot();
            
            ADMIN.OnInterval(200, a =>
            {
                var tagback = testbot.Origin;tagback.Z += 25;//Call<Vector3>(33128, "tag_origin");//"GetTagOrigin"
                var myback = SG.Call<Vector3>(33128, "tag_aim");
                Vector3 angle = Call<Vector3>(247, tagback - myback);//vectortoangles
                ADMIN.Call(33531, angle);//SetPlayerAngles
                return true;
            });
        }
        Entity testbot;
        void testBot()
        {
            if (testbot == null)
            {
                Entity bot = Utilities.AddTestClient();
                if (bot != null) testbot = bot;
            }
            Vector3 or= new Vector3();
            testbot.SpawnedPlayer += delegate
            {
                testbot.Notify("menuresponse", "team_marinesopfor", "allies");
                testbot.Call("setorigin", or);
            };
            testbot.AfterDelay(1000, t =>
            {
                testbot.Call("setorigin", ADMIN.Origin);
                or = ADMIN.Origin;
            });
        }
        void ShowHealth()
        {
            for(int i = 0; i < 18; i++)
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
            else if (type == "qm") QueryModel();

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


        public override void OnSay(Entity player, string name, string text)
        {
            if (ADMIN == null) GetADMIN();

            string[] texts = text.Split(' '); string value = null, txt = null;
            if (texts.Length > 1) { value = texts[1]; txt = texts[0]; } else  txt = text;

            switch (txt)
            {
                case "health": ShowHealth();break;
                case "move0": Call(42, "testClients_doMove", 0); break;
                case "crouch0": Call(42, "testClients_doCrouch", 0); break;
                case "move1": Call(42, "testClients_doMove", 1); break;
                case "crouch1": Call(42, "testClients_doCrouch", 1); break;
                case "ag": SentryAngle(); break;
                case "bp": ADMIN.TakeAllWeapons(); testBot(); break;
                //case "a": SetSentryBotPos(); break;
                case "soff": SentryOffline(); break;
                case "sron": SentryRemoteControl(true); break;
                case "sroff": SentryRemoteControl(false); break;

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
        Entity Query;
        void QueryModel()
        {
            //Entity vehicle_brushmodel = Call<Entity>("getent", "destructible_vehicle", "targetname");
            //if (vehicle_brushmodel != null)
            //{

            //    Entity model = Call<Entity>("spawn", "script_model", ADMIN.Origin+ new Vector3(0,0,50));
            //    model.Call("setmodel", "destructible_vehicle");
            //    model.SetField("angles", new Vector3());
            //    model.Call(33353, vehicle_brushmodel);
            //}
            //return;
            for (int i = 0; i < 2048; i++)
            {
                Entity e = Entity.GetEntity(i);
                if (e == null) continue;
                var fieldname = e.GetField<string>("player");
                if (fieldname != null)
                {
                    Print(fieldname + e.EntRef);
                }
            }
            return;
            Query = Call<Entity>("getEntArray", "care_package", "targetname");
            if (Query == null) Print("꽝");
            else
            {
                string s = Query.Name;
                Print(s);
                //Print(GetFieldContent("classname"));
                //Print(GetFieldContent("targetname"));
                //airdropCollision = Call<Entity>("getent", care_package.GetField<string>("target"), "targetname");
            }
            var ent = Call<Entity>("getent", "script_model", "classname");

            var ent2 = Call<Entity>("getent", "???????????", "targetname");
            //code_classname
            //target
            //script_linkname

            /*
            ■ classname

            player
            script_origin
            script_model



            ■ targetname
            destructible_vehicle
            destructible
            destructible_toy
            light_destructible
            toggle
             vending_machine
             civilian_jet_origin
             grenade
             remoteMissileSpawn

            Function.AddMapping("laseron", 32930);
            Function.AddMapping("laseroff", 32931);
            Function.AddMapping("laseraltviewon", 32932);
            Function.AddMapping("laseraltviewoff", 32933);

             */
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
