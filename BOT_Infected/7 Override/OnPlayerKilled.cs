using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;
using System.Diagnostics;

namespace Infected
{
    public partial class Infected
    {
        string ERROR_PATH = @"z:\mw3_error.txt"; 

        void writeErrorLoc(ref Exception ex)
        {
            var st = new StackTrace(ex, true);
            var sf = st.GetFrame(0);
            var line = sf.GetFileLineNumber();
            var name = sf.GetMethod().Name;

            string contents = "■ ■ ■ IMPORTANT void: " + name + " line: " + line;
            print(contents);
            if (!File.Exists(ERROR_PATH)) File.WriteAllText(ERROR_PATH, "/");
            File.AppendAllText(ERROR_PATH, contents);
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (player == attacker && weapon == "rpg")
            {
                player.Health += damage;
                return;
            }

            if (attacker == null || player == null || !attacker.IsPlayer || !player.IsPlayer) return;

            if (player.Name.StartsWith("bot"))
            {
                if (weapon[2] == '5')
                {
                    Field F = FL[player.EntRef];
                    if (F.human_target_idx != -1) return;
                    F.human_target_idx = attacker.EntRef;
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
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (attacker == null || player == null || !attacker.IsPlayer || !player.IsPlayer) return;

            AfterDelay(100, () => OnPlayerKilled_(ref player, ref attacker));
        }

        void OnPlayerKilled_(ref Entity player, ref Entity attacker)
        {
            Field pf = FL[player.EntRef];
            bool BotKilled = pf.BOT;

            if (player == attacker)
            {
                if (!BotKilled) pf.BY_SUICIDE = true;
                return;
            }

            if (BotKilled)//퍼크 주기
            {
                pf.human_target_idx = -1;
                int i = HUMAN_LIST.IndexOf(attacker);
                if (i != -1) pf.killerIdx = i;
            }
            else
            {
                Field af = FL[attacker.EntRef];

                if (af.BOT) //stop fire if bot kill target
                {
                    af.human_target_idx = -1;
                    Utilities.RawSayAll("^1BAD Luck :) ^7" + player.Name + " killed by " + attacker.Name);
                }
                else
                {
                    pf.BY_SUICIDE = false;//human die from attack
                }
            }
        }

    }
}
