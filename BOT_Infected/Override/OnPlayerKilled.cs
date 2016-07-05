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

        void BotTempFire(B_SET B, Entity bot, Entity target)
        {
            B.temp_fire = true;

            int i = 0;
            bot.Call(33468, B.wep, 500);//setweaponammoclip
            bot.Call(33523, B.wep);//givemaxammo

            bot.OnInterval(400, bb =>
            {
                if (i == 6 || B.target != null || !B.fire)
                {
                    return B.temp_fire = false;
                }

                var TO = target.Origin;
                var BO = bb.Origin;

                float dx = TO.X - BO.X;
                float dy = TO.Y - BO.Y;
                float dz = BO.Z - TO.Z + 50;

                int dist = (int)Math.Sqrt(dx * dx + dy * dy);
                BO.X = (float)Math.Atan2(dz, dist) * 57.3f;
                BO.Y = -10 + (float)Math.Atan2(dy, dx) * 57.3f;
                BO.Z = 0;
                bot.Call(33531, BO);//SetPlayerAngles
                i++;
                return true;
            });

        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (weapon[2] != '5' && weapon[0] != 'r') return;

            int pe = player.EntRef;
            if (IsBOT[pe])//in case of BOT
            {
                if (pe == BOT_RIOT_ENTREF) return;
                else if (pe == BOT_RPG_ENTREF)
                {
                    if (attacker == player && weapon[0] == 'r')//"rpg_mp")
                    {
                        player.Health += damage;
                        return;
                    }
                }

                B_SET B = B_FIELD[pe];
                if (B.temp_fire || B.target != null) return;

                BotTempFire(B, player, attacker);
              
            }
            else//in case of HUMAN
            {
                if (mod[4] == 'M')//"MOD_MELEE")
                {
                    if (H_FIELD[pe].AXIS) return;
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

        }

        void initBot(B_SET B)
        {
            B.target = null;
            B.fire = false;
            B.temp_fire = false;
            B.death += 1;
        }
        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled)//봇이 죽은 경우
            {
                initBot(B_FIELD[ke]);
            }

            if (mod == "MOD_SUICIDE" || killed == attacker)
            {
                if (!BotKilled)//사람이 죽은 경우
                {
                    if (H_FIELD[ke].AXIS) H_FIELD[ke].AX_WEP = 2;//자살로 죽음
                }

                return;
            }

            if (attacker == null || !attacker.IsPlayer) return;

            if (!IsBOT[attacker.EntRef])//공격자가 사람인 경우, 퍼크 주기
            {
                if (weapon[2] == '5' && BotKilled)
                {
                    B_FIELD[ke].killer = human_List.IndexOf(attacker);
                }
            }
            else if (!BotKilled)//봇이 사람을 죽인 경우, 봇 사격 중지
            {
                Utilities.RawSayAll("^1BAD Luck :) ^7" + killed.Name + " killed by BOT");
                B_SET B = B_FIELD[attacker.EntRef];
                B.fire = false;
                B.target = null;
            }
        }

    }
}
