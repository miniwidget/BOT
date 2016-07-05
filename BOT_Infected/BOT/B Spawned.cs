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
        void BotCheckPerk(int k)
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

                    H.CAN_USE_HELI = true;
                    HCT.HeliAttachFlagTag(killer);
                }
            }
        }
        /// <summary>
        /// Bot spawnded
        /// </summary>
        private void BotSpawned(Entity bot)
        {
            #region general
            bot.Health = -1;
            bot.Call(32848);//hide
            bot.Call(33220, 0f);//setmovespeedscale

            int num = bot.EntRef;
            B_SET B = B_FIELD[num];

            int delay_time = 6100;

            if (num == BOT_RPG_ENTREF || num == BOT_RIOT_ENTREF) delay_time = 10000;
            //if (num == BOT_SENTRY_ENTREF)
            //{
            //    SentryExplode();
            //}
            //else
            //{
                if (B.wep == null) B.wep = bot.CurrentWeapon;

                bot.Call(33469, B.wep, 0);//setweaponammostock
                bot.Call(33468, B.wep, 0);//setweaponammoclip
            //}
            #endregion

            #region check perk to killer

            int k = B.killer;
            if (k != -1)
            {
                BotCheckPerk(k);
                B.killer = -1;
            }

            #endregion

#if DEBUG
            if (bot.EntRef != BOT_JUGG_ENTREF)
            {
                bot.Call("show");
                bot.Health = 100;
                bot.Call("setmovespeedscale", 0.5f);
                return;

            }
#endif

            bot.AfterDelay(delay_time, bot_ =>
            {
                if (GAME_ENDED_) return;

                if (num == BOT_RPG_ENTREF) BotSearchOn_slow(bot_, B);

                else if (num == BOT_RIOT_ENTREF) { bot.Call(33220, 2f); bot.Health = 100; }

                else if (num == BOT_JUGG_ENTREF) BotSearchOn(bot_, B, true);

                //else if (num == BOT_SENTRY_ENTREF) BotSerchOn_sentry(B);

                else BotSearchOn(bot_, B, false);

                bot_.Call(32847);//show
            });
        }

        /// <summary>
        /// Normal bots start searching humans
        /// </summary>
        private void BotSearchOn(Entity bot, B_SET B, bool Jugg)
        {
            bot.Call(33220, 1f);//setmovespeedscale
            if (!Jugg) bot.Health = 120;
            else bot.Health = 100;

            int death = B.death;
            byte blockCount = 0;

            string weapon = B.wep;

            bot.OnInterval(2000, b =>
            {
                if (death != B.death) return B.fire = false;
                if (HUMAN_DIED_ALL_) return !(B.fire = false);

                Vector3 bo = b.Origin;

                if (B.target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(B.target))
                    {
                        var TOD = B.target.Origin.DistanceTo(bo);
                        if (TOD < FIRE_DIST)
                        {
                            b.Call(33468, weapon, 500);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo
                            return true;
                        }
                    }

                    B.target = null;
                    b.Call(33469, weapon, 0);//setweaponammostock
                    b.Call(33468, weapon, 0);//setweaponammoclip
                }

                B.fire = false;

                foreach (Entity human in human_List)
                {
                    var HOD = human.Origin.DistanceTo(bo);

                    if (HOD < FIRE_DIST)
                    {
                        B.target = human;

                        b.Call(33468, weapon, 500);//setweaponammoclip
                        b.Call(33523, weapon);//givemaxammo

                        if (Jugg) if (human.Name != null) human.Call(33466, "AF_victory_music");//"playlocalsound"

                        blockCount++;
                        if (blockCount == 6) blockCount = 0;
                        byte bc = blockCount;
                        B.fire = true;

                        b.OnInterval(410, bb =>
                        {
                            if (bc != blockCount ||  !B.fire) return false;

                           //Print(bc + " " + human.Name + " " + bot.Name);

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
            int death = B.death;
            byte blockCount = 0;

            bot.OnInterval(2000, bot_ =>
            {
                if (death != B.death) return B.fire = false;
                if (HUMAN_DIED_ALL_) return !(B.fire = false);
                Vector3 bo = bot.Origin;

                if (B.target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(B.target))
                    {
                        if (B.target.Origin.DistanceTo(bo) < FIRE_DIST) return true;
                    }

                    B.target = null; //타겟과 거리가 멀어진 경우, 타겟 제거
                    bot_.Call(33468, "rpg_mp", 0);//setweaponammoclip
                }

                B.fire = false;

                foreach (Entity human in human_List)
                {
                    var POD = human.Origin.DistanceTo(bo);

                    if (POD < FIRE_DIST)
                    {
                        B.target = human;
                        blockCount++;
                        byte bc = blockCount;

                        if (human.Name != null) human.Call(33466, "missile_incoming");
                        B.fire = true;
                        bot_.OnInterval(1500, bb =>
                        {
                            if (bc != blockCount || !B.fire) return false;

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
            bot.Call(33220, 1f);//setmovespeedscale
            Entity target = null;
            B_SET B = B_FIELD[bot.EntRef];
            int death = B.death;

            BOT_SERCH_ON_LUCKY_FINISHED = true;
            string weapon = bot.CurrentWeapon;
            byte blockCount = 0;
            bot.Call(33469, weapon, 0);//setweaponammostock
            bot.Call(33468, weapon, 0);//setweaponammoclip

            List<Entity> HumanAxis = new List<Entity>();

            foreach (Entity player in Players)
            {
                if (player == null || !player.IsPlayer) continue;
                if (player.Name.StartsWith("bot")) continue;

                HumanAxis.Add(player);
            }

            bot.OnInterval(2000, b =>
            {
                if (death != B.death) return false;

                Vector3 bo = b.Origin;

                if (target != null)//이미 타겟을 찾은 경우
                {
                    if (HumanAxis.Contains(target))
                    {
                        if (target.Origin.DistanceTo(bo) < FIRE_DIST)
                        {
                            b.Call(33468, weapon, 500);//setweaponammoclip
                            b.Call(33523, weapon);//givemaxammo
                            return true;
                        }
                    }

                    target = null;
                    b.Call(33469, weapon, 0);//setweaponammostock
                    b.Call(33468, weapon, 0);//setweaponammoclip
                }

                B.fire = false;

                foreach (Entity human in HumanAxis)
                {
                    var HOD = human.Origin.DistanceTo(bo);

                    if (HOD < FIRE_DIST)
                    {
                        target = human;

                        b.Call(33468, weapon, 500);//setweaponammoclip
                        b.Call(33523, weapon);//givemaxammo

                        blockCount++;
                        if (blockCount == 6) blockCount = 0;
                        byte bc = blockCount;
                        B.fire = true;

                        b.OnInterval(300, bb =>
                        {
                            if (!B.fire || bc != blockCount) return false;

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

        /*

        /// <summary>
        /// Sentry bot starts searching humans
        /// </summary>
        private void BotSerchOn_sentry(B_SET B)
        {
            SG_BOT.Call(33220, 1f);//setmovespeedscale
            SG_BOT.Health = 300;
            int death = B.death;
            B.fire = true;
            if (SG == null) SG = SpawnSentry();
            SG.Call(32847);//show
            SG.Call(33006, SG_BOT);//setsentryowner
            SG.Call(33007, SG_BOT);//setsentrycarrier
            bool remote = false;

            byte blockCount = 0;

            SG_BOT.OnInterval(2000, b =>
            {
                if (death != B.death) return B.fire = false;
                if (HUMAN_DIED_ALL_) return !(B.fire = false);
                //Print(SG.Health);
                Vector3 bo = b.Origin;

                if (B.target != null)//이미 타겟을 찾은 경우
                {
                    if (human_List.Contains(B.target))
                    {
                        if (B.target.Origin.DistanceTo(bo) < FIRE_DIST) return true;
                    }

                    B.target = null;
                    SG_BOT.Call(32980, SG);//RemoteControlTurretOff
                    remote = false;
                }

                B.fire = false;

                foreach (Entity human in human_List)
                {

                    if (human.Origin.DistanceTo(bo) < FIRE_DIST)
                    {
                        B.target = human;

                        blockCount++;
                        if (blockCount == 6) blockCount = 0;
                        byte bc = blockCount;

                        SG_BOT.Call(32979, SG);//remotecontrolturret
                        remote = true;
                        B.fire = true;
                        b.OnInterval(200, bb =>
                        {
                            if (bc != blockCount || !B.fire) return false;

                            var tagback = human.Origin; tagback.Z += 25;//Call<Vector3>(33128, "tag_weapon_left");//.
                            var aim = SG.Call<Vector3>(33128, "tag_aim");
                            Vector3 angle = Call<Vector3>(247, tagback - aim);//vectortoangles
                            SG_BOT.Call(33531, angle);//SetPlayerAngles
                            return true;
                        });

                        return true;
                    }

                }

                if (remote)
                {
                    remote = false;
                    SG_BOT.Call(32980, SG);//RemoteControlTurretOff
                }
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
            var angle = SG_BOT.Call<Vector3>("getplayerangles");
            SG.SetField("angles", new Vector3(0, angle.Y, 0));
            SG.Call(33008, true);//"setturretminimapvisible"
            SG.Call(32929, "sentry_minigun_weak");//setModel
            SG.Call(33084, 180f);//SetLeftArc
            SG.Call(33083, 180f);//SetRightArc
            SG.Call(32864, "sentry");//setmode : sentry sentry_offline
            //SG.Call("setCanDamage", true);
            SG.Call("makeTurretSolid");
            //SG.Health = 100;
            return SG;
        }
        void SentryExplode()
        {
            if (SG == null) return;

            SG.Call(32929, "sentry_minigun_weak_destroyed");//setmodel
            SG_BOT.Call(32980, SG);//"remotecontrolturretoff"
            int i = Call<int>(303, "explosions/sentry_gun_explosion");//loadfx
            Call(305, i, SG, "tag_origin");//playfxontag
            SG.AfterDelay(2500, sg =>
            {
                SG.Call(32848);
                SG.Call(32929, "sentry_minigun_weak");//setModel
            });//hide
        }
        */
    }
}
