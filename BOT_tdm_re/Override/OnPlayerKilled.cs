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

            if (mod[4] == 'M') return;

            if (weapon[2] != '5')//iw5_
            {
                if (weapon != "rpg_mp") return;//rpg

                if (attacker == player) { player.Health += damage; return; }
            }

            int pe = player.EntRef;

            if (!IsBOT[pe]) return;
            if (B_FIELD[pe].TARGET == null)
            {
                if(IsAxis[pe] == IsAxis[attacker.EntRef]) return;//deny if attacks from same team
                B_FIELD[pe].TARGET = attacker;
            }
        }

        public override void OnPlayerKilled(Entity killed, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            int ke = killed.EntRef;

            bool BotKilled = IsBOT[ke];

            if (BotKilled)  B_FIELD[ke].WAIT = true;//봇이 죽은 경우
            
            if (weapon[2] != '5') if (weapon != "rpg_mp") return; //iw5_ rpg_ //deny all killstreak weapon

            int ae = attacker.EntRef;
            
            if (!IsBOT[ae])//공격자가 사람인 경우, 퍼크 주기
            {
                KILLER_ENTREF[ke] = human_List.IndexOf(attacker);
            }
            else //봇이 킬러인 경우, 봇 사격 중지
            {
                B_FIELD[ae].TARGET = null;
            }
        }

    }
}
