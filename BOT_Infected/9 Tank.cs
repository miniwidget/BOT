using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Common
    {
        internal static void StartOrEndThermal(Entity player, bool start)
        {

            if (start)
            {
                player.Call(32936);//thermalvisionfofoverlayon
                player.Health = 300;

              
                return;
            }

            player.Call(32937);//thermalvisionfofoverlayoff
            player.Health = 100;
            player.Call(33531, Infected.ZERO);
        }
    }
    class Tank
    {
        internal int RMT1_OWNER=-1;
        internal int RMT2_OWNER=-1;
        internal int RMTK_OWNER=-1;
        internal Entity REMOTETANK, RMT1, RMT2;
        Vector3 TANK_WAY_POINT;
        Vector3 GetVector(float x, float y, float z)
        {
            TANK_WAY_POINT.X = x;
            TANK_WAY_POINT.Y = y;
            TANK_WAY_POINT.Z = z;
            return TANK_WAY_POINT;
        }

        internal void SetTank(Entity player)
        {
           

            TANK_WAY_POINT = player.Origin;

            SetTankPort(TANK_WAY_POINT);
            Function.SetEntRef(-1);
            REMOTETANK = Function.Call<Entity>(449, "vehicle_ugv_talon_mp", "remote_tank", "remote_ugv_mp", TANK_WAY_POINT, Infected.ZERO, player);//"SpawnVehicle"

            Vector3 turretAttachTagOrigin = REMOTETANK.Call<Vector3>(33128, "tag_turret_attach");//"GetTagOrigin"

            string turret_mp = "sentry_minigun_mp";//remote_turret_mp 
            string reamModel_turret = "weapon_minigun";//mp_remote_turret

            Function.SetEntRef(-1);
            Entity ugv = Function.Call<Entity>(19, "misc_turret", turretAttachTagOrigin, "ugv_turret_mp", false);//"SpawnTurret" ugv_turret_mp
            ugv.Call(32929, "vehicle_ugv_talon_gun_mp");//SetModel vehicle_ugv_talon_gun_mp
            ugv.Call(32841, REMOTETANK, "tag_turret_attach", Infected.ZERO, Infected.ZERO);
            ugv.Call(32942);
            ugv.Call(33088, 0);

            Function.SetEntRef(-1);
            RMT1 = Function.Call<Entity>(19, "misc_turret", turretAttachTagOrigin, turret_mp, false);
            RMT1.Call(32929, reamModel_turret);
            RMT1.Call(32841, ugv, "tag_headlight_right", GetVector(0, -20f, 45f), Infected.ZERO);
            RMT1.Call(33084, 180f);
            RMT1.Call(33083, 180f);
            RMT1.Call(33086, 180f);

            Function.SetEntRef(-1);
            RMT2 = Function.Call<Entity>(19, "misc_turret", turretAttachTagOrigin, turret_mp, false);
            RMT2.Call(32929, reamModel_turret);
            RMT2.Call(32841, ugv, "tag_headlight_right", GetVector(0, 20f, 45f), Infected.ZERO);
            RMT2.Call(33084, 180f);
            RMT2.Call(33083, 180f);
            RMT2.Call(33086, 180f);

            player.Call(33220, 0f);
            player.Health = 300;
        }
        internal void SetTankPort(Vector3 origin)
        {
            Function.SetEntRef(-1);
            Function.Call(431, 20, "active"); // objective_add
            Function.SetEntRef(-1);
            Function.Call(435, 20, origin); // objective_position
            Function.SetEntRef(-1);
            Function.Call(434, 20, "compass_waypoint_bomb"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon
        }

        sbyte IsTankOwner_(Entity player)
        {
            var pe = player.EntRef;
            if (RMTK_OWNER == pe)
            {
                if (RMT1_OWNER == pe) return 1;
                if (RMT2_OWNER == pe) return 2;
            }
            if (RMT1_OWNER == pe) return 3;
            if (RMT2_OWNER == pe) return 4;

            return 5;
        }
        internal bool IfTankOwner_DoEnd(Entity player)
        {
            if (REMOTETANK == null) return false;

            int ti = IsTankOwner_(player);
            if (ti != 5)
            {
                TankEnd(player, ti);
               
                return true;
            }
            return false;
        }
        internal bool IsTankArea_(Entity player)
        {
            var po = player.Origin;
            
            if (po.DistanceTo(RMT1.Origin) > 140)
            {
                if (po.DistanceTo(RMT2.Origin) > 140)
                {
                    return false;
                }
            }
            return true;
        }
   
        internal void TankStart(Entity player)
        {
            var po = player.Origin;

            bool InRMT1Area = (po.DistanceTo(RMT1.Origin) < 140);
            bool InRMT2Area = (po.DistanceTo(RMT2.Origin) < 140);

            if (!InRMT1Area)
            {
                if (!InRMT2Area)
                {
                    //Log.Write(LogLevel.None, "{0}", 3);
                    return;
                }
            }
            
            if (RMTK_OWNER==-1)
            {
                player.Call(33256, REMOTETANK);//remotecontrolvehicle  
                RMTK_OWNER = player.EntRef;
            }

            if (InRMT1Area)
            {
                RMT1_OWNER = player.EntRef;
            }
            else if (InRMT2Area)
            {
                RMT2_OWNER = player.EntRef;
            }

            player.Health = 300;
            Common.StartOrEndThermal(player, true);

        }
        internal void TankEnd(Entity player, int i)
        {
            if (i < 3)
            {
                RMTK_OWNER = -1;

                if (i == 1) RMT1_OWNER = -1;
                
                else  RMT2_OWNER = -1;
                
                SetTankPort(REMOTETANK.Origin);
                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff
            }
            else
            {
                if (i == 3) RMT1_OWNER = -1;

                else RMT2_OWNER = -1;
            }
           
            player.AfterDelay(500, p => player.Call(33529, REMOTETANK.Origin));//setorigin
           
            Common.StartOrEndThermal(player, false);
        }
    }
}
