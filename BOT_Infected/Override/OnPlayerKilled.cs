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
        void BotBulletRefill(Entity bot, Entity target,int entref)
        {
            B_SET B = B_FIELD[entref];
            if (B.wait) return;

            bot.Call(33468, B.weapon, B.ammoClip);//setweaponammoclip
            if (B.target == null) B.target = target;

        }
        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (mod == "MOD_MELEE")//none
            {
                int pe_ = player.EntRef;
                if(pe_== BOT_LUCKY_ENTREF)
                {
                    BotBulletRefill(player, attacker, pe_);
                }
                else if (!IsBOT[pe_] && !H_FIELD[pe_].AXIS) player.Health += damage;
                
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
            BotBulletRefill(player, attacker, pe);
        }

        void initBot(int ke)
        {
            B_SET B = B_FIELD[ke];
            B.target = null;
            B.wait = true;
        }
        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled)
            {
                initBot(ke);//봇이 죽은 경우
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
