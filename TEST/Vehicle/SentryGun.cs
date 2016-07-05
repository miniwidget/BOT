using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class SentryGun : InfinityBase
    {
        internal Entity SENTRY_GUN;
        Entity ADMIN;

        public SentryGun()
        {
            ADMIN = test.ADMIN;
            test.ADMIN.AfterDelay(100, x =>
            {
                SentrySpawn(test.ADMIN, "sentry_offline");
            });
            
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
            if (control)
            {
                ADMIN.Call("remotecontrolturret", SG);
                SG.Call(32929, "sentry_minigun_weak");//setModel
            }
            else
            {
                ADMIN.Call("remotecontrolturretoff", SG);
                SG.Call(32929, "sentry_minigun_weak_obj");//setModel
            }
        }
        Entity testbot;
        void testBot()
        {
            if (testbot == null)
            {
                Entity bot = Utilities.AddTestClient();
                if (bot != null) testbot = bot;
            }
            Vector3 or = new Vector3();
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
        void SentryAngle()
        {
            if (testbot == null) testBot();

            ADMIN.OnInterval(200, a =>
            {
                var tagback = testbot.Origin; tagback.Z += 25;//Call<Vector3>(33128, "tag_origin");//"GetTagOrigin"
                var myback = SG.Call<Vector3>(33128, "tag_aim");
                Vector3 angle = Call<Vector3>(247, tagback - myback);//vectortoangles
                ADMIN.Call(33531, angle);//SetPlayerAngles
                return true;
            });
        }

        internal bool SentrySpawn(Entity bot, string mode)
        {
            // SENTRY_GUN.Call("delete");
            if (SENTRY_GUN == null)
            {
                SENTRY_GUN = Call<Entity>(19, "misc_turret", bot.Origin, "sentry_minigun_mp");//spawnTurret
                SENTRY_GUN.Call(32929, "sentry_minigun_weak");//setModel
            }

            if (mode == "sentry")
            {
                SENTRY_GUN.Call("setturretminimapvisible", true);
                SENTRY_GUN.Call(33006, bot);//setsentryowner
                SENTRY_GUN.Call(33051, "axis");//setturretteam
                SENTRY_GUN.Call(33084, 130f);//SetLeftArc
                SENTRY_GUN.Call(33083, 130f);//SetRightArc

            }
            else
            {
                SENTRY_GUN.Call(33007, bot);//setsentrycarrier
            }

            SENTRY_GUN.Call(32864, mode);//setmode : sentry sentry_offline
            return true;
        }

        internal bool SentryMode(Entity bot, string mode)
        {
            if (SENTRY_GUN != null)
            {
                SENTRY_GUN.Call("delete");
            }
            else
            {
                SENTRY_GUN = Call<Entity>(19, "misc_turret", bot.Origin, "sentry_minigun_mp");//spawnTurret
                SENTRY_GUN.Call(32929, "sentry_minigun_weak");//setModel

                SENTRY_GUN.Call(33006, bot);//setsentryowner
                SENTRY_GUN.Call(33051, "allies");//setturretteam
                SENTRY_GUN.Call(33084, 90f);//SetLeftArc
                SENTRY_GUN.Call(33083, 90f);//SetRightArc    
                SENTRY_GUN.Call("setturretminimapvisible", true);
                SENTRY_GUN.Call("makeunUsable");
            }

           
            if (mode == "sentry")
            {
                bot.Notify("carried");
                SENTRY_GUN.Call("makeTurretSolid");
                SENTRY_GUN.Call(33007, "");//setsentrycarrier
                SENTRY_GUN.Call("setcandamage", true);
            }
            else if (mode == "sentry_offline")
            {
                SENTRY_GUN.Call("setcandamage", false);
                SENTRY_GUN.Call("setcontents", 0);
                SENTRY_GUN.Call(33007, bot);//setsentrycarrier
            }

            SENTRY_GUN.Call(32864, mode);//setmode : sentry sentry_offline
            return true;
        }


        internal void shoot(Entity owner)
        {

            //setturretmodechangewait

            string
                weaponInfo = "sentry_minigun_mp",
                modelBase = "sentry_minigun_weak";

            Entity sentryGun = Call<Entity>("spawnTurret", "misc_turret", owner.Origin, weaponInfo);

            sentryGun.Call("setModel", modelBase);
            sentryGun.Call("setmode", "sentry_offline");//sentry sentry_offline
            sentryGun.Call("setturretmodechangewait", true);
            //sentryGun.Call("setcontents", 0);
            sentryGun.Call("setsentryowner", owner);
            sentryGun.Call("setturretteam", "allies");
            //sentryGun.Call("setsentrycarrier", owner);

            sentryGun.AfterDelay(1000, sg =>
            {
                owner.Call("remotecontrolturret", sentryGun);
                //sentryGun.Call("setcontents", 1);
                //sentryGun.SetField("angles", owner.Call<Vector3>("getPlayerAngles"));

                //sentryGun.Call(33084, 90f);//SetLeftArc
                //sentryGun.Call(33083, 0f);//SetRightArc
                //sentryGun.Call("makeUnusable");
                sentryGun.Notify("place_sentry");
                sentryGun.Call("setmode", "sentry");//sentry sentry_offline
                sentryGun.Call("maketurretsolid");
                //sentryGun.Call("setmode", "sentry");
                sentryGun.Call("setcandamage", true);
                sentryGun.Call("setturretminimapvisible", true);

                //bool m = false;
                return;
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


    }
}
