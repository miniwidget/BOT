using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Infected
{
    public partial class Infected
    {
        void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }

        void BotTempFire(B_SET B, Entity bot, Entity target)
        {
            B.temp_fire = true;
            string weapon = B.wep;

            int i = 0;
            bot.Call(33468, weapon, 500);//setweaponammoclip
            bot.Call(33523, weapon);//givemaxammo

            bot.OnInterval(400, bb =>
            {
                if (i == 6 || B.target != null)
                {
                    return B.temp_fire = false;
                }

                var TO = target.Origin;
                var BO = bb.Origin;

                float dx = TO.X - BO.X;
                float dy = TO.Y - BO.Y;
                float dz = BO.Z - TO.Z + 50;

                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                BO.X = (float)Math.Atan2(dz, dist) * 57.32f;
                BO.Y = (float)Math.Atan2(dy, dx) * 57.32f;
                bb.Call(33531, BO);//SetPlayerAngles
                i++;
                return true;
            });

        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            try
            {
                int i = player.EntRef;

                if (IsBOT[i])
                {
                    if (attacker == player && weapon == "rpg_mp")
                    {
                        player.Health += damage;
                        return;
                    }
                    if (i == BOT_RIOT_ENTREF) return;
                    if (weapon[2] != '5') return;
                    if (IsAXIS[attacker.EntRef]) return;

                    B_SET B = B_FIELD[i];
                    if (B.temp_fire || B.target != null) return;

                    BotTempFire(B, player, attacker);
                }
                else if (mod == "MOD_MELEE")
                {
                    if (IsAXIS[attacker.EntRef]) return;
                    player.Health += damage;
                }
                else if (USE_ADMIN_SAFE_)
                {
                    if (ADMIN != null && player == ADMIN)
                    {
                        player.Health += damage;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Print("데미지 에러");
            }
        }

        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            try
            {
                if (attacker == null || !attacker.IsPlayer) return;

                int ke = killed.EntRef;

                if (mod == "MOD_SUICIDE" || killed == attacker)
                {
                    if (!IsBOT[ke]) H_FIELD[ke].BY_SUICIDE = true;//자살로 죽음

                    return;
                }

                int ae = attacker.EntRef;
                bool BotAttker = IsBOT[ae];

                if (!BotAttker)//공격자가 사람인 경우, 퍼크 주기
                {
                    if (IsAXIS[ae]) return;

                    int p = IsPERK[ae];
                    if (weapon[2] == '5' && p < 34)
                    {
                        B_FIELD[ke].killer = human_List.IndexOf(attacker);
                    }
                }
                else if (!IsBOT[ke])//사람이 죽은 경우
                {
                    if (BotAttker) // 봇이 사람을 죽인 경우, 봇 사격 중지
                    {
                        Utilities.RawSayAll("^1BAD Luck :) ^7" + killed.Name + " killed by " + attacker.Name);
                        B_SET B = B_FIELD[ae];
                        B.fire = false;
                        B.target = null;
                    }
                    else
                    {
                        H_FIELD[ke].BY_SUICIDE = false;//공격으로 죽음
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Print("킬드 에러");
            }
        }

    }
}
