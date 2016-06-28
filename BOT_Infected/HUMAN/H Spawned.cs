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
        bool temp_ = true, Human_FIRST_INFECTED_;
        bool IsFirstInfectdHuman()
        {
            temp_ = false;

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
        void human_spawned(Entity player)//LIFE 1 or 2
        {
            if (GAME_ENDED_) return;

            int pe = player.EntRef;
            H_SET H = H_FIELD[pe];

            if (temp_) if (IsFirstInfectdHuman()) H.LIFE = -1;

            var LIFE = H.LIFE;
            if (LIFE > -1)//3 2
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
                    H.RESPAWN = false;
                    H.USE_HELI = 0;


                    WP.GiveRandomWeaponTo(player);
                    WP.GiveRandomOffhandWeapon(player);

                    if (!human_List.Contains(player)) human_List.Add(player);

                    Set_hset(H, false, false);

                    SetTeamName();

                    if (HCT.HELI_ON_USE_) HCT.IfHeliOwner_DoEnd(player);
                    if (TK.RMT1_OWNER != -1 || TK.RMT2_OWNER != -1) TK.IfTankOwner_DoEnd(player);
                }
            }
            else if (LIFE == -1)//change to AXIS
            {
                //if (!Human_FIRST_INFECTED_)
                //{
                //    int now = DateTime.Now.Minute;
                //    int elapsed_time = now - SET.TIME;
                //    if (elapsed_time < 2)
                //    {
                //        H.LIFE = 0;
                //        H.RESPAWN = true;
                //        player.Notify("menuresponse", "team_marinesopfor", "allies");
                //        player.Call(33344, "^2[ ^7UNLIMETED LIFE ^2] UNTIL 10 MINUTE");
                //        return;
                //    }
                //    else Human_FIRST_INFECTED_ = true;
                //}
                Set_hset(H, true, false);

                player.SetField("sessionteam", "axis");
                human_List.Remove(player);
                player.Call(33341);
                player.Notify("menuresponse", "changeclass", "axis_recipe4");
                Print(player.Name + " : Infected ⊙..⊙");

                if (HCT.HELI_ON_USE_) HCT.IfHeliOwner_DoEnd(player);
                if (TK.RMT1_OWNER != -1 || TK.RMT2_OWNER != -1) TK.IfTankOwner_DoEnd(player);
            }
            else
            {
                var aw = H.AX_WEP;

                if (HCT.HELI == null)
                {
                    H.USE_HELI = 1;
                    player.Call(33344, HCT.MESSAGE_CALL);
                }
                else if (!HCT.HELI_ON_USE_)
                {
                    Info.MessageRoop(player, 0, HCT.MESSAGE_ACTIVATE);
                    H.USE_HELI = 2;
                }
                else if(aw==1) player.Call(33344, "^2[ ^7DISABLED ^2] Melee of the Infected");

                player.SetPerk("specialty_longersprint", true, false);
                player.SetPerk("specialty_lightweight", true, false);

                if (aw == 1)
                {
                    HUD.AxisHud(player);
                    AxisWeapon_by_init(player);

                    AfterDelay(1000, () => player.Call(32771, SET.SOUND_ALERTS[rnd.Next(SET.SOUND_ALERTS.Length)], "allies"));//playsoundtoteam
                }
                else
                {

                    if (!H.BY_SUICIDE)//by attack
                    {
                        AxisWeapon_by_Attack(player, H.AX_WEP += 1);
                    }
                    else
                    {
                        AxisWeapon_by_init(player);
                    }
                }
            }
        }

        /// <summary>
        /// 죽은 사람 무기 초기화
        /// </summary>
        /// <param name="dead"></param>
        void AxisWeapon_by_init(Entity dead)
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
        }

        /// <summary>
        /// 감염자가 계속 죽을 경우 총기를 주는 어드밴티지를 줌.
        /// </summary>
        /// <param name="player"></param>
        void AxisWeapon_by_Attack(Entity dead, int aw)
        {
            dead.TakeWeapon(dead.CurrentWeapon);
            dead.Health = 70;

            string deadManWeapon;
            int bullet = 0;
            if (aw < 3)
            {
                deadManWeapon = "m320_mp";
                bullet = 1;
            }
            else if (aw == 3)
            {
                deadManWeapon = "xm25_mp";
                bullet = 1;
            }
            else if (aw == 4)
            {
                deadManWeapon = "iw5_mp412_mp";
                bullet = 1;
            }
            else if (aw < 7)
            {
                deadManWeapon = "iw5_44magnum_mp";
                bullet = 1;
            }
            else if (aw == 7)
            {
                deadManWeapon = "iw5_msr_mp_msrscopevz_xmags";
                bullet = 1;
            }
            else if (aw == 8)
            {
                deadManWeapon = "iw5_mp412_mp";
                bullet = 1;
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
                dead.Call(33344, "^2[ ^7AGAIN ^2] Init Weapon of the Infected");//"iPrintlnBold"
                return;
            }

            dead.GiveWeapon(deadManWeapon);
            dead.AfterDelay(100, x =>
            {
                dead.SwitchToWeaponImmediate(deadManWeapon);

                dead.Call(33469, deadManWeapon, 0);
                dead.Call(33468, deadManWeapon, bullet);
            });

            dead.Call(33344, "^2[ ^7" + deadManWeapon + " ^2] Weapon of the Infected");
            dead.Notify("open_");
        }
    }
}
