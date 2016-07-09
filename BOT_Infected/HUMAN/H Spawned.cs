﻿using System;
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
        bool UNLIMITED_LIEF_ = true;

        /// <summary>
        /// change initial human infected to Allies
        /// </summary>
        /// <param name="player"></param>
        void CheckInf(Entity player)
        {
            BOT_TO_AXIS_COMP = true;
            player.Notify("menuresponse", "team_marinesopfor", "allies");
            player.Call("suicide");
        }

        void human_spawned(Entity player)
        {
            if (GAME_ENDED_) return;

            if (!BOT_TO_AXIS_COMP) CheckInf(player);

            int pe = player.EntRef;
            H_SET H = H_FIELD[pe];

            if (H.REMOTE_STATE != 0)
            {
                if (H.REMOTE_STATE == 1) HCT.IfUsetHeli_DoEnd(player, false);
                else if (H.REMOTE_STATE == 2) TK.IfUseTank_DoEnd(player);

                H.REMOTE_STATE = 0;
            }

            #region Allies
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
                    WP.GiveRandomWeaponTo(player);
                    WP.GiveRandomOffhandWeapon(player);

                    if (!human_List.Contains(player)) human_List.Add(player);

                    SetZero_hset(H, false, --H.LIFE);

                    if (HUMAN_DIED_ALL_) HUMAN_DIED_ALL_ = false;
                }
            }
            #endregion

            #region Axis
            else if (LIFE == -1)//change to AXIS
            {
                if (!SET.TEST_ && UNLIMITED_LIEF_)
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

                SetZero_hset(H, true, 0);

                player.SetField("sessionteam", "axis");
                human_List.Remove(player);
                player.Call(33341);//suicde
                player.Notify("menuresponse", "changeclass", "axis_recipe4");
                Print(player.Name + " : Infected ⊙..⊙");

                if (human_List.Count == 0)
                {
                    HUMAN_DIED_ALL_ = true;
                    if (!BOT_SERCH_ON_LUCKY_FINISHED) BotSerchOn_lucky(BOTs_List[BOT_LUCKY_IDX]);
                }

                Utilities.RawSayTo(player, Info.GetStr("*[ ^7DISABLED *] Melee of the Infected", true));
                HUD.AxisHud(player);
                player.Call(32771, SET.SOUND_ALERTS[rnd.Next(SET.SOUND_ALERTS.Length)], "allies");//playsoundtoteam
            }
            else
            {
                player.SetPerk("specialty_scavenger", true, false);
                player.SetPerk("specialty_longersprint", true, false);
                player.SetPerk("specialty_lightweight", true, false);

                H.AX_WEP += 1;

                string deadManWeapon = null;
                deadManWeapon = AxisWeapon(player, H.AX_WEP);

                H.CAN_USE_HELI = true;

                if (H.AX_WEP > 3)
                {
                    player.Call(33344, Info.GetStr("*[ ^7" + deadManWeapon + " *] Weapon of the Infected", true));
                    return;
                }

                if (HCT.HELI == null)
                {
                    Info.MessageRoop(player, 0, new[] { "*[ ^7" + deadManWeapon + " *] Weapon of the Infected", "*PRESS[^7 [{+activate}] *] TO CALL HELI TURRET" });
                }
                else if (!HCT.HELI_ON_USE_)
                {
                    Info.MessageRoop(player, 0, new[] { "*[ ^7" + deadManWeapon + " *] Weapon of the Infected", "*PRESS [^7 [{+activate}] *] AT THE HELI TURRET AREA", "YOU CAN RIDE IN HELICOPTER" });
                }
            }
            #endregion

        }

        /// <summary>
        /// 감염자가 계속 죽을 경우 총기를 주는 어드밴티지를 줌.
        /// </summary>
        /// <param name="player"></param>
        string AxisWeapon(Entity dead, int aw)
        {
            dead.TakeWeapon(dead.CurrentWeapon);
            dead.Health = 70;
            string deadManWeapon;
            int bullet = 0;
            if (aw == 3)
            {
                deadManWeapon = "m320_mp";
                bullet = 1;
            }
            else if (aw == 4)
            {
                deadManWeapon = "xm25_mp";
                bullet = 1;
            }
            else if (aw == 5)
            {
                deadManWeapon = "iw5_mp412_mp";
                bullet = 1;
            }
            else if (aw == 6)
            {
                deadManWeapon = "iw5_44magnum_mp";
                bullet = 2;
            }
            else if (aw == 7)
            {
                deadManWeapon = "iw5_msr_mp_msrscopevz_xmags";
                bullet = 1;
            }
            else if (aw == 8)
            {
                deadManWeapon = "iw5_mp412_mp";
                bullet = 2;
            }
            else if (aw == 9)
            {
                deadManWeapon = "iw5_as50_mp_as50scopevz_xmags";
                bullet = 1;
            }
            else if (aw == 10)
            {
                deadManWeapon = "iw5_44magnum_mp";
                bullet = 2;
            }
            else if (aw == 11)
            {
                deadManWeapon = "iw5_l96a1_mp_l96a1scopevz_xmags";
                bullet = 1;
            }
            else if (aw == 12)
            {
                deadManWeapon = "m320_mp";
                bullet = 1;
            }
            else if (aw == 13)
            {

                deadManWeapon = "iw5_ak47_mp_gp25";//10
                bullet = 6;
            }
            else if (aw == 14)
            {
                deadManWeapon = "iw5_pecheneg_mp_grip";//10
                bullet = 6;
            }
            else if (aw == 15)
            {
                deadManWeapon = "iw5_44magnum_mp";
                bullet = 2;
            }
            else
            {
                deadManWeapon = "iw5_ak47_mp_gp25";//10
                bullet = 6;
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
