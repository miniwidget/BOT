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
        //int ttt;
        void BotTempFire(B_SET B, Entity bot, Entity target)
        {
            int i = 0;
            bot.Call(33468, B.wep, 500);//setweaponammoclip
            bot.Call(33523, B.wep);//givemaxammo

            //int entref = bot.EntRef;
            //ttt++;
            bot.OnInterval(300, bb =>
            {
                if (i == 5 || B.target != null)//|| !B.fire
                {
                    return B.temp_fire = false;
                }
                //if(entref == BOT_JUGG_ENTREF) Print(ttt +"데미지 " + bot.Name + "//" + target.Name);

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
            //Print(mod);
            if (mod[4] == 'M')
            {
                if (!IsBOT[player.EntRef] && !H_FIELD[player.EntRef].AXIS) player.Health += damage;
                return;
            }

            if (weapon[2] != '5') if( weapon[1] != 'p') return;//iw5 rpg

            int pe = player.EntRef;

            if (!IsBOT[pe]) return;
           

            if (pe == BOT_RIOT_ENTREF) return;
            else if (pe == BOT_RPG_ENTREF)
            {
                if (attacker == player)
                {
                    player.Health += damage;
                    return;
                }
            }

            if (IsBOT[attacker.EntRef]) return;

            B_SET B = B_FIELD[pe];
            if (B.wait || B.temp_fire || B.target != null) return;
            B.temp_fire = true;

            BotTempFire(B, player, attacker);
        }

        void initBot(B_SET B)
        {
            B.target = null;
            B.fire = false;
            B.temp_fire = false;
            B.death += 1;
            B.wait = true;
        }
        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled) initBot(B_FIELD[ke]);//봇이 죽은 경우
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
                B_SET B = B_FIELD[attacker.EntRef];
                B.fire = false;
                B.target = null;
            }
        }

    }
}
