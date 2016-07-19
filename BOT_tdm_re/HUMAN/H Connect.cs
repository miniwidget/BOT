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
        byte BALANCE_COUNT = 10;

        void Human_Connected(Entity player, string name)
        {

            int pe = player.EntRef; if (human_List.Count >= 18 - SET.BOT_SETTING_NUM) { Utilities.ExecuteCommand("dropclient " + pe + " \"MAX players count overflow\""); return; }

            string team = HumanTeamAssgin();

            player.Notify("menuresponse", "team_marinesopfor", team);
            human_List.Add(player);

            if (H_FIELD[pe] == null) H_FIELD[pe] = new H_SET();
            H_SET H = H_FIELD[pe];
            H.ENTREF = player.EntRef;
            Print(name + " connected ♥");

            if (name == ADMIN_NAME) SET.SetADMIN((ADMIN = player));

            player.AfterDelay(100, p =>
            {
                string recipe = null; int n = rnd.Next(1, 4);

                if (GetPlayerTeam(player) == "axis")
                {
                    H.AXIS = true; recipe = "axis_recipe" + n;
                    if (!Axis_List.Contains(player)) Axis_List.Add(player);
                    if (Allies_List.Contains(player)) Allies_List.Remove(player);
                }
                else
                {
                    H.AXIS = false; recipe = "allies_recipe" + n;
                    if (!Allies_List.Contains(player)) Allies_List.Add(player);
                    if (Axis_List.Contains(player)) Axis_List.Remove(player);
                }

                SetZero_hset(H, true);
                SetPlayer(H, player);

                player.Notify("menuresponse", "changeclass", recipe);
                player.AfterDelay(100, pp =>
                {
                    player.SpawnedPlayer += delegate
                    {
                        if (GAME_ENDED_) return;

                        if (H.REMOTE_STATE != State.remote_not_using)
                        {
                            if (H.REMOTE_STATE == State.remote_helicopter) HCT.IfUsetHeli_DoEnd(player, false);
                            else if (H.REMOTE_STATE == State.remote_turretTank) TK.IfUseTank_DoEnd(player);
                            else if (H.REMOTE_STATE == State.remote_predator) if (PRDT != null) PRDT.PredatorEnd(player, H, true, null);
                        }

                        SetZero_hset(H, false);

                        WP.GiveRandomWeaponTo(player);
                        WP.GiveRandomOffhandWeapon(player);

                        GivePerkToHumanKiller(pe);
                    };

                    player.SetPerk("specialty_scavenger", true, false);

                });
            });
        }

        void SetPlayer(H_SET H, Entity player)
        {

            if (GET_TEAMSTATE_FINISHED) CheckTeamState(player, H.AXIS);

            #region SetClientDvar

            //player.SetClientDvar("cl_maxpackets", "100");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");

            #endregion

            #region NOTIFY

            player.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                string weap = newWeap.ToString();
                if (weap[2] == '5')
                {
                    H.GUN = weap;
                    return;
                }
                if (weap == "none") return;

                //if (TEST_) Print("WEAPON_CHANGE: " + weap);

                if (weap == "killstreak_remote_tank_remote_mp") VHC.VehicleAddTank(player, H);
                else if (vehicles.Contains(weap)) VHC.Vehicles(player, weap);
            });

            //HELICOPTER or TURRET TANK or RIDE PREDATOR
            player.Call(33445, "ACTIVATE", "+activate");//"notifyonplayercommand"
            player.OnNotify("ACTIVATE", ent =>
            {
                if (player.CurrentWeapon[2] != '5') return;//deny when using killstreak 

                bool isUsingTurret = player.Call<int>(33539) == 1;

                if (!isUsingTurret)//isUsingTurret : deny when not using turrent
                {
                    if (H.WAIT) return;

                    WaitOnCall(player, H);
                    return;
                }

                WaitOnRemote(player, H);
            });

            player.OnNotify("menuresponse", (p, Menu, Response) =>//deny change team or change Jugg class
            {
                string menu = Menu.ToString();
                string resp = Response.ToString();

                if (menu == "class" && resp == "changeteam")
                {
                    player.AfterDelay(100, x => player.Notify("menuresponse", "team_marinesopfor", "back"));
                }
                else if (menu == "changeclass")
                {
                    if (resp == "allies_recipe5" || resp == "axis_recipe5")
                    {
                        player.AfterDelay(1000, xx =>
                        {
                            if (H.AXIS) player.Notify("menuresponse", "changeclass", "axis_recipe" + rnd.Next(1, 4));
                            else player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 4));
                            player.Call("suicide");
                        });
                    }
                }
            });
            #endregion

            HUD.PlayersHud(player, H, GET_TEAMSTATE_FINISHED);
        }
        void SetZero_hset(H_SET H, bool init)
        {
            H.REMOTE_STATE = State.remote_not_using;
            H.MARKER_TYPE = State.marker_none;
            H.CAN_USE_HELI = false;
            H.ON_MESSAGE = false;

            H.CAN_USE_HELI = false;
            H.CAN_USE_PREDATOR = false;
            H.PERK = 2;
            if (H.HUD_PERK_COUNT != null) H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "PRDT **");

            if (init)
            {
                H.PREDATOR_FIRE_NOTIFIED = false;
                H.VEHICLE_FIRE_NOTIFIED = false;
                H.MARKER_NOTIFIED = false;
            }
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
                if (TK.TL.Origin.DistanceTo2D(handPos) < 11) return 2;
                if (TK.TR.Origin.DistanceTo2D(handPos) < 11) return 3;
            }
            if (HCT.HELI != null)
            {
                if (HCT.TL.Origin.DistanceTo2D(handPos) < 11) return 0;
                if (HCT.TR.Origin.DistanceTo2D(handPos) < 11) return 1;
            }
            return 4;
        }

        void WaitOnCall(Entity player, H_SET H)
        {
            if (CP.CARE_PACKAGE == null && H.REMOTE_STATE == State.remote_not_using && H.CAN_USE_PREDATOR)
            {
                H.WAIT = true;
                WaitEndCall(player, H, () => CP.Marker(player, H, State.marker_predator));
                return;
            }
            if (CP.CARE_PACKAGE != null && player.Origin.DistanceTo(CP.CARE_PACKAGE_ORIGIN) < 90)
            {
                H.WAIT = true;
                WaitEndCall(player, H, () => CP.CarePackageDo(player, H));
                return;
            }
            if (HCT.HELI == null && H.CAN_USE_HELI)
            {
                H.WAIT = true;
                if (!HCT.HELI_WAY_ENABLED)

                    WaitEndCall(player, H, () => CP.Marker(player, H, State.marker_helicopter));
                else
                    WaitEndCall(player, H, () => HCT.HeliCall(player, H.AXIS));

                return;
            }

        }
        void WaitEndCall(Entity player, H_SET H, Action func)
        {
            player.AfterDelay(500, p =>
            {
                H.WAIT = false;
                if (player.Call<int>(33533) == 1) return;

                func.Invoke();
            });
        }
        void WaitOnRemote(Entity player, H_SET H)
        {
            player.Call(33436, "black_bw", 0.5f);//VisionSetNakedForPlayer

            player.AfterDelay(500, x =>
            {
                if (player.Call<int>(33539) == 1)//isUsingTurret
                {

                    byte ts = TurretState(player);

                    if (ts == 4)//다른 튜렛 붙잡은 경우 종료
                    {
                        player.Call(33436, "", 0f);//VisionSetNakedForPlayer
                        H.REMOTE_STATE = State.remote_not_using;
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
                        H.REMOTE_STATE = State.remote_not_using;//state 0 or 1
                        HCT.HELI_GUNNER = player;

                        if (H.PERK < 10) Info.MessageRoop(player, 0, new[] { "*" + (11 - H.PERK) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI IF ANOTHER PLAYER ONBOARD" });
                        else Info.MessageRoop(player, 0, HCT.MESSAGE_WAIT_PLAYER);

                        Common.StartOrEndThermal(player, true);
                    }

                }
                else
                {
                    bool ended = false;

                    if (H.REMOTE_STATE == State.remote_helicopter) ended = HCT.IfUsetHeli_DoEnd(player, true);

                    else if (H.REMOTE_STATE == State.remote_turretTank) ended = TK.IfUseTank_DoEnd(player);

                    if (!ended) Common.StartOrEndThermal(player, false);

                    H.REMOTE_STATE = State.remote_not_using;
                }

            });


        }

    }
}
