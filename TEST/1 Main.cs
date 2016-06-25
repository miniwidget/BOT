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

        public override void OnSay(Entity player, string name, string text)
        {
            if (ADMIN == null) GetADMIN();

            string[] texts = text.Split(' ');
            if (texts.Length == 1)
            {
                CommandsOne(text);
            }
            else
            {
                CommandsTwo(texts);
            }

        }
        string GetFieldContent(string s)
        {
            return Query.GetField<string>(s);
        }
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
        void CommandsOne(string text)
        {
            switch (text)
            {
                case "qm": QueryModel(); break;
                case "3rd": Viewchange(); break;
                case "fx": fx.LoadFX(text.Split(' ')[1]); break;

                case "selector": marker._beginLocationSelection(ADMIN, "mobile_mortar", "map_artillery_selector", false, 500); break;
                case "ti": marker.TI(); break;
                case "marker": marker.airdropMarker(); break;
                case "ww": marker.uavStrikerMarker(); break;

                case "p": Print(ADMIN.Origin); break;
                case "loc": ADMIN.Call("setorigin", new Vector3(-2943, 453, 527)); break;

                case "ds": ADMIN.Call("disableoffhandweapons"); break;

                case "tank": tank.SetTank(); break;
                case "turret": vehicle.spawnTurrent(); break;
                case "heli": break;
                case "rmharr": vehicle.remoteHarrir(); break;
                case "rmheli": vehicle.remoteHeli(); break;
                case "end": vehicle.EndRemoteControl(); break;
                case "start": vehicle.StartRemoteControl(); break;
            }
        }
        void CommandsTwo(string[] texts)
        {
            if (ADMIN == null) GetADMIN();
            string t = texts[0];
            string t1 = texts[1];
            switch (t)
            {
                case "lfx": fx.GetLoadFXInt(t1); break;
                case "fx": fx.LoadFX(t1); break;

                case "st": sound.PlaySound(t1); break;
                case "so": sound.PlayLocalSound(t1); break;

                case "tn": table.GetTeamName(t1); break;
                case "qq": table.tableValue(t1); break;

                case "sp": vehicle.spawn(t1); break;
                case "plane": vehicle.spawnPlane(); break;
                case "rm": vehicle.remoteTestModel(t1); break;
            }
        }


    }
}
