using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tdm
{
    class Helicopter : Inf
    {
        internal readonly string[] MESSAGE_ALERT = { "YOU ARE NOT IN THE HELI AREA", "GO TO HELI AREA AND", "PRESS  *[  [{+activate}]  ]  ^7AT THE HELI AREA" };
        internal readonly string[] MESSAGE_WAIT_PLAYER = { "YOU CAN RIDE HELLI", "IF ANOTHER PLAYER ONBOARD" };
        readonly string[] MESSAGE_KEY_INFO = { "HELICOPTER CONTROL INFO", "PRESS  *[  [{+breath_sprint}]  ] ^7TO MOVE DOWN", "PRESS  *[  [{+gostand}]  ]  ^7TO MOVE UP" };
        internal readonly string[] MESSAGE_ACTIVATE = { "PRESS  *[  [{+activate}]  ]  ^7AT THE HELI TURRET AREA", "YOU CAN RIDE IN HELICOPTER" };
        internal readonly string MESSAGE_CALL = "PRESS  *[  [{+activate}]  ]  ^7TO CALL HELI TURRET";
        internal Entity HELI, TL, TR, HELI_OWNER, HELI_GUNNER;
        Vector3 HELI_WAY_POINT;
        internal bool HELI_WAY_ENABLED;
        internal void SetHeliPort(Vector3 HWP)
        {
            HELI_WAY_ENABLED = true;
            HELI_WAY_POINT = HWP;
            Call(431, 17, "active"); // objective_add
            Call(435, 17, HWP); // objective_position
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
                player.Call(33344, Info.GetStr(MESSAGE_CALL, Tdm.H_FIELD[player.EntRef].AXIS));
            }
            else
            {
                Info.MessageRoop(player, 0, MESSAGE_ACTIVATE);
            }

            player.Call(32792, "prop_flag_neutral", "tag_shield_back", true);//attachshieldmodel
        }
        #endregion

        #region Call heli

        internal void HeliCall(Entity player, bool Axis)
        {
            if (HELI != null) return;
            Tdm.H_FIELD[player.EntRef].CAN_USE_HELI = true;

            var w = player.CurrentWeapon;
            player.TakeWeapon(w);
            string kh = "killstreak_helicopter_mp";
            player.GiveWeapon(kh);
            player.SwitchToWeaponImmediate(kh);
            HeliSetup(player);
            player.AfterDelay(500, p =>
            {
                Tdm.PlayDialog(player, Axis, 14);

                player.AfterDelay(500, x =>
                {
                    player.TakeWeapon(kh);
                    player.GiveWeapon(w);
                    player.Call(33523, w); //givemaxammo
                    player.SwitchToWeaponImmediate(w);
                });
            });

            Info.MessageRoop(player, 0, MESSAGE_ACTIVATE);

            AlertToTeam(Axis, player.EntRef, "HELICOPTER ENABLED. GO TO THE AREA", Info.GetStr("*[ ^7" + player.Name + " *] CALLED HELICOPTER. WATCH OUT", Axis), -1, false);
        }
        internal void HeliSetup(Entity player)
        {
            if (HELI != null) return;

            string minimap_model = "attack_littlebird_mp";
            string realModel = "vehicle_little_bird_armed";

            string printModel = "pavelow_minigun_mp";// littlebird_guard_minigun_mp sentry_minigun_mp apache_minigun_mp
            string realModel_turret = "weapon_minigun";//turret_minigun_mp weapon_minigun
            if (Set.TURRET_MAP) printModel = "turret_minigun_mp";

            Vector3 point = Tdm.VectorAddZ(HELI_WAY_POINT, 1000);

            Entity heli = Call<Entity>("spawn", "script_model", point);
            heli.Call(32929, "vehicle_little_bird_armed");//setmodel
            Vector3 angle = Common.GetVector(0, player.Call<Vector3>(33532).Y, 0);//getplayerangles
            heli.SetField("angles", angle);
            heli.Call(33402, -860, 5, 0, 2);//movez
            angle.Y -= 180;
            heli.Call(33406, angle, 4);//rotateto

            player.AfterDelay(5000, x =>
            {
                point = Tdm.VectorAddZ(HELI_WAY_POINT, 140);

                HELI = Call<Entity>(369, player, point, angle, minimap_model, realModel);//spawnHelicopter

                TL = Call<Entity>(19, "misc_turret", point, printModel, false);
                TL.Call(32929, realModel_turret);//setmodel
                TL.Call(32841, HELI, "tag_minigun_attach_left", Common.GetVector(30f, 30f, 0), Common.ZERO);
                TL.Call(33084, 180f);//SetLeftArc
                TL.Call(33083, 180f);//SetRightArc
                TL.Call(33086, 180f);//SetBottomArc

                TR = Call<Entity>(19, "misc_turret", point, printModel);
                TR.Call(32929, realModel_turret);//setmodel
                TR.Call(32841, HELI, "tag_minigun_attach_right", Common.GetVector(30f, -30f, 0), Common.ZERO);
                TR.Call(33084, 180f);
                TR.Call(33083, 180f);
                TR.Call(33086, 180f);

                heli.Call(32928);//delete
            });
        }

        #endregion

        void AlertToTeam(bool Axis, int ownerEntref, string messageToAllies, string messageToAxis, int soundIdx, bool soundToOtherTeam)
        {
            for (int i = 0; i < Tdm.human_List.Count; i++)
            {
                Entity human = Tdm.human_List[i];
                int he = human.EntRef;
                if (he > 17) continue;
                if (he== ownerEntref)
                {
                    if (soundIdx == -1) continue;
                    Tdm.PlayDialogToTeam(human, Axis, soundIdx, soundToOtherTeam);
                    continue;
                }
                if (Tdm.IsAxis[he] == Axis) Utilities.RawSayTo(human, messageToAllies);
                else Utilities.RawSayTo(human, messageToAxis);
            }
        }

        #region board heli
        internal bool HELI_ON_USE_;
        byte helicount;
        internal State HeliStart(Entity player, bool Axis)
        {
            Common.StartOrEndThermal(player, true);
            if (HELI_ON_USE_)
            {
                HELI_GUNNER = player;
                return State.remote_not_using;
            }

            helicount++;
            int hc = helicount;

            Tdm.H_FIELD[player.EntRef].CAN_USE_HELI = false;

            HELI_ON_USE_ = true;
            HELI_OWNER = player;

            if (HELI_GUNNER == player) HELI_GUNNER = null;

            int time = 80000;

            AlertToTeam(Axis, player.EntRef, Info.GetStr("*FRIENDLY HELICOPTER INBOUND", Axis), Info.GetStr("*ENEMEY HELICOPTER INBOUND", Axis), 12,true);

            Info.MessageRoop(player, 0, MESSAGE_KEY_INFO);

            player.Call(33256, HELI);//remotecontrolvehicle  
            player.AfterDelay(time, x =>
            {
                if (hc != helicount) return;

                if (player != null && HELI_OWNER == player && HELI != null)
                {
                    HeliEndUse(player, true);
                }
            });
            return State.remote_helicopter;
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
