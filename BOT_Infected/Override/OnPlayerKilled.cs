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
        bool LUCK_TEMP_FIRE;
        void TempFireLuckyBot(Entity bot, Entity target)
        {
            LUCK_TEMP_FIRE = true;

            B_SET B = B_FIELD[BOT_LUCKY_ENTREF];

            if ( B.target != null) return;

            int i = 0;
            bot.Call(33468, B.wep, 500);//setweaponammoclip
            bot.Call(33523, B.wep);//givemaxammo

            bot.OnInterval(300, bb =>
            {
                if (i == 5 || B.target != null)//|| !B.fire
                {
                    return LUCK_TEMP_FIRE = false;
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
            if (mod[4] == 'M')
            {
                int pe_ = player.EntRef;
                if(pe_== BOT_LUCKY_ENTREF)
                {
                    if (LUCK_TEMP_FIRE) return;
                    TempFireLuckyBot(player, attacker);
                    return;
                }

                if (!IsBOT[pe_] && !H_FIELD[pe_].AXIS) player.Health += damage;
                
                return;
            }

            if (weapon[2] != '5') if( weapon[1] != 'p') return;//iw5_  rpg

            int pe = player.EntRef;

            if (!IsBOT[pe]) return;//player 가 사람인 경우 return

            if (IsBOT[attacker.EntRef])//attacker 가 봇인 경우 return;
            {
                if (pe == BOT_RPG_ENTREF)//rpg 봇인 경우
                {
                    if (attacker == player)  player.Health += damage;
                }

                return;
            }

            B_SET B = B_FIELD[pe];
            if (B.not_fire) return;
            if (B.target != null) return;
            B.target = attacker;
            player.Call(33468, B.wep, 500);//setweaponammoclip
            player.Call(33523, B.wep);//givemaxammo
        }

        void initBot(B_SET B)
        {
            B.target = null;
            B.death += 1;
        }
        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled)
            {
                initBot(B_FIELD[ke]);//봇이 죽은 경우
            }
            else if (killed == attacker)//사람이 죽은 경우
            {
                if (!BotKilled) if (H_FIELD[ke].AXIS) H_FIELD[ke].AX_WEP = 2;//자살로 죽음

                return;
            }

            if (weapon[2] != '5') if (weapon[1] != 'p') return; //iw5_ rpg_

            if (!IsBOT[attacker.EntRef])//공격자가 사람인 경우, 퍼크 주기
            {
                if (BotKilled) B_FIELD[ke].killer = human_List.IndexOf(attacker);
            }
            else if (!BotKilled)//봇이 사람을 죽인 경우, 봇 사격 중지
            {
                Utilities.RawSayAll("^1BAD Luck :) ^7" + killed.Name + " killed by BOT");
                B_FIELD[attacker.EntRef].target = null;
            }
        }

    }
}
