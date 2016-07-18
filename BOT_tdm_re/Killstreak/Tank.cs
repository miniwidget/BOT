using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tdm
{
    class Tank : Inf
    {
        internal int TL_LEFT_USER_ENTREF = -1;
        internal int TL_RIGHT_USER_ENTREF = -1;
        internal int RMTK_OWNER_ENTREF = -1;
        internal Entity REMOTETANK, TL, TR;

        internal void SetTank(Entity player)
        {
            REMOTETANK = Call<Entity>(449, "vehicle_ugv_talon_mp", "remote_tank", "remote_ugv_mp", player.Origin, Common.ZERO, player);//"SpawnVehicle"

            Vector3 turretAttachTagOrigin = REMOTETANK.Call<Vector3>(33128, "tag_turret_attach");//"GetTagOrigin"

            string printModel = "sentry_minigun_mp";//remote_turret_mp 
            string reamModel_turret = "weapon_minigun";//mp_remote_turret
            if (Set.TURRET_MAP) printModel = "turret_minigun_mp";
           
            Entity ugv = Call<Entity>(19, "misc_turret", turretAttachTagOrigin, "ugv_turret_mp", false);//"SpawnTurret" ugv_turret_mp
            ugv.Call(32929, "vehicle_ugv_talon_gun_mp");//SetModel vehicle_ugv_talon_gun_mp
            ugv.Call(32841, REMOTETANK, "tag_turret_attach", Common.ZERO, Common.ZERO);
            ugv.Call(32942);
            ugv.Call(33088, 0);

            TL = Call<Entity>(19, "misc_turret", turretAttachTagOrigin, printModel, false);
            TL.Call(32929, reamModel_turret);
            TL.Call(32841, ugv, "tag_headlight_right", Common.GetVector(0, 20f, 45f), Common.ZERO);
            TL.Call(33084, 180f);
            TL.Call(33083, 180f);
            TL.Call(33086, 180f);

            TR = Call<Entity>(19, "misc_turret", turretAttachTagOrigin, printModel, false);
            TR.Call(32929, reamModel_turret);
            TR.Call(32841, ugv, "tag_headlight_right", Common.GetVector(0, -20f, 45f), Common.ZERO);
            TR.Call(33084, 180f);
            TR.Call(33083, 180f);
            TR.Call(33086, 180f);
        }

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

        internal State TankStart(Entity player, byte turretHolding, bool axis)
        {
            Common.StartOrEndThermal(player, true);

            if (!CheckDist(RMTK_OWNER_ENTREF,0)) RMTK_OWNER_ENTREF = -1;
            if (!CheckDist(TL_LEFT_USER_ENTREF,1)) TL_LEFT_USER_ENTREF = -1;
            if (!CheckDist(TL_RIGHT_USER_ENTREF,2)) TL_RIGHT_USER_ENTREF = -1;

            if (turretHolding == 2) TL_LEFT_USER_ENTREF = player.EntRef;
            else TL_RIGHT_USER_ENTREF = player.EntRef;

            if (RMTK_OWNER_ENTREF == -1 || RMTK_OWNER_ENTREF == player.EntRef)
            {
                player.Call(33256, REMOTETANK);//remotecontrolvehicle  
                RMTK_OWNER_ENTREF = player.EntRef;
                player.Call(33344, Info.GetStr("*TANK RUNNER START [ ^7MOVE & FIRE *]", axis)); 
                return State.remote_turretTank;//remote tank state
            }
            else
            {
                player.Call(33344, Info.GetStr("*TANK GUNNER START [ ^7FIRE *]",axis));
                return State.remote_not_using;//not using remote state
            }
        }
        bool CheckDist(int entref,byte type)
        {
            if (entref == -1) return true;

            Entity ent = Entity.GetEntity(entref);

            if (ent == null || !ent.IsPlayer) return false;

            Vector3 handpos = ent.Call<Vector3>(33128, "tag_weapon_left");
            if (type == 0) ent = REMOTETANK;
            else if (type == 1) ent = TL;
            else ent = TR;

            if (handpos.DistanceTo2D(ent.Origin) > 10) return false;

            return true;
        }

        /// <summary>
        /// 1: remote left /
        /// 2: remote right /
        /// 3: gunner left /
        /// 4: gunner right /
        /// 5: not 
        /// </summary>
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
            player.Call(33529, REMOTETANK.Origin);//setorigin

            Common.StartOrEndThermal(player, false);
        }
    }
}
