﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Timers;

namespace Infected
{

    public partial class Infected
    {

        //Vector3 VectorToAngle(Vector3 TO, Vector3 BO)
        //{
        //    float dx = TO.X - BO.X;
        //    float dy = TO.Y - BO.Y;
        //    float dz = BO.Z - TO.Z + 50;

        //    int dist = (int)Math.Sqrt(dx * dx + dy * dy);
        //    BO.X = (float)Math.Atan2(dz, dist) * 57.3f;
        //    BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.3f;
        //    BO.Z = 0;
        //    return BO;
        //}

        Vector3 VectorToAngleY(Vector3 TO, Vector3 BO)
        {
            float dx = TO.X - BO.X;
            float dy = TO.Y - BO.Y;

            BO.X = 0;
            BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.3f;
            BO.Z = 0;
            return BO;
        }
        Vector3 VectorAddZ(Vector3 origin, float add)
        {
            origin.Z += add;
            return origin;
        }

        Vector3 BOT_HELI_TARGET_POS;

        Entity BOT_HELI, BOT_HELI_FLARE;
        string[] MAGICS = { "sam_projectile_mp", "javelin_mp", "ims_projectile_mp", "ac130_40mm_mp", "ac130_105mm_mp", "rpg_mp", "uav_strike_projectile_mp" };
        bool BOT_HELI_INTERVAL_STOP;
        int FX_EXPLOSION, FX_FLARE_AMBIENT, FX_GREEN_LIGHT;
        byte BOT_HELI_FIRE;

