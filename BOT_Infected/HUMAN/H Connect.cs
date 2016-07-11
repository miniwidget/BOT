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
        Dictionary<string, int> PLAYER_STATE = new Dictionary<string, int>();

        void Human_Connected(Entity player)
        {
            if (human_List.Count > 6)
            {
                Utilities.ExecuteCommand("dropclient " + player.EntRef + " \"MAX players count overflow\"");
                return;
            }

            if (GET_TEAMSTATE_FINISHED && HUMAN_DIED_ALL_) BotDoAttack(true);
            if (HUMAN_DIED_ALL_) HUMAN_DIED_ALL_ = false;

            string name = player.Name;

            if (!PLAYER_STATE.ContainsKey(name)) PLAYER_STATE.Add(name, SET.PLAYER_LIFE);

            if (name == ADMIN_NAME) SET.SetADMIN((ADMIN = player));

            if (player.GetField<string>("sessionteam") == "allies")
            {
                Print(name + " connected ♥");

                SetPlayer(player, SET.PLAYER_LIFE, name);
            }
            else
            {
                if (PLAYER_STATE[name] == -2)
                {
                    Print("AXIS connected ☜");
                    player.Notify("menuresponse", "changeclass", "axis_recipe4");
                    H_FIELD[player.EntRef].LIFE = -1;

                    player.Call(33341);//"suicide"
                }
                else
                {
                    player.AfterDelay(100, x =>
                    {
                        SetPlayer(player, PLAYER_STATE[name] + 1, name);
                        player.Call(33341);//"suicide"
                    });
                }
            }

            player.SpawnedPlayer += () => human_spawned(player, name);
        }

        void SetZero_hset(H_SET H, bool Axis, int life, string name)
        {
            H.LOC_DO = false;
            H.AXIS = Axis;
            H.REMOTE_STATE = 0;

            if (Axis)
            {
                H.LIFE = -2;
                H.CAN_USE_HELI = true;
                H.AX_WEP = 1;
                H.ON_MESSAGE = false;
            }
            else
            {
                H.RESPAWN = false;
                H.CAN_USE_HELI = false;
                H.USE_PREDATOR = false;
                H.AX_WEP = 0;
                H.PERK = 2;
                H.LIFE = life;
                SetTeamName();
                if (H.PERK_TXT != "**") H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "HELI **");
            }

            PLAYER_STATE[name] = H.LIFE;
        }

        void SetPlayer(Entity player, int life, string name)
        {

            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            if (!human_List.Contains(player)) human_List.Add(player);
            int pe = player.EntRef;
            B_FIELD[pe] = null;
            H_SET H = H_FIELD[pe];
            SetZero_hset(H, false, life, name);
            H.PREDATOR_NOTIFIED = false;

            #region SetClientDvar

            //player.SetClientDvar("cl_maxpackets", "100");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");

            #endregion

            #region TANK

            Entity TANK = null;
            bool use_tank = false;
            player.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                if (use_tank) return;

                string weap = newWeap.ToString();
                if (weap != "killstreak_remote_tank_remote_mp") return;
                use_tank = true;
                TANK = null;

                bool found = false;
                for (int i = 18; i < 2048; i++)
                {
                    TANK = Entity.GetEntity(i);
                    if (TANK == null || TANK == TK.REMOTETANK) continue;
                    var model = TANK.GetField<string>("model");
                    if (model == "vehicle_ugv_talon_mp")
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return;

                player.Call(32936);
                human_List.Add(TANK);
                human_List.Remove(player);
            });
            player.OnNotify("end_remote", ent =>
           {
               if (!use_tank) return;
               use_tank = false;
               player.Call(32937);
               human_List.Remove(TANK);
               human_List.Add(player);
           });

            //player.Call(33445, "LINK_AGAIN", "+smoke");//notifyonplayercommand
            //player.OnNotify("LINK_AGAIN", ent =>
            //{
            //    if (H.TURRET_STATE == 0) return;

            //    byte th = TurretHolding(player);

            //    if (th > 1)
            //    {
            //        if (TK.IfTankOwner_DoEnd(player)) TK.TankStart(player, th);
            //    }
            //    else
            //    {
            //        if (HCT.HELI_OWNER != player) return;
            //        player.Call(32843);//unlink
            //        player.Call(33257);//remotecontrolvehicleoff
            //        player.Call(33256, HCT.HELI);//remotecontrolvehicle  
            //    }
            //});

            #endregion

            #region helicopter

            bool wait = false;
            player.Call(33445, "ACTIVATE", "+activate");//"notifyonplayercommand"
            player.OnNotify("ACTIVATE", ent =>
            {
                if (use_tank) return;
                if (!H.AXIS && player.CurrentWeapon[2] != '5') return;//deny when using killstreak 
                bool isUsingTurret = player.Call<int>(33539) == 1;

                if (!isUsingTurret && H.CAN_USE_HELI && HCT.HELI == null)//isUsingTurret : deny when not using turrent
                {
                    if (wait) return; wait = true;
                    player.AfterDelay(500, p =>
                    {
                        wait = false;
                        if (player.Call<int>(33533) == 1) return;//usebuttonpressed : deny when catching carepackage 
                        HCT.HeliCall(player, H.AXIS);
                    });
                    return;
                }
                if (!isUsingTurret)
                {
                    if (CARE_PACKAGE != null && player.Origin.DistanceTo(CARE_PACKAGE.Origin) < 80)
                    {
                        CarePackageDo(player, H);
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

            HUD.AlliesHud(player, GET_TEAMSTATE_FINISHED);

            WP.GiveRandomWeaponTo(player);
            WP.GiveRandomOffhandWeapon(player);

            player.Call(33531, Common.ZERO);//setplayerangles
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

        void SetTeamName()
        {
            Call(42, "g_TeamName_Allies", "ALIVE");//setdvar
            Call(42, "g_TeamName_Axis", "BOTs");//setdvar
        }


        Entity CARE_PACKAGE;

        void CarePackage(Entity player)
        {
            player.Call(33344, Info.GetStr("*THROW MARKER ^7to GET CARE PACKAGE", false));

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
                player.AfterDelay(3000, p =>
                {
                    finished = true;
                    Vector3 MO = Marker.Origin; MO.Z += 45;
                    Marker.Call(32928);//delete

                    Entity ent = Call<Entity>("getent", "mp_dom_spawn", "classname"); if (ent == null) return;

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
                            Print(targetName + " entref " + i);
                            CARE_PACKAGE = Call<Entity>("spawn", "script_model", MO);
                            CARE_PACKAGE.Call("setmodel", "com_plasticcase_friendly");//com_plasticcase_friendly
                            CARE_PACKAGE.Call(33353, brushmodel);

                            Call(431, 20, "active"); // objective_add
                            Call(435, 20, MO); // objective_position
                            Call(434, 20, "compass_waypoint_bomb"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

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
            player.Call(33469, weapon, 500);//setweaponammostock
            player.Call(33468, weapon, 500);//setweaponammoclip

            player.Call(33466, "ammo_crate_use");//playLocalSound
            if (H.AXIS) return;

            if (USE_PLANE)
            {
                player.Call(33344, "PREDATOR IS ALREADY IN THE AIR. WAIT");
                return;
            }
            if (H.PERK < 8)
            {
                player.Call(33344, 8 - H.PERK + "KILL MORE to RIDE PREDATOR");
                return;
            }
            if (H.USE_PREDATOR)
            {
                player.Call(33344, "PREDATOR FINISHED");
                return;
            }

            PredatorStart(player, H);
        }
    }
}
