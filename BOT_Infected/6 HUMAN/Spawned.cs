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
        #region isFirstInfected
        bool HUMAN_FIRST_SPAWNED = true;
        bool isFirstInfected()
        {
                HUMAN_FIRST_SPAWNED = false;

                if (BOTs_List.Count == 0) return false;

                foreach (Entity bot in BOTs_List)
                {
                    if (!isSurvivor(bot)) return false;//감염된 봇이 있는 경우
                }
                //감염된 봇이 없는 경우
                return true;
        }
        #endregion

        #region human_spawned

        void playSoundTeam(ref Entity player)
        {
            player.Call("playsoundtoteam", MT.soundAlert[rnd.Next(5)], "allies");
        }
        bool START_LAST_BOT_SEARCH;
        void human_spawned(ref Entity player)//LIFE 1 or 2
        {
            Field H = FL[player.EntRef];

            if (HUMAN_FIRST_SPAWNED) if (isFirstInfected()) H.LIFE = -1;

            if (HELI_OWNER == player) HeliEndUse(player, false);
            else if (HELI_GUNNER == player) HeliEndGunner();

            var LIFE = H.LIFE;
            if (LIFE > -1)//3 2
            {
                if (!H.RESPAWN)
                {
                    H.RESPAWN = true;
                    player.Notify("menuresponse", "team_marinesopfor", "allies");
                    setTeamName();
                }
                else
                {
                    H.LIFE -= 1;
                    H.PERK = 2;
                    H.RESPAWN = false;
                    H.USE_HELI = 0;
                    H.USE_TANK = false;

                    player.Call(33466, "mp_last_stand");// playlocalsound
                    player.Call(33344, "^2[ ^7" + (LIFE + 1) + " LIFE ^2] MORE");//iPrintlnBold

                    WP.giveWeaponToInit(ref player, WP.getRandomWeapon());
                    
                }
            }
            else if (LIFE == -1)//change to AXIS
            {
                H.LIFE = -2;
                H.PERK = 50;
                H.AX_WEP = 1;
                H.USE_HELI = 4;
                H.USE_TANK = false;
                H.AXIS = true;

                player.SetField("sessionteam", "axis");
                human_List.Remove(player);
                player.Call(33341);//suicide
                player.Notify("menuresponse", "changeclass", "axis_recipe4");
                print(player.Name + " : Infected ⊙..⊙");
            }
            else
            {
                var aw = H.AX_WEP;
                if (aw == 1)
                {
                    if (!HUMAN_AXIS_LIST.Contains(player)) HUMAN_AXIS_LIST.Add(player);

                    if (!START_LAST_BOT_SEARCH)
                    {
                        if (human_List.Count == 0) StartAllyBotSearch();
                    }

                    player.Call(33344, "^2[ DISABLED ] ^7Melee of the Infected");

                    HudAxis(ref player);
                    AxisWeapon_by_init(ref player);
                    playSoundTeam(ref player);
                }
                else
                {

                    if (!H.BY_SUICIDE)//by attack
                    {
                        H.AX_WEP += 1;
                        AxisWeapon_by_Attack(ref player, H.AX_WEP);
                    }
                    else
                    {
                        AxisWeapon_by_init(ref player);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 죽은 사람 무기 초기화
        /// </summary>
        /// <param name="dead"></param>
        void AxisWeapon_by_init(ref Entity dead)
        {
            string DEAD_GUN = "iw5_deserteagle_mp_tactical";

            FL[dead.EntRef].AX_WEP = 2;
            dead.TakeWeapon(dead.CurrentWeapon);
            dead.GiveWeapon(DEAD_GUN);
            dead.Call("SetWeaponAmmoClip", DEAD_GUN, 3);
            dead.Call("SetWeaponAmmoStock", DEAD_GUN, 0);
            dead.Notify("open_");
            dead.AfterDelay(100, x => x.SwitchToWeaponImmediate(DEAD_GUN));
        }

        /// <summary>
        /// 감염자가 계속 죽을 경우 총기를 주는 어드밴티지를 줌.
        /// </summary>
        /// <param name="player"></param>
        void AxisWeapon_by_Attack(ref Entity dead, int aw)
        {
            dead.TakeWeapon(dead.CurrentWeapon);
            dead.Health = 70;

            string deadManWeapon = "";
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
                AxisWeapon_by_init(ref dead);
                dead.Call("iPrintlnBold", "^2[ ^7AGAIN ^2] Init Weapon of the Infected");
                return;
            }

            dead.GiveWeapon(deadManWeapon);
            dead.Call("SetWeaponAmmoStock", deadManWeapon, 0);
            dead.Call("SetWeaponAmmoClip", deadManWeapon, bullet);

            dead.Call("iPrintlnBold", "^2[ ^7" + deadManWeapon + " ^2] Weapon of the Infected");
            dead.Notify("open_");
            dead.AfterDelay(100,x=>x.SwitchToWeaponImmediate(deadManWeapon));
        }
    }
}
