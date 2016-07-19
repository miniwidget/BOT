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

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (USE_ADMIN_SAFE_) if (player == ADMIN) { player.Health += damage; return; }// damage;

            if (mod[4] == 'M') { if (!IsAxis[player.EntRef]) player.Health += damage; return; }

            if (weapon[2] != '5')//iw5_
            {
                if (weapon != "rpg_mp") return;//rpg

                if (attacker == player) { player.Health += damage; return; }
            }

            int pe = player.EntRef;
            if (B_FIELD[pe] == null || IsBOT[attacker.EntRef]) return;
            if (B_FIELD[pe].TARGET == null) B_FIELD[pe].TARGET = attacker;
        }

        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled)  B_FIELD[ke].WAIT = true;//봇이 죽은 경우
            
            if (weapon[2] != '5') if (weapon != "rpg_mp") return; //iw5_ rpg_ //deny all killstreak weapon

            if (!IsBOT[attacker.EntRef])//공격자가 사람인 경우, 퍼크 주기
            {
                if (BotKilled) B_FIELD[ke].KILLER = human_List.IndexOf(attacker);
            }
            else if (!BotKilled)//봇이 사람을 죽인 경우, 봇 사격 중지
            {
                Utilities.RawSayAll("^1BAD Luck :) ^7" + killed.Name + " killed by BOT");
                B_FIELD[attacker.EntRef].TARGET = null;
            }
        }

    }
}
