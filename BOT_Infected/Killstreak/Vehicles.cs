using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    public partial class Infected
    {
        class Vehicle : Infinity
        {
            internal Entity VEHICLE;
            string VEHICLE_NAME;

            internal void VehicleAddTank(Entity player, H_SET H)
            {
                Entity tank = VehicleSearch("vehicle_ugv_talon_mp", true, false);
                if (tank == null) return;

                H.REMOTE_STATE = State.remote_assaultDrone;
                player.Call(32936);//thermalvisionfofoverlayon

                Allies_List.Add(tank);
                Allies_List.Remove(player);
                tank.OnNotify("death", e =>
                {
                    Allies_List.Remove(tank);
                    if (!H.AXIS) Allies_List.Add(player);
                    if (H.REMOTE_STATE == State.remote_assaultDrone)
                    {
                        H.REMOTE_STATE = State.remote_not_using;
                        player.Call(32937);//thermalvisionfofoverlayoff
                    }
                });
            }

            string[] HELI_MODELS = { "vehicle_cobra_helicopter_fly_low", "vehicle_mi24p_hind_mp", "vehicle_pavelow","vehicle_pavelow_opfor" };

            internal void Vehicles(Entity player, string weapon)
            {
                if (weapon == "killstreak_helicopter_mp") VehicleStartRemote(player, VehicleSearch(HELI_MODELS), "tag_light_belly");

                else
                {
                    if (VEHICLE != null) return;

                    if (weapon == "mortar_remote_mp") VehicleAdd(player, "vehicle_predator_b", "REMOTE MORTAR");//ok
                    else if (weapon == "heli_remote_mp") VehicleAdd(player, "vehicle_v22_osprey_body_mp", "OSPREY");//ok
                }
            }

            void VehicleAdd(Entity player, string vehicleModel, string shortName)
            {
                VEHICLE = VehicleSearch(vehicleModel, false, true);
                if (VEHICLE == null) return;

                VEHICLE_NAME = shortName + "+" + player.EntRef;

                MessageToHuman("TYPE *[  ^7" + VEHICLE_CODE + "  *]  ^7TO RIDE " + shortName, player.EntRef);
            }
#if DEBUG
        void VehicleTest(Entity player)
        {
            Entity vehicle = Call<Entity>("spawnPlane", player, "script_model", VectorAddZ(player.Origin, 2000), "compass_objpoint_reaper_friendly", "compass_objpoint_reaper_enemy");
            vehicle.Call("setModel", "vehicle_predator_b");
            VEHICLE_NAME = "REMOTE MORTAR+"+11;
            VehicleStartRemote(player, vehicle, null);
        }
#endif
            internal void VehicleStartRemote(Entity player, Entity vehicle, string tag)
            {
                if (vehicle == null) return;

                H_SET H = H_FIELD[player.EntRef];

                if (H.REMOTE_STATE != State.remote_not_using) return;
                H.VEHICLE = vehicle;
                string notifyString = null;

                bool remoteTurret = true;

                if (tag == null)
                {
                    string[] vehs = VEHICLE_NAME.Split('+');
                    if (vehs[1] == player.EntRef.ToString()) return;//자신이 직접 리모트 컨트롤 한 차량의 경우, 차단

                    switch (vehs[0])
                    {
                        case "REMOTE MORTAR": tag = "tag_player"; notifyString = "remote_done"; remoteTurret = false; break;
                        case "OSPREY": tag = "tag_turret_attach"; notifyString = "leaving"; break;//leaving & helicopter_done & death & death
                        default: return;
                    }
                    VEHICLE_NAME = null;
                    VEHICLE_CODE = null;
                    VEHICLE = null;
                }
                else notifyString = "leaving";//choper

                int death = player.GetField<int>("deaths");

                if (remoteTurret)
                {
                    H.REMOTE_STATE = State.remote_vehicleTurret;
                    Entity turret = Call<Entity>(19, "misc_turret", vehicle.Call<Vector3>(33128, "tag_origin"), "pavelow_minigun_mp", false);//spawnturret
                    turret.Call(32929, "mp_remote_turret");//setmodel
                    turret.Call(32841, vehicle, tag, Common.GetVector(30f, 30f, -50f), vehicle.Call<Vector3>("getTagAngles", tag));//linkto
                    turret.Call(33084, 180f);
                    turret.Call(33083, 180f);
                    turret.Call(33086, 180f);
                    player.AfterDelay(1500, xx =>
                    {
                        if (H.REMOTE_STATE != State.remote_vehicleTurret) return;

                        player.TakeWeapon("killstreak_helicopter_mp");
                        player.GiveWeapon("killstreak_remote_turret_laptop_mp");
                        player.SwitchToWeapon("killstreak_remote_turret_laptop_mp");
                        Print(H.REMOTE_STATE + " " + 6);
                        player.AfterDelay(2500, x =>
                        {
                            if (H.REMOTE_STATE != State.remote_vehicleTurret) return;
                            Print(H.REMOTE_STATE + " " + 7);
                            Common.StartOrEndThermal(player, true);
                            player.GiveWeapon("killstreak_remote_turret_remote_mp");
                            player.SwitchToWeapon("killstreak_remote_turret_remote_mp");
                            player.Call("RemoteControlTurret", turret);//RemoteControlTurret

                            vehicle.OnNotify(notifyString, e =>//important. 
                            {
                                if (H.REMOTE_STATE == State.remote_vehicleTurret && death == player.GetField<int>("deaths"))
                                {
                                    player.Call(32980, turret);//RemoteControlTurretOff
                                    player.Call(32843);//unlink

                                    player.TakeWeapon("killstreak_remote_turret_remote_mp");
                                    player.TakeWeapon("killstreak_remote_turret_laptop_mp");

                                    player.Call(33504);//enableOffhandWeapons
                                    Common.StartOrEndThermal(player, false);

                                    GUN.GiveGunTo(player);

                                    H.REMOTE_STATE = State.remote_not_using;
                                }

                                turret.Call("delete");//delete
                            });
                        });
                    });


                 

                }
                else
                {
                    H.REMOTE_STATE = State.remote_mortar;
                    Common.StartOrEndThermal(player, true);

                    player.GiveWeapon("killstreak_ac130_mp");
                    player.SwitchToWeapon("killstreak_ac130_mp");
                    player.AfterDelay(2000, x =>
                    {
                        if (H.REMOTE_STATE != State.remote_mortar) return;

                        player.GiveWeapon("mortar_remote_mp");//zoom_
                        player.GiveWeapon("mortar_remote_zoom_mp");//zoom_
                        player.SwitchToWeapon("mortar_remote_mp");//zoom_

                        player.Call(32887, vehicle, tag, 1, 180, 180, 180, 180);//PlayerLinkWeaponviewToDelta
                        player.Call(33531, vehicle.Call<Vector3>("getTagAngles", "tag_player"));//setPlayerAngles

                        H.MISSILE_COUNT = 10;

                        Common.BulletHudInfoCreate(player, H, H.MISSILE_COUNT);

                        vehicle.OnNotify(notifyString, e =>
                        {
                            if (death == player.GetField<int>("deaths")) VehicleEndMissile(player, H);
                        });

                    });

                    if (H.VEHICLE_FIRE_NOTIFIED) return;
                    H.VEHICLE_FIRE_NOTIFIED = true;

                    player.Call(33445, "VEHICLE_MISSILE", "+frag");//notifyonplayercommand
                    player.OnNotify("VEHICLE_MISSILE", e =>
                    {
                        if (H.REMOTE_STATE != State.remote_mortar) return;
                        if (H.MISSILE_COUNT <= 0 || H.VEHICLE == null)
                        {
                            VehicleEndMissile(player, H);
                            return;
                        }
                        H.MISSILE_COUNT--;
                        VehicleFireMissile(player, H.VEHICLE.Origin);
                        H.HUD_BULLET_INFO.SetText(H.MISSILE_COUNT.ToString());
                    });

                    player.Call(33445, "RMBC", "+toggleads_throw");//notifyonplayercommand
                    player.OnNotify("RMBC", e =>
                    {
                        if (H.REMOTE_STATE != State.remote_mortar) return;

                        string cw = player.CurrentWeapon;
                        if (cw == "mortar_remote_mp") player.SwitchToWeapon("mortar_remote_zoom_mp");
                        else if (cw == "mortar_remote_zoom_mp") player.SwitchToWeapon("mortar_remote_mp");
                    });

                }
            }
            void VehicleFireMissile(Entity player, Vector3 sp)
            {
                Vector3 angle = player.Call<Vector3>(33532);//getPlayerAngles

                float[] GMP = Common.GetMissilePos(angle, sp);
                float x = GMP[0], y = GMP[1];
                Vector3 targetPos = Common.GetVector(sp.X + x, sp.Y + y, 0);
                Vector3 startPos = Common.GetVector(sp.X - x, sp.Y - y, sp.Z);
                Entity rocket = Call<Entity>(404, "remote_tank_projectile_mp", startPos, targetPos, player);//MagicBullet
            }
            void VehicleEndMissile(Entity player, H_SET H)
            {
                Common.BulletHudInfoDestroy(H);

                if (!player.CurrentWeapon.StartsWith("mortar_remote")) return;

                player.TakeWeapon("killstreak_ac130_mp");
                player.TakeWeapon("mortar_remote_mp");
                player.TakeWeapon("mortar_remote_zoom_mp");

                player.Call(32843);//unlink
                player.Call(33531, Common.ZERO);//setplayerangles

                player.Call(33504);//enableOffhandWeapons
                player.Call(33513, false);//freezeControls

                player.GiveWeapon(H.GUN);
                player.SwitchToWeapon(H.GUN);

                Common.StartOrEndThermal(player, false);
                if (H.REMOTE_STATE == State.remote_mortar) H.REMOTE_STATE = State.remote_not_using;

            }

            Entity VehicleSearch(string vehicleModel, bool addHumanList, bool endNotify)
            {
                for (int i = 18; i < 1024; i++)
                {
                    Entity vehicle = Entity.GetEntity(i);
                    if (vehicle == null) continue;
                    var model = vehicle.GetField<string>("model");
                    if (model == vehicleModel)
                    {
                        if (addHumanList)
                        {
                            if (vehicle == TK.REMOTETANK || human_List.Contains(vehicle)) continue;
                        }
                        else if (endNotify) AddVehicleEndNotify(vehicle, vehicleModel);
                        return vehicle;
                    }
                }
                return null;
            }
            Entity VehicleSearch(string[] vehicleModels)
            {
                for (int i = 18; i < 1024; i++)
                {
                    Entity vehicle = Entity.GetEntity(i);
                    if (vehicle == null) continue;
                    var model = vehicle.GetField<string>("model");
                    if (vehicleModels.Contains(model)) return vehicle;
                }
                return null;
            }
            void AddVehicleEndNotify(Entity vehicle, string vehicleModel)
            {
                string notifyString = null;

                if (vehicleModel == "vehicle_predator_b") notifyString = "remote_done";

                else if (vehicleModel == "vehicle_v22_osprey_body_mp") notifyString = "leaving";

                VEHICLE_CODE = rnd.Next(1000, 9999).ToString();

                string vhc = VEHICLE_CODE;

                vehicle.OnNotify(notifyString, e =>
                {
                    if (vhc != VEHICLE_CODE) return;
                    VEHICLE_CODE = null;
                    VEHICLE = null;
                });
            }
            void MessageToHuman(string message, int pe)
            {
                foreach (Entity player in human_List)
                {
                    int entref = player.EntRef;

                    if (entref == pe) continue;
                    if (entref > 17) continue;
                    player.Call(33344, Info.GetStr(message, IsAxis[entref]));
                }
            }
        }
    }
}
