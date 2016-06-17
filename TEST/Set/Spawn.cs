using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace TEST
{
    public partial class test
    {
        void spawnTurrent()
        {
            CreateTurret(ADMIN.Origin + new Vector3(0, 0, 50), new Vector3(0f, 90f, 0f));
            ////	mgTurret = SpawnTurret( "misc_turret", lb.origin, level.heliGuardSettings[ heliGuardType ].weaponInfo );

            //TURRET = Call<Entity>("spawnturret", "misc_turret", ADMIN.Origin + new Vector3(0, 0, 150), "sentry_minigun_mp");
            //TURRET.Call(32841, VEHICLE, "tag_origin", new Vector3(30, 30, 0), new Vector3(0, 0, 0));
            //TURRET.Call("setmodel", "weapon_minigun");//setmodel mp_remote_turret
            //                                          //리얼모델 weapon_minigun , mp_remote_turret
            //                                          //스크립트 pavelow_minigun_mp, littlebird_guard_minigun_mp , sentry_minigun_mp , manned_minigun_turret_mp
        }

        private void CreateTurret(Vector3 location, Vector3 angles)
        {
            Entity turret = Call<Entity>("spawn", "script_model");
            turret.SetField("angles", new Parameter(angles));
            if (angles.Equals(null)) angles = new Vector3(0f, 90f, 0f);
            turret = Call<Entity>("spawnTurret", "misc_turret", new Parameter(location), "sentry_minigun_mp");
            turret.Call("setmodel", "weapon_minigun");
            turret.SetField("angles", angles);
        }


//getstartoriginB

#region test
Entity TEST_SCRIPT_MODEL;
        void spawn(string model)
        {
            if (TEST_SCRIPT_MODEL != null) TEST_SCRIPT_MODEL.Call("delete");
            //model = "vehicle_phantom_ray";
            TEST_SCRIPT_MODEL = Call<Entity>("spawn", "script_model", Origin);
            TEST_SCRIPT_MODEL.Call("setmodel", model);
        }
        void remoteTestModel(string realModel)
        {
            if (VEHICLE != null)  EndRemoteControl();
            
            string minimap_model = "attack_littlebird_mp";
            VEHICLE = Call<Entity>("spawnhelicopter", ADMIN, Origin, Angles, minimap_model, realModel);
            //ADMIN.Notify("using_remote");
            ADMIN.Call("remotecontrolvehicle", VEHICLE);
        }
        #endregion
        #region 움직임
        /// <summary>
        /// MoveTo(position, time);
        /// </summary>
        void moveTo(Action act, int sec)
        {
            VEHICLE.Call("moveto", Origin, sec);
            VEHICLE.AfterDelay(sec * 1100, v => act.Invoke());
        }

        void rotateTo()
        {

        }
        #endregion

        #region Entity
        Entity KILL_CAM;
        Entity KillCam()
        {
            KILL_CAM = Call<Entity>("Spawn", "script_model", killCamOrigin);
            KILL_CAM.Call("LinkTo", VEHICLE, "tag_origin");
            return KILL_CAM;
        }
        Entity TURRET;

        #endregion

        #region remote
        /// <summary>
        /// plane = spawnplane( owner, "script_model", origin, "allie compass icon", "enemy compass icon" );
        /// </summary>
        void spawnPlane()
        {
            Entity JET = Call<Entity>("spawnplane", ADMIN, "script_model", ADMIN.Origin + new Vector3(0, 2000, 1000), "compass_objpoint_airstrike_friendly", "compass_objpoint_airstrike_busy");
            JET.Call("setmodel", "vehicle_mig29_desert"); //for model, a jet example
                                                          //jet.angles = (pitch,yaw,roll); //for angles
                                                          //jet EnableLinkTo(); //to be possible to link to it
            moveTo(null, 5);
            rotateTo();//JET.Call("rotateto" RotateTo((pitch,yaw,roll),time); //to rotate it
            ADMIN.Call("linkto", JET, "tag_origin", new Vector3(0, 100, 100), Angles);

            ADMIN.Notify("using_remote");
            ADMIN.Call(33256, JET);
        }
        void remoteHarrir()
        {
            if (isVehicleOn) return;
            // vehicle_b2_bomber | harrier_mp
            // vehicle_mig29_desert | harrier_mp
            // vehicle_av8b_harrier_jet_mp | harrier_mp

            //var o = ADMIN.Origin; o.Z += 1000;
            string realModel = "vehicle_b2_bomber";//vehicle_av8b_harrier_jet_mp
            string minimap_model = "harrier_mp";
            VEHICLE = Call<Entity>("spawnhelicopter", ADMIN, ADMIN.Origin + new Vector3(0, 0, 1000), ADMIN.GetField<Vector3>("angles"), minimap_model, realModel);

            //TURRET = Call<Entity>("spawnturret", "misc_turret", VEHICLE.Origin, "littlebird_guard_minigun_mp", false);
            //TURRET.Call(32841, VEHICLE, "tag_origin", new Vector3(30, 30, 0), new Vector3(0, 0, 0));
            //TURRET.Call(32929, "mp_remote_turret");//setmodel mp_remote_turret

            //VEHICLE.Call("moveto", ADMIN.Origin + new Vector3(0, 0, 100), 5);
            //StartRemoteControl();
            //    bot.Call("linkto", VEHICLE, "tag_origin", TAG_ORIGIN[0], TAG_ORIGIN[1]);
            VEHICLE.OnInterval(6000, v =>
            {
                if (VEHICLE == null) return false;

                var o = ADMIN.Origin + new Vector3(0, 0, 100);
                VEHICLE.Call("moveto", o, 5);

                return true;
            });
        }

        void remoteHeli()
        {
            if (isVehicleOn) return;

            var o = ADMIN.Origin; o.Z += 100; o.X += 1000;
            string realModel = null;
            string minimap_model = null;
            int i = 2;
            switch (i)
            {
                case 0: minimap_model = "cobra_mp"; realModel = "vehicle_apache_mp"; break;
                case 1: minimap_model = "cobra_mp"; realModel = "vehicle_cobra_helicopter_fly_low"; break;
                case 2: minimap_model = "attack_littlebird_mp"; realModel = "vehicle_little_bird_armed"; break;
                case 3: minimap_model = "pavelow_mp"; realModel = "vehicle_pavelow"; break;
            }
            var spawnPos = ADMIN.Origin + new Vector3(500, 500, 500);
            VEHICLE = Call<Entity>("spawnhelicopter", ADMIN, spawnPos, Angles, minimap_model, realModel);
            o = ADMIN.Origin + new Vector3(0, 0, 200);

            Entity turret = Call<Entity>("spawn", "script_model");
            turret.SetField("angles", new Vector3(0f, 90f, 0f));
            turret = Call<Entity>("spawnTurret", "misc_turret", VEHICLE.Origin, "sentry_minigun_mp", false);
            turret.Call("setmodel", "weapon_minigun");
            
            //tag origin angle
            turret.Call(32841, VEHICLE, "tag_minigun_attach_left", new Vector3(30, 30, 0), new Vector3(0, 0, 0));
            VEHICLE.Call("moveto", o, 5);

            //TURRET = Call<Entity>("spawnturret", "misc_turret", VEHICLE.Origin, "littlebird_guard_minigun_mp", false);
            //TURRET.Call(32841, VEHICLE, "tag_origin", new Vector3(30, 30, 0), new Vector3(0, 0, 0));
            //TURRET.Call("setmodel", "mp_remote_turret");//setmodel mp_remote_turret

            ADMIN.AfterDelay(10000, a =>
            {
                //giveWeaponTo("xm25_mp");
                //ADMIN.Call("disableoffhandweapons");//disableoffhandweapons
                //ADMIN.Notify("using_remote");
                ADMIN.Call("remotecontrolvehicle", VEHICLE);//remotecontrolvehicle
                //ADMIN.Call("remotecontrolturret", TURRET);
            });

            //bot.Call("linkto", VEHICLE, "tag_origin", TAG_ORIGIN[0], TAG_ORIGIN[1]);

        }

        void StartRemoteControl()
        {
            if (VEHICLE == null) return;
            ADMIN.SetClientDvar("cg_thirdperson", "0");
            ADMIN.SetClientDvar("camera_thirdPerson", "1");
            ADMIN.SetClientDvar("camera_thirdPersonCrosshairOffset", "0");//default 0.35 //0
            ADMIN.SetClientDvar("camera_thirdPersonOffsetAds", "-20 10 8");//default 2
            ADMIN.SetClientDvar("camera_thirdPersonOffset", "-50 10 8");//default -120커지면확대 0-좌+우 14커지면 위에서, 작아지면 밑에서 봄


            giveWeaponTo("mortar_remote_zoom_mp");

            ADMIN.Call(33503);//disableoffhandweapons
            ADMIN.Notify("using_remote");

            ADMIN.Call(33256, VEHICLE);//remotecontrolvehicle
            ADMIN.Call(32979, TURRET);//remotecontrolturret

        }
        void EndRemoteControl()
        {
            if (VEHICLE == null) return;

            ADMIN.Call(32843);//unlink
            ADMIN.Call(33257);//remotecontrolvehicleoff
            ADMIN.Call(32980);//remotecontrolturretoff

            VEHICLE.Call(32928);//delete
            TURRET.Call(32928);//delete 

            VEHICLE = null;
            TURRET = null;

            //ADMIN.SetClientDvar("camera_thirdPerson", "0");
            //ADMIN.SetClientDvar("cg_thirdperson", "0");

        }
        void helicopter2(Entity player)
        {
            Restore(player, true);

            string[] tag = { "tag_player", "tag_light_belly", "tag_origin", "tag_minigun_attach_left" };

            //spawn LB & turret
            var origin = player.Origin;
            origin.Z += 200;

            Entity LB = Call<Entity>(369, player, origin, player.GetField<Vector3>("angles"), "littlebird_mp", "vehicle_little_bird_armed");
            Entity turret = Call<Entity>(19, "misc_turret", player.Origin, "littlebird_guard_minigun_mp", false);
            turret.Call(32841, LB, tag[3], new Vector3(30, 30, 0), new Vector3(0, 0, 0));
            turret.Call(32929, "mp_remote_turret");//setmodel mp_remote_turret
            turret.Call(32942);//MakeUnusable
            turret.Call(33052);//maketurretsolid
            turret.Call(33417, true);//setcandamage

            //player side
            //giveWeaponTo(player, "mortar_remote_zoom_mp");

            player.Call(33503);//disableoffhandweapons
            player.Notify("using_remote");

            player.Call(33256, LB);//remotecontrolvehicle
            player.Call(32979, turret);//remotecontrolturret

            player.AfterDelay(3000, p =>
            {
                //LB.Notify("drop_crate");
            });

            player.AfterDelay(60000, e =>
            {
                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff
                player.Call(32980);//remotecontrolturretoff
                turret.Call(32928);//delete
                LB.Call(32923);//stoploopsound

                LB.Call(33413, Call<Vector3>(251, LB.Origin), 100, 4, 2);//vibrate

                LB.AfterDelay(2000, ex =>
                {
                    Call(304, 96, LB.Origin);//playfx

                    LB.Call(32915, "cobra_helicopter_crash");//playsound

                    LB.Call(32928);//delete

                });
                Restore(player, false);
            });

            // lbSupport_lightFX(LB);
        }
        #endregion



        #region --
        void moveHelicopter()
        {
            var dropSite = ADMIN.Origin;
            var dropYaw = ADMIN.Origin;
            pathGoal = new Vector3(dropSite.X, dropSite.Y, 0) + new Vector3(0, 0, dropSite.Z + 1000);
            pathStart = dropSite + new Vector3(1000, 1000, 0); //getPathStart(pathGoal, dropYaw);
            pathEnd = dropSite + new Vector3(1000, 1000, 0);// getPathEnd(pathGoal, dropYaw);

            pathGoal = ADMIN.Origin + new Vector3(0, 500, 200);//pathGoal + (AnglesToForward(new Vector3(0,dropYaw.Y,0))* -50);

            Entity chopper = heliSetup(ADMIN, pathStart, pathGoal);
            setVehGoalPos(chopper, pathGoal, 1);
            //chopper.Call("dropTheCrate", ADMIN.Origin,)
            //chopper.Call("waittill", "goal");
            //	chopper thread dropTheCrate( dropSite, dropType, flyHeight, false, crateOverride, pathStart );

            AfterDelay(10000, () =>
            {
                chopper.Notify("lbSupport_moveToPlayer");
                //chopper.Notify("drop_crate");
                //print("notified");
            });
            //print("test");
        }
        void helicopter_wait_anotherPlayer(Entity player)
        {
            Entity LB = Call<Entity>(369, player, player.Origin, player.GetField<Vector3>("angles"), "littlebird_mp", "vehicle_little_bird_armed");
        }
        /// <summary>
        /// TEST
        /// </summary>
        /// <param name="player"></param>
        //헬리콥터 봇
        #region 헬리콥터 봇
        void spawnd_bot_heli(Entity player)
        {
        }
        void helicopter(Entity player)
        {

        }
        #endregion

        // http://pastebin.com/c2xDcfGd
        // http://pastebin.com/Bb1mqEua
        // http://pastebin.com/aP0caf99
        #endregion
    }
}
