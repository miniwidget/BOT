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
        SentryGun SG;
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
        }

        void Sentry(string type)
        {
            if (SG == null) SG = new SentryGun();
            if (type=="s1") SG.SentryMode(ADMIN, "sentry_offline");
            else SG.SentryMode(ADMIN, "sentry");
        }
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
            else if(type=="usm")
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
        void Vision(string type)
        {

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
        void Vehicle_(string type,string value)
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
            else if (type =="start") vehicle.StartRemoteControl(); 
            else if (type =="uav")vehicle.StartRemoteUAV(ADMIN); 
        }
        void Vision(string type, string value)
        {
            if(type == "v0")
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
            }
            else
            {
                txt = text;
            }

            switch (txt)
            {

                case "s1": 
                case "s2": Sentry(value); break;

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
                case "uav":Vehicle_(txt, value);break;

                case "model": 
                case "qq":
                case "tn":
                case "qm": Table_(txt, value); break;

                //
                case "bc": Call(42, "testClients_doCrouch", 0); break;
                case "3rd": Viewchange(ADMIN); break;
                case "p": Print(ADMIN.Origin); break;
                case "dow": ADMIN.Call("disableoffhandweapons"); break;

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

    }
}
