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

        /// <summary>
        /// change initial human infected to Allies
        /// </summary>
        void HumanCheckInf(Entity player)
        {
            if (player == null)
            {
                if (F_INF_IDX == -1) return;
                if (F_INF_IDX >= human_List.Count) return;

                player = human_List[F_INF_IDX];
                if (player == null) return;
                player.Notify("menu response", "team_marinesopfor", "allies");
                player.Call("suicide");
                return;
            }
            player.AfterDelay(200, x =>
            {
                player.Health = 9999;
                player.Notify("menuresponse", "team_marinesopfor", "back");
                x.AfterDelay(100, xx =>
                {
                    player.Notify("menuresponse", "class", "back");
                    player.Call(33436, "black_bw", 0.5f);
                    HudElem START = HudElem.CreateFontString(player, "objective", 2f);
                    START.SetPoint("CENTER", "CENTER", 0, 50);
                    START.Foreground = true;
                    START.HideWhenInMenu = false;
                    START.Alpha = 0.8f;
                    START.SetField("glowcolor", new Vector3(0.8f, 1f, 0.8f));
                    START.GlowAlpha = 0.2f;
                    START.Call("setpulsefx", 100, 4000, 2000);
                    START.SetText("WAIT...TO CHANGE STATE TO ALIVE");
                });
            });
            F_INF_IDX = human_List.IndexOf(player);
            BOT_TO_AXIS_COMP = true;

            Players.FirstOrDefault(ent => ent.Name.StartsWith("bot") && ent.GetField<string>("sessionteam") == "allies").Call("suicide");

        }

        void HumanAlliesSpawned(Entity player, string name, H_SET H)
        {
            if (!SET.DEPLAY_BOT_) return;
            if (!BOT_TO_AXIS_COMP) HumanCheckInf(player);
            
            var LIFE = H.LIFE;
            if (LIFE > -1)
            {
                if (!H.RESPAWN)
                {
                    H.RESPAWN = true;
                    player.Call(33466, "mp_last_stand");// playlocalsound
                    player.Notify("menuresponse", "team_marinesopfor", "allies");
                    
                    player.Call(33344, Info.GetStr("*[ ^7" + (LIFE + 1) + " LIFE *] MORE", false));
                }
                else
                {
                    player.AfterDelay(250,x=>GUN.GiveRandomWeaponTo(player));

                    if (!Allies_List.Contains(player)) Allies_List.Add(player);

                    SetZero_hset(H, false, --H.LIFE, name, false);

                    if (HUMAN_DIED_ALL_) HUMAN_DIED_ALL_ = false;

                    player.SetPerk("specialty_scavenger", true, false);
                }
            }
            else if (LIFE == -1)
            {
                if (!TEST_ && UNLIMITED_LIEF_)
                {
                    if (DateTime.Now < GRACE_TIME)
                    {
                        H.LIFE = 0;
                        H.RESPAWN = true;
                        player.Notify("menuresponse", "team_marinesopfor", "allies");
                        player.Call(33344, Info.GetStr("*[ ^7UNLIMETED LIFE *] UNTIL 9 MINUTE", false));
                        return;
                    }
                    else UNLIMITED_LIEF_ = false;
                }
                HumanAlliesToAxis(player, name, H);
            }

        }
        void HumanAlliesToAxis(Entity player, string name, H_SET H)
        {
            SetZero_hset(H, true, 0, name, false);

            Allies_List.Remove(player);
            player.Call(33341);//suicde
            player.Notify("menuresponse", "changeclass", "axis_recipe4");
            Print(name + " : Infected ⊙..⊙");

            if (Allies_List.Count == 0) HUMAN_DIED_ALL_ = true;

            Utilities.RawSayTo(player, Info.GetStr("*[ ^7DISABLED *] Melee of the Infected", true));
            HUD.AxisHud(player);
            player.Call(32771, SET.SOUND_ALERTS[rnd.Next(SET.SOUND_ALERTS.Length)], "allies");//playsoundtoteam

            if (!BOT_SERCH_ON_LUCKY_FINISHED) BotSerchOn_lucky();
        }
        void HumanAxisSpawned(Entity player, string name, H_SET H)
        {
            if (!H.CAN_USE_HELI) H.CAN_USE_HELI = true;
            if (!Axis_List.Contains(player)) Axis_List.Add(player);

            player.SetPerk("specialty_scavenger", true, false);
            player.SetPerk("specialty_longersprint", true, false);
            player.SetPerk("specialty_lightweight", true, false);

            H.AX_WEP += 1;

            string deadManWeapon = HumanAxisWeapon(player, H.AX_WEP);

            H.CAN_USE_HELI = true;

            if (H.AX_WEP > 3)
            {
                player.Call(33344, Info.GetStr("*[ ^7" + deadManWeapon + " *]  Weapon of the Infected", true));
                return;
            }

            if (HCT.HELI == null)
            {
                Info.MessageRoop(player, 0, new[] { "*[ ^7" + deadManWeapon + " *]  Weapon of the Infected", "*PRESS  [  ^7[{+activate}]  *]  TO CALL HELI TURRET" });
            }
            else if (!HCT.HELI_ON_USE_)
            {
                Info.MessageRoop(player, 0, new[] { "*[ ^7" + deadManWeapon + " *]  Weapon of the Infected", "*PRESS  [  ^7[{+activate}]  *]  AT THE HELI TURRET AREA"});
            }
        }
        string HumanAxisWeapon(Entity dead, int aw)
        {
            dead.TakeWeapon(dead.CurrentWeapon);
            dead.Health = 70;
            string deadManWeapon;
            int bullet = 0;
            bullet = 2;

            if (aw <= 3) deadManWeapon = "m320_mp";
            else if (aw == 4) deadManWeapon = "xm25_mp";
            else if (aw == 5) deadManWeapon = "iw5_mp412_mp";
            else if (aw == 6) deadManWeapon = "iw5_44magnum_mp";
            else if (aw == 7) deadManWeapon = "iw5_msr_mp_msrscopevz_xmags";
            else if (aw == 8) deadManWeapon = "iw5_mp412_mp";
            else if (aw == 9) deadManWeapon = "iw5_as50_mp_as50scopevz_xmags";
            else if (aw == 10) deadManWeapon = "iw5_44magnum_mp";
            else if (aw == 11) deadManWeapon = "iw5_l96a1_mp_l96a1scopevz_xmags";
            else if (aw == 12) deadManWeapon = "m320_mp";
            else if (aw == 13)
            {
                deadManWeapon = "iw5_ak47_mp_gp25"; bullet = 6;
            }
            else if (aw == 14)
            {
                deadManWeapon = "iw5_pecheneg_mp_grip"; bullet = 6;
            }
            else if (aw == 15) deadManWeapon = "iw5_44magnum_mp";

            else
            {
                deadManWeapon = "iw5_ak47_mp_gp25"; bullet = 6;
            }

            dead.GiveWeapon(deadManWeapon);
            dead.AfterDelay(100, x =>
            {
                dead.SwitchToWeaponImmediate(deadManWeapon);

                dead.Call(33469, deadManWeapon, 0);
                dead.Call(33468, deadManWeapon, bullet);
            });

            return deadManWeapon;
        }
    }
}
