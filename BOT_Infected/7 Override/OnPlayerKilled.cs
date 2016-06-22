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
            if (player.Name.StartsWith("bot"))
            {
                if (attacker == null) return;

                int pi = player.EntRef;
                if (pi == RPG_BOT_ENTREF && weapon == "rpg_mp") { player.Health += damage; return; }
                if (pi == RIOT_BOT_ENTREF) return;
                Field F = FL[pi];
                if (F.target != null) return;
                if (weapon[2] == '5')
                {
                    F.target = attacker;
                    F.damaged = true;
                }
                return;
            }

            /*HUMAN side*/
            if (mod == "MOD_FALLING")
            {
                player.Health += damage;
            }
            else if (mod == "MOD_MELEE")
            {
                if (!FL[player.EntRef].AXIS) player.Health += damage;
            }
            //else if (USE_ADMIN_SAFE_)
            //{
            //    if (ADMIN != null && player == ADMIN) player.Health += damage;
            //}

        }
        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (attacker == null) return;

            int pe = player.EntRef;
            if (pe == -1 || pe > 17) return;
            Field F = FL[player.EntRef];
            bool BotKilled = F.BOT;

            if (player == attacker)
            {
                if (!BotKilled) F.BY_SUICIDE = true;//자살로 죽음
                return;
            }

            if (BotKilled)
            {
                F.target = null;
                if (human_List.Contains(attacker))
                {
                    F.killerIdx = human_List.IndexOf(attacker);
                }
                return;
            }
            else//사람이 죽은 경우
            {
                int ae = attacker.EntRef;
                if (ae > 17) return;
                Field af = FL[ae];

                if (af.BOT) // 봇이 사람을 죽인 경우, 봇 사격 중지
                {
                    Utilities.RawSayAll("^1BAD Luck :) ^7" + player.Name + " killed by " + attacker.Name);
                    af.target = null;
                }
                else
                {
                    F.BY_SUICIDE = false;//공격으로 죽음
                }
            }
        }

    }
}
