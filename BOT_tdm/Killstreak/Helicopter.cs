﻿using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tdm
{
    class Helicopter : Inf
    {
        internal readonly string[] MESSAGE_ALERT = { "YOU ARE NOT IN THE HELI AREA", "GO TO HELI AREA AND", "PRESS ^2[ [{+activate}] ] ^7AT THE HELI AREA" };
        internal readonly string[] MESSAGE_WAIT_PLAYER = { "YOU CAN RIDE HELLI", "IF ANOTHER PLAYER ONBOARD" };
        readonly string[] MESSAGE_KEY_INFO = { "^2HELICOPTER CONTROL INFO", "^2MOVE DOWN [^7 [{+breath_sprint}] ^2]", "^2MOVE UP [^7 [{+gostand}] ^2]", "^2PRESS [ ^7[{+smoke}] ^2] IF STUCK" };
        internal readonly string[] MESSAGE_ACTIVATE = { "^2PRESS [^7 [{+activate}] ^2] AT THE HELI TURRET AREA", "YOU CAN RIDE IN HELICOPTER" };
        internal readonly string MESSAGE_CALL = "^2PRESS [^7 [{+activate}] ^2] TO CALL HELI TURRET";
        internal Entity HELI, TL, TR, HELI_OWNER, HELI_GUNNER;
        internal static Vector3 HELI_WAY_POINT;
        internal void SetHeliPort()
        {
            Call(431, 17, "active"); // objective_add
            Call(435, 17, HELI_WAY_POINT); // objective_position
            Call(434, 17, "compass_objpoint_ac130_friendly"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon
        }
        #region wait heli
        /// <summary>
        /// 10 킬 이상 하면, remote control enabled
        /// </summary>
        internal void HeliAttachFlagTag(Entity player)
        {

            if (HELI == null)
            {
                player.Call(33344, MESSAGE_CALL);
            }
            else
            {
                Info.MessageRoop(player, 0, MESSAGE_ACTIVATE);
            }

            player.Call(32792, "prop_flag_neutral", "tag_shield_back", true);//attachshieldmodel
        }
        #endregion

        #region Call heli

        //internal bool IsHeliArea(Entity player)
        //{
        //    if (HELI == null) return false;

        //    Vector3 handpos = player.Call<Vector3>(33128, "tag_weapon_left");

        //    if (handpos.DistanceTo2D(TL.Origin) < 9) return true;
        //    if (handpos.DistanceTo2D(TR.Origin) < 9 ) return true;

        //    return false; 
        //}
        //internal bool IsUsingTurret(Entity player)
        //{
        //    return (player.Call<int>(33539) == 1);
        //}

        internal void HeliCall(Entity player)
        {
            Tdm.H_FIELD[player.EntRef].CAN_USE_HELI = true;

            var w = player.CurrentWeapon;
            player.TakeWeapon(w);
            string kh = "killstreak_helicopter_mp";
            player.GiveWeapon(kh);
            player.SwitchToWeaponImmediate(kh);
            HeliSetup(player);
            player.AfterDelay(500, p =>
            {
                player.Call(33466, "US_1mc_KS_lbd_inposition");//playlocalsound
                player.AfterDelay(500, x =>
                {
                    player.TakeWeapon(kh);
                    player.GiveWeapon(w);
                    player.Call(33523, w); //givemaxammo
                    player.SwitchToWeaponImmediate(w);
                });
            });

            Info.MessageRoop(player, 0, MESSAGE_ACTIVATE);

            Utilities.RawSayAll("HELICOPTER ENABLED. GO TO THE AREA");
        }
        internal void HeliSetup(Entity player)
        {
            if (HELI != null) return;

            string realModel = "vehicle_little_bird_armed";
            string minimap_model = "attack_littlebird_mp";

            string printModel = "pavelow_minigun_mp";// littlebird_guard_minigun_mp sentry_minigun_mp apache_minigun_mp
            string realModel_turret = "weapon_minigun";//turret_minigun_mp weapon_minigun
            //if (turret_map) printModel = "turret_minigun_mp";

            HELI = Call<Entity>(369, player, HELI_WAY_POINT, Common.ZERO, minimap_model, realModel);//spawnHelicopter
            HELI.Call(33417, true);//setcandamage

            TL = Call<Entity>(19, "misc_turret", HELI.Origin, printModel, false);
            TL.Call(32929, realModel_turret);//setmodel
            TL.Call(32841, HELI, "tag_minigun_attach_left", Common.GetVector(30f, 30f, 0), Common.ZERO);
            TL.Call(33084, 180f);//SetLeftArc
            TL.Call(33083, 180f);//SetRightArc
            TL.Call(33086, 180f);//SetBottomArc

            TR = Call<Entity>(19, "misc_turret", HELI.Origin, printModel);
            TR.Call(32929, realModel_turret);//setmodel
            TR.Call(32841, HELI, "tag_minigun_attach_right", Common.GetVector(30f, -30f, 0), Common.ZERO);
            TR.Call(33084, 180f);
            TR.Call(33083, 180f);
            TR.Call(33086, 180f);
        }

        #endregion

        #region board heli
        internal bool HELI_ON_USE_;
        byte helicount;
        internal byte HeliStart(Entity player)
        {
            helicount++;
            int hc = helicount;
            Common.StartOrEndThermal(player, true);
            if (HELI_ON_USE_)
            {
                HELI_GUNNER = player;
                return 0;
            }
            Tdm.H_FIELD[player.EntRef].CAN_USE_HELI = false;

            HELI_ON_USE_ = true;
            HELI_OWNER = player;

            if (HELI_GUNNER == player) HELI_GUNNER = null;
            Utilities.RawSayAll( "^1[ ^7"+player.Name + " ^1] HELICOPTER INBOUND");

            if (player.GetField<string>("sessionteam")=="axis") player.Call(32771, "PC_1mc_enemy_ah6guard", "allies");//playsoundtoteam
            else player.Call(32771, "PC_1mc_enemy_ah6guard", "axis");//playsoundtoteam

            Info.MessageRoop(player, 0, MESSAGE_KEY_INFO);

            player.Call(33256, HELI);//remotecontrolvehicle  
            player.AfterDelay(120000, x =>
            {
                if (hc != helicount) return;

                if (player != null && HELI_OWNER == player && HELI != null)
                {
                    HeliEndUse(player, true);
                }
            });
            return 1;
        }
        #endregion

        #region end heli
        internal void HeliEndGunner()
        {
            if (HELI_GUNNER == null) return;
            HELI_GUNNER.Call(33531, Common.ZERO);
            HELI_GUNNER.Call(32843);//unlink
            HELI_GUNNER.Call(32937);
            Common.StartOrEndThermal(HELI_GUNNER, false);
            HELI_GUNNER = null;
        }
        internal void HeliEndUse(Entity player, bool unlink)
        {
            Tdm.H_FIELD[player.EntRef].CAN_USE_HELI = false;

            if (unlink && Tdm.human_List.Contains(player))
            {
                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff

                player.Call(32805, "prop_flag_neutral", "tag_shield_back", true);//detachShieldModel
            }
            HELI_ON_USE_ = false;
            player.Call(33531, Common.ZERO);

            HELI_OWNER = null;
            if (HELI_GUNNER != null) HeliEndGunner();
            Common.StartOrEndThermal(player, false);

            TL.Call(32928);//delete
            TR.Call(32928);
            HELI.Call(32928);
            HELI = null;
        }
        internal bool IfUsetHeli_DoEnd(Entity player, bool unlink)
        {
            if (HELI_OWNER == player)
            {
                HeliEndUse(player, unlink);
                return true;
            }
            else if (HELI_GUNNER == player)
            {
                HeliEndGunner();
                return true;
            }
            return false;
        }
        #endregion
    }
}