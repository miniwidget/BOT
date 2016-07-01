using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class SentryGun : InfinityBase
    {

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

        internal void shoot(Entity owner)
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


    }
}
