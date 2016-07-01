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
                Utilities.ExecuteCommand("dropclient " + player.EntRef + " \"MAX players count overflows\"");
                return;
            }

            if (GET_TEAMSTATE_FINISHED && !HUMAN_CONNECTED_) BotDoAttack(true);
            if (!HUMAN_CONNECTED_) HUMAN_CONNECTED_ = true;


            string name = player.Name;

            if (name == ADMIN_NAME)
            {
                SET.SetADMIN((ADMIN = player));
            }

            if (player.GetField<string>("sessionteam") == "allies")
            {
                Print(name + " connected ♥");
                SetPlayer(player);
                if (HUMAN_DIED_ALL) HUMAN_DIED_ALL = false;
            }
            else
            {
                Print("AXIS connected ☜");
                H_FIELD[player.EntRef].LIFE = -1;
                player.SpawnedPlayer += () => human_spawned(player);
                player.Call(33341);//"suicide"
            }
        }

        void Set_hset(H_SET H, bool Axis)
        {
            H.BY_SUICIDE = false;
            H.LOC_DO = false;
            H.AXIS = Axis;
            H.TURRET_STATE = 0;

            if (Axis)
            {
                H.LIFE = -2;
                H.USE_HELI = 1;
                H.AX_WEP = 1;
            }
            else
            {
                H.RESPAWN = false;
                H.USE_HELI = 0;
                H.AX_WEP = 0;
                H.PERK = 2;
                H.LIFE -= 1;
                SetTeamName();
            }

        }
        void SetPlayer(Entity player)
        {
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));

            human_List.Add(player);
            H_SET H = new H_SET(PLAYER_LIFE);
            int pe = player.EntRef;
            H_FIELD[pe] = H;

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

            player.Call(33445, "HOLD_STRAFE", "+strafe");//notifyonplayercommand
            player.OnNotify("HOLD_STRAFE", ent =>
            {
                if (H.AXIS) return;
                var weapon = player.CurrentWeapon;
                if (weapon[2] == '5')
                {
                    player.Call(33523, weapon);//"givemaxammo"
                }
            });

            player.Call(33445, "HOLD_CROUCH", "+movedown");//notifyonplayercommand
            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                if (H.AXIS) return;
                WP.GiveAttachScope(player);
            });

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
            player.OnNotify("end_remote", (Entity ent) =>
            {
                if (!use_tank) return;
                use_tank = false;
                player.Call(32937);
                human_List.Remove(TANK);
                human_List.Add(player);
            });

            player.Call(33445, "LINK_AGAIN", "+smoke");//notifyonplayercommand
            player.OnNotify("LINK_AGAIN", ent =>
            {
                if (H.TURRET_STATE == 0) return;

                byte th = TurretHolding(player);

                if (th > 1)
                {
                    if (TK.IfTankOwner_DoEnd(player)) TK.TankStart(player, th);
                }
                else
                {
                    if (HCT.HELI_OWNER != player) return;
                    player.Call(32843);//unlink
                    player.Call(33257);//remotecontrolvehicleoff
                    player.Call(33256, HCT.HELI);//remotecontrolvehicle  
                }
            });

            #endregion

            #region helicopter

            /* H.USE_HELI
             0 : Allies under 10kill
             1 : ready to call heli
             2 : start heli
             3 : end heli 
             4 : axis
            */
            player.Call(33445, "ACTIVATE", "+activate");//"notifyonplayercommand"
            player.OnNotify("ACTIVATE", ent =>
            {
                if (use_tank) return;
                if (player.CurrentWeapon[2] != '5') return;//killstreak deny
                if (H.USE_HELI == 1 && HCT.HELI == null)//heli 생성
                {
                    HCT.HeliCall(player, H.AXIS); H.USE_HELI = 2;
                    return;
                }
                if (player.Call<int>(33539) != 1) return;//isUsingTurret
                player.Call(33436, "black_bw", 0.5f);//VisionSetNakedForPlayer
                /* H.TURRET_STATE
                 
                1 rmheli
                2 heli gunner
                3 tk runner
                4 tk gunner

                */
                int ts = H.TURRET_STATE;
                if (ts != 0)
                {
                    player.AfterDelay(500, x =>
                    {
                        if (ts == 1) { HCT.HeliEndUse(player, true); H.USE_HELI = 0; }

                        else if (ts == 2) HCT.HeliEndGunner();

                        else TK.IfTankOwner_DoEnd(player);

                        H.TURRET_STATE = 0;
                    });

                    return;
                }

                byte th = TurretHolding(player);

                //player.Call(33436, "black_bw", 0.5f);//VisionSetNakedForPlayer
                player.AfterDelay(500, x =>
                {
                    if (th > 1)
                    {
                        H.TURRET_STATE = TK.TankStart(player, th);//state 3 ~ 4
                        return;
                    }

                    int h = H.USE_HELI;
                    if (h == 2)// remote control 시작
                    {
                        H.TURRET_STATE = HCT.HeliStart(player, H.AXIS);//state 1
                        H.USE_HELI = 3;
                    }
                    else if (h == 1)//헬리를 탈 자격이 되는 상태 (10킬 이상)
                    {
                        if (!HCT.HELI_ON_USE_)//다른 사람이 헬리 불러 놓고 죽거나, 아직 타지 않은 경우
                        {
                            H.TURRET_STATE = HCT.HeliStart(player, H.AXIS);//state 1
                            H.USE_HELI = 3;
                        }
                        return;
                    }
                    else if (h == 0)//헬리를 탈 자격이 안 되는 상태에서, owner가 도착하지 않은 경우
                    {
                        H.TURRET_STATE = 2;//state 2
                        HCT.HELI_GUNNER = player;

                        if (H.PERK < 10) Info.MessageRoop(player, 0, new[] { "^2" + (11 - H.PERK) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI", "IF ANOTHER PLAYER ONBOARD" });
                        else Info.MessageRoop(player, 0, HCT.MESSAGE_WAIT_PLAYER);

                        Common.StartOrEndThermal(player, true);
                    }

                });

            });

            #endregion


            HUD.AlliesHud(player);

            WP.GiveRandomWeaponTo(player);
            WP.GiveRandomOffhandWeapon(player);

            player.SpawnedPlayer += () => human_spawned(player);
            player.Call(33531, Common.ZERO);//setplayerangles
        }
        void SetTeamName()
        {
            Call(42, "g_TeamName_Allies", "ALIVE");//setdvar
            Call(42, "g_TeamName_Axis", "BOTs");//setdvar
        }

        void Relocation(Entity player, bool getback)
        {
            if (!TK.IfTankOwner_DoEnd(player)) HCT.IfHeliOwner_DoEnd(player);

            H_SET H = H_FIELD[player.EntRef];

            if (getback)
            {
                if (!H.LOC_NOTIFIED) return;
                player.Call(33529, H.RELOC);
                return;
            }
            player.Call(33344, "^2THROW MARKER ^7to Relocate Your Position");

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
                                player.Call(33344, "TYPE ^2RELOC ^7IF GET BACK LOCATION");
                            }
                        });
                    });
                });
            }
        }
        byte TurretHolding(Entity player)
        {
            if (HCT.HELI == null)
            {
                int TKL = (int)TK.RMT1.Origin.DistanceTo(player.Origin);
                int TKR = (int)TK.RMT2.Origin.DistanceTo(player.Origin);

                if (TKL < TKR) return 2;
                else return 3;
            }
            else
            {
                int HL = (int)HCT.TL.Origin.DistanceTo(player.Origin);
                int HR = (int)HCT.TR.Origin.DistanceTo(player.Origin);
                int TKL = (int)TK.RMT1.Origin.DistanceTo(player.Origin);
                int TKR = (int)TK.RMT2.Origin.DistanceTo(player.Origin);

                int[] DIFFS = new int[4] { HL, HR, TKL, TKR };
                byte b = (byte)Array.IndexOf(DIFFS, DIFFS.Min());
                //Print(HL + "/" + HR + "/" + TKL + "/" + TKR + "/" + b);
                return b;
            }
        }

        /* 
        void SetLocationByTI(Entity player)
        {
            player.Call(33344, "^2PRESS [{+attack}] ^7TO SAVE YOUR POSITION");

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
                        player.Call(33344, "^2NEXT SPAWN POS ^7SAVED TO " + (int)pos.X + "," + (int)pos.Y);
                        H.LOC = new[] { pos.X, pos.Y, pos.Z };

                        player.AfterDelay(10000, p => Effect.Call(32928));

                    }
                });
            }
        }
        */
    }
}
