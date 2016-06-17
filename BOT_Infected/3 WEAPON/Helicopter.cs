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

        bool HELI_ON_USE_, HELI_END_USE_,HELI_SETUP_FINISH_;

        string HELI_OWNER_NAME
        {
            get
            {
                return HELI_OWNER.Name;
            }
        }
        #endregion
 
        /// <summary>
        /// 10 킬 이상 하면, remote control enabled
        /// </summary>
        void ShowFlagTag(Entity player)
        {
            if (!HELI_ON_USE_)
            {
                player.Call("playsoundtoteam", "PC_1mc_kill_confirmed", "allies");
                foreach (Entity p in human_List)
                {
                    roopMessage(player, 0, HELI_MESSAGE_ACTIVATE);
                }
            }
            H_FIELD[player.EntRef].USE_HELI = true;
            AfterDelay(500, () =>//서버다운 fix
            {
                player.Call("AttachShieldModel", "prop_flag_neutral", "tag_shield_back", true);
            });
        }

        void SetupHelicopter(Entity player)
        {
            if (HELI_SETUP_FINISH_) return; HELI_SETUP_FINISH_ = true;
            if (player == null) return;

            HELI_ON_USE_ = false;
            HELI_END_USE_ = false;

            string realModel = "vehicle_little_bird_armed";
            string minimap_model = "attack_littlebird_mp";

            HELI = Call<Entity>(369, player, HELI_WAY_POINT, ZERO, minimap_model, realModel);

            /* mini map point */
            Call(431, 1, "active"); // objective_add
            Call(435, 1, HELI_WAY_POINT); // objective_position
            Call(434, 1, "compass_objpoint_ac130_friendly"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

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

        }

        void EndGunner()
        {
            if (HELI_GUNNER == null) return;
            HELI_GUNNER.SetField("angles", ZERO);
            HELI_GUNNER = null;
            HELI_GUNNER.Call(32843);//unlink
        }
        void EndUseHeli()
        {
            if (HELI_END_USE_ || HELI_OWNER == null) return;
            if (HELI_OWNER.EntRef == -1) return;
            HELI_END_USE_ = true;

            HELI_OWNER.Call(32843);//unlink
            HELI_OWNER.Call(33257);//remotecontrolvehicleoff
            H_FIELD[HELI_OWNER.EntRef].USE_HELI = false;
            HELI_OWNER.SetField("angles", ZERO);
            HELI_OWNER.Call("thermalvisionfofoverlayoff");
            HELI_OWNER = null;
            if (HELI_GUNNER != null) EndGunner();
            TL.Call("delete");
            TR.Call("delete");
            HELI.Call("delete");
            HELI = null;

            AfterDelay(10000, () =>
            {
                if (GAME_ENDED_) return;
              
                if (human_List.Count > 0)
                {
                    HELI_SETUP_FINISH_ = false;
                    SetupHelicopter(human_List[0]);
                }
            });
        }

        void testset()
        {
            H_SET H = H_FIELD[ADMIN.EntRef];
            H.USE_HELI = true;
            H.PERK = 10;
            ShowFlagTag(ADMIN);
            ADMIN.Call("setorigin", HELI_WAY_POINT);
        }
    }
}
