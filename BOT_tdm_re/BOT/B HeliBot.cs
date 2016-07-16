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

        Entity BOT_HELI, RIDER, FLARE, TARGET;

        readonly string[] MAGICS = { "sam_projectile_mp", "javelin_mp", "ims_projectile_mp", "ac130_40mm_mp", "ac130_105mm_mp", "rpg_mp", "uav_strike_projectile_mp" };

        bool OVER, BOT_HELI_INTERVAL_STOP = true;
        int FX_EXPLOSION, FX_FLARE_AMBIENT, FX_GREEN_LIGHT = -1;
        byte BH_COUNT, BOT_HELI_FIRE;
        string RIDER_TEAM;
        #endregion


        void SetRider(Entity bot, B_SET B)
        {
            B.wait = true;
            RIDER = bot;

            RIDER_TEAM = bot.GetField<string>("sessionteam");

            BOT_HELI = Call<Entity>(367, bot, "script_model", VectorAddZ(bot.Origin, BOT_HELI_HEIGHT), "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            BOT_HELI.Call(32929, "vehicle_remote_uav");//"setmodel" 
            
            if (FX_GREEN_LIGHT != -1)
            {
                AfterDelay(500, () =>
                {
                    Call(305, FX_GREEN_LIGHT, BOT_HELI, "tag_light_tail1");//playFXOnTag
                    Call(305, FX_GREEN_LIGHT, BOT_HELI, "tag_light_nose");//playFXOnTag
                });
                
            }
            RIDER.Call(32841, BOT_HELI);//"linkto"
            RIDER.TakeAllWeapons();

            BOT_HELI_INTERVAL_STOP = false;
            if (B.RiderState == 0)
            {
                bot.SpawnedPlayer += delegate
                {
                    if (B.RiderState == 1)
                    {
                        B.RiderState = 2;
                        EndRider(B);
                    }
                };
            }
            B.RiderState = 1;
        }
        void EndRider(B_SET B)
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
                BOT_HELI.Call("delete");
                BOT_HELI = null;
            }

            RIDER.AfterDelay(10000, r =>
            {
                if (RIDER_TEAM == "allies") SetRider(BOTs_List[1],B);
                else SetRider(BOTs_List[0],B);
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

                if (RIDER_TEAM == "allies") TARGET = Axis_List[rnd.Next(Axis_List.Count)];
                else TARGET = Allies_List[rnd.Next(Allies_List.Count)];

                TARGET_POS = VectorAddZ(TARGET.Origin, 40);
                FLARE = Call<Entity>(308, FX_FLARE_AMBIENT, TARGET_POS);//"spawnFx"
                Call(309, FLARE);//"triggerfx"

                if (TARGET.Name != null) TARGET.Call(33466, "javelin_clu_lock");//"playlocalsound" //deny remote tank //deny remote tank !important if not deny, server cause crash
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
