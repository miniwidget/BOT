using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {
        class CarePackage : Inf
        {
            internal Entity CARE_PACKAGE;
            internal Vector3 CARE_PACKAGE_ORIGIN;
            internal void Marker(Entity player,H_SET H,byte type)
            {
                string message = null;

                if (type == 1)
                
                    message = Info.GetStr("*THROW MARKER and GET [ ^7RIDE PREDATOR *]", H.AXIS);
                else
                    message = Info.GetStr("*THROW MARKER and GET [ ^7HELICOPTER *]", H.AXIS);
                
                
                H.MARKER_TYPE = type;

                player.Call(33344, message);
                player.Call(33466, "ammo_crate_use");//playlocalsound

                string marker = "airdrop_sentry_marker_mp";
                player.GiveWeapon(marker);
                player.SwitchToWeaponImmediate(marker);

                if (H.MARKER_NOTIFIED) return; H.MARKER_NOTIFIED = true;

                player.OnNotify("grenade_fire", (Entity owner, Parameter mk, Parameter weaponName) =>
                {
                    if (H.MARKER_TYPE == 0) return;

                    if (weaponName.ToString() != "airdrop_sentry_marker_mp") return;

                    Entity Marker = mk.As<Entity>();
                    if (Marker == null) return;

                    if (H.MARKER_TYPE == 1) PredatorMarker(player, Marker,H.AXIS);
                    else HelicopterMarker(player, Marker,H.AXIS);

                    H.MARKER_TYPE = 0;
                });
            }
            internal void HelicopterMarker(Entity player,Entity Marker, bool Axis)
            {
                player.Call(33466, "PC_1mc_use_ah6guard");

                player.AfterDelay(3000, p =>
                {
                    player.Call(33344, Info.GetStr("*PRESS [  ^7[{+activate}]  *] AT THE HELI TURRET", Axis));
                    HCT.SetHeliPort(Marker.Origin);
                    HCT.HeliCall(player, Axis);
                    Marker.Call(32928);//delete
                });
            }
            internal void PredatorMarker(Entity player, Entity Marker,bool Axis)
            {
                if (PRDT == null) PRDT = new Predator();
                player.Call(33466, "PC_1mc_use_hellfire");

                player.AfterDelay(3000, p =>
                {
                    player.Call(33344, Info.GetStr("*PRESS [  ^7[{+activate}]  *] AT THE CARE PACKAGE", Axis));

                    CARE_PACKAGE_ORIGIN = VectorAddZ(Marker.Origin, 8);
                    Marker.Call(32928);//delete

                    Entity brushmodel = Call<Entity>("getent", "pf1_auto1", "targetname");

                    if (brushmodel == null) brushmodel = Call<Entity>("getent", "pf3_auto1", "targetname");
                    if (brushmodel != null)
                    {
                        CarePackageSpawn(brushmodel);
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

                            CarePackageSpawn(brushmodel);

                            break;
                        }
                    }
                });
            }
            void CarePackageSpawn(Entity brushmodel)
            {

                CARE_PACKAGE = Call<Entity>("spawn", "script_model", CARE_PACKAGE_ORIGIN);
                CARE_PACKAGE.Call("setmodel", "com_plasticcase_friendly");//com_plasticcase_friendly
                if (brushmodel != null) CARE_PACKAGE.Call(33353, brushmodel);

                Call(431, 20, "active"); // objective_add
                Call(435, 20, CARE_PACKAGE_ORIGIN); // objective_position
                Call(434, 20, "compass_objpoint_ammo_friendly"); //objective_icon compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

                brushmodel = Call<Entity>("spawn", "script_model", VectorAddZ(CARE_PACKAGE_ORIGIN, 25));
                brushmodel.Call("setmodel", "projectile_cbu97_clusterbomb");
            }
            internal void CarePackageDo(Entity player, H_SET H)
            {
                string weapon = player.CurrentWeapon;
                player.Call(33523, weapon);//givemaxammo
                player.Call(33468, weapon, 100);//setweaponammoclip

                player.Call(33466, "ammo_crate_use");//playLocalSound

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
                if (!H.CAN_USE_PREDATOR)
                {
                    player.Call(33344, "PREDATOR FINISHED");
                    return;
                }
                PRDT.PredatorStart(player, H, BOT_HELI_HEIGHT);
            }
        }
    }
}
