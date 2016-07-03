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

        /// <summary>
        /// Bot spawnded
        /// </summary>
        private void BotSpawned(Entity bot)
        {
            #region general

            bot.Call(32848);//hide
            bot.Call(33220, 0f);//setmovescale

            int num = bot.EntRef;

            if (num == BOT_SENTRY_ENTREF)
            {
                if (SG != null) SentryExplode(true);
            }

            B_SET B = B_FIELD[num];
            if (B.wep == null) B.wep = bot.CurrentWeapon;

            bot.Call(33469, B.wep, 0);//setweaponammostock
            bot.Call(33468, B.wep, 0);//setweaponammoclip

            #endregion

            #region check perk to killer

            int k = B.killer;
            if (k != -1)
            {
                if (human_List.Count > k)
                {
                    Entity killer = human_List[k];
                    if (killer.EntRef > 17) return;

                    H_SET H = H_FIELD[killer.EntRef];
                    if (H.PERK > 34) return;

                    if (H.PERK_TXT.Length != 12) H.PERK_COUNT_HUD.SetText(H.PERK_TXT += "*");

                    var i = (H.PERK += 1);

                    if (i > 2 && i % 3 == 0)
                    {
                        i = i / 3; if (i > 10) return;
                        PK.Perk_Hud(killer, i);
                        killer.Call(33466, "mp_killstreak_radar");
                    }
                    else if (i == 11)
                    {
                        H.PERK_COUNT_HUD.SetText(H.PERK_TXT = "^1**********");

                        H.USE_HELI = 1;
                        HCT.HeliAttachFlagTag(killer);
                    }
                }

                B.killer = -1;
            }

            #endregion

            int delay_time = 6100;

            if (num == BOT_RPG_ENTREF || num == BOT_RIOT_ENTREF) delay_time = 10000;
            else if (num != BOT_JUGG_ENTREF) bot.Health = -1;

            bot.AfterDelay(delay_time, bot_ =>
            {
                if (GAME_ENDED_) return;

                if (num == BOT_RPG_ENTREF) BotSearchOn_slow(bot_, B);

                else if (num == BOT_RIOT_ENTREF) bot.Call(33220, 2f);

                else if (num == BOT_JUGG_ENTREF) BotSearchOn(bot_, B, true);

                else if (num == BOT_SENTRY_ENTREF) BotSerchOn_sentry(B);

                else BotSearchOn(bot_, B, false);

                bot_.Call(32847);//show
            });
        }

        /// <summary>
        /// Normal bots start searching humans
        /// </summary>
        private void BotSearchOn(Entity bot, B_SET B, bool Jugg)
        {
            bot.Call(33220, 1f);//setmovescale
            if (!Jugg) bot.Health = 120;
            bool pause = false;
            int death = B.death;
            B.fire = true;

            string weapon = B.wep;

            bot.OnInterval(2000, b =>
            {
                if (death != B.death || GAME_ENDED_) return !(pause = true);
                if (!HUMAN_CONNECTED_ || HUMAN_DIED_ALL) pause = true;

                Vector3 bo = b.Origin;

                var target = B.target;
                if (target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(target))
                    {
                        var POD = target.Origin.DistanceTo(bo);
                        if (POD < FIRE_DIST)
                        {
                            b.Call(33468, weapon, 500);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo

                            pause = false;
                            return true;
                        }
                    }

                    B.target = null;
                    B.fire = false;
                }
                b.Call(33469, weapon, 0);//setweaponammostock
                b.Call(33468, weapon, 0);//setweaponammoclip
                pause = true;

                //타겟 찾기 시작
                foreach (Entity human in human_List)
                {
                    var POD = human.Origin.DistanceTo(bo);

                    if (POD < FIRE_DIST)
                    {
                        B.target = human;
                        B.fire = true;
                        pause = false;
                        b.Call(33468, weapon, 500);//setweaponammoclip
                        b.Call(33523, weapon);//givemaxammo

                        if (Jugg) if (human.Name != null) human.Call(33466, "AF_victory_music");//"playlocalsound"

                        b.OnInterval(410, bb =>
                        {
                            if (pause || !B.fire) return false;

                            var TO = human.Origin;
                            var BO = bb.Origin;

                            float dx = TO.X - BO.X;
                            float dy = TO.Y - BO.Y;
                            float dz = BO.Z - TO.Z + 50;

                            int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                            BO.X = (float)Math.Atan2(dz, dist) * 57.32f;
                            BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.32f;
                            BO.Z = 0;

                            bb.Call(33531, BO);//SetPlayerAngles
                            return true;
                        });

                        return true;
                    }

                }
                return true;

            });

        }

        /// <summary>
        /// Rpg bot starts searching humans
        /// </summary>
        private void BotSearchOn_slow(Entity bot, B_SET B)
        {
            bot.Call(33220, 0.7f);//setmovespeedscale
            bot.Health = 120;
            bool pause = false;
            int death = B.death;
            B.fire = true;

            bot.OnInterval(2000, bot_ =>
            {
                if (death != B.death || GAME_ENDED_) return !(pause = true);
                if (!HUMAN_CONNECTED_ || HUMAN_DIED_ALL) pause = true;
                Vector3 bo = bot.Origin;
                var target = B.target;
                if (target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(target))
                    {
                        var POD = target.Origin.DistanceTo(bo);
                        if (POD < FIRE_DIST)
                        {
                            pause = false;
                            return true;
                        }
                    }

                    B.target = null; //타겟과 거리가 멀어진 경우, 타겟 제거
                    B.fire = false;
                    bot_.Call(33468, "rpg_mp", 0);//setweaponammoclip
                }

                pause = true;
                //타겟 찾기 시작
                foreach (Entity human in human_List)
                {

                    var POD = human.Origin.DistanceTo(bo);

                    if (POD < FIRE_DIST)
                    {
                        B.target = human;
                        B.fire = true;
                        pause = false;

                        if (human.Name != null) human.Call(33466, "missile_incoming");

                        bot_.OnInterval(1500, bb =>
                        {

                            if (pause || !B.fire) return false;

                            var TO = human.Origin;
                            var BO = bb.Origin;

                            float dx = TO.X - BO.X;
                            float dy = TO.Y - BO.Y;
                            float dz = BO.Z - TO.Z + 50;

                            int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                            BO.X = (float)Math.Atan2(dz, dist) * 57.32f;
                            BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.32f;
                            BO.Z = 0;

                            bb.Call(33531, BO);//SetPlayerAngles
                            bb.Call(33468, "rpg_mp", 2);//setweaponammoclip
                            return true;
                        });

                        return true;
                    }

                }

                return true;

            });
        }

        /// <summary>
        /// Survivor bot starts searching Infected humans
        /// </summary>
        private void BotSerchOn_lucky(Entity bot)
        {
            BOT_SERCH_ON_LUCKY_FINISHED = true;
            bot.Call(33220, 1f);//setmovescale
            string weapon = bot.CurrentWeapon;

            List<Entity> HumanAxis = new List<Entity>();
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;
                if (!ent.Name.StartsWith("bot"))
                {
                    HumanAxis.Add(ent);
                }
            }
            Entity target = null;
            bool fire = false;
            bool pause = false;
            B_SET B = B_FIELD[bot.EntRef];
            int death = B.death;

            bot.OnInterval(2000, b =>
            {
                if (death != B.death || GAME_ENDED_) return !(pause = true);
                if (!HUMAN_CONNECTED_ || HUMAN_DIED_ALL) pause = true;
                Vector3 bo = b.Origin;

                if (target != null)//이미 타겟을 찾은 경우
                {
                    if (HumanAxis.Contains(target))
                    {
                        var POD = target.Origin.DistanceTo(bo);
                        if (POD < FIRE_DIST)
                        {
                            b.Call(33468, weapon, 500);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo

                            pause = false;
                            return true;
                        }
                    }

                    target = null;
                    fire = false;
                }
                b.Call(33469, weapon, 0);//setweaponammostock
                b.Call(33468, weapon, 0);//setweaponammoclip
                pause = true;

                //타겟 찾기 시작
                foreach (Entity human in HumanAxis)
                {
                    var POD = human.Origin.DistanceTo(bo);

                    if (POD < FIRE_DIST)
                    {
                        target = human;
                        fire = true;
                        pause = false;
                        b.Call(33468, weapon, 500);//setweaponammoclip
                        b.Call(33523, weapon);//givemaxammo
                        b.OnInterval(300, bb =>
                        {
                            if (pause || GAME_ENDED_ || !fire) return false;

                            var TO = human.Origin;
                            var BO = bb.Origin;

                            float dx = TO.X - BO.X;
                            float dy = TO.Y - BO.Y;
                            float dz = BO.Z - TO.Z + 50;

                            int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                            BO.X = (float)Math.Atan2(dz, dist) * 57.32f;
                            BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.32f;
                            BO.Z = 0;

                            bb.Call(33531, BO);//SetPlayerAngles
                            return true;
                        });

                        return true;
                    }
                }
                return true;

            });
        }

        /// <summary>
        /// Sentry bot starts searching humans
        /// </summary>
        private void BotSerchOn_sentry(B_SET B)
        {
            SG_BOT.Call(33220, 1f);//setmovescale
            SG_BOT.Health = 400;
            int death = SENTRY_COUNT = B.death;

            SentryOffline(true);

            SG_BOT.OnInterval(3000, b =>
            {
                if (death != B.death || GAME_ENDED_) return false;
                if (!HUMAN_CONNECTED_ || HUMAN_DIED_ALL) return SentryStopFire = true;

                Vector3 bo = b.Origin;

                if (B.target != null)//이미 타겟을 찾은 경우        
                {
                    if (human_List.Contains(B.target))
                    {
                        var POD = B.target.Origin.DistanceTo(bo);
                        if (POD < FIRE_DIST)
                        {
                            SentryStopFire = false;
                            return true;
                        }
                    }
                    B.target = null; //타겟과 거리가 멀어진 경우, 타겟 제거
                    SentryOffline(false);
                }

                foreach (Entity human in human_List)
                {
                    var POD = human.Origin.DistanceTo(bo);

                    if (POD < FIRE_DIST)
                    {
                        Print(human.Name);

                        B.target = human;
                        Vector3 angle = Call<Vector3>(247, human.Origin - bo);//vectortoangles
                        SetSentryBotPos(angle);
                        return true;
                    }
                }
                if(B.target!=null) B.target = null;

                return true;

            });
        }

        /// <summary>
        /// Sentry Gun Entity
        /// </summary>
        Entity SG;
        /// <summary>
        /// Sentry Gun Bot Entity
        /// </summary>
        Entity SG_BOT;
        Entity SpawnSentry()
        {
            SG = Call<Entity>(19, "misc_turret", SG_BOT.Origin, "sentry_minigun_mp");
            SG.SetField("angles", new Vector3(0, SG_BOT.Call<Vector3>("getplayerangles").Y, 0));
            SG.Call(33084, 80f);//SetLeftArc
            SG.Call(33083, 80f);//SetRightArc
            SG.Call(33008, true);//"setturretminimapvisible"
            SG.Health = 400;
            SG.Call(33051, "allies");//setturretteam
            return SG;
        }
        bool SentryStopFire;
        int SENTRY_COUNT;

        bool SentryOnline()
        {
            //Print("online");
            if (SG != null)
            {
                SentryStopFire = true;
                SG.Call(32928);//delete
                SG = null;
            }
            if (SG == null) SG = SpawnSentry();
            SentryStopFire = true;

            int bullet_count = 40;


            SG.Call(32929, "sentry_minigun_weak");//setModel
            SG.Call(33417, true);//setCanDamage
            SG.Call(33052);//makeTurretSolid
            SG.Call(32864, "sentry");//setmode : sentry sentry_offline
            SG.Call(32941);//makeUsable
            //SG.Call("SetDefaultDropPitch", -89f);
            SG.Notify("placed");
            
            Call(42, "testClients_doCrouch", 1);

            SG.AfterDelay(1000, sg =>
            {
                if (SG != null)
                {
                    SentryStopFire = false;
                    int cc = SENTRY_COUNT;
                    SG.OnInterval(100, sgg =>
                    {
                        if (cc != SENTRY_COUNT || SG == null || SentryStopFire)
                        {
                            Print(cc + "//" + SENTRY_COUNT + "stopFire:"+SentryStopFire.ToString());
                            return false;
                        }
                        if (SG.Health < 0) return SentryExplode(false);

                        for (int i = 0; i < bullet_count; i++) SG.Call("shootTurret");

                        return true;
                    });
                }
            });

            return true;
        }
        void SentryOffline(bool init)
        {
            //Print("offline");
            SentryStopFire = true;
            SG_BOT.Call(33220, 1.5f);//setmovescale
            if (init)
            {
                if (SG != null)
                {
                    SG.Call("delete");
                    SG = null;
                }
            }

            if (SG == null) SG = SpawnSentry();

            SG.Call(32929, "sentry_minigun_weak_obj");//setModel
            SG.Call(33008, false);//setturretminimapvisible
            SG.Call(33006, SG_BOT);//setsentryowner
            SG.Call(33007, SG_BOT);//setsentrycarrier
            SG.Call(32864, "sentry_offline");//setmode : sentry sentry_offline

            Call(42, "testClients_doCrouch", 0);
        }
        bool SentryExplode(bool died)
        {
            //Print("delete");
            SG_BOT.Call(33220, 1.5f);//setmovescale
            Call(42, "testClients_doCrouch", 0);

            if (SG == null) return false;

            if (!died) SG_BOT.Call("setorigin", SG.Origin);
            
            SG.Call(32929, "sentry_minigun_weak_destroyed");//setmodel
            SentryStopFire = true;

            //SG.Call("playSound", "sentry_explode");//playSound
            SG.Call(33008, false);//SetTurretMinimapVisible
            int i = Call<int>(303, "explosions/sentry_gun_explosion");//loadfx
            Call(305, i, SG, "tag_origin");//playfxontag
            SG.AfterDelay(2500, sg =>
            {
                if (SG == null) return;
                SG.Call(32928);//delete
                SG = null;
            });
            return false;
        }
        void SetSentryBotPos(Vector3 angle)
        {
            SG_BOT.Call(33531, angle);//SetPlayerAngles

            if (SentryOnline())
            {
                int ang = (int)angle.Y;
                if (ang > 0)
                {
                    if (ang < 90)//2사분면
                    {
                        angle.X = -50;
                        angle.Y = -50;
                    }
                    else//3사분면
                    {
                        angle.X = +50;
                        angle.Y = -50;
                    }
                }
                else
                {
                    if (ang < -90)//3사분면
                    {
                        angle.X = 50;
                        angle.Y = 50;
                    }
                    else//4사분면
                    {
                        angle.X = -50;
                        angle.Y = +50;
                    }
                }
                angle.Z = 0;
                Vector3 repos = SG_BOT.Origin + angle;
                SG_BOT.Call(33529, repos);//setorigin
                SG_BOT.Call(33220, 0f);//setmovescale
            }
        }

    }
}
