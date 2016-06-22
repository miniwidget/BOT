using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    public partial class Infected
    {
        void testset()
        {
            Field H = FL[ADMIN.EntRef];
            H.USE_HELI = 1;
            H.PERK = 11;
            HeliAttachFlagTag(ref ADMIN);
            ADMIN.Call("setorigin", HELI_WAY_POINT);
        }

        #region wait heli
        /// <summary>
        /// 10 킬 이상 하면, remote control enabled
        /// </summary>
        void HeliAttachFlagTag(ref Entity player)
        {
            //player.Call(32771, "PC_1mc_take_positions", "allies");//playsoundtoteam

            if (HELI == null)
            {
                showMessage(player, "PRESS ^2[ [{+activate}] ] ^7TO CALL HELI TURRET");
            }
            else
            {
                RM(player, 0, MT.HELI_MESSAGE_ACTIVATE);
            }


            player.Call(32792, "prop_flag_neutral", "tag_shield_back", true);//attachshieldmodel

        }
        #endregion

        #region Call heli
        bool isHeliArea(ref Entity player)
        {
            float dist = player.Origin.DistanceTo(TL.Origin);
            if (dist > 140)
            {
                dist = player.Origin.DistanceTo(TR.Origin);

                if (dist > 140)
                {
                    return false;
                }
            }
            return true;
        }
        bool isUsingTurret(ref Entity player)
        {
            return (player.Call<int>(33539) == 1);
        }
        void HeliCall(Entity player)
        {
            var w = player.CurrentWeapon;
            player.TakeWeapon(w);
            string kh = "killstreak_helicopter_mp";
            player.GiveWeapon(kh);
            player.SwitchToWeapon(kh);
            player.Call(33466, "mp_killstreak_radar");//playlocalsound
            HeliSetup(ref player);
            player.AfterDelay(t2, x =>
            {
                player.TakeWeapon(kh);
                player.GiveWeapon(w);
                player.Call(33523, w); //givemaxammo
                player.SwitchToWeaponImmediate(w);
            });

            RM(player, 0, MT.HELI_MESSAGE_ACTIVATE);
            Utilities.RawSayAll("HELICOPTER ENABLED. GO TO THE AREA");
        }
        void HeliSetup(ref Entity player)
        {
            string realModel = "vehicle_little_bird_armed";
            string minimap_model = "attack_littlebird_mp";

            string turret_mp = "sentry_minigun_mp";
            string reamModel_turret = "weapon_minigun";
            HELI = Call<Entity>(369, player, HELI_WAY_POINT, ZERO, minimap_model, realModel);
            HELI.Call(32923);
            HELI.Call(32924);


            TL = Call<Entity>(19, "misc_turret", HELI.Origin, turret_mp, false);
            TL.Call(32929, reamModel_turret);//setmodel
            TL.Call(32841, HELI, "tag_minigun_attach_left", new Vector3(30f, 30f, 0), new Vector3(0, 0, 0));
            TL.Call(33084, 180f);//SetLeftArc
            TL.Call(33083, 180f);//SetRightArc
            TL.Call(33086, 180f);//SetBottomArc

            TR = Call<Entity>(19, "misc_turret", HELI.Origin, turret_mp);
            TR.Call(32929, reamModel_turret);
            TR.Call(32841, HELI, "tag_minigun_attach_right", new Vector3(30f, -30f, 0), new Vector3(0, 0, 0));
            TR.Call(33084, 180f);
            TR.Call(33083, 180f);
            TR.Call(33086, 180f);

            //Call(431, 1, "inactive"); // objective_add
        }

        #endregion
        
        #region board heli
        void HeliStart(Entity player)
        {
            HELI_ON_USE_ = true;
            HELI_OWNER = player;
            if (HELI_GUNNER == player) HELI_GUNNER = null;

            RM(player, 0, MT.HELI_MESSAGE_KEY_INFO);

            player.Call(33256, HELI);//remotecontrolvehicle  
            player.Call(32936);//thermalvisionfofoverlayon

            player.AfterDelay(120000, x =>
            {
                if (player != null && HELI_OWNER == player && HELI != null)
                {
                    FL[player.EntRef].USE_HELI = 0;
                    HeliEndUse(player, true);
                }
            });

        }

        //void
        #endregion

        #region end heli
        void HeliEndGunner()
        {
            if (HELI_GUNNER == null) return;
            HELI_GUNNER.Call(33531, ZERO);//unlink
            HELI_GUNNER.Call(32843);//unlink
            HELI_GUNNER = null;
        }
        void HeliEndUse(Entity player, bool unlink)
        {

            if (unlink && human_List.Contains(player))
            {
                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff
                player.Call(32937);//thermalvisionfofoverlayoff

                player.Call(32805, "prop_flag_neutral", "tag_shield_back", true);//detachShieldModel
            }
            HELI_ON_USE_ = false;
            player.Call(33531, ZERO);

            HELI_OWNER = null;
            if (HELI_GUNNER != null) HeliEndGunner();
            TL.Call(32928);//delete
            TR.Call(32928);
            HELI.Call(32928);
            HELI = null;
            //ShowHelipoint();
        }

        #endregion
    }
}
