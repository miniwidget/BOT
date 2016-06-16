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
        #region Vector3
        Vector3 killCamOrigin
        {
            get
            {
                return VEHICLE.Origin + ((AnglesToForward(VEHICLE.GetField<Vector3>("angles") * -100) + (AnglesToRight(VEHICLE.Origin) * -100))) + new Vector3(0, 0, 50);
            }
        }
        Vector3 AnglesToForward(Vector3 dir)
        {
            return Call<Vector3>("anglestoforward", dir);
        }
        Vector3 AnglesToRight(Vector3 dir)
        {
            return Call<Vector3>("anglestoright", dir);
        }
        Vector3 Angles
        {
            get
            {
                return ADMIN.GetField<Vector3>("angles");
            }
        }
        /// <summary>
        /// ADMIN ORIGIN + z(200)
        /// </summary>
        Vector3 Origin
        {
            get
            {
                var o = ADMIN.Origin;
                o.Z += 200;
                return o;
            }
        }
        Vector3 pathGoal, pathStart, pathEnd, startPoint, endPoint, direction;


        Vector3 getPathStart(Vector3 coord, Vector3 dropYaw)
        {
            direction = new Vector3(0, dropYaw.Y, 0);
            startPoint = coord + (AnglesToForward(direction) * (-1 * 15000));
            //startPoint += 
            return startPoint;
        }
        Vector3 getPathEnd(Vector3 coord, Vector3 yaw)
        {
            //int pathRandomness = 150;
            int lbHalfDistance = 15000;

            direction = new Vector3(0, yaw.Y, 0);

            endPoint = coord + (AnglesToForward(direction + new Vector3(0, 90, 0)) * lbHalfDistance);
            // endPoint += ((randomfloat(2) - 1) * pathRandomness  , (randomfloat(2) - 1) * pathRandomness  , 0 );

            return endPoint;
        }

        #endregion


        void giveWeaponTo(string weapon)
        {
            ADMIN.TakeWeapon(ADMIN.CurrentWeapon);
            ADMIN.GiveWeapon(weapon);
            ADMIN.AfterDelay(100, x => ADMIN.SwitchToWeaponImmediate(weapon));
        }

        bool isVehicleOn
        {
            get
            {
                if (VEHICLE != null)
                {
                    EndRemoteControl();
                    return true;
                }
                else return false;
            }
        }

        void line()
        {
            //moveHelicopter();
            Vector3 start = ADMIN.Origin;
            Vector3 end = start; end.Z += 1000;
            Call("line", start, end, new Vector3(1, 0, 0), false, 1);
        }

        #region UTIL
        void setVehGoalPos(Entity ent, Vector3 path, int i)
        {
            ent.Call("setVehGoalPos", path);
        }
        Entity heliSetup(Entity owner, Vector3 pathStart, Vector3 pathGoal)
        {
            pathStart += new Vector3(100, 500, 300);
            Entity LB = Call<Entity>(369, ADMIN, pathStart, ADMIN.GetField<Vector3>("angles"), "littlebird_mp", "vehicle_little_bird_armed");
            //Vector3 forward = Call<Vector3>("vectortoangle", pathGoal - pathStart);
            //Entity lb = Call<Entity>("spawnhelicopter", ADMIN, pathStart, forward, "littlebird_mp", "vehicle_little_bird_armed");
            return LB;
        }

        private T getDvar<T>(string dvar)
        {
            //print(typeof(T).ToString());
            return default(T);
        }
        void Restore(Entity player, bool enabled)
        {

            if (enabled)
            {
                player.SetField("restoreWeapon", player.CurrentWeapon);
                player.SetField("pos", player.Origin);

            }
            else
            {
                player.Call(32935);

                //if (!isSurvivor(player)) return;

                player.Call(33529, player.GetField<Vector3>("pos"));

                //giveWeaponTo(player, player.GetField<string>("restoreWeapon"));
            }
        }
        #endregion
        void testHealth()
        {
            foreach (Entity p in Players)
            {
                p.Health = 999;

            }
            Utilities.SayAll("health : 999");
        }

    }
}
