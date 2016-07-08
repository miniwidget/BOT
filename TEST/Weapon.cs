using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace TEST
{
    class Weapon : InfinityBase
    {
        Entity rp = null;

        internal Vector3 AnglesToForward(Vector3 angle, Vector3 v, int scalar)
        {

            float hor = angle.Y;
            float vert = angle.X;
            float y = (float)Math.Abs(Math.Tan(angle.Y / 57.3) * scalar);

            float x0 = v.X;
            float y0 = v.Y;

            if (hor > 0)
            {
                if (hor < 90)//2사분면
                {
                    v.X += scalar;
                }
                else//3사분면
                {
                    v.X -= scalar;
                }
                v.Y += y;
            }
            else
            {
                if (hor > -90)//1사분면
                {
                    v.X += scalar;
                }
                else//4사분면
                {
                    v.X -= scalar;
                }
                v.Y -= y;
            }

            //x0 = v.X - x0;
            //y0 = v.Y - y0;
            //float dist = (float)Math.Sqrt(x0 * x0 + y0 * y0);
            //float radians = angle.X * 57.3f;

            //if (angle.Z < 0)//위로 보고 있을 경우
            //{
            //    v.Z += dist * (float)Math.Tan(radians);
            //}
            //else//아래로 보고 있을 경우
            //{
            //    v.Z -= dist * (float)Math.Tan(radians);
            //}
            return v;
        }
        internal Vector3 AnglesToBack(Vector3 angle, Vector3 v, int scalar)
        {
            float hor = angle.Y;
            float vert = angle.X;
            float y = (float)Math.Abs(Math.Tan(angle.Y / 57.3) * scalar);

            if (hor > 0)
            {
                if (hor < 90)//2사분면
                {
                    v.X -= scalar;
                }
                else//3사분면
                {
                    v.X += scalar;
                }
                v.Y -= y;
            }
            else
            {
                if (hor > -90)//1사분면
                {
                    v.X -= scalar;
                }
                else//4사분면
                {
                    v.X += scalar;
                }
                v.Y += y;
            }
            v.Z += 200;
            return v;
        }

        internal void magicBullet(Entity player)
        {
            player.Call("setorigin" , new Vector3(-2944,-29,527));
            player.OnNotify("weapon_fired", (p, weaponName) =>
            {
                if (rp != null) return;
                Vector3 angle = player.Call<Vector3>("getPlayerAngles");
                Vector3 handpos = player.Call<Vector3>("getTagOrigin", "tag_weapon_left");

                Vector3 startPos = AnglesToBack(angle, handpos, 2000);
                Vector3 endPos = AnglesToForward(angle, handpos, 1000);

                rp = Call<Entity>("MagicBullet", "remotemissile_projectile_mp", startPos, endPos, player);

                //Call<Vector3>("anglestoforward", player.Call<Vector3>("getPlayerAngles")) * 1000000, // end point
                //player); // ignore entity
                rp.Call("setCanDamage", true);
                player.Call("VisionSetMissilecamForPlayer", "black_bw", 0f);
                player.Call("VisionSetMissilecamForPlayer", "thermalVision", 1f);
                player.Call("CameraLinkTo", rp, "tag_origin");
                player.Call("ControlsLinkTo", rp);
                rp.OnNotify("death", _rp =>
                {
                    player.Call("ControlsUnlink");
                    player.Call("freezeControls", false);
                    player.Call("CameraUnlink");
                    player.Notify("stopped_using_remote");
                    rp = null;
                });
            });
        }

        internal void giveWeapon(Entity player, string weapon)
        {
            //player.Call("freezecontrols", true);
            player.GiveWeapon(weapon);
            player.SwitchToWeaponImmediate(weapon);
        }


    }
}
