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
        int FX_EXPLOSION, FX_FLARE_AMBIENT;
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
            }

            B_SET B = B_FIELD[bot.EntRef];
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
                if (GAME_ENDED_) return;

                BotHeliInterval(bot);
            });
        }

        void BotHeliInterval(Entity bot)
        {
            bot.Health = 120;
            bot.Call(32847);//"show"
            BOT_HELI.Call(32847);//"show"
            bot.Call(32841, BOT_HELI);//"linkto"
            BOT_HELI_INTERVAL_STOP = false;

            byte count = 0;
            bool slow = true;
            bool wait = true;

            BOT_HELI.OnInterval(5000, b =>
            {
                if (BOT_HELI_INTERVAL_STOP) return false;

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
                        if (PRDT.PLANE != null)
                        {
                            targetPos= VectorAddZ(BOTs_List[rnd.Next(BOTs_List.Count)].Origin, 1500);

                            PRDT.PLANE.Call(33406, VectorToAngleY(targetPos, PRDT.PLANE.Origin), 2f);// "rotateto"
                            PRDT.PLANE.Call(33399, targetPos, 15, 2, 2);//"moveto"
                        }
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
                    BOT_HELI_FIRE = 1;

                    Entity target = human_List[rnd.Next(hc)];
                    if (target != null)
                    {
                        BOT_HELI_TARGET_POS = VectorAddZ(target.Origin, 40);
                        BOT_HELI_FLARE = Call<Entity>(308, FX_FLARE_AMBIENT, BOT_HELI_TARGET_POS);//"spawnFx"
                        Call(309, BOT_HELI_FLARE);//"triggerfx"

                        if (target.Name != null) target.Call(33466, "javelin_clu_lock");//"playlocalsound" //deny remote tank //deny remote tank !important if not deny, server cause crash
                    }
                }
                else
                {
                    if (BOT_HELI_FIRE == 1)
                    {
                        BOT_HELI_FIRE = 2;

                        Entity rocket = Call<Entity>(404, MAGICS[rnd.Next(MAGICS.Length)], VectorAddZ(BOT_HELI.Origin, -200), BOT_HELI_TARGET_POS, bot);//"magicbullet"
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

    }
}
