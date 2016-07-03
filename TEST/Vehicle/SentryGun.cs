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

        public SentryGun()
        {
            test.ADMIN.AfterDelay(100, x =>
            {
                SentrySpawn(test.ADMIN, "sentry_offline");
            });
            
        }

      internal  bool SentrySpawn(Entity bot, string mode)
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
