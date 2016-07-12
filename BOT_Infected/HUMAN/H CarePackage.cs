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
        Entity CARE_PACKAGE;

        void CarePackage(Entity player)
        {
            Info.MessageRoop(player, 0, new[] { "THROW MARKER TO GET *RIDE PREDATOR", "PRESS *[ [{+activate}] ] ^7AT THE CARE PACKAGE" });

            string marker = "airdrop_sentry_marker_mp";
            player.GiveWeapon(marker);
            player.SwitchToWeaponImmediate(marker);

            bool finished = false;
            player.OnNotify("grenade_fire", (Entity owner, Parameter mk, Parameter weaponName) =>
            {
                if (finished) return;
                if (weaponName.ToString() != "airdrop_sentry_marker_mp") return;

                Entity Marker = mk.As<Entity>();
                if (Marker == null) return;

                if(PRDT==null) PRDT = new Predator();

                player.AfterDelay(3000, p =>
                {
                    finished = true;
                    Vector3 MO = VectorAddZ( Marker.Origin, 8);
                    Marker.Call(32928);//delete

                    Entity brushmodel = Call<Entity>("getent", "pf1_auto1", "targetname");

                    if (brushmodel == null) brushmodel = Call<Entity>("getent", "pf3_auto1", "targetname");
                    if (brushmodel != null)
                    {

                        CARE_PACKAGE = Call<Entity>("spawn", "script_model", MO);
                        CARE_PACKAGE.Call("setmodel", "com_plasticcase_friendly");//com_plasticcase_friendly
                        CARE_PACKAGE.Call(33353, brushmodel);

                        Call(431, 20, "active"); // objective_add
                        Call(435, 20, MO); // objective_position
                        Call(434, 20, "compass_objpoint_ammo_friendly"); //objective_icon compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

                        brushmodel = Call<Entity>("spawn", "script_model", VectorAddZ(MO, 25));
                        brushmodel.Call("setmodel", "projectile_cbu97_clusterbomb");
                        return;
                    }

                    for (int i = 18; i < 1024; i++)
                    {
                        brushmodel = Entity.GetEntity(i);
                        if (brushmodel == null) continue;
                        if (brushmodel.GetField<string>("classname") == "script_brushmodel")
                        {

                            string targetName = brushmodel.GetField<string>("targetname");

                            if (targetName == null) continue;
                            Print(targetName + " entref " + i);//map : 5 exchange // taxi_ad_clip entref ( 425 )
                            CARE_PACKAGE = Call<Entity>("spawn", "script_model", MO);
                            CARE_PACKAGE.Call("setmodel", "com_plasticcase_friendly");//com_plasticcase_friendly
                            CARE_PACKAGE.Call(33353, brushmodel);

                            Call(431, 20, "active"); // objective_add
                            Call(435, 20, MO); // objective_position
                            Call(434, 20, "compass_waypoint_bomb"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

                            brushmodel = Call<Entity>("spawn", "script_model",VectorAddZ( MO,25));
                            brushmodel.Call("setmodel", "projectile_cbu97_clusterbomb");
                            break;
                        }
                    }

                  
                });
            });
        }
        void CarePackageDo(Entity player, H_SET H)
        {
            string weapon = player.CurrentWeapon;
            player.Call(33523, weapon);//givemaxammo
            player.Call(33468, weapon, 100);//setweaponammoclip

            player.Call(33466, "ammo_crate_use");//playLocalSound
            if (H.AXIS) return;

            if (USE_PREDATOR)
            {
                player.Call(33344, "PREDATOR IS ALREADY IN THE AIR. WAIT");
                return;
            }
            if (H.PERK < 8)
            {
                player.Call(33344, 8 - H.PERK + " KILL MORE to RIDE PREDATOR");
                return;
            }
            if (H.USE_PREDATOR)
            {
                player.Call(33344, "PREDATOR FINISHED");
                return;
            }

            PRDT.PredatorStart(player, H);
        }
    }
}
