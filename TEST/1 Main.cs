﻿using System;
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

        public test()
        {
            marker = new Marker();
            fx = new FX();
            vehicle = new Vehicle();
            sound = new Sound();
            table = new Table();
            tank = new Tank();
        }
        void model(Entity owner,string m)
        {
            Entity model = Call<Entity>("spawn", "script_model", ADMIN.Origin + new Vector3(0, 0, 20));
            model.Call("setmodel", m);
        }
        void move()
        {
            string value = Call<string>("tableLookupIString", "mp/factionTable.csv", 0, "allies", 1);
            Print(value);
        }

        string getTeam()
        {
            /*
game[teamref]

"delta_multicam": //US_
"sas_urban": //UK_
"gign_paris": //FR_
"pmc_africa": //PC_
"opforce_air":// RU_
"opforce_snow":// RU_
"opforce_urban":// RU_
"opforce_woodland":// RU_
"opforce_africa":// AF_
"opforce_henchmen": // IC_

*/

            
            return null;
        }
        void shoot(Entity owner)
        {

            //setturretmodechangewait

            string
                weaponInfo = "sentry_minigun_mp", 
                modelBase = "sentry_minigun_weak";

            Entity sentryGun = Call<Entity>("spawnTurret", "misc_turret", owner.Origin, weaponInfo);

            sentryGun.Call("setModel", modelBase);

            sentryGun.Call("setturretmodechangewait", true);
            sentryGun.Call("setcontents", 0);
            sentryGun.Call("setsentryowner", owner);
            //sentryGun.Call("setturretteam", "allies");
            sentryGun.Call("setsentrycarrier", owner);

            sentryGun.AfterDelay(1000, sg =>
            {
                //sentryGun.Call("setcontents", 1);
                //sentryGun.SetField("angles", owner.Call<Vector3>("getPlayerAngles"));

                sentryGun.Call(33084, 0f);//SetLeftArc
                sentryGun.Call(33083, 0f);//SetRightArc
                                          //sentryGun.Call("makeUnusable");
                sentryGun.Call("setmode", "sentry");//sentry sentry_offline
                                                   

                //sentryGun.Call("setmode", "sentry");
                //sentryGun.Call("setcandamage", true);
                //sentryGun.Call("setturretminimapvisible", true);

                //bool m = false;
                sentryGun.OnInterval(4000, sgs =>
                {
                    //if (!m)
                    //{
                    //    sentryGun.Call("setModel", "sentry_minigun_weak_obj");
                    //}
                    //else
                    //{
                    //    sentryGun.Call("setModel", modelBase);
                    //}
                    //m = !m;
                    //sentryGun.SetField("angles", owner.Call<Vector3>("getPlayerAngles"));

                    //Print(sentryGun.Health);
                    int i = 0;
                    sg.OnInterval(200, xx =>
                    {
                        if (i > 15)
                        {
                            return false;
                        }
                        i++;
                        sentryGun.Call("shootturret");
                        return true;
                    });

                    return true;
                });

            });

        }

        Ospray osp;
        public override void OnSay(Entity player, string name, string text)
        {
            if (ADMIN == null) GetADMIN();

            string[] texts = text.Split(' ');
            string value = null;
            string txt = null;
            if (texts.Length > 1)
            {
                value = texts[1];
                txt = texts[0];
            }else
            {
                txt = text;
            }
            
            switch (txt)
            {
                case "tn":  table.GetTeamName(value); break;
                   
                case "bc": Call(42, "testClients_doCrouch", 0); break;
                case "3rd": Viewchange(ADMIN); break; 
                case "mv":move();break;
                case "sh": shoot(ADMIN);break;
                case "130":AC130 ac = new AC130();break;
                case "osp": osp = new Ospray(); break;
                case "uav": vehicle.StartRemoteUAV(ADMIN); break;
                case "qm": QueryModel(); break;

                case "selector": marker._beginLocationSelection(ADMIN, "mobile_mortar", "map_artillery_selector", false, 500); break;
                case "ti": marker.TI(); break;
                case "marker": marker.airdropMarker(ADMIN); break;
                case "ww": marker.uavStrikerMarker(ADMIN); break;

                case "p": Print(ADMIN.Origin); break;

                case "ds": ADMIN.Call("disableoffhandweapons"); break;

                case "tank": tank.SetTank(); break;
                case "turret": vehicle.spawnTurrent(); break;
                case "heli": break;
                case "rmharr": vehicle.remoteHarrir(); break;
                case "rmheli": vehicle.remoteHeli(); break;
                case "end": vehicle.EndRemoteControl(); break;
                case "start": vehicle.StartRemoteControl(); break;

                case "v0": VisionGlobal(value); break;
                case "v1": VisionNaked(ADMIN, value); break;
                case "v2": VisionNight(ADMIN, value); break;
                case "v3": VisionPain(ADMIN, value); break;
                case "v4": VisionMissile(ADMIN, value); break;

                case "tfx": fx.triggerfx(value); break;
                case "pfx": fx.PlayFX(null); break;
                case "st": sound.PlaySound(value); break;
                case "so": sound.PlayLocalSound(value); break;

                case "qq": table.tableValue(value); break;

                case "sp": vehicle.spawn(value); break;
                case "plane": vehicle.spawnPlane(); break;
                case "rm": vehicle.remoteTestModel(value); break;
                case "model": table.GetModel();break;
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
        string GetFieldContent(string s)
        {
            return Query.GetField<string>(s);
        }
        #endregion

        void VisionGlobal(string value)
        {
            if (value == null) return;
            /*
                cobra_sunset3
            */
            Call("visionsetnaked", value, true);

            
        }
        void VisionNaked(Entity player, string value)
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
            if (value == null) return; player.Call("visionsetnakedforplayer", value, 1f);
        }
        void VisionNight(Entity player, string value)
        {
            /*
                cobra_sunset3 ac130_thermal_mp default_night_mp aftermath coup_sunblind
                thermal_snowlevel_mp thermal_mp
            */
            if (value == null) return; player.Call("visionsetnightforplayer", value, 1f);
            Print(value);
        }
        void VisionPain(Entity player, string value)
        {
            /*
                //near_death_mp
            */

            if (value == null) return; player.Call("visionsetpainforplayer", value, 1f);
            Print(value);
        }
        void VisionMissile(Entity player, string value)
        {
            if (value == null) return; player.Call("VisionSetMissilecamForPlayer", value, 1f);
            Print(value);
        }

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
        void ViewModel(string teamRef)
        {
            foreach (Entity p in Players)
            {
                var s = p.GetField<string>("model");
                Print(s);
            }
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

    }
}
