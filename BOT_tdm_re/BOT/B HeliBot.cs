using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Timers;

namespace Tdm
{

    public partial class Tdm
    {
        #region field
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

        Entity BOT_HELI, RIDER, FLARE, TARGET,JUGG_BOT_ALLIES,JUGG_BOT_AXIS;

        readonly string[] MAGICS = { "sam_projectile_mp", "javelin_mp", "ims_projectile_mp", "ac130_40mm_mp", "ac130_105mm_mp", "rpg_mp", "uav_strike_projectile_mp" };

        bool OVER, BOT_HELI_INTERVAL_STOP = true;
        int FX_FLARE_AMBIENT, FX_GREEN_LIGHT = -1, FX_RED_LIGHT = -1;
        internal static int FX_EXPLOSION;
        byte BH_COUNT, BOT_HELI_FIRE;
        #endregion

        void BotAddWatch()
        {
            BOT_ADD_WATCH_FINISHED = true;

            List<B_SET> bots_fire = new List<B_SET>();
            foreach (B_SET B in B_FIELD)
            {
                if (B == null) continue;
                bots_fire.Add(B);
            }

            ORIGINS = Players.Select(ent => ent.Origin).ToArray();
            FX_EXPLOSION = Call<int>(303, "explosions/aerial_explosion");//loadfx
            FX_FLARE_AMBIENT = Call<int>(303, "misc/flare_ambient");//"loadfx"
            FX_GREEN_LIGHT = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"
            FX_RED_LIGHT = Call<int>(303, "misc/aircraft_light_wingtip_red");//"loadfx"

            SetRider(BOTs_List[SET.MAP_IDX % 2]);

            OnInterval(2500, () =>
            {
                if (GAME_ENDED_) return false;
                if (HUMAN_ZERO_) return true;

                foreach (B_SET B in bots_fire) B.Search();

                HeliBotSearch();

                return true;
            });
        }


        void SetRider(Entity bot)
        {
            RIDER = bot;

            BOT_HELI = Call<Entity>(367, bot, "script_model", VectorAddZ(bot.Origin, BOT_HELI_HEIGHT), "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            BOT_HELI.Call(32929, "vehicle_remote_uav");//"setmodel" 

            if (FX_GREEN_LIGHT != -1)
            {
                AfterDelay(500, () =>
                {
                    int light = 0;
                    if (IsAxis[bot.EntRef]) light = FX_RED_LIGHT; else light = FX_GREEN_LIGHT;
                    Call(305, light, BOT_HELI, "tag_light_tail1");//playFXOnTag
                    Call(305, light, BOT_HELI, "tag_light_nose");//playFXOnTag
                });

            }
            bot.Call("setorigin", BOT_HELI.Origin);//setorigin
            bot.Call(32841, BOT_HELI);//"linkto"
            bot.TakeAllWeapons();

            BOT_HELI_INTERVAL_STOP = false;

            B_SET B = B_FIELD[bot.EntRef];
            B.WAIT = true;

            if (B.RIDER_STATE == 0)
            {
                bot.SpawnedPlayer += delegate
                {
                    if (B.RIDER_STATE == 1)
                    {
                        B.RIDER_STATE = 2;
                        B.WAIT = false;
                        EndRider();
                    }
                };
            }
            B.RIDER_STATE = 1;
        }
        void EndRider()
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
                Call(304, FX_EXPLOSION, BOT_HELI.Origin);//"PlayFX"
                BOT_HELI.Call(32928);//delete
                BOT_HELI = null;
            }

            RIDER.AfterDelay(10000, r =>
            {
                if (BALANCE_STATE == State.balance_off) SetRider(RIDER);

                else
                {
                    if (IsAxis[RIDER.EntRef]) SetRider(JUGG_BOT_ALLIES); else SetRider(JUGG_BOT_AXIS);
                }
            });

        }

        void HeliBotSearch()
        {
            if (BOT_HELI_INTERVAL_STOP) return;

            if (!OVER) BotHeliRide();//5초 간격

            OVER = !OVER;
        }

        void BotHeliRide()
        {
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

                if (IsAxis[RIDER.EntRef]) TARGET = Axis_List[rnd.Next(Axis_List.Count)];
                else TARGET = Allies_List[rnd.Next(Allies_List.Count)];

                TARGET_POS = VectorAddZ(TARGET.Origin, 40);
                FLARE = Call<Entity>(308, FX_FLARE_AMBIENT, TARGET_POS);//"spawnFx"
                Call(309, FLARE);//"triggerfx"

                if (TARGET.EntRef < 18) TARGET.Call(33466, "javelin_clu_lock");//"playlocalsound" //deny remote tank //deny remote tank !important if not deny, server cause crash
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
            if (BOT_HELI != null)
            {
                BOT_HELI.Call(33406, VectorToAngleY(targetPos, BOT_HELI.Origin), 2f);// "rotateto"
                BOT_HELI.Call(33399, targetPos, 15, 2, 2);//"moveto"
            }
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
