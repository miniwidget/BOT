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

        Entity BOT_HELI, BOT_HELI_FLARE, BOT_HELI_MINIMAP;
        Vector3 BOT_HELI_TARGET_POS;
        bool BOT_HELI_SHOW;
        int FX_EXPLOSION;
        byte BOT_HELI_STATE;
        string[] MAGICS = { "sam_projectile_mp", "javelin_mp", "ims_projectile_mp", "ac130_40mm_mp", "ac130_105mm_mp", "rpg_mp", "uav_strike_projectile_mp" };

        bool BOT_HELI_RIDER_SPAWNED;
        void BotHeliSpawned(Entity bot)
        {
            if (GAME_ENDED_) return;
            BOT_HELI_RIDER_SPAWNED = true;
            BOT_HELI_SHOW = false;
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

        }

        Entity BotHeliSpawn(Entity bot)
        {
            int fx_light = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"

            Vector3 origin = bot.Origin + Common.GetVector(0, 0, 900);
            BOT_HELI = Call<Entity>(85, "script_model", origin);//"spawn"
            BOT_HELI.Call(32929, "vehicle_remote_uav");//"setmodel"

            BOT_HELI.AfterDelay(200, c =>
            {
                Call(305, fx_light, BOT_HELI, "tag_light_tail1");//"playFXOnTag"
                Call(305, fx_light, BOT_HELI, "tag_light_nose");//"playFXOnTag"
            });

            FX_EXPLOSION = Call<int>(303, "explosions/aerial_explosion");//
            ///*미니맵 모델*/
            BOT_HELI_MINIMAP = Call<Entity>(367, bot, "script_model", BOT_HELI.Origin, "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            BOT_HELI_MINIMAP.Call(33416);//notSolid
            BOT_HELI_MINIMAP.Call(32841, BOT_HELI, "tag_player");//linkTo

            BOT_HELI_MINIMAP.Call(32848);//"hide"
            BOT_HELI.Call(32848);//"hide"

            int fx_flare_ambient = Call<int>(303, "misc/flare_ambient");//"loadfx"
            byte showTime = 0;
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
                if (BOT_HELI_RIDER_SPAWNED)
                {
                    if (showTime == 4)
                    {
                        showTime = 0;
                        BOT_HELI_RIDER_SPAWNED = false;
                        BOT_HELI_SHOW = true;

                        bot.Health = 120;
                        bot.Call(32847);//"show"
                        BOT_HELI_MINIMAP.Call(32847);
                        BOT_HELI.Call(32847);
                        bot.Call(32841, BOT_HELI);//"linkto"
                    }
                    else showTime++;
                }

                if (BOT_HELI_STATE == 1)
                {
                    Entity target = human_List[rnd.Next(hc)];
                    if (target.Name == null) return true;//deny remote tank

                    BOT_HELI_TARGET_POS = target.Origin;
                    BOT_HELI_FLARE = Call<Entity>(308, fx_flare_ambient, BOT_HELI_TARGET_POS);//"spawnFx"
                    Call(309, BOT_HELI_FLARE);//"triggerfx"
                    BOT_HELI_STATE = 2;
                    target.Call(33466, "javelin_clu_lock");//"playlocalsound"
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
                        Vector3 angle = Call<Vector3>(247, targetPos - BOT_HELI.Origin);
                        angle.X = 0;
                        angle.Z = 0;

                        targetPos.Z += 800;

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

    }
}
