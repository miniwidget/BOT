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
            OnServerCommand("/", (string[] texts) =>
            {
                if (texts.Length == 1) return;
                string key = texts[1].ToLower();
                if (key == "start") SetTimer(true);
                else if (key == "stop") SetTimer(false);
            });
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
            else if (type == "uav")
            {
                Vector3 angle = ADMIN.Call<Vector3>("getplayerangles");
                Vector3 spawnPos = WEAPON.AnglesToForward(angle, ADMIN.Origin, 50);

                //StartRemoteUAV(ADMIN, spawnPos, angle);
            }

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
        Weapon WEAPON;
        void Weapon(string type, string value)
        {
            if (WEAPON == null) WEAPON = new Weapon();
            if (type == "mb") WEAPON.magicBullet(ADMIN);
            else if (type == "back") ADMIN.Call("setorigin", WEAPON.AnglesToBack(ADMIN.Call<Vector3>("getplayerangles"), ADMIN.Origin, 50));
            else if (type == "forward") ADMIN.Call("setorigin", WEAPON.AnglesToForward(ADMIN.Call<Vector3>("getplayerangles"), ADMIN.Origin, 50));
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

        int TIME_VALUE;
        List<Timer> TIMER_LIST = new List<Timer>();
        bool TIMER_INITIALIZED_;
        List<Entity> Bots_LIST;
        void TestVoid()
        {
            if (Bots_LIST == null)
            {
                Bots_LIST = new List<Entity>();
                for (int i = 0; i < 17; i++)
                {
                    Entity ent = Entity.GetEntity(i);
                    if (ent == null) continue;
                    if (ent.Name.StartsWith("bot"))
                    {
                        Bots_LIST.Add(ent);
                        Print("yes");
                    }
                }
            }
            string name = null;
            foreach (Entity bot in Bots_LIST)
            {
                name += bot.Name + " " + bot.Origin + " ";
            }
            Print(name);
        }
        void SetTimer(bool start)
        {
            if (!TIMER_INITIALIZED_)
            {

                TIMER_INITIALIZED_ = true;
                for (int i = 0; i < 18; i++)
                {

                    Timer t = new Timer();

                    t.Interval = 100;
                    t.Elapsed += delegate
                    {
                        TIME_VALUE++;
                    };

                    TIMER_LIST.Add(t);
                }

            }
            if (start)
            {
                Print("스타트");
                for (int i = 0; i < 18; i++)
                {
                    TIMER_LIST[i].Start();
                }
            }
            else
            {
                Print("스톱" + TIME_VALUE);
                for (int i = 0; i < 18; i++)
                {
                    TIMER_LIST[i].Stop();
                }
            }
        }

        Entity rp = null;

        internal Vector3 AnglesToForward(Vector3 angle, Vector3 v, int scalar)
        {

            float hor = angle.Y;
            float vert = angle.X;
            float y = (float)Math.Abs(Math.Tan(angle.Y / 57.3) * scalar);

            float x0 = v.X;
            float y0 = v.Y;

            if (hor > 0)
            {
                if (hor < 90)//2사분면
                {
                    v.X += scalar;
                }
                else//3사분면
                {
                    v.X -= scalar;
                }
                v.Y += y;
            }
            else
            {
                if (hor > -90)//1사분면
                {
                    v.X += scalar;
                }
                else//4사분면
                {
                    v.X -= scalar;
                }
                v.Y -= y;
            }

            //x0 = v.X - x0;
            //y0 = v.Y - y0;
            //float dist = (float)Math.Sqrt(x0 * x0 + y0 * y0);
            //float radians = angle.X * 57.3f;

            //if (angle.Z < 0)//위로 보고 있을 경우
            //{
            //    v.Z += dist * (float)Math.Tan(radians);
            //}
            //else//아래로 보고 있을 경우
            //{
            //    v.Z -= dist * (float)Math.Tan(radians);
            //}
            v.Z += 500;
            return v;
        }
        internal Vector3 AnglesToBack(Vector3 angle, Vector3 v, int scalar)
        {
            float hor = angle.Y;
            float vert = angle.X;
            float y = (float)Math.Abs(Math.Tan(angle.Y / 57.3) * scalar);

            if (hor > 0)
            {
                if (hor < 90)//2사분면
                {
                    v.X -= scalar;
                }
                else//3사분면
                {
                    v.X += scalar;
                }
                v.Y -= y;
            }
            else
            {
                if (hor > -90)//1사분면
                {
                    v.X -= scalar;
                }
                else//4사분면
                {
                    v.X += scalar;
                }
                v.Y += y;
            }
            v.Z += 1000;
            return v;
        }

        internal void magicBullet(Entity player)
        {
            player.Call("setorigin", new Vector3(-2944, -29, 527));
            player.OnNotify("weapon_fired", (p, weaponName) =>
            {
                if (rp != null) return;
                Vector3 angle = player.Call<Vector3>("getPlayerAngles");
                Vector3 handpos = player.Call<Vector3>("getTagOrigin", "tag_weapon_left");

                Vector3 startPos = AnglesToBack(angle, handpos, 2000);
                Vector3 endPos = AnglesToForward(angle, handpos, 1000);

                rp = Call<Entity>("MagicBullet", "remotemissile_projectile_mp", startPos, endPos, player);

                //Call<Vector3>("anglestoforward", player.Call<Vector3>("getPlayerAngles")) * 1000000, // end point
                //player); // ignore entity
                rp.Call("setCanDamage", true);
                player.Call("VisionSetMissilecamForPlayer", "black_bw", 0f);
                player.Call("VisionSetMissilecamForPlayer", "thermalVision", 1f);
                player.Call("CameraLinkTo", rp, "tag_origin");
                player.Call("ControlsLinkTo", rp);
                rp.OnNotify("death", _rp =>
                {
                    player.Call("ControlsUnlink");
                    player.Call("freezeControls", false);
                    player.Call("CameraUnlink");
                    player.Notify("stopped_using_remote");
                    rp = null;
                });
            });
        }
        void smaw(Entity player)
        {
            player.GiveWeapon("smaw_mp");
            player.SwitchToWeaponImmediate("smaw_mp");

            Print("왔다");
            player.Call("notifyonplayercommand", "fff", "+toggleads_throw");
            player.OnNotify("fff", ent =>
            {
                string weapon = ent.CurrentWeapon;
                Print(weapon);
                if (weapon != "iw5_smaw_mp") return;
                player.Call(33469, weapon, 1);//setweaponammostock
                player.Call(33468, weapon, 1);//setweaponammoclip
                if (rp != null) return;
                Vector3 angle = player.Call<Vector3>("getPlayerAngles");
                Vector3 handpos = player.Call<Vector3>("getTagOrigin", "tag_weapon_left");

                Vector3 startPos = AnglesToBack(angle, handpos, 2000);
                Vector3 endPos = AnglesToForward(angle, handpos, 1000);

                rp = Call<Entity>("MagicBullet", "remotemissile_projectile_mp", startPos, endPos, player);

                //Call<Vector3>("anglestoforward", player.Call<Vector3>("getPlayerAngles")) * 1000000, // end point
                //player); // ignore entity
                rp.Call("setCanDamage", true);
                player.Call("VisionSetMissilecamForPlayer", "black_bw", 0f);
                player.Call("VisionSetMissilecamForPlayer", "thermalVision", 1f);
                player.Call("CameraLinkTo", rp, "tag_origin");
                player.Call("ControlsLinkTo", rp);
                rp.OnNotify("death", _rp =>
                {
                    player.Call("ControlsUnlink");
                    player.Call("freezeControls", false);
                    player.Call("CameraUnlink");
                    //player.Notify("stopped_using_remote");
                    rp = null;
                });
            });

        }
        Entity remoteUAV;
        internal void StartRemoteUAV(Entity player)
        {
            //if (Call<int>("getdvarint", "camera_thirdPerson") == 1)
            //{

            //}
            Vector3 v30 = new Vector3(0, 0, 30);
            //if (WEAPON == null) WEAPON = new TEST.Weapon();
            //player.GiveWeapon("uav_remote_mp");
            //player.SwitchToWeaponImmediate("uav_remote_mp");
            //player.Call("VisionSetNakedForPlayer","black_bw", 0f );
            //player.Notify("remoteuav_unlock");

            //Vector3 angle = player.Call<Vector3>("getplayerangles");
            //Vector3 spawnPos = WEAPON.AnglesToForward(player.Origin, ADMIN.Origin, 50);

            //Print(spawnPos + "." + angle);
            remoteUAV = Call<Entity>("spawnhelicopter", player, player.Origin + new Vector3(0,0,250), new Vector3(),
                 "attack_littlebird_mp",//minimap model
                 "vehicle_remote_uav");//real model vehicle_remote_uav
        
            Entity TL = Call<Entity>(19, "misc_turret", player.Origin + v30, "sentry_minigun_mp", false);
            TL.Call(32929, "weapon_minigun");
            TL.Call(32841, remoteUAV, "tag_light_nose", new Vector3(0,0,-100), new Vector3());
            TL.Call(33084, 180f);
            TL.Call(33083, 180f);
            TL.Call(33086, 180f);

       
            //Entity carepackage = Call<Entity>("spawn", "script_model", remoteUAV.Origin + v50);
            //carepackage.Call("setmodel", "com_plasticcase_friendly");
            //carepackage.Call("linkto", remoteUAV);

            //pack.Call(33353, brushmodel);

            //player.Call("setorigin", pack.Origin + new Vector3(0, 0, 50));
            //player.Call("linkto", pack);
            //player.Call("setorigin", carepackage.Origin);


            //ADMIN.Call("linkto", carepackage);
            //player.Call("cameralinkto", remoteUAV,"tag_origin");

            //player.Call("remotecontrolvehicle", remoteUAV);

            //TL.Call("hide");
            //return;
            //ADMIN.Call("controllinkto", carepackage);
            //int fx = Call<int>("loadfx", "misc/aircraft_light_wingtip_green");
            //remoteUAV.AfterDelay(200, c =>
            //{
            //    Call("playFXOnTag", fx, remoteUAV, "tag_light_tail1");
            //    Call("playFXOnTag", fx, remoteUAV, "tag_light_nose");
            //});
        }
        void testremote()
        {
            ADMIN.Call(33256, remoteUAV);
            //for(int i = 0; i < 17; i++)
            //{
            //    Entity ent = Entity.GetEntity(i);
            //    if (ent == null) continue;
            //    if (ent.Name.StartsWith("bot"))
            //    {
            //        ent.Call(33256, remoteUAV);
            //        Call(42, "testClients_doMove", 1);

            //    }
            //}
        }
        public override void OnSay(Entity player, string name, string text)
        {
            if (ADMIN == null) GetADMIN();

            string[] texts = text.Split(' '); string value = null, txt = null;
            if (texts.Length > 1) { value = texts[1]; txt = texts[0]; } else txt = text;

            switch (txt)
            {
                case "smaw": smaw(ADMIN); break;
                //case "rr": testremote(); break;
                //case "ru": StartRemoteUAV(ADMIN); break;
                case "back":
                case "forward":
                case "mb": Weapon(txt, value); break;

                case "start": SetTimer(true); break;
                case "stop": SetTimer(false); break;

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
                //case "end":
                //case "start":
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
