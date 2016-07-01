using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Tank : Inf
    {
        internal int RMT1_OWNER = -1;
        internal int RMT2_OWNER = -1;
        internal int RMTK_OWNER = -1;
        internal Entity REMOTETANK, RMT1, RMT2;

        internal void SetTank(Entity player)
        {
            Vector3 point = player.Origin;

            SetTankPort(point);
            Function.SetEntRef(-1);
            REMOTETANK = Function.Call<Entity>(449, "vehicle_ugv_talon_mp", "remote_tank", "remote_ugv_mp", point, Common.ZERO, player);//"SpawnVehicle"

            Vector3 turretAttachTagOrigin = REMOTETANK.Call<Vector3>(33128, "tag_turret_attach");//"GetTagOrigin"

            string printModel = "sentry_minigun_mp";//remote_turret_mp 
            string reamModel_turret = "weapon_minigun";//mp_remote_turret
            if (Set.TURRET_MAP) printModel = "turret_minigun_mp";

            Entity ugv = Call<Entity>(19, "misc_turret", turretAttachTagOrigin, "ugv_turret_mp", false);//"SpawnTurret" ugv_turret_mp
            ugv.Call(32929, "vehicle_ugv_talon_gun_mp");//SetModel vehicle_ugv_talon_gun_mp
            ugv.Call(32841, REMOTETANK, "tag_turret_attach", Common.ZERO, Common.ZERO);
            ugv.Call(32942);
            ugv.Call(33088, 0);

            RMT1 = Call<Entity>(19, "misc_turret", turretAttachTagOrigin, printModel, false);
            RMT1.Call(32929, reamModel_turret);
            RMT1.Call(32841, ugv, "tag_headlight_right", Common.GetVector(0, 20f, 45f), Common.ZERO);
            RMT1.Call(33084, 180f);
            RMT1.Call(33083, 180f);
            RMT1.Call(33086, 180f);

            RMT2 = Call<Entity>(19, "misc_turret", turretAttachTagOrigin, printModel, false);
            RMT2.Call(32929, reamModel_turret);
            RMT2.Call(32841, ugv, "tag_headlight_right", Common.GetVector(0, -20f, 45f), Common.ZERO);
            RMT2.Call(33084, 180f);
            RMT2.Call(33083, 180f);
            RMT2.Call(33086, 180f);
        }
        internal void SetTankPort(Vector3 origin)
        {
            Call(431, 20, "active"); // objective_add
            Call(435, 20, origin); // objective_position
            Call(434, 20, "compass_waypoint_bomb"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon
        }
        readonly string[] MESSAGE_RUNNER = { "^2TANK RUNNER START [ ^7MOVE & FIRE ^2]", "^2PRESS [ ^7[{+smoke}] ^2] IF STUCK" };

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

        internal byte TankStart(Entity player, byte turretHolding)
        {
            Common.StartOrEndThermal(player, true);

            if (turretHolding == 2)
            {
                RMT1_OWNER = player.EntRef;
            }
            else
            {
                RMT2_OWNER = player.EntRef;
            }

            if (RMTK_OWNER == -1|| RMTK_OWNER == player.EntRef)
            {
                player.Call(33256, REMOTETANK);//remotecontrolvehicle  
                RMTK_OWNER = player.EntRef;
                Info.MessageRoop(player, 0, MESSAGE_RUNNER);
                return 3;
            }
            else
            {
                player.Call(33344, "^2TANK GUNNER START [ ^7FIRE ^2]");
                return 4;
            }
           


        }
        internal void TankEnd(Entity player, int i)
        {
            if (i < 3)
            {
                RMTK_OWNER = -1;

                if (i == 1) RMT1_OWNER = -1;

                else RMT2_OWNER = -1;

                SetTankPort(REMOTETANK.Origin);
                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff
            }
            else
            {
                if (i == 3) RMT1_OWNER = -1;

                else RMT2_OWNER = -1;
            }

            Common.StartOrEndThermal(player, false);
        }
    }
}
