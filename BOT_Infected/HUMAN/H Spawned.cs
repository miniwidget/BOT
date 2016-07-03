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
        bool IsFirstInfectdHuman()
        {
            IS_FIRST_INFECTD_HUMAN_FINISHED = true;

            if (BOTs_List.Count == 0) return false;

            foreach (Entity bot in BOTs_List)
            {
                if (bot.GetField<string>("sessionteam") == "axis") return false;//감염된 봇이 있는 경우
            }
            Human_FIRST_INFECTED_ = true;
            return true;
        }

        /// <summary>
        /// 사람이 스폰한 경우
        /// </summary>
        /// <param name="player"></param>
        void human_spawned(Entity player)
        {
            if (GAME_ENDED_) return;

            int pe = player.EntRef;
            H_SET H = H_FIELD[pe];

            if (!IS_FIRST_INFECTD_HUMAN_FINISHED) if (IsFirstInfectdHuman()) H.LIFE = -1;

            if (H.REMOTE_STATE != 0)
            {
                if (H.REMOTE_STATE == 1) HCT.IfUsetHeli_DoEnd(player, false);
                else if (H.REMOTE_STATE == 2) TK.IfUseTank_DoEnd(player);
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
                    player.Call(33344, "^2[ ^7" + (LIFE + 1) + " LIFE ^2] MORE");
                }
                else
                {
                    WP.GiveRandomWeaponTo(player);
                    WP.GiveRandomOffhandWeapon(player);

                    if (!human_List.Contains(player)) human_List.Add(player);

                    SetZero_hset(H, false, --H.LIFE);

                    if (HUMAN_DIED_ALL) HUMAN_DIED_ALL = false;
                }
            }
            #endregion
            #region Axis
            else if (LIFE == -1)//change to AXIS
            {
                if (!SET.TEST_&& !Human_FIRST_INFECTED_)
                {
                        if (DateTime.Now < GRACE_TIME)
                        {
                            H.LIFE = 0;
                            H.RESPAWN = true;
                            player.Notify("menuresponse", "team_marinesopfor", "allies");
                            player.Call(33344, "^2[ ^7UNLIMETED LIFE ^2] UNTIL 9 MINUTE");
                            return;
                        }
                        else Human_FIRST_INFECTED_ = true;
                }


                SetZero_hset(H, true, 0);

                player.SetField("sessionteam", "axis");
                human_List.Remove(player);
                player.Call(33341);//suicde
                player.Notify("menuresponse", "changeclass", "axis_recipe4");
                Print(player.Name + " : Infected ⊙..⊙");

                if (human_List.Count == 0)
                {
                    HUMAN_DIED_ALL = true;
                    if (!BOT_SERCH_ON_LUCKY_FINISHED) BotSerchOn_lucky(BOTs_List[BOT_LUCKY_IDX]);
                }
            }
            else
            {
                var aw = H.AX_WEP;
                player.SetPerk("specialty_scavenger", true, false);
                player.SetPerk("specialty_longersprint", true, false);
                player.SetPerk("specialty_lightweight", true, false);

                if (aw == 1)
                {
                    player.Call(33344, "^2[ ^7DISABLED ^2] Melee of the Infected");
                    HUD.AxisHud(player);
                    AxisWeapon_by_init(player);

                    AfterDelay(1000, () => player.Call(32771, SET.SOUND_ALERTS[rnd.Next(SET.SOUND_ALERTS.Length)], "allies"));//playsoundtoteam
                }
                else
                {
                    string deadManWeapon = null;
                    deadManWeapon = AxisWeapon_by_Attack(player, H.AX_WEP += 1);

                    if (HCT.HELI == null)
                    {
                        H.USE_HELI = 1;
                        if (H.ON_MESSAGE) return;
                        Info.MessageRoop(player, 0, new[] { "^2[ ^7" + deadManWeapon + " ^2] Weapon of the Infected", "^1PRESS[^7 [{+activate}] ^1] TO CALL HELI TURRET" });
                    }
                    else if (!HCT.HELI_ON_USE_)
                    {
                        H.USE_HELI = 2;
                        if (H.ON_MESSAGE) return;
                        Info.MessageRoop(player, 0, new[] { "^2[ ^7" + deadManWeapon + " ^2] Weapon of the Infected", "^1PRESS [^7 [{+activate}] ^1] AT THE HELI TURRET AREA", "YOU CAN RIDE IN HELICOPTER" });
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 죽은 사람 무기 초기화
        /// </summary>
        /// <param name="dead"></param>
        string AxisWeapon_by_init(Entity dead)
        {
            string DEAD_GUN = "iw5_deserteagle_mp_tactical";

            H_FIELD[dead.EntRef].AX_WEP = 2;
            dead.TakeWeapon(dead.CurrentWeapon);
            dead.GiveWeapon(DEAD_GUN);
            dead.AfterDelay(100, d =>
            {
                dead.SwitchToWeaponImmediate(DEAD_GUN);
                dead.Call(33468, DEAD_GUN, 3);//SetWeaponAmmoClip
                dead.Call(33469, DEAD_GUN, 0);//SetWeaponAmmoStock
            });

            dead.AfterDelay(2000, d => dead.Notify("open_"));
            return DEAD_GUN;
        }

        /// <summary>
        /// 감염자가 계속 죽을 경우 총기를 주는 어드밴티지를 줌.
        /// </summary>
        /// <param name="player"></param>
        string AxisWeapon_by_Attack(Entity dead, int aw)
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
                AxisWeapon_by_init(dead);
                return null;
            }

            dead.GiveWeapon(deadManWeapon);
            dead.AfterDelay(100, x =>
            {
                dead.SwitchToWeaponImmediate(deadManWeapon);

                dead.Call(33469, deadManWeapon, 0);
                dead.Call(33468, deadManWeapon, bullet);
            });

            dead.Notify("open_");
            return deadManWeapon;
        }
    }
}
