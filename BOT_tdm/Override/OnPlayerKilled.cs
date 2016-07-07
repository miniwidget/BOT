using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {
        void tempFire(B_SET B, Entity bot, Entity target)
        {

            int i = 0;
            bot.Call(33468, B.wep, 500);//setweaponammoclip
            bot.Call(33523, B.wep);//givemaxammo

            bot.OnInterval(FIRE_TIME, x =>
            {
                if (i == 5 || B.target != null)
                {
                    return B.temp_fire = false;
                }

                var TO = target.Origin;
                var BO = bot.Origin;

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
        void initBot(B_SET B)
        {
            B.target = null;
            B.fire = false;
            B.temp_fire = false;
            B.death += 1;
            B.wait = true;
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (weapon[2] != '5' && weapon[0] != 'r') return;

            if (attacker == player && weapon[0] == 'r')//"rpg_mp")
            {
                player.Health += damage;
                return;
            }
            if (weapon[0] == 'r' && weapon[1] == 'i') return;

            if (IsBOT[attacker.EntRef])
            {
                if (USE_ADMIN_SAFE_)
                {
                    if (ADMIN != null && player == ADMIN) player.Health += damage;
                    return;
                }

                return;
            }

            int pe = player.EntRef;

            if (IsBOT[pe])
            {
                B_SET B = B_FIELD[pe];
                if (B.wait || B.temp_fire || B.target != null) return;
                B.temp_fire = true;

                tempFire(B, player, attacker);
            }

        }
        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {

            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled)//봇이 죽은 경우
            {
                initBot(B_FIELD[ke]);
            }
            if (weapon[2] != '5' && weapon[0] != 'r') return;

            if (!IsBOT[attacker.EntRef])//공격자가 사람인 경우, 퍼크 주기
            {
                if (BotKilled)
                {
                    B_FIELD[ke].killer = human_List.IndexOf(attacker);
                }
            }
            else if (!BotKilled)//봇이 사람을 죽인 경우, 봇 사격 중지
            {
                B_SET B = B_FIELD[attacker.EntRef];
                B.fire = false;
                B.target = null;
            }
        }

    }
}
