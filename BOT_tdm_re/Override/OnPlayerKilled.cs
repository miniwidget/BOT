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


        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (USE_ADMIN_SAFE_) if (player == ADMIN) { player.Health += damage; return; }// damage;

            if (mod[4] == 'M')  return;

            if (weapon[2] != '5')//iw5_
            {
                if (weapon != "rpg_mp") return;//rpg

                if (attacker == player) { player.Health += damage; return; }
            }

            int pe = player.EntRef;

            if (!IsBOT[pe] ) return;
            if (B_FIELD[pe].target == null) B_FIELD[pe].target = attacker;
        }

        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled)  B_FIELD[ke].wait = true;//봇이 죽은 경우
            
            if (weapon[2] != '5') if (weapon != "rpg_mp") return; //iw5_ rpg_ //deny all killstreak weapon

            if (!IsBOT[attacker.EntRef])//공격자가 사람인 경우, 퍼크 주기
            {
                if (BotKilled) B_FIELD[ke].killer = human_List.IndexOf(attacker);
            }
            else if (!BotKilled)//봇이 사람을 죽인 경우, 봇 사격 중지
            {
                B_FIELD[attacker.EntRef].target = null;
            }
        }

    }
}
