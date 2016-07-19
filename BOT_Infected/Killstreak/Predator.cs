using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Predator : Inf
    {
        internal Entity PLANE;
        Entity PREDATOR_OWNER;
        int FX_GREEN_LIGHT = -1;
        string THERMALVISION;
        Vector3 PRDT_POS;

        public Predator()
        {
            if (Call<string>(221, "thermal") == "invert")//getMapCustom
                THERMALVISION = "thermal_snowlevel_mp";
            else
                THERMALVISION = "thermal_mp";
        }
        internal void PredatorStart(Entity player, H_SET H, int height)
        {
            if (H.REMOTE_STATE != State.remote_not_using) return;

            PRDT_POS = player.Origin;
            Infected.USE_PREDATOR = true;
            H.MISSILE_COUNT = 8;
            PREDATOR_OWNER = player;
            H.REMOTE_STATE = State.remote_predator;
            H.CAN_USE_PREDATOR = false;
            player.Health = 9999;
            if (PLANE != null) PLANE.Call(32928);//delete
            if (FX_GREEN_LIGHT == -1) FX_GREEN_LIGHT = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"

            PLANE = Call<Entity>(367, player, "script_model", player.Origin, "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            PLANE.Call(32929, "vehicle_remote_uav");//setModel

            player.Call(32841, PLANE, "tag_origin");//linkto
            PLANE.Call(33402, height, 7);//movez

            Infected.PlayDialog(player, H.AXIS, 11);

            PLANE.AfterDelay(7000, v =>
            {
                if (H.REMOTE_STATE != State.remote_predator) return;
                if (FX_GREEN_LIGHT != -1)
                {
                    Call(305, FX_GREEN_LIGHT, PLANE, "tag_light_tail1");//playFXOnTag
                    Call(305, FX_GREEN_LIGHT, PLANE, "tag_light_nose");//playFXOnTag
                }

                player.Health = 100;
                player.GiveWeapon("heli_remote_mp");

                Common.BulletHudInfoCreate(player, H, 6);

                player.SwitchToWeapon("heli_remote_mp");

                H.CAN_USE_PREDATOR = true;
                Common.StartOrEndThermal(player, true);
            });


            if (H.PREDATOR_FIRE_NOTIFIED) return;
            H.PREDATOR_FIRE_NOTIFIED = true;
            bool wait = false;
            player.Call(33445, "MISSILE", "+frag");//notifyonplayercommand
            player.OnNotify("MISSILE", ent =>
            {
                if (H.REMOTE_STATE != State.remote_predator) return;
                if (!H.CAN_USE_PREDATOR) return;
                if (H.MISSILE_COUNT <= 0) { PredatorEnd(player, H, false); return; }
                if (wait) return; wait = true;

                Vector3 angle = player.Call<Vector3>(33532);//getPlayerAngles
                Vector3 Ori = player.Origin;

                float[] GMP = Common.GetMissilePos(angle, Ori);
                float x = GMP[0], y = GMP[1];
                Vector3 targetPos = Common.GetVector(Ori.X + x, Ori.Y + y, 0);
                Vector3 startPos = Common.GetVector(Ori.X - x, Ori.Y - y, Ori.Z * 4);
                Entity missile = Call<Entity>(404, "remotemissile_projectile_mp", startPos, targetPos, player);//MagicBullet
                missile.Call(33417, true);//setCanDamage
                player.Call(33438, THERMALVISION, 1f);//VisionSetMissilecamForPlayer
                player.Call(33221, missile, "tag_origin");//CameraLinkTo
                player.Call(33251, missile);//ControlsLinkTo
                H.MISSILE_COUNT--;
                H.HUD_BULLET_INFO.SetText(H.MISSILE_COUNT.ToString());
                missile.OnNotify("death", ms =>
                {
                    wait = false;
                    player.Call(33252);//ControlsUnlink
                    player.Call(33222);//CameraUnlink
                });
            });
        }
        internal void PredatorEnd(Entity player, H_SET H, bool respawn)
        {
            if (!respawn)
            {
                player.Call(32843);//unlink
                player.Call(33529, PRDT_POS);
                player.TakeWeapon("heli_remote_mp");
                player.GiveWeapon(H.GUN);
                player.SwitchToWeaponImmediate(H.GUN);
                Common.StartOrEndThermal(player, false);
            }

            H.REMOTE_STATE = State.remote_not_using;
            H.CAN_USE_PREDATOR = false;

            Common.BulletHudInfoDestroy(H);

            if (PREDATOR_OWNER == null || PREDATOR_OWNER == player)
            {
                PREDATOR_OWNER = null;
                Infected.USE_PREDATOR = false;

                if (PLANE != null)
                {
                    PLANE.Call(32928);//delete
                    PLANE = null;
                }
            }
        }
    }
}
