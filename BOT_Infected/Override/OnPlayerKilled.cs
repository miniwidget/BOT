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

        void DamageModMelee(Entity player, Entity attacker, int damage)
        {
            int pe = player.EntRef;
            if (H_FIELD[pe] == null) return;
            if (!H_FIELD[pe].AXIS) player.Health += damage;
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (USE_ADMIN_SAFE_) if (player == ADMIN) player.Health += damage;// damage;

            if (mod[4] == 'M') { DamageModMelee(player, attacker, damage); return; }

            if (weapon[2] != '5')//iw5_
            {
                if (weapon != "rpg_mp") return;//rpg

                if (attacker == player) { player.Health += damage; return; }
            }

            int pe = player.EntRef;

            if (IsBOT[pe] == null || IsBOT[attacker.EntRef] != null) return;
            if (B_FIELD[pe].target == null) B_FIELD[pe].target = attacker;
        }

        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke] != null;

            if (BotKilled)  B_FIELD[ke].wait = true;//봇이 죽은 경우
            
            //else if (killed == attacker)//사람이 죽은 경우
            //{
            //    if (!BotKilled) if (H_FIELD[ke].AXIS) H_FIELD[ke].AX_WEP = 2;//자살로 죽음

            //    return;
            //}

            if (weapon[2] != '5') if (weapon != "rpg_mp") return; //iw5_ rpg_ //deny all killstreak weapon

            if (IsBOT[attacker.EntRef] == null)//공격자가 사람인 경우, 퍼크 주기
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
