using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {

        void BotDeplay()
        {
            if (!SET.DEPLAY_BOT_) return;

            int diff = SET.BOT_SETTING_NUM - BOTs_List.Count;

            if (diff == 0) return;
            else if (diff < 0)
            {
                diff = -diff;
                for (int i = BOTs_List.Count - 1; i < BOTs_List.Count; i--)
                {
                    if (diff == 0) break;
                    Call("kick", BOTs_List[i].EntRef);
                    diff--;

                }
            }
            else if (diff > 0)
            {
                OnInterval(250, () =>
                {
                    if (diff == 0) return false;
                    diff--;

                    Entity bot = Utilities.AddTestClient();
                    if (bot == null) diff++;

                    return true;
                });
            }


        }

        private void Bot_Connected(Entity bot, int i)
        {

            int be = bot.EntRef;
            if (be == -1 || i > SET.BOT_SETTING_NUM)
            {
                Call(286, be);//kick
                return;
            }


            string wp = bot.CurrentWeapon;
            string sound = null;
            if (wp == "rpg_mp") sound = "AF_victory_music";
            else if (wp.StartsWith("iw5_m60")) sound = "missile_incoming";
            bot.Call(33523, wp);//giveMaxaAmmo

            B_FIELD[be] = new B_SET(bot)
            {
                AXIS = bot.GetField<string>("sessionteam") == "axis",
                alertSound = sound,
                weapon = wp,
                ammoClip = bot.Call<int>(33460, wp)
            };
            IsBOT[be] = "1";

            B_SET B = B_FIELD[be];

            bot.Call(33469, wp, 0);//setweaponammostock
            bot.Call(33468, wp, 0);//setweaponammoclip
            bot.Call(33427);//disableweaponpickup
            if (B.AXIS) Axis_List.Add(bot);
            else Allies_List.Add(bot);

            if (i == SET.BOT_SETTING_NUM) GetTeamState();

            bot.SpawnedPlayer += delegate
            {
                if (GAME_ENDED_) return;
                bot.Call(33469, B.weapon, 0);//setweaponammostock
                bot.Call(33468, B.weapon, 0);//setweaponammoclip
                bot.Call(33427);//disableweaponpickup

                int k = B.killer;
                if (k != -1)
                {
                    BotCheckPerk(k);
                    B.killer = -1;
                }

                B.wait = false;
            };

            if (B.alertSound == "missile_incoming")
            {
                bot.OnNotify("weapon_fired", (p, w) => B.SetAngle());
                return;
            }

            byte fire = 1;
            bot.OnNotify("weapon_fired", (p, w) =>
            {
                if (fire == 1) fire = 2;
                else
                {
                    if (fire == 3) fire = 0;
                    fire++;
                    return;
                }
                B.SetAngle();
            });
        }

        void GetTeamState()
        {
            TK.SetTank(BOTs_List[rnd.Next(BOTs_List.Count)]);

            Log.Write(LogLevel.None, "■ BOTs:{0} ■ MAP:{1}", BOTs_List.Count, SET.MAP_IDX);

            Call(42, "scr_infect_timelimit", "20");

            GET_TEAMSTATE_FINISHED = true;

            foreach (H_SET H in H_FIELD)
            {
                if (H == null) continue;
                H.HUD_SERVER.Alpha = 0.7f;
                H.HUD_RIGHT_INFO.Alpha = 0.7f;
            }

            if (!HUMAN_ZERO_) BotDoAttack(true);
        }

        bool BotDoAttack(bool attack)
        {
            if (attack)
            {
                if (!BOT_ADD_WATCH_FINISHED) BotAddWatch();

                Call(42, "testClients_doCrouch", 0);
                Call(42, "testClients_doMove", 1);
                Call(42, "testClients_doAttack", 1);
            }
            else
            {
                Call(42, "testClients_doCrouch", 1);
                Call(42, "testClients_doMove", 0);
                Call(42, "testClients_doAttack", 0);
            }
            return false;
        }


        void BotCheckPerk(int k)
        {
            if (human_List.Count <= k) return;

            Entity killer = human_List[k];
            if (killer.EntRef > 17) return;//deny tank

            H_SET H = H_FIELD[killer.EntRef];
            if (H.PERK > 34) return;

            var i = H.PERK += 1;
            if (i == 9) H.PERK_TXT = H.PERK_TXT.Replace("^1PRDT", "HELI");

            if (H.PERK_TXT.Length != 17) H.HUD_PERK_COUNT.SetText(H.PERK_TXT += "*");

            if (i > 2 && i % 3 == 0)
            {
                i = i / 3; if (i > 10) return;
                PK.Perk_Hud(killer, i);
                killer.Call(33466, "mp_killstreak_radar");
            }
            else if (i == 8)
            {
                H.CAN_USE_PREDATOR = true;

                if (CP.CARE_PACKAGE != null) killer.Call(33344, Info.GetStr("PRESS *[ [{+activate}] ] ^7AT THE CARE PACKAGE", H.AXIS));
                else killer.Call(33344, Info.GetStr("PRESS *[ [{+activate}] ] ^7TO CALL PREDATOR", H.AXIS));

                string txt = H.PERK_TXT;
                H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "^1" + txt);
            }
            else if (i == 11)
            {
                H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "^1HELI **********");

                H.CAN_USE_HELI = true;
                HCT.HeliAttachFlagTag(killer);
            }
        }

        void BotAddWatch()
        {
            BOT_ADD_WATCH_FINISHED = true;

            List<B_SET> bots_fire = new List<B_SET>();
            foreach (B_SET B in B_FIELD)
            {
                if (B == null) continue;
                bots_fire.Add(B);
            }

            ORIGINS = Players.Select(ent => ent.Origin).ToArray();
            FX_EXPLOSION = Call<int>(303, "explosions/aerial_explosion");//loadfx
            FX_FLARE_AMBIENT = Call<int>(303, "misc/flare_ambient");//"loadfx"
            FX_GREEN_LIGHT = Call<int>(303, "misc/aircraft_light_wingtip_green");//"loadfx"
            Entity rider = BOTs_List[SET.MAP_IDX % 2];
            SetRider(rider, B_FIELD[rider.EntRef]);

            OnInterval(2500, () =>
            {
                if (GAME_ENDED_) return false;
                if (HUMAN_ZERO_) return true;

                foreach (B_SET B in bots_fire) B.Search();

                HeliBotSearch();

                return true;
            });
        }
    }

}
