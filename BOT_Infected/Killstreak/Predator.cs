using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Predator : Inf
    {
        internal Entity PLANE, MISSILE, PREDATOR_OWNER;
        byte MISSILE_COUNT;
        int FX_GREEN_LIGHT;
        string weapon;

        internal void PredatorEnd(Entity player, H_SET H, bool respawn, string weapon)
        {
            if (!respawn)
            {
                player.Call(32843);//unlink
                if (MISSILE != null)
                {
                    player.Call(33529, MISSILE.Origin);//setorigin

                    if (player.Call<int>(33538) == 0)//Not IsGround
                    {
                        player.Call(33529, Helicopter.HELI_WAY_POINT);
                    }
                }
                else player.Call(33529, Helicopter.HELI_WAY_POINT);

                player.TakeWeapon("heli_remote_mp");
                player.GiveWeapon(weapon);
                player.SwitchToWeaponImmediate(weapon);
                Common.StartOrEndThermal(player, false);
            }
            H.HUD_KEY_INFO.Call(32897);//destroy
            H.HUD_BULLET_INFO.Call(32897);//destroy
            H.USE_PREDATOR = false;

            if (PREDATOR_OWNER != player) return;

            PREDATOR_OWNER = null;
            Infected.USE_PREDATOR = false;
            PLANE.Call(32928);//delete
            PLANE = null;
        }

        internal void PredatorStart(Entity player, H_SET H)
        {
            weapon = player.CurrentWeapon;

            Infected.USE_PREDATOR = true;
            MISSILE_COUNT = 0;
            PREDATOR_OWNER = player;
            H.USE_PREDATOR = true;
            player.Health = 9999;
            if (PLANE != null) PLANE.Call(32928);//delete
            if (FX_GREEN_LIGHT == 0) FX_GREEN_LIGHT = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"

            Vector3 origin = player.Origin; origin.Z += 100;

            PLANE = Call<Entity>(367, player, "script_model", origin, "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            PLANE.Call(32929, "vehicle_remote_uav");//setModel

            player.Call(32841, PLANE, "tag_origin");//linkto
            PLANE.Call(33402, 1500, 7);//movez


            H.HUD_KEY_INFO = HudElem.CreateFontString(player, "hudbig", 0.6f);
            H.HUD_BULLET_INFO = HudElem.CreateFontString(player, "hudbig", 0.6f);

            PLANE.AfterDelay(7000, v =>
            {
                if (FX_GREEN_LIGHT != 0)
                {
                    Call(305, FX_GREEN_LIGHT, PLANE, "tag_light_tail1");//playFXOnTag
                    Call(305, FX_GREEN_LIGHT, PLANE, "tag_light_nose");//playFXOnTag
                }

                player.Health = 100;
                player.GiveWeapon("heli_remote_mp");

                H.HUD_KEY_INFO.HorzAlign = "center";
                H.HUD_KEY_INFO.AlignX = "center";
                H.HUD_KEY_INFO.VertAlign = "bottom";
                H.HUD_KEY_INFO.Y = 10;
                H.HUD_KEY_INFO.SetText("^2PRESS [ ^7[{+frag}] ^2]");

                H.HUD_BULLET_INFO.HorzAlign = "right";
                H.HUD_BULLET_INFO.AlignX = "center";
                H.HUD_BULLET_INFO.VertAlign = "bottom";
                H.HUD_BULLET_INFO.Y = 10;
                player.SwitchToWeapon("heli_remote_mp");

                Common.StartOrEndThermal(player, true);
            });

            if (H.PREDATOR_NOTIFIED) return;
            H.PREDATOR_NOTIFIED = true;

            player.Call(33445, "MISSILE", "+frag");//notifyonplayercommand
            player.OnNotify("MISSILE", ent =>
            {
                if (!H.USE_PREDATOR) return;
                if (MISSILE_COUNT >= 10) return;
                if (MISSILE != null) return;

                Vector3 angle = player.Call<Vector3>("getPlayerAngles");
                Vector3 Ori = player.Origin;

                float[] GMP = GetMissilePos(angle, Ori);
                float x = GMP[0], y = GMP[1];
                Vector3 targetPos = Common.GetVector(Ori.X + x, Ori.Y + y, 0);
                Vector3 startPos = Common.GetVector(Ori.X - x, Ori.Y - y, Ori.Z * 4);
                MISSILE = Call<Entity>(404, "remotemissile_projectile_mp", startPos, targetPos, player);//MagicBullet

                MISSILE.Call(33417, true);//setCanDamage
                player.Call(33438, "thermalVision", 1f);//VisionSetMissilecamForPlayer
                player.Call(33221, MISSILE, "tag_origin");//CameraLinkTo
                player.Call(33251, MISSILE);//ControlsLinkTo
                MISSILE.OnNotify("death", ms =>
                {
                    MISSILE_COUNT++;
                    H.HUD_BULLET_INFO.SetText((10 - MISSILE_COUNT).ToString());
                    player.Call(33252);//ControlsUnlink
                    player.Call(33222);//CameraUnlink

                    if (MISSILE_COUNT == 10) PredatorEnd(player, H, false, weapon);

                    MISSILE = null;
                });

            });
        }
        float[] GMP = { 0, 0 };
        float[] GetMissilePos(Vector3 angle, Vector3 origin)
        {
            var degreeToRadian = 0.01745f;// (float)Math.PI / 180;

            var dist = (float)Math.Abs(origin.Z / Math.Tan(angle.X * degreeToRadian));

            float Hor_Degree = angle.Y;
            float HD = Math.Abs(Hor_Degree);

            if (HD > 90) HD = 180 - HD;

            var rad = HD * degreeToRadian;

            var x = dist * (float)Math.Abs(Math.Cos(rad));
            var y = dist * (float)Math.Abs(Math.Sin(rad));

            // Print("(" + (int)origin.X + ", " + (int)origin.Y + ")[" + (int)angle.X + "," + (int)angle.Y + "]" + (int)x + " " + (int)y);

            if (Hor_Degree < 0)
            {
                if (Hor_Degree > -90)//4사분면
                {
                    y = y * -1;
                }
                else//3사분면
                {
                    x *= -1;
                    y *= -1;
                }
            }
            else
            {
                if (Hor_Degree > 90)
                {
                    x *= -1;
                }
            }
            GMP[0] = x; GMP[1] = y;
            return GMP;
        }

    }
}
