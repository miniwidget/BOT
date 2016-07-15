using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    public partial class Infected
    {
        class Vehicle : Inf
        {
            internal Entity VEHICLE;
            string VEHICLE_NAME;

            internal void VehicleAddTank(Entity player, H_SET H)
            {
                Entity tank = VehicleSearch("vehicle_ugv_talon_mp", true);
                if (tank == null) return;

                H.REMOTE_STATE = 3;
                player.Call(32936);//thermalvisionfofoverlayon
                human_List.Add(tank);
                human_List.Remove(player);
                tank.OnNotify("death", e =>
                {
                    human_List.Remove(tank);
                    if (!H.AXIS) human_List.Add(player);
                    if (H.REMOTE_STATE == 3) H.REMOTE_STATE = 0;
                });
            }

            internal void Vehicles(Entity player, string weapon)
            {
                if (weapon == "killstreak_helicopter_mp") VehicleStartRemote(player, VehicleSearch("vehicle_cobra_helicopter_fly_low", false), "tag_light_belly");
                else if (weapon == "mortar_remote_mp") VehicleAdd(player, "vehicle_predator_b", "REMOTE MORTAR");//ok
                else if (weapon == "heli_remote_mp") VehicleAdd(player, "vehicle_v22_osprey_body_mp", "OSPREY");//ok
            }

            void VehicleAdd(Entity player, string vehicleModel, string shortName)
            {
                if (VEHICLE != null) VEHICLE = null;

                VEHICLE = VehicleSearch(vehicleModel, false);
                if (VEHICLE == null) return;

                VEHICLE_CODE = rnd.Next(10000, 1000000).ToString();
                VEHICLE_NAME = shortName + "+" + player.EntRef;

                MessageToTeam("TYPE ^2[ " + VEHICLE_CODE + " ] ^7TO RIDE " + shortName, player.EntRef);
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

                if (H.REMOTE_STATE != 0) return;
                H.VEHICLE = vehicle;
                string notifyString = null;

                bool remoteTurret = true;
                string weapon = player.CurrentWeapon;

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
                }
                else notifyString = "leaving";//choper

                int death = player.GetField<int>("deaths");

                if (remoteTurret)
                {
                    Entity turret = Call<Entity>(19, "misc_turret", vehicle.Call<Vector3>(33128, "tag_origin"), "pavelow_minigun_mp", false);//spawnturret
                    turret.Call(32929, "mp_remote_turret");//setmodel
                    turret.Call(32841, vehicle, tag, Common.GetVector(30f, 30f, -50f), vehicle.Call<Vector3>("getTagAngles", tag));//linkto
                    turret.Call(33084, 180f);
                    turret.Call(33083, 180f);
                    turret.Call(33086, 180f);
                    player.AfterDelay(1500, xx =>
                    {
                        player.TakeWeapon("killstreak_helicopter_mp");
                        player.GiveWeapon("killstreak_remote_turret_laptop_mp");
                        player.SwitchToWeapon("killstreak_remote_turret_laptop_mp");

                        player.AfterDelay(2500, x =>
                        {
                            Common.StartOrEndThermal(player, true);
                            player.GiveWeapon("killstreak_remote_turret_remote_mp");
                            player.SwitchToWeapon("killstreak_remote_turret_remote_mp");
                            player.Call("RemoteControlTurret", turret);//RemoteControlTurret
                        });
                    });

                    H.REMOTE_STATE = 4;

                    vehicle.OnNotify(notifyString, e =>
                    {
                        if (death == player.GetField<int>("deaths"))
                        {
                            player.Call(32980, turret);//RemoteControlTurretOff
                            player.Call(32843);//unlink

                            player.TakeWeapon("killstreak_remote_turret_remote_mp");
                            player.TakeWeapon("killstreak_remote_turret_laptop_mp");

                            player.Call(33504);//enableOffhandWeapons
                            Common.StartOrEndThermal(player, false);

                            WP.GiveRandomWeaponTo(player);

                            if (H.REMOTE_STATE == 4) H.REMOTE_STATE = 0;
                        }

                        if (turret != null) turret.Call("delete");//delete
                    });

                }
                else
                {
                    Common.StartOrEndThermal(player, true);

                    player.GiveWeapon("killstreak_ac130_mp");
                    player.SwitchToWeapon("killstreak_ac130_mp");
                    player.AfterDelay(2000, x =>
                    {
                        player.GiveWeapon("mortar_remote_mp");//zoom_
                        player.GiveWeapon("mortar_remote_zoom_mp");//zoom_
                        player.SwitchToWeapon("mortar_remote_mp");//zoom_

                        player.Call(32887, vehicle, tag, 1, 180, 180, 180, 180);//PlayerLinkWeaponviewToDelta
                        player.Call(33531, vehicle.Call<Vector3>("getTagAngles", "tag_player"));//setPlayerAngles

                        H.REMOTE_STATE = 5;
                        H.MISSILE_COUNT = 10;

                        Common.BulletHudInfoCreate(player, H, H.MISSILE_COUNT);

                        vehicle.OnNotify(notifyString, e =>
                        {
                            if (death == player.GetField<int>("deaths")) VehicleEndMissile(player, weapon, H);
                        });
                    });

                    if (H.VEHICLE_FIRE_NOTIFIED) return;
                    H.VEHICLE_FIRE_NOTIFIED = true;

                    player.Call(33445, "VEHICLE_MISSILE", "+frag");//notifyonplayercommand
                    player.OnNotify("VEHICLE_MISSILE", e =>
                    {
                        if (H.REMOTE_STATE != 5) return;
                        if (H.MISSILE_COUNT <= 0 || H.VEHICLE == null)
                        {
                            VehicleEndMissile(player, weapon, H);
                            return;
                        }
                        H.MISSILE_COUNT--;
                        VehicleFireMissile(player, H.VEHICLE.Origin);
                        H.HUD_BULLET_INFO.SetText(H.MISSILE_COUNT.ToString());
                    });

                    player.Call(33445, "RMBC", "+toggleads_throw");//notifyonplayercommand
                    player.OnNotify("RMBC", e =>
                    {
                        if (H.REMOTE_STATE != 5) return;

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
            void VehicleEndMissile(Entity player, string weapon, H_SET H)
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

                player.GiveWeapon(weapon);
                player.SwitchToWeapon(weapon);

                Common.StartOrEndThermal(player, false);
                if (H.REMOTE_STATE == 5) H.REMOTE_STATE = 0;

            }

            Entity VehicleSearch(string vehicleModel, bool addHumanList)
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
                        return vehicle;
                    }
                }
                return null;
            }

            void MessageToTeam(string message, int pe)
            {
                foreach (Entity player in human_List)
                {
                    if (player.EntRef == pe) continue;
                    if (player.EntRef > 17) continue;
                    player.Call(33344, message);
                }
            }
        }
    }
}
