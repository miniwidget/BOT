using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Tank
    {
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
            remoteTank = Function.Call<Entity>("SpawnVehicle", "vehicle_ugv_talon_mp", "remote_tank", "remote_ugv_mp", TANK_WAY_POINT, Infected.ZERO, player);
            Vector3 turretAttachTagOrigin = remoteTank.Call<Vector3>("GetTagOrigin", "tag_turret_attach");
            remoteTank.SetField("owner", -1);


            Function.SetEntRef(-1);
            rmt1 = Function.Call<Entity>("SpawnTurret", "misc_turret", turretAttachTagOrigin, "remote_turret_mp", false);//ugv_turret_mp
            rmt1.Call("SetModel", "mp_remote_turret");//vehicle_ugv_talon_gun_mp
            rmt1.Call(32841, remoteTank, "tag_turret_attach", GetVector(0, -20f, 45f), Infected.ZERO);
            rmt1.Call(33084, 180f);//SetLeftArc
            rmt1.Call(33083, 180f);//SetRightArc
            rmt1.Call(33086, 180f);//SetBottomArc
            rmt1.SetField("owner", -1);

            Function.SetEntRef(-1);
            rmt2 = Function.Call<Entity>("SpawnTurret", "misc_turret", turretAttachTagOrigin, "remote_turret_mp", false);
            rmt2.Call("SetModel", "mp_remote_turret");
            rmt2.Call(32841, remoteTank, "tag_turret_attach", GetVector(0, 20f, 45f), Infected.ZERO);
            rmt2.Call(33084, 180f);//SetLeftArc
            rmt2.Call(33083, 180f);//SetRightArcff
            rmt2.Call(33086, 180f);//SetBottomArc
            rmt2.SetField("owner", -1);

            //player.Call(32792, "prop_flag_neutral", "tag_shield_back", true);//attachshieldmodel

            Function.SetEntRef(-1);
            Entity flag = Function.Call<Entity>("spawn", "script_model", turretAttachTagOrigin);
            flag.Call("setmodel", "prop_flag_neutral");
            flag.Call("LinkTo", remoteTank, "tag_turret_attach", GetVector(0f, 0f, 45f), new Vector3(90f,0,0));
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
            if (remoteTank == null) return 5;

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
        internal bool IfTankOwnerEnd(Entity player)
        {
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
   
        internal bool TANK_ON_USE_;
        internal void TankStart(Entity player)
        {
            float pod = player.Origin.DistanceTo(rmt1.Origin);
            float pod2 = player.Origin.DistanceTo(rmt2.Origin);
            if (pod > 140)
            {
                if (pod2 > 140) return;
            }
            
            if (!TANK_ON_USE_)
            {
                TANK_ON_USE_ = true;
                player.Call(33256, remoteTank);//remotecontrolvehicle  
                remoteTank.SetField("owner", player.EntRef);
            }

            if (pod < 140) rmt1.SetField("owner", player.EntRef);
            else if (pod2 < 140) rmt2.SetField("owner", player.EntRef);

            player.Health = 300;

        }
        internal void TankEnd(Entity player, int i)
        {
            if (i < 3)
            {
                TANK_ON_USE_ = false;
                remoteTank.SetField("owner", -1);
                if (i == 1) rmt1.SetField("owner", -1);
                else rmt2.SetField("owner", -1);

                SetTankPort(remoteTank.Origin);
            }
            else
            {
                if (i == 3) rmt1.SetField("owner", -1);
                else rmt2.SetField("owner", null);
            }
            player.Call(32843);//unlink
            player.Call(33257);//remotecontrolvehicleoff
            player.Call(33531, Infected.ZERO);
            player.AfterDelay(250, p =>
            {
                player.Call("setorigin", remoteTank.Origin);
            });
            player.Health = 100;
        }
  
    }
}
