using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Tdm
{
    public partial class Tdm
    {
        #region field
        string
            SERVER_NAME, ADMIN_NAME,
            NEXT_MAP,
            WELLCOME_MESSAGE;

        bool
            USE_ADMIN_SAFE_, DEPLAY_BOT_,
            GAME_ENDED_, HUMAN_CONNECTED_;

        float
            PLAYERWAIT_TIME, MATCHSTART_TIME;

        int
            t0 = 100, t1 = 1000, t2 = 2000, t3 = 3000,
            SEARCH_TIME, FIRE_TIME, BOT_DELAY_TIME, BOT_SETTING_NUM, FIRE_DIST;

        bool[] IsBOT = new bool[18];
        void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }

        Random rnd = new Random();
        Entity ADMIN;
        #endregion

        #region server side
        /// <summary>
        /// server side dvar
        /// </summary>
        void Server_SetDvar()
        {
            Call("setdvar", "scr_game_playerwaittime", PLAYERWAIT_TIME);
            Call("setdvar", "scr_game_matchstarttime", MATCHSTART_TIME);

            Call("setdvar", "testClients_watchKillcam", 0);
            Call("setdvar", "testClients_doReload", 0);
            //Call("setdvar", "testClients_doCrouch", 0);
            //Call("setdvar", "testClients_doMove", 1);
            //Call("setdvar", "testClients_doAttack", 1);

            //if (SERVER_NAME == "^2BOT ^7TDM SERVER" || SERVER_NAME == "^2BOT ^7TDM test server") SERVER_NAME += " " + rnd.Next(1000);
            Utilities.ExecuteCommand("sv_hostname " + SERVER_NAME);
            for (int i = 0; i < 18; i++)
            {
                B_FIELD.Add(new B_SET());
                H_FIELD.Add(new H_SET());
            }

        }
        void readMAP()
        {
            string currentMAP = Call<string>("getdvar", "mapname");
            string ENTIRE_MAPLIST = "mp_plaza2|mp_mogadishu|mp_bootleg|mp_carbon|mp_dome|mp_exchange|mp_lambeth|mp_hardhat|mp_interchange|mp_alpha|mp_bravo|mp_radar|mp_paris|mp_seatown|mp_underground|mp_village|mp_morningwood|mp_park|mp_overwatch|mp_italy|mp_cement|mp_qadeem|mp_meteora|mp_hillside_ss|mp_restrepo_ss|mp_aground_ss|mp_courtyard_ss|mp_terminal_cls|mp_burn_ss|mp_nola|mp_six_ss|mp_moab";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            int index = map_list.IndexOf(currentMAP);

            int[] smallMap = { 23, 24, 25, 26, 28, 29, 30 };
            int[] largeMap = { 5, 8, 16, 17, 31 };
            //set bot's fire distance
            if (smallMap.Contains(index))
            {
                FIRE_DIST = 600;
            }
            else if (largeMap.Contains(index))
            {
                FIRE_DIST = 850;
            }
            else
            {
                FIRE_DIST = 750;
            }

            //set next map
            if (index >= max || index < 0) index = 0; else index++;
            NEXT_MAP = map_list[index];

            Call("setdvar", "sv_nextmap", NEXT_MAP);

            if (TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
                Utilities.ExecuteCommand("sv_hostname TEST");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");

                string content = NEXT_MAP + ",TDM,1";
                File.WriteAllText("admin\\TDM.dspl", content);
            }

        }

        #endregion

        #region Bots side

        /// <summary>
        /// BOT SET class for custom fields set
        /// </summary>
        class B_SET
        {
            internal Entity target { get; set; }
            internal int death { get; set; }
            internal bool fire { get; set; }
            internal bool temp_fire { get; set; }
            internal bool wait { get; set; }
            internal string wep;
            internal int killer = -1;
            internal bool Axis;
        }
        List<B_SET> B_FIELD = new List<B_SET>(18);
        List<Entity> BOTs_List = new List<Entity>();
        #endregion

        #region Human side
        /// <summary>
        /// HUMAN PLAYER SET class for custom fields set
        /// </summary>
        internal class H_SET
        {
            int att = 0;
            internal int SIRENCERorHB
            {
                get
                {
                    this.att++;
                    if (this.att > 2) this.att = 0;
                    return this.att;
                }
            }
            internal int PERK { get; set; }
            internal HudElem PERK_COUNT_HUD;
            internal string PERK_TXT = "**";

            /// <summary>
            /// 0: Allies under 10kill IN ALLIES /
            /// 1: ready to call heli /
            /// </summary>
            internal bool CAN_USE_HELI;

            /// <summary>
            /// 0 not using remote /
            /// 1 remote helicopter /
            /// 2 remote tank /
            /// </summary>
            internal byte REMOTE_STATE;
            /// <summary>
            /// when roop massage, if on_message state, it blocks reapeted roop
            /// </summary>
            internal bool ON_MESSAGE;

            internal bool Axis;
        }
        internal static List<H_SET> H_FIELD = new List<H_SET>(18);
        //Dictionary<int, int> H_ID = new Dictionary<int, int>();
        internal static List<Entity> human_List = new List<Entity>();

        List<Entity> H_AXIS_LIST = new List<Entity>();
        List<Entity> H_ALLIES_LIST = new List<Entity>();
        #endregion



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
                    if (H.Axis)
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
                }else
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
                if (use_tank) return;
                if (player.CurrentWeapon[2] != '5') return;//deny when using killstreak 
                bool isUsingTurret = player.Call<int>(33539) == 1;

                if (!isUsingTurret && H.CAN_USE_HELI && HCT.HELI == null)//isUsingTurret : deny when not using turrent
                {
                    if (wait) return; wait = true;
                    player.AfterDelay(500, p =>
                    {
                        wait = false;
                        if (player.Call<int>(33533) == 1) return;//usebuttonpressed : deny when catching carepackage 
                        HCT.HeliCall(player);
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
                            H.REMOTE_STATE = HCT.HeliStart(player);//state 0 or 1
                        }
                        else //헬리를 탈 자격이 안 되는 상태에서, owner가 도착하지 않은 경우
                        {
                            H.REMOTE_STATE = 0;//state 0 or 1
                            HCT.HELI_GUNNER = player;

                            if (H.PERK < 10) Info.MessageRoop(player, 0, new[] { "^2" + (11 - H.PERK) + " KILL MORE ^7TO RIDE HELI", "YOU CAN RIDE HELI", "IF ANOTHER PLAYER ONBOARD" });
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
            AlliesHud(player, offhand.Replace("_mp", "").ToUpper());
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

    }
}
