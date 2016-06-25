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
        bool _temp = true;
        bool IsINF()
        {
            _temp = false;

            if (BOTs_List.Count == 0) return false;

            foreach (Entity bot in BOTs_List)
            {
                if (!IsSurvivor(bot)) return false;//감염된 봇이 있는 경우
            }
            return true;

        }
        readonly string[] SOUND_ALERTS =
        {
            "AF_1mc_losing_fight", "AF_1mc_lead_lost", "PC_1mc_losing_fight", "PC_1mc_take_positions", "PC_1mc_positions_lock" , "PC_1mc_enemy_take_a" , "PC_1mc_enemy_take_b", "PC_1mc_enemy_take_c"
        };

        #region human_spawned

        void human_spawned(Entity player)//LIFE 1 or 2
        {
            if (GAME_ENDED_) return;

            int pe = player.EntRef;
            H_SET H = H_FIELD[pe];

            if (_temp) if (IsINF()) H.LIFE = -1;

            if (HCT.HELI != null)
            {
                if (HCT.HELI_OWNER == player) HCT.HeliEndUse(player, false);
                else if (HCT.HELI_GUNNER == player) HCT.HeliEndGunner();
            }

            var LIFE = H.LIFE;
            if (LIFE > -1)//3 2
            {
                if (!H.RESPAWN)
                {
                    H.LIFE -= 1;
                    H.RESPAWN = true;
                    player.Call(33466, "mp_last_stand");// playlocalsound
                    player.Notify("menuresponse", "team_marinesopfor", "allies");
                    SetTeamName();
                }
                else
                {
                    H.RESPAWN = false;
                    H.USE_HELI = 0;

                    player.Call(33344, "^2[ ^7" + (LIFE + 1) + " LIFE ^2] MORE");
                    WP.GiveRandomWeaponTo(player);
                    WP.GiveRandomOffhandWeapon(player);

                    if (!human_List.Contains(player)) human_List.Add(player);

                    TK.IfTankOwnerEnd(player);
                }
            }
            else if (LIFE == -1)//change to AXIS
            {
                SetAxis(pe);

                player.SetField("sessionteam", "axis");
                human_List.Remove(player);
                player.Call(33341);
                player.Notify("menuresponse", "changeclass", "axis_recipe4");
                TK.IfTankOwnerEnd(player);
                my.print(player.Name + " : Infected ⊙..⊙");
            }
            else
            {
                var aw = H.AX_WEP;
                if (aw == 1)
                {
                    player.Call(33344, "^2[ ^7DISABLED ^2] Melee of the Infected");
                    HUD.AxisHud(player);
                    AxisWeapon_by_init(player);

                    AfterDelay(1000, () => player.Call(32771, SOUND_ALERTS[rnd.Next(SOUND_ALERTS.Length)], "allies"));//playsoundtoteam
                }
                else
                {

                    if (!H.BY_SUICIDE)//by attack
                    {
                        H.AX_WEP += 1;
                        AxisWeapon_by_Attack(player, H.AX_WEP);
                    }
                    else
                    {
                        AxisWeapon_by_init(player);
                    }
                }
            }
        }
        #endregion

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
                dead.Call("iPrintlnBold", "^2[ ^7AGAIN ^2] Init Weapon of the Infected");
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
