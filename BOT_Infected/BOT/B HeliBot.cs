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

        Vector3 VectorToAngle(Vector3 TO, Vector3 BO)
        {
            float dx = TO.X - BO.X;
            float dy = TO.Y - BO.Y;
            float dz = BO.Z - TO.Z + 50;

            int dist = (int)Math.Sqrt(dx * dx + dy * dy);
            BO.X = (float)Math.Atan2(dz, dist) * 57.3f;
            BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.3f;
            BO.Z = 0;
            return BO;
        }
        Vector3 VectorToAngleY(Vector3 TO, Vector3 BO)
        {
            float dx = TO.X - BO.X;
            float dy = TO.Y - BO.Y;

            BO.X = 0;
            BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.3f;
            BO.Z = 0;
            return BO;
        }

        Entity BOT_HELI, BOT_HELI_FLARE, BOT_HELI_MINIMAP;
        Vector3 BOT_HELI_TARGET_POS;
        bool BOT_HELI_SHOW, BOT_HELI_SPAWNED;
        int FX_EXPLOSION;
        byte BOT_HELI_STATE;
        string[] MAGICS = { "sam_projectile_mp", "javelin_mp", "ims_projectile_mp", "ac130_40mm_mp", "ac130_105mm_mp", "rpg_mp", "uav_strike_projectile_mp" };

        void BotHeliSpawned(Entity bot)
        {
            //return;
            if (GAME_ENDED_) return;
            BOT_HELI_SPAWNED = BOT_HELI_SHOW = false;
            BOT_HELI_STATE = 3;
            if (BOT_HELI_FLARE != null)
            {
                BOT_HELI_FLARE.Call(32928);//"delete"
                BOT_HELI_FLARE = null;
            }
            if (BOT_HELI != null)
            {
                BOT_HELI_MINIMAP.Call(32848);//"hide"
                BOT_HELI.Call(32848);//"hide"
                Call(304, FX_EXPLOSION, BOT_HELI.Origin);//"PlayFX"
            }
            else BotHeliSpawn(bot);

            var B = B_FIELD[bot.EntRef];
            if (B.killer != -1)
            {
                BotCheckPerk(B.killer);
                B.killer = -1;
            }



            bot.Health = -1;
            bot.Call(32848);//hide
            bot.Call(33220, 0f);//setmovespeedscale

            bot.AfterDelay(10000, b =>
            {
                BOT_HELI_SPAWNED = BOT_HELI_SHOW = true;
            });
        }
   
        Entity BotHeliSpawn(Entity bot)
        {
            int fx_light = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"
            FX_EXPLOSION = Call<int>(303, "explosions/aerial_explosion");//

            Vector3 origin = bot.Origin + Common.GetVector(0, 0, 900);
            BOT_HELI = Call<Entity>(85, "script_model", origin);//"spawn"
            BOT_HELI.Call(32929, "vehicle_remote_uav");//"setmodel"

            ///*미니맵 모델*/
            BOT_HELI_MINIMAP = Call<Entity>(367, bot, "script_model", BOT_HELI.Origin, "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            BOT_HELI_MINIMAP.Call(33416);//notSolid
            BOT_HELI_MINIMAP.Call(32841, BOT_HELI, "tag_player");//linkTo

            BOT_HELI_MINIMAP.Call(32848);//"hide"
            BOT_HELI.Call(32848);//"hide"

            int fx_flare_ambient = Call<int>(303, "misc/flare_ambient");//"loadfx"

            BOT_HELI.OnInterval(5000, b =>
            {
                if (!BOT_HELI_SHOW) return true;
                if (GAME_ENDED_) return false;

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

                if (BOT_HELI_SPAWNED)
                {
                    BOT_HELI_SPAWNED = false;
                    bot.Health = 120;
                    bot.Call(32847);//"show"
                    BOT_HELI_MINIMAP.Call(32847);
                    BOT_HELI.Call(32847);
                    bot.Call(32841, BOT_HELI);//"linkto"
                    Call(305, fx_light, BOT_HELI, "tag_light_tail1");//"playFXOnTag"
                    Call(305, fx_light, BOT_HELI, "tag_light_nose");//"playFXOnTag"
                }

                if (BOT_HELI_STATE == 1)
                {
                    Entity target = human_List[rnd.Next(hc)];

                    BOT_HELI_TARGET_POS = target.Origin;
                    BOT_HELI_FLARE = Call<Entity>(308, fx_flare_ambient, BOT_HELI_TARGET_POS);//"spawnFx"
                    Call(309, BOT_HELI_FLARE);//"triggerfx"
                    BOT_HELI_STATE = 2;
                    if (target.Name != null) target.Call(33466, "javelin_clu_lock");//"playlocalsound" //deny remote tank //deny remote tank !important if not deny, server cause crash
                }
                else
                {
                    if (BOT_HELI_STATE == 2)
                    {
                        Vector3 startPos = BOT_HELI.Origin; startPos.Z -= 200;
                        Entity rocket = Call<Entity>(404, MAGICS[rnd.Next(MAGICS.Length)], startPos, BOT_HELI_TARGET_POS, bot);//"magicbullet"
                        BOT_HELI_STATE = 3;
                    }
                    else
                    {
                        Vector3 targetPos = BOTs_List[rnd.Next(BOTs_List.Count)].Origin;
                        Vector3 angle = VectorToAngleY(targetPos, BOT_HELI.Origin);
                        targetPos.Z += 1000;

                        BOT_HELI.Call(33406, angle, 2f);// "rotateto"
                        BOT_HELI.Call(33399, targetPos, 15, 2, 2);//"moveto"
                        BOT_HELI_STATE = 1;
                    }

                    if (BOT_HELI_FLARE != null)
                    {
                        BOT_HELI_FLARE.Call(32928);//"delete"
                        BOT_HELI_FLARE = null;
                    }
                }

                return true;
            });

            return BOT_HELI;
        }

        //bool BotHeliExplode()
        //{
        //    if (BOT_HELI == null) return false;

        //    if (BOT_HELI_FLARE != null) BOT_HELI_FLARE.Call(32928);//"delete"
        //    //Call(305, FX_EXPLODE, BOT_HELI, "tag_light_tail1");//"playfxontag"
        //    //BOT_HELI.GetField<Entity>("objModel").Call(32928);//"delete"
        //    BOT_HELI.Call("hide");//"delete"
        //    //BOT_HELI = null;

        //    return false;
        //}

    }
}
