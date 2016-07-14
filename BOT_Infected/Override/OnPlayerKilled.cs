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
        void BotBulletRefill(Entity bot, Entity target)
        {
            B_SET B = B_FIELD[bot.EntRef];
            if (B.wait) return;

            bot.Call(33468, B.weapon, B.ammoClip);//setweaponammoclip
            if (B.target == null) B.target = target;

        }
        void DamageModMelee(Entity player, Entity attacker,int damage)
        {
            int pe = player.EntRef;

            if (!IsBOT[pe] && !H_FIELD[pe].AXIS) player.Health +=damage;
        }
        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            //if (player == ADMIN) player.Health += damage;// damage;

            if (mod[4] == 'M'){ DamageModMelee(player, attacker,damage); return; }

            if (weapon[2] != '5')//iw5_
            {
                if (weapon != "rpg_mp") return;//rpg

                if (attacker == player){player.Health += damage; return;}
            }

            if (!IsBOT[player.EntRef] || IsBOT[attacker.EntRef]) return;//player 가 사람인 경우 or attacker 가 봇인 경우 return;

            BotBulletRefill(player, attacker);
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

            if (weapon[2] != '5') if (weapon != "rpg_mp") return; //iw5_ rpg_

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
