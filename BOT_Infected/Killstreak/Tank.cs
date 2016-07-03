using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Tank : Inf
    {
        internal int TL_LEFT_USER_ENTREF = -1;
        internal int TL_RIGHT_USER_ENTREF = -1;
        internal int RMTK_OWNER_ENTREF = -1;
        internal Entity REMOTETANK, RMT1, RMT2;

        internal void SetTank(Entity player)
        {
            Vector3 point = player.Origin;

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

        //readonly string[] MESSAGE_RUNNER = { "^2TANK RUNNER START [ ^7MOVE & FIRE ^2]", "^2PRESS [ ^7[{+smoke}] ^2] IF STUCK" };

        /// <summary>
        /// 1: remote left /
        /// 2: remote right /
        /// 3: gunner left /
        /// 4: gunner right /
        /// 5: not 
        /// </summary>
        sbyte IsTankOwner_(Entity player)
        {
            var pe = player.EntRef;
            if (RMTK_OWNER_ENTREF == pe)
            {
                if (TL_LEFT_USER_ENTREF == pe) return 1;
                if (TL_RIGHT_USER_ENTREF == pe) return 2;
            }
            if (TL_LEFT_USER_ENTREF == pe) return 3;
            if (TL_RIGHT_USER_ENTREF == pe) return 4;

            return 5;
        }
        internal bool IfUseTank_DoEnd(Entity player)
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

        bool CheckDist(int entref)
        {
            if (entref == -1) return true;
            Entity ent = Entity.GetEntity(RMTK_OWNER_ENTREF);
            if (ent == null || !ent.IsPlayer || ent.Origin.DistanceTo(REMOTETANK.Origin) > 150)
            {
                return false;
            }
            return true;
        }
        bool CheckUsers(byte th)
        {
            if (!CheckDist(RMTK_OWNER_ENTREF)) RMTK_OWNER_ENTREF = -1;

            if (th==2)
            {
                if (!CheckDist(TL_RIGHT_USER_ENTREF)) TL_RIGHT_USER_ENTREF = -1;
                return true;
            }

            if (!CheckDist(TL_LEFT_USER_ENTREF)) TL_LEFT_USER_ENTREF = -1;
            return true;
        }
        internal byte TankStart(Entity player, byte turretHolding)
        {
            Common.StartOrEndThermal(player, true);

            if (turretHolding == 2)   TL_LEFT_USER_ENTREF = player.EntRef;
            
            else TL_RIGHT_USER_ENTREF = player.EntRef;
            
            CheckUsers(turretHolding);

            if (RMTK_OWNER_ENTREF == -1 || RMTK_OWNER_ENTREF == player.EntRef)
            {
                player.Call(33256, REMOTETANK);//remotecontrolvehicle  
                RMTK_OWNER_ENTREF = player.EntRef;
                player.Call(33344, "^2TANK RUNNER START [ ^7MOVE & FIRE ^2]"); 
                return 2;
            }
            else
            {
                player.Call(33344, "^2TANK GUNNER START [ ^7FIRE ^2]");
                return 0;
            }
        }

        internal void TankEnd(Entity player, int i)
        {
            if (i < 3)
            {
                RMTK_OWNER_ENTREF = -1;

                if (i == 1) TL_LEFT_USER_ENTREF = -1;

                else TL_RIGHT_USER_ENTREF = -1;

                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff

            }
            else
            {
                if (i == 3) TL_LEFT_USER_ENTREF = -1;

                else TL_RIGHT_USER_ENTREF = -1;
            }
            player.Call("setorigin", REMOTETANK.Origin);

            Common.StartOrEndThermal(player, false);
        }
    }
}
