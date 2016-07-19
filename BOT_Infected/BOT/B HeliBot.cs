using System;
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
        class BotHeli : Inf
        {
            Vector3 VectorToAngleY(Vector3 TO, Vector3 BO)
            {
                float dx = TO.X - BO.X;
                float dy = TO.Y - BO.Y;

                BO.X = 0;
                BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.3f;
                BO.Z = 0;
                return BO;
            }
            Vector3 TARGET_POS;
            Vector3[] ORIGINS;

            internal static Entity BOT_HELI;
            Entity FLARE, RIDER;

            readonly string[] MAGICS = { "sam_projectile_mp", "javelin_mp", "ims_projectile_mp", "ac130_40mm_mp", "ac130_105mm_mp", "rpg_mp", "uav_strike_projectile_mp" };

            bool BOT_HELI_INTERVAL_STOP = true;
            int FX_FLARE_AMBIENT;
            internal static int FX_EXPLOSION;
            byte BOT_HELI_FIRE;

            public BotHeli(Entity bot, Vector3[] origins)
            {
                RIDER = bot;
                ORIGINS = origins;

                BOT_HELI = Call<Entity>(367, bot, "script_model", VectorAddZ(bot.Origin, BOT_HELI_HEIGHT), "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
                BOT_HELI.Call(32929, "vehicle_uav_static_mp");//"setmodel" vehicle_remote_uav
                BOT_HELI.Call(32848);//"hide"

                FX_EXPLOSION = Call<int>(303, "explosions/aerial_explosion");//
                FX_FLARE_AMBIENT = Call<int>(303, "misc/flare_ambient");//"loadfx"

                bot.SpawnedPlayer += () => BotHeliSpawned();
                BotHeliSpawned();
            }
            void BotHeliSpawned()
            {
                BOT_HELI_INTERVAL_STOP = true;
                if (GAME_ENDED_) return;
                
                BOT_HELI_FIRE = 2;

                if (FLARE != null)
                {
                    FLARE.Call(32928);//"delete"
                    FLARE = null;
                }
                if (BOT_HELI != null)
                {
                    BOT_HELI.Call(32848);
                    Call(304, FX_EXPLOSION, BOT_HELI.Origin);//"PlayFX"
                }

                RIDER.Health = -1;
                RIDER.Call(32848);//hide
                RIDER.Call(32841, BOT_HELI);//"linkto"
                RIDER.TakeAllWeapons();
                RIDER.AfterDelay(11000, x =>
                {
                    if (GAME_ENDED_) return;

                    RIDER.Health = 120;
                    BOT_HELI.Call(32847);//"show"
                    RIDER.Call(32847);//"show"
                    BOT_HELI_INTERVAL_STOP = false;
                });
            }

            bool OVER;
            internal void HeliBotSearch()
            {
                if (!OVER) BotHeliRide();//5초 간격

                OVER = !OVER;
            }

            byte BH_COUNT;
            void BotHeliRide()
            {
                if (BOT_HELI_INTERVAL_STOP) return;

                int hc = human_List.Count;

                if (hc == 0)
                {
                    if (FLARE != null)
                    {
                        FLARE.Call(32928);//"delete"
                        FLARE = null;
                    }
                    return;
                }

                if (BH_COUNT == 0) BotHeliMove();//15초 간격

                if (BH_COUNT == 2) BH_COUNT = 0;
                else BH_COUNT++;

                if (BOT_HELI_FIRE == 0)
                {
                    BOT_HELI_FIRE = 1;

                    Entity target = human_List[rnd.Next(hc)];
                    if (target != null)
                    {
                        TARGET_POS = VectorAddZ(target.Origin, 40);
                        FLARE = Call<Entity>(308, FX_FLARE_AMBIENT, TARGET_POS);//"spawnFx"
                        Call(309, FLARE);//"triggerfx"

                        if (target.Name != null) target.Call(33466, "javelin_clu_lock");//"playlocalsound" //deny remote tank //deny remote tank !important if not deny, server cause crash
                    }
                }
                else
                {
                    if (BOT_HELI_FIRE == 1) Call<Entity>(404, MAGICS[rnd.Next(MAGICS.Length)], VectorAddZ(BOT_HELI.Origin, -200), TARGET_POS, RIDER);//"magicbullet"

                    if (BOT_HELI_FIRE != 4) BOT_HELI_FIRE++;
                    else BOT_HELI_FIRE = 0;

                    if (FLARE != null)
                    {
                        FLARE.Call(32928);//"delete"
                        FLARE = null;
                    }
                }
            }

            void BotHeliMove()//15초간격
            {
                Vector3 targetPos = VectorAddZ(ORIGINS[rnd.Next(ORIGINS.Length)], BOT_HELI_HEIGHT);

                BOT_HELI.Call(33406, VectorToAngleY(targetPos, BOT_HELI.Origin), 2f);// "rotateto"
                BOT_HELI.Call(33399, targetPos, 15, 2, 2);//"moveto"

                if (USE_PREDATOR)
                {
                    if (PRDT.PLANE != null)
                    {
                        targetPos = VectorAddZ(ORIGINS[rnd.Next(ORIGINS.Length)], BOT_HELI_HEIGHT + 500);

                        PRDT.PLANE.Call(33406, VectorToAngleY(targetPos, PRDT.PLANE.Origin), 2f);// "rotateto"
                        PRDT.PLANE.Call(33399, targetPos, 15, 2, 2);//"moveto"
                    }
                }
            }
        }

    }
}
