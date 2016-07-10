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

            if (name == ADMIN_NAME)
            {
                SET.SetADMIN((ADMIN = player));
            }

            if (player.GetField<string>("sessionteam") == "allies")
            {
                Print(name + " connected ♥");
                SetPlayer(player);
            }
            else
            {
                player.SpawnedPlayer += () => human_spawned(player);

                player.Notify("menuresponse", "changeclass", "axis_recipe4");
                Print("AXIS connected ☜");
                H_FIELD[player.EntRef].LIFE = -1;

                player.Call(33341);//"suicide"
            }
        }

        void SetZero_hset(H_SET H, bool Axis, int life)
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
                H.AX_WEP = 0;
                H.PERK = 2;
                H.LIFE = life;
                SetTeamName();
                if (H.PERK_TXT != "**") H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "**");
            }

        }

        void SetPlayer(Entity player)
        {
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            if (!human_List.Contains(player)) human_List.Add(player);
            int pe = player.EntRef;
            B_FIELD[pe] = null;
            H_SET H = H_FIELD[pe];

            SetZero_hset(H, false, SET.PLAYER_LIFE);
            
            #region SetClientDvar

            //player.SetClientDvar("cl_maxpackets", "100");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");

            #endregion

            #region notifyonplayercommand

            //player.Call(33445, "HOLD_STRAFE", "+strafe");//notifyonplayercommand
            //player.OnNotify("HOLD_STRAFE", ent =>
            //{
            //    if (H.AXIS) return;
            //    var weapon = player.CurrentWeapon;
            //    if (weapon[2] == '5')
            //    {
            //        player.Call(33523, weapon);//"givemaxammo"
            //    }
            //});

            //player.Call(33445, "HOLD_CROUCH", "+movedown");//notifyonplayercommand
            //player.OnNotify("HOLD_CROUCH", ent =>//view scope
            //{
            //    if (H.AXIS) return;
            //    WP.GiveAttachScope(player);
            //});

            //player.Call(33445, "HOLD_STANCE", "+stance");
            //player.OnNotify("HOLD_STANCE", ent =>//offhand weapon
            //{
            //    if (IsAXIS[pe]) return;
            //    WP.GiveRandomOffhandWeapon(player);
            //});

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
            player.OnNotify("end_remote",  ent =>
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
                    /*
                    if (CARE_PACKAGE != null&&player.Origin.DistanceTo(CARE_PACKAGE.Origin) < 80)
                    {
                        player.Call(33344, "TEST. WHAT DO I DO FOR YOU?");
                        
                        //if (ac130 == null) ac130 = new AC130();
                        //ac130.start(player);
                    }
                    */
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
                            H.REMOTE_STATE = TK.TankStart(player, ts);//state 0 or 2
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

            player.SpawnedPlayer += () => human_spawned(player);
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

        void Relocation(Entity player, bool getback)
        {
            if (!TK.IfUseTank_DoEnd(player)) HCT.IfUsetHeli_DoEnd(player,true);

            H_SET H = H_FIELD[player.EntRef];

            if (getback)
            {
                if (!H.LOC_NOTIFIED) return;
                player.Call(33529, H.RELOC);
                return;
            }
            player.Call(33344, Info.GetStr("*THROW MARKER ^7to Relocate Your Position",H.AXIS));

            string marker = "airdrop_sentry_marker_mp";
            player.GiveWeapon(marker);
            player.SwitchToWeaponImmediate(marker);

            H.LOC_DO = true;

            if (!H.LOC_NOTIFIED)
            {
                H.LOC_NOTIFIED = true;

                player.OnNotify("grenade_fire", (Entity owner, Parameter mk, Parameter weaponName) =>
                {
                    if (!H.LOC_DO) return;
                    if (weaponName.ToString() != "airdrop_sentry_marker_mp") return;

                    H.LOC_DO = false;
                    Entity ent = mk.As<Entity>();
                    if (ent == null) return;
                    H.RELOC = player.Origin;
                    player.Call(32841, ent);//linkto
                    player.AfterDelay(3000, p =>
                    {
                        player.Call(32843);//unlink
                        ent.Call(32928);//delete

                        p.AfterDelay(200, x =>
                        {
                            int ground = player.Call<int>(33538);
                            if (ground == 0)
                            {
                                var pos = player.Origin; pos.Z += 10;
                                player.Call(33529, pos);//setorigin
                                player.Call(33344, Info.GetStr("TYPE *RELOC ^7IF GET BACK LOCATION",H.AXIS));
                            }
                        });
                    });
                });
            }
        }

        /* 
        void SetLocationByTI(Entity player)
        {
            player.Call(33344, "*PRESS [{+attack}] ^7TO SAVE YOUR POSITION");

            string flare_mp = "flare_mp";
            player.GiveWeapon(flare_mp);
            player.SwitchToWeaponImmediate(flare_mp);

            int pe = player.EntRef;
            H_SET H = H_FIELD[pe];
            H.TI_DO = true;

            if (!H.TI_NOTIFIED)
            {
                H.TI_NOTIFIED = true;

                player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
                {
                    if (!H.TI_DO) return;
                    if (H.AXIS) return;
                    if (weaponName.ToString() != flare_mp) return;
                    H.TI_DO = false;
                    Vector3 pos = player.Origin;
                    int i = Call<int>(303, "misc/flare_ambient");//loadfx
                    if (i > 0)
                    {
                        Entity Effect = Call<Entity>(308, i, pos);//spawnFx
                        Call(309, Effect);//triggerfx
                        player.Call(33344, "*NEXT SPAWN POS ^7SAVED TO " + (int)pos.X + "," + (int)pos.Y);
                        H.LOC = new[] { pos.X, pos.Y, pos.Z };

                        player.AfterDelay(10000, p => Effect.Call(32928));

                    }
                });
            }
        }
        */
    }
}
