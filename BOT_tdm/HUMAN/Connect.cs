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
        bool GET_TEAMSTATE_FINISHED;

        void Human_Connected(Entity player)
        {
            string name = player.Name;

            if (player.Name == ADMIN_NAME)
            {
                ADMIN = player;
                setADMIN();
            }

            if (!HUMAN_CONNECTED_) HUMAN_CONNECTED_ = true;
            Print(name + " connected ♥");
            Client_init_GAME_SET(player);

        }
        void Tdm_PlayerDisConnected(Entity player)
        {
            H_ALLIES_LIST.Remove(player);
            H_AXIS_LIST.Remove(player);
            human_List.Remove(player);
            if (human_List.Count == 0) HUMAN_CONNECTED_ = false;
        }

        void setADMIN()
        {
            ADMIN.Call("notifyonplayercommand", "SPECT", "centerview");
            bool spect = false;
            ADMIN.OnNotify("SPECT", a =>
            {
                if (!spect)
                {
                    ADMIN.Call("allowspectateteam", "freelook", true);
                    ADMIN.SetField("sessionstate", "spectator");
                }
                else
                {
                    ADMIN.Call("allowspectateteam", "freelook", false);
                    ADMIN.SetField("sessionstate", "playing");
                }
                spect = !spect;
            });
            if (TEST_)
            {
                ADMIN.Call("thermalvisionfofoverlayon");
                ADMIN.Call("setmovespeedscale", 1.5f);
            }
        }



        #region client side

        /// <summary>
        /// client side dvar & set notifycommand & give weapon & set HUD & change class
        /// </summary>
        void Client_init_GAME_SET(Entity player)
        {
            player.Notify("menuresponse", "team_marinesopfor", "autoassign");
            #region set
            human_List.Add(player);

            H_SET H = H_FIELD[player.EntRef];
            H.PERK = 2;
            #endregion

            #region SetClientDvar

            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");

            #endregion

            #region notifyonplayercommand

            player.Call("notifyonplayercommand", "+TAB", "+scores");
            player.Call("notifyonplayercommand", "-TAB", "-scores");
            player.Call("notifyonplayercommand", "HOLD_STRAFE", "+strafe");
            player.Call("notifyonplayercommand", "HOLD_CROUCH", "+movedown");
            player.Call("notifyonplayercommand", "HOLD_PRONE", "+prone");
            player.Call("notifyonplayercommand", "HOLD_STANCE", "+stance");
            //+strafe

            player.OnNotify("HOLD_STRAFE", ent =>
            {
                var weapon = player.CurrentWeapon;
                if (weapon.Length > 3 && weapon[2] == '5')
                {
                    player.Call("givemaxammo", weapon);
                }
            });

            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                giveAttachScope(player);
            });

            player.OnNotify("HOLD_PRONE", ent =>//attachment silencer heartbeat,
            {
                giveAttachHeartbeat(player);
            });

            string offhand = "";
            switch (rnd.Next(4))
            {
                case 0: offhand = "frag_grenade_mp"; break;
                case 1: offhand = "semtex_mp"; break;//OK
                case 2: offhand = "bouncingbetty_mp"; break;//OK
                case 3: offhand = "claymore_mp"; break;//OK
            }

            player.OnNotify("HOLD_STANCE", ent =>//offhand weapon
            {
                giveOffhandWeapon(player, offhand);
            });

            player.OnNotify("menuresponse", (p, Menu, Response) =>
            {
                string menu = Menu.ToString();
                string resp = Response.ToString();

                if (menu == "class" && resp == "changeteam")
                {
                    p.AfterDelay(100, x =>
                    {
                        p.Notify("menuresponse", "team_marinesopfor", "back");
                    });
                }
            });

            #region TANK

            Entity TANK = null;
            bool use_tank = false;
            bool axis = false;
            player.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                if (use_tank) return;
                string weap = newWeap.ToString();
                //print(weap);
                if (weap == "killstreak_remote_tank_remote_mp")
                {
                    use_tank = true;
                    TANK = null;

                    bool found = false;
                    for (int i = 0; i < 2048; i++)
                    {
                        TANK = Entity.GetEntity(i);
                        if (TANK == null) continue;
                        var model = TANK.GetField<string>("model");
                        if (model == "vehicle_ugv_talon_mp")
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found) return;

                    player.Call(32936);
                    if (H.AXIS)
                    {
                        H_AXIS_LIST.Remove(player);
                        H_AXIS_LIST.Add(TANK);
                        axis = true;
                    }
                    else
                    {
                        H_ALLIES_LIST.Remove(player);
                        H_ALLIES_LIST.Add(TANK);
                        axis = false;
                    }
                }
            });
            player.OnNotify("end_remote", (Entity ent) =>
            {
                if (!use_tank) return;
                use_tank = false;
                player.Call(32937);
                if (axis)
                {
                    H_AXIS_LIST.Remove(TANK);
                    H_AXIS_LIST.Add(player);
                }
                else
                {
                    H_ALLIES_LIST.Add(player);
                    H_ALLIES_LIST.Remove(player);
                }
            });
            #endregion

            #endregion



            #region helicopter

            bool wait = false;
            player.Call(33445, "ACTIVATE", "+activate");//"notifyonplayercommand"
            player.OnNotify("ACTIVATE", ent =>
            {
                if (!H.AXIS && player.CurrentWeapon[2] != '5') return;//deny when using killstreak 
                bool isUsingTurret = player.Call<int>(33539) == 1;

                if (!isUsingTurret)
                {
                    if (CARE_PACKAGE != null && player.Origin.DistanceTo(CARE_PACKAGE.Origin) < 90)
                    {
                        CarePackageDo(player, H);
                        return;
                    }

                    if (HCT.HELI == null && H.CAN_USE_HELI)//isUsingTurret : deny when not using turrent
                    {
                        if (wait) return; wait = true;
                        player.AfterDelay(500, p =>
                        {
                            wait = false;
                            if (player.Call<int>(33533) == 1) return;//usebuttonpressed : deny when catching carepackage 
                            HCT.HeliCall(player, H.AXIS);
                        });
                    }
                    return;
                }


                player.Call(33436, "black_bw", 0.5f);//VisionSetNakedForPlayer

                player.AfterDelay(500, x =>
                {
                    if (player.Call<int>(33539) == 1)//isUsingTurret
                    {
                        byte ts = TurretState(player);

                        if (ts == 4)//다른 튜렛 붙잡은 경우 종료
                        {
                            player.Call(33436, "", 0f);//VisionSetNakedForPlayer
                            H.REMOTE_STATE = 0;
                            return;
                        }
                        if (ts > 1)//탱크 튜렛을 붙잡은 경우
                        {
                            H.REMOTE_STATE = TK.TankStart(player, ts, H.AXIS);//state 0 or 2
                            return;
                        }

                        if (H.CAN_USE_HELI)
                        {
                            H.REMOTE_STATE = HCT.HeliStart(player, H.AXIS);//state 0 or 1
                        }
                        else //헬리를 탈 자격이 안 되는 상태에서, owner가 도착하지 않은 경우
                        {
                            H.REMOTE_STATE = 0;//state 0 or 1
                            HCT.HELI_GUNNER = player;

                            if (H.PERK < 10) Info.MessageRoop(player, 0, new[] { "*" + (11 - H.PERK) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI IF ANOTHER PLAYER ONBOARD" });
                            else Info.MessageRoop(player, 0, HCT.MESSAGE_WAIT_PLAYER);

                            Common.StartOrEndThermal(player, true);
                        }

                    }
                    else
                    {

                        byte rms = H.REMOTE_STATE;
                        bool ended = false;

                        if (rms == 1) ended = HCT.IfUsetHeli_DoEnd(player, true);

                        else if (rms == 2) ended = TK.IfUseTank_DoEnd(player);

                        if (!ended) Common.StartOrEndThermal(player, false);

                        H.REMOTE_STATE = 0;
                    }

                });


            });

            #endregion




            #region AlliesHud
            AlliesHud(player, GET_TEAMSTATE_FINISHED);
            #endregion

            player.SpawnedPlayer += () => human_spawned(player);
        }
        #endregion

        bool IsAxis(Entity player)
        {
            return player.GetField<string>("sessionteam") == "axis";
        }
        /// <summary>
        /// 0: Helicopter Left Turret /
        /// 1: Helicopter Right Turret /
        /// 2: Tank Left Turret /
        /// 3: Tank Right Turret /
        /// 4: Other turret /
        /// </summary>
        byte TurretState(Entity player)
        {

            var handPos = player.Call<Vector3>(33128, "tag_weapon_left");

            if (TK.REMOTETANK != null)
            {
                if (TK.TL.Origin.DistanceTo2D(handPos) < 9) return 2;
                if (TK.TR.Origin.DistanceTo2D(handPos) < 9) return 3;
            }
            if (HCT.HELI != null)
            {
                if (HCT.TL.Origin.DistanceTo2D(handPos) < 9) return 0;
                if (HCT.TR.Origin.DistanceTo2D(handPos) < 9) return 1;
            }
            return 4;
        }

        Entity CARE_PACKAGE;

        void CarePackage(Entity player)
        {
            if (CARE_PACKAGE != null) return;

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

                //if (PRDT == null) PRDT = new Predator();

                player.AfterDelay(3000, p =>
                {
                    finished = true;
                    Vector3 MO = VectorAddZ(Marker.Origin, 8);
                    Marker.Call(32928);//delete

                    Entity brushmodel = Call<Entity>("getent", "pf1_auto1", "targetname");

                    if (brushmodel == null) brushmodel = Call<Entity>("getent", "pf3_auto1", "targetname");
                    if (brushmodel != null)
                    {
                        SpawnCarePackage(MO, brushmodel);
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

                            SpawnCarePackage(MO, brushmodel);

                            break;
                        }
                    }


                });
            });
        }
        void SpawnCarePackage(Vector3 origin, Entity brushmodel)
        {
            CARE_PACKAGE = Call<Entity>("spawn", "script_model", origin);
            CARE_PACKAGE.Call("setmodel", "com_plasticcase_friendly");//com_plasticcase_friendly
            if (brushmodel != null) CARE_PACKAGE.Call(33353, brushmodel);

            Call(431, 20, "active"); // objective_add
            Call(435, 20, origin); // objective_position
            Call(434, 20, "compass_objpoint_ammo_friendly"); //objective_icon compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

            brushmodel = Call<Entity>("spawn", "script_model", VectorAddZ(origin, 25));
            brushmodel.Call("setmodel", "projectile_cbu97_clusterbomb");
        }
        void CarePackageDo(Entity player, H_SET H)
        {
            string weapon = player.CurrentWeapon;
            player.Call(33523, weapon);//givemaxammo
            player.Call(33468, weapon, 100);//setweaponammoclip

            player.Call(33466, "ammo_crate_use");//playLocalSound


            //if (H.AXIS) return;

            //if (USE_PREDATOR)
            //{
            //    player.Call(33344, "PREDATOR IS ALREADY IN THE AIR. WAIT");
            //    return;
            //}
            //if (H.PERK < 8)
            //{
            //    player.Call(33344, 8 - H.PERK + " KILL MORE to RIDE PREDATOR");
            //    return;
            //}
            //if (H.USE_PREDATOR)
            //{
            //    player.Call(33344, "PREDATOR FINISHED");
            //    return;
            //}

            //PRDT.PredatorStart(player, H);
        }
        Vector3 VectorAddZ(Vector3 origin, float add)
        {
            origin.Z += add;
            return origin;
        }

    }
}
