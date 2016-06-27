using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Tank
    {
        internal bool USE_RMT1;
        internal bool USE_RMT2;
        internal bool USE_RMTK;
        internal Entity remoteTank, rmt1, rmt2;
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
            remoteTank = Function.Call<Entity>(449, "vehicle_ugv_talon_mp", "remote_tank", "remote_ugv_mp", TANK_WAY_POINT, Infected.ZERO, player);//"SpawnVehicle"

            Vector3 turretAttachTagOrigin = remoteTank.Call<Vector3>(33128, "tag_turret_attach");//"GetTagOrigin"
            remoteTank.SetField("owner", -1);

            Function.SetEntRef(-1);
            rmt1 = Function.Call<Entity>(19, "misc_turret", turretAttachTagOrigin, "remote_turret_mp", false);//"SpawnTurret" ugv_turret_mp
            rmt1.Call(32929, "mp_remote_turret");//SetModel vehicle_ugv_talon_gun_mp
            rmt1.Call(32841, remoteTank, "tag_turret_attach", GetVector(0, -20f, 45f), Infected.ZERO);
            rmt1.Call(33084, 180f);//SetLeftArc
            rmt1.Call(33083, 180f);//SetRightArc
            rmt1.Call(33086, 180f);//SetBottomArc
            rmt1.SetField("owner", -1);

            Function.SetEntRef(-1);
            rmt2 = Function.Call<Entity>(19, "misc_turret", turretAttachTagOrigin, "remote_turret_mp", false);
            rmt2.Call(32929, "mp_remote_turret");
            rmt2.Call(32841, remoteTank, "tag_turret_attach", GetVector(0, 20f, 45f), Infected.ZERO);
            rmt2.Call(33084, 180f);
            rmt2.Call(33083, 180f);
            rmt2.Call(33086, 180f);
            rmt2.SetField("owner", -1);

            Function.SetEntRef(-1);
            Entity flag = Function.Call<Entity>(85, "script_model", turretAttachTagOrigin);//"spawn"
            flag.Call(32929, "prop_flag_neutral");
            flag.Call(32841, remoteTank, "tag_turret_attach", GetVector(0f, 0f, 45f), new Vector3(-90f,0,0));
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
            if (remoteTank.GetField<int>("owner") == pe)
            {
                if (rmt1.GetField<int>("owner") == pe) return 1;
                if (rmt2.GetField<int>("owner") == pe) return 2;
            }
            if (rmt1.GetField<int>("owner") == pe) return 3;
            if (rmt2.GetField<int>("owner") == pe) return 4;

            return 5;
        }
        internal bool IfTankOwner_DoEnd(Entity player)
        {
            if (remoteTank == null) return false;

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
            float dist = player.Origin.DistanceTo(rmt1.Origin);
            if (dist > 140)
            {
                if (player.Origin.DistanceTo(rmt2.Origin) > 140)
                {
                    return false;
                }
            }
            return true;
        }
   
        internal void TankStart(Entity player)
        {
            float pod = player.Origin.DistanceTo(rmt1.Origin);
            float pod2 = player.Origin.DistanceTo(rmt2.Origin);
            if (pod > 140)
            {
                if (pod2 > 140) return;
            }
            
            if (!USE_RMTK)
            {
                player.Call(33256, remoteTank);//remotecontrolvehicle  
                remoteTank.SetField("owner", player.EntRef);
                USE_RMTK = true;
            }

            if (pod < 140)
            {
                rmt1.SetField("owner", player.EntRef);
                USE_RMT1 = true;
            }
            else if (pod2 < 140)
            {
                rmt2.SetField("owner", player.EntRef);
                USE_RMT2 = true;
            }

            player.Health = 300;

        }
        internal void TankEnd(Entity player, int i)
        {
            if (i < 3)
            {
                //TANK_ON_USE_ = false;
                remoteTank.SetField("owner", -1);

                if (i == 1)
                {
                    rmt1.SetField("owner", -1);
                    USE_RMT1 = false;
                    USE_RMTK = false;
                }
                else
                {
                    rmt2.SetField("owner", -1);
                    USE_RMT2 = false;
                    USE_RMTK = false;
                }

                SetTankPort(remoteTank.Origin);
            }
            else
            {
                if (i == 3)
                {
                    rmt1.SetField("owner", -1);
                    USE_RMT1 = false;
                }
                else
                {
                    rmt2.SetField("owner", null);
                    USE_RMT2 = false;
                }
            }
            player.Call(32843);//unlink
            player.Call(33257);//remotecontrolvehicleoff
            player.Call(33531, Infected.ZERO);
            player.AfterDelay(250, p =>
            {
                player.Call(33529, remoteTank.Origin);//setorigin
            });
            player.Health = 100;
        }
  
    }
}
