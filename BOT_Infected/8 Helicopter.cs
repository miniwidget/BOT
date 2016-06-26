﻿using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class Helicopter
    {
        internal readonly string[] HELI_MESSAGE_ALERT = { "YOU ARE NOT IN THE HELI AREA", "GO TO HELI AREA AND", "PRESS ^2[ [{+activate}] ] ^7AT THE HELI AREA" };
        internal readonly string[] HELI_MESSAGE_WAIT_PLAYER = { "YOU CAN RIDE HELLI", "IF ANOTHER PLAYER ONBOARD" };
        readonly string[] HELI_MESSAGE_KEY_INFO = { "HELI INFO", "^2[ [{+breath_sprint}] ] ^7MOVE DOWN", "^2[ [{+gostand}] ] ^7MOVE UP" };
        readonly string[] HELI_MESSAGE_ACTIVATE = { "PRESS ^2[ [{+activate}] ] ^7AT THE HELI TURRET AREA", "YOU CAN RIDE IN HELICOPTER" };

        internal Entity HELI, TL, TR, HELI_OWNER, HELI_GUNNER;
        internal static Vector3 HELI_WAY_POINT;

        internal void SetHeliPort()
        {
            Function.SetEntRef(-1);
            Function.Call(431, 17, "active"); // objective_add
            Function.SetEntRef(-1);
            Function.Call(435, 17, HELI_WAY_POINT); // objective_position
            Function.SetEntRef(-1);
            Function.Call(434, 17, "compass_objpoint_ac130_friendly"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon
        }
        #region wait heli
        /// <summary>
        /// 10 킬 이상 하면, remote control enabled
        /// </summary>
        internal void HeliAttachFlagTag(Entity player)
        {
            //player.Call(32771, "PC_1mc_take_positions", "allies");//playsoundtoteam

            if (HELI == null)
            {
                player.Call(33344, "PRESS ^2[ [{+activate}] ] ^7TO CALL HELI TURRET");
            }
            else
            {
                Info.MessageRoop(player, 0, HELI_MESSAGE_ACTIVATE);
            }

            player.Call(32792, "prop_flag_neutral", "tag_shield_back", true);//attachshieldmodel

        }
        #endregion

        #region Call heli
        internal bool IsHeliArea(Entity player)
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
        internal bool IsUsingTurret(Entity player)
        {
            return (player.Call<int>(33539) == 1);
        }
        internal void HeliCall(Entity player)
        {
            var w = player.CurrentWeapon;
            player.TakeWeapon(w);
            string kh = "killstreak_helicopter_mp";
            player.GiveWeapon(kh);
            player.SwitchToWeapon(kh);
            player.Call(33466, "mp_killstreak_radar");//playlocalsound
            HeliSetup(player);
            player.AfterDelay(2000, x =>
            {
                player.TakeWeapon(kh);
                player.GiveWeapon(w);
                player.Call(33523, w); //givemaxammo
                player.SwitchToWeaponImmediate(w);
            });

            Info.MessageRoop(player, 0, HELI_MESSAGE_ACTIVATE);
            Utilities.RawSayAll("HELICOPTER ENABLED. GO TO THE AREA");
        }
        internal void HeliSetup(Entity player)
        {

            string realModel = "vehicle_little_bird_armed";
            string minimap_model = "attack_littlebird_mp";

            string turret_mp = "sentry_minigun_mp";
            string reamModel_turret = "weapon_minigun";

            Function.SetEntRef(-1);
            HELI = Function.Call<Entity>(369, player, HELI_WAY_POINT, Infected.ZERO, minimap_model, realModel);
            HELI.Call(32923);
            HELI.Call(32924);

            Function.SetEntRef(-1);
            TL = Function.Call<Entity>(19, "misc_turret", HELI.Origin, turret_mp, false);
            TL.Call(32929, reamModel_turret);//setmodel
            TL.Call(32841, HELI, "tag_minigun_attach_left", new Vector3(30f, 30f, 0), new Vector3(0, 0, 0));
            TL.Call(33084, 180f);//SetLeftArc
            TL.Call(33083, 180f);//SetRightArc
            TL.Call(33086, 180f);//SetBottomArc

            Function.SetEntRef(-1);
            TR = Function.Call<Entity>(19, "misc_turret", HELI.Origin, turret_mp);
            TR.Call(32929, reamModel_turret);
            TR.Call(32841, HELI, "tag_minigun_attach_right", new Vector3(30f, -30f, 0), new Vector3(0, 0, 0));
            TR.Call(33084, 180f);
            TR.Call(33083, 180f);
            TR.Call(33086, 180f);
        }

        #endregion

        #region board heli
        internal bool HELI_ON_USE_;
        internal void HeliStart(Entity player)
        {
            HELI_ON_USE_ = true;
            HELI_OWNER = player;
            if (HELI_GUNNER == player) HELI_GUNNER = null;

            Info.MessageRoop(player, 0, HELI_MESSAGE_KEY_INFO);

            player.Call(33256, HELI);//remotecontrolvehicle  
            player.Call(32936);//thermalvisionfofoverlayon

            player.AfterDelay(120000, x =>
            {
                if (player != null && HELI_OWNER == player && HELI != null)
                {
                    Infected.H_FIELD[player.EntRef].USE_HELI = 0;
                    HeliEndUse(player, true);
                }
            });

        }
        #endregion

        internal void HeliEndGunner()
        {
            if (HELI_GUNNER == null) return;
            HELI_GUNNER.Call(33531, Infected.ZERO);
            HELI_GUNNER.Call(32843);//unlink
            HELI_GUNNER = null;
        }
        internal void HeliEndUse(Entity player, bool unlink)
        {
            if (unlink && Infected.human_List.Contains(player))
            {
                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff
                player.Call(32937);//thermalvisionfofoverlayoff

                player.Call(32805, "prop_flag_neutral", "tag_shield_back", true);//detachShieldModel
            }
            HELI_ON_USE_ = false;
            player.Call(33531, Infected.ZERO);

            HELI_OWNER = null;
            if (HELI_GUNNER != null) HeliEndGunner();
            TL.Call(32928);//delete
            TR.Call(32928);
            HELI.Call(32928);
            HELI = null;
        }
        internal void IfHeliOwner_DoEnd(Entity player)
        {
            if (HELI_OWNER == player) HeliEndUse(player, false);
            else if (HELI_GUNNER == player) HeliEndGunner();
        }
    }
}