        void BotHeliSpawned(Entity bot)
        {
            BOT_HELI_INTERVAL_STOP = true;
            if (GAME_ENDED_) return;

            BOT_HELI_INTERVAL_STOP = true;
            BOT_HELI_FIRE = 2;
            if (BOT_HELI_FLARE != null)
            {
                BOT_HELI_FLARE.Call(32928);//"delete"
                BOT_HELI_FLARE = null;
            }
            if (BOT_HELI != null)
            {
                BOT_HELI.Call(32848);
                Call(304, FX_EXPLOSION, BOT_HELI.Origin);//"PlayFX"
            }
            else
            {
                BOT_HELI = Call<Entity>(367, bot, "script_model", VectorAddZ(bot.Origin, 5000), "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
                BOT_HELI.Call(32929, "vehicle_uav_static_mp");//"setmodel" vehicle_remote_uav
                BOT_HELI.Call(32848);//"hide"

                FX_EXPLOSION = Call<int>(303, "explosions/aerial_explosion");//
                FX_FLARE_AMBIENT = Call<int>(303, "misc/flare_ambient");//"loadfx"
                FX_GREEN_LIGHT = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"
            }

            var B = B_FIELD[bot.EntRef];
            if (B.killer != -1)
            {
                BotCheckPerk(B.killer);
                B.killer = -1;
            }

            bot.Health = -1;
            bot.Call(32848);//hide
            bot.Call(33220, 0f);//setmovespeedscale

            bot.AfterDelay(11000, b =>
            {
                BOT_HELI_INTERVAL_STOP = false;
                bot.Health = 120;
                bot.Call(32847);//"show"
                BOT_HELI.Call(32847);//"show"
                bot.Call(32841, BOT_HELI);//"linkto"

                BotHeliInterval(bot);
            });
        }

        void BotHeliInterval(Entity bot)
        {
            byte count = 0;
            bool slow = true;
            bool wait = true;

            BOT_HELI.OnInterval(5000, b =>
            {
                if (GAME_ENDED_ || BOT_HELI_INTERVAL_STOP) return false;

                int hc = human_List.Count;
                if (hc == 0)
                {
                    if (BOT_HELI_FLARE != null)
                    {
                        BOT_HELI_FLARE.Call(32928);//"delete"
                        BOT_HELI_FLARE = null;
                    }
                    return true;
                }
                else if (hc > 2)
                {
                    slow = false;
                }

                if (count % 3 == 0)
                {
                    Vector3 targetPos = VectorAddZ(BOTs_List[rnd.Next(BOTs_List.Count)].Origin, 1000);

                    BOT_HELI.Call(33406, VectorToAngleY(targetPos, BOT_HELI.Origin), 2f);// "rotateto"
                    BOT_HELI.Call(33399, targetPos, 15, 2, 2);//"moveto"

                    if (USE_PREDATOR)
                    {
                        if (PLANE == null) return true;
                        Vector3 targetPos2 = VectorAddZ(BOTs_List[rnd.Next(BOTs_List.Count)].Origin, 1500);

                        PLANE.Call(33406, VectorToAngleY(targetPos2, PLANE.Origin), 2f);// "rotateto"
                        PLANE.Call(33399, targetPos2, 15, 2, 2);//"moveto"
                    }
                    if (count > 33)
                    {
                        bot.Call(33341);//"suicide"
                        return false;
                    }
                }
                count++;

                if (slow)
                {
                    if (wait && BOT_HELI_FIRE == 0) return !(wait = false);

                    wait = true;
                }

                if (BOT_HELI_FIRE == 0)
                {
                    Entity target = human_List[rnd.Next(hc)];

                    BOT_HELI_TARGET_POS = VectorAddZ(target.Origin, 40);
                    BOT_HELI_FLARE = Call<Entity>(308, FX_FLARE_AMBIENT, BOT_HELI_TARGET_POS);//"spawnFx"
                    Call(309, BOT_HELI_FLARE);//"triggerfx"

                    BOT_HELI_FIRE = 1;
                    if (target.Name != null) target.Call(33466, "javelin_clu_lock");//"playlocalsound" //deny remote tank //deny remote tank !important if not deny, server cause crash

                }
                else
                {
                    if (BOT_HELI_FIRE == 1)
                    {
                        Entity rocket = Call<Entity>(404, MAGICS[rnd.Next(MAGICS.Length)], VectorAddZ(BOT_HELI.Origin, -200), BOT_HELI_TARGET_POS, bot);//"magicbullet"

                        BOT_HELI_FIRE = 2;
                    }
                    else if (BOT_HELI_FIRE == 2)
                    {
                        BOT_HELI_FIRE = 0;
                    }

                    if (BOT_HELI_FLARE != null)
                    {
                        BOT_HELI_FLARE.Call(32928);//"delete"
                        BOT_HELI_FLARE = null;
                    }

                }

               
                return true;
            });
        }


        Entity PLANE, MISSILE, PREDATOR_OWNER;
        bool USE_PREDATOR;
        byte MISSILE_COUNT;

        void PredatorEnd(Entity player, H_SET H, bool respawn)
        {
            if (!respawn)
            {
                player.Call(32843);//unlink
                if (MISSILE != null) player.Call(33529, MISSILE.Origin);//setorigin
                else player.Call(33529, Helicopter.HELI_WAY_POINT);
                player.TakeWeapon("heli_remote_mp");
                WP.GiveRandomWeaponTo(player);
                Common.StartOrEndThermal(player, false);
            }
            H.HUD_KEY_INFO.Call(32897);//destroy
            H.HUD_BULLET_INFO.Call(32897);//destroy
            H.USE_PREDATOR = false;

            if (PREDATOR_OWNER != player) return;

            PREDATOR_OWNER = null;
            USE_PREDATOR = false;
            PLANE.Call(32928);//delete
            PLANE = null;
        }
        void PredatorStart(Entity player, H_SET H)
        {
            USE_PREDATOR = true;
            MISSILE_COUNT = 0;
            PREDATOR_OWNER = player;
            H.USE_PREDATOR = true;
            player.Health = 9999;
            if (PLANE != null) PLANE.Call(32928);//delete

            Vector3 origin = player.Origin; origin.Z += 100;

            PLANE = Call<Entity>(367, player, "script_model", origin, "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            PLANE.Call(32929, "vehicle_remote_uav");//setModel

            player.Call(32841, PLANE, "tag_origin");//linkto
            PLANE.Call(33402, 1500, 7);//movez


            H.HUD_KEY_INFO = HudElem.CreateFontString(player, "hudbig", 0.6f);
            H.HUD_BULLET_INFO = HudElem.CreateFontString(player, "hudbig", 0.6f);

            PLANE.AfterDelay(7000, v =>
            {
                if (FX_GREEN_LIGHT == 0) FX_GREEN_LIGHT = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"
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

                    if (MISSILE_COUNT == 10) PredatorEnd(player, H, false);

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
