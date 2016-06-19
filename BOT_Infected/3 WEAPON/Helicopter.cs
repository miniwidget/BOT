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
        #region field
        Vector3 HELI_WAY_POINT, ZERO = new Vector3(0, 0, 0);
        Entity HELI, TL, TR, HELI_OWNER, HELI_GUNNER;

        bool HELI_ENABLED_, HELI_ON_USE_;

        string HELI_OWNER_NAME
        {
            get
            {
                if (HELI_OWNER == null) return "";
                return HELI_OWNER.Name;
            }
        }
        #endregion

        /// <summary>
        /// 10 킬 이상 하면, remote control enabled
        /// </summary>
        void AttachFlagTag(Entity player)
        {
            if (!HELI_ENABLED_)
            {
                player.Call("playsoundtoteam", "PC_1mc_kill_confirmed", "allies");
                foreach (Entity p in human_List)
                {
                    if (p != player) RM(player, 0, HELI_MESSAGE_ACTIVATE);
                    else showMessage(player, "^2PRESS [^7 [{+activate}] ^2] TO CALL HELI TURRET");
                }
            }

            player.AfterDelay(500, x =>//서버다운 fix
            {
                player.Call("AttachShieldModel", "prop_flag_neutral", "tag_shield_back", true);
            });
        }

        void ShowHelipoint()
        {

            /* mini map point */
            Call(431, 1, "active"); // objective_add
            Call(435, 1, HELI_WAY_POINT); // objective_position
            Call(434, 1, "compass_objpoint_ac130_friendly"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon
            HELI_ENABLED_ = false;

        }
        bool IsHeliArea(Entity player)
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

        void CallHeli(Entity player)
        {
            var w = player.CurrentWeapon;
            player.TakeWeapon(w);
            player.GiveWeapon("killstreak_helicopter_mp");
            player.SwitchToWeapon("killstreak_helicopter_mp");
            player.Call("playlocalsound", "mp_killstreak_radar");
            SetupHelicopter(player);
            player.AfterDelay(t2, x =>
            {
                giveWeaponTo(player, w);
            });
        }
        void StartHeli(Entity player)
        {
            HELI_ON_USE_ = true;
            HELI_OWNER = player;
            if (HELI_GUNNER == player) HELI_GUNNER = null;

            RM(player, 0, HELI_MESSAGE_KEY_INFO);

            player.Call(33256, HELI);//remotecontrolvehicle  
            player.Call("thermalvisionfofoverlayon");

            player.AfterDelay(120000, x =>
            {
                if (player != null && HELI_OWNER == player && HELI != null)
                {
                    H_FIELD[player.EntRef].USE_HELI = 0;
                    EndUseHeli(player, true);
                }
            });

        }
        void SetupHelicopter(Entity player)
        {
            if (HELI != null) return;

            string realModel = "vehicle_little_bird_armed";
            string minimap_model = "attack_littlebird_mp";

            HELI = Call<Entity>(369, player, HELI_WAY_POINT, ZERO, minimap_model, realModel);
            HELI.Call(32923);
            HELI.Call(32924);


            TL = Call<Entity>(19, "misc_turret", HELI.Origin, "sentry_minigun_mp", false);
            TL.Call("setmodel", "weapon_minigun");
            TL.Call(32841, HELI, "tag_minigun_attach_left", new Vector3(30f, 30f, 0), new Vector3(0, 0, 0));
            TL.Call("SetLeftArc", 180f);
            TL.Call("SetRightArc", 180f);
            TL.Call("SetBottomArc", 180f);

            TR = Call<Entity>(19, "misc_turret", HELI.Origin, "sentry_minigun_mp");
            TR.Call("setmodel", "weapon_minigun");
            TR.Call(32841, HELI, "tag_minigun_attach_right", new Vector3(30f, -30f, 0), new Vector3(0, 0, 0));
            TR.Call("SetLeftArc", 180f);
            TR.Call("SetRightArc", 180f);
            TR.Call("SetBottomArc", 180f);

            Call(431, 1, "inactive"); // objective_add
            HELI_ENABLED_ = true;
        }

        void EndGunner()
        {
            if (HELI_GUNNER == null) return;
            HELI_GUNNER.SetField("angles", ZERO);
            HELI_GUNNER.Call(32843);//unlink
            HELI_GUNNER = null;
        }
        void EndUseHeli(Entity player, bool unlink)
        {
            if (unlink)
            {
                player.Call(32843);//unlink
                player.Call(33257);//remotecontrolvehicleoff
                player.Call("thermalvisionfofoverlayoff");

                player.AfterDelay(500, x =>//서버다운 fix
                {
                    player.Call("detachShieldModel", "prop_flag_neutral", "tag_shield_back", true);
                });
            }
            HELI_ON_USE_ = false;
            player.SetField("angles", ZERO);
            HELI_OWNER = null;
            if (HELI_GUNNER != null) EndGunner();
            TL.Call("delete");
            TR.Call("delete");
            HELI.Call("delete");
            HELI = null;
            ShowHelipoint();
        }

        void testset()
        {
            H_SET H = H_FIELD[ADMIN.EntRef];
            H.USE_HELI = 1;
            H.PERK = 11;
            AttachFlagTag(ADMIN);
            ADMIN.Call("setorigin", HELI_WAY_POINT);
        }
    }
}
