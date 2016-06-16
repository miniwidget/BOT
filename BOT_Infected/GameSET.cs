using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    public partial class Infected
    {
        #region field
        string
            SERVER_NAME, ADMIN_NAME,
            TEAMNAME_ALLIES, TEAMNAME_AXIS,
            NEXT_MAP,
            WELLCOME_MESSAGE;

        bool
            USE_ADMIN_SAFE_, Disable_Melee_,
            DEPLAY_BOT_, SUICIDE_BOT_, OVERFLOW_BOT_,

           GAME_ENDED_, HUMAN_CONNECTED_;

        float
            INFECTED_TIMELIMIT, PLAYERWAIT_TIME, MATCHSTART_TIME;

        int
            t0 = 100, t1 = 1000, t2 = 2000, t3 = 3000,
            SEARCH_TIME, FIRE_TIME, BOT_DELAY_TIME, BOT_SETTING_NUM, FIRE_DIST,
            PLAYER_LIFE;

        bool isSurvivor(Entity player) { return player.GetField<string>("sessionteam") == "allies"; }
        //char[] numChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        Random rnd = new Random();
        Entity ADMIN, First_Infed_Player;
        string[] BOTs_CLASS = { "axis_recipe1", "axis_recipe2", "axis_recipe3", "class0", "class1", "class2", "class4", "class5", "class6", "class6" };
        #endregion

        #region Bots side

        /// <summary>
        /// BOT SET class for custom fields set
        /// </summary>
        class B_SET
        {
            public Entity target { get; set; }
            public int death { get; set; }
            public bool fire { get; set; }
            public bool temp_fire { get; set; }
            public string wep { get; set; }
        }
        List<B_SET> B_FIELD = new List<B_SET>(18);
        //Dictionary<int, int> BOT_ID = new Dictionary<int, int>();
        List<Entity> BOTs_List = new List<Entity>();
        #endregion

        #region Human side
        /// <summary>
        /// HUMAN PLAYER SET class for custom fields set
        /// </summary>
        class H_SET
        {
            int att = 0;
            public int SIRENCERorHB
            {
                get
                {
                    this.att++;
                    if (this.att > 2) this.att = 0;
                    return this.att;
                }
            }
            public int PERK { get; set; }
            public int AX_WEP { get; set; }
            public bool BY_SUICIDE { get; set; }
            public int LIFE { get; set; }
            public bool RESPAWN { get; set; }
            //public bool CLASS_CHANGED { get; set; }
            //public HudElem WEAPONINFO { get; set; }
            public bool USE_TANK { get; set; }

        }
        List<H_SET> H_FIELD = new List<H_SET>(18);
        List<Entity> human_List = new List<Entity>();
        List<Entity> HUMAN_AXIS_LIST = new List<Entity>();
        #endregion


        #region server side
        /// <summary>
        /// server side dvar
        /// </summary>
        void Server_SetDvar()
        {
            Call("setdvar", "scr_game_playerwaittime", PLAYERWAIT_TIME);
            Call("setdvar", "scr_game_matchstarttime", MATCHSTART_TIME);
            Call("setdvar", "scr_game_allowkillcam", "0");
            Call("setdvar", "scr_infect_timelimit", INFECTED_TIMELIMIT);

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
            string ENTIRE_MAPLIST = "mp_plaza2|mp_mogadishu|mp_bootleg|mp_carbon|mp_dome|mp_exchange|mp_lambeth|mp_hardhat|mp_interchange|mp_alpha|mp_bravo|mp_radar|mp_paris|mp_seatown|mp_underground|mp_village|mp_morningwood|mp_park|mp_overwatch|mp_italy|mp_cement|mp_qadeem|mp_meteora|mp_hillside_ss|mp_restrepo_ss|mp_aground_ss|mp_courtyard_ss|mp_terminal_cls";
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int max = map_list.Count - 1;
            int index = map_list.IndexOf(currentMAP);

            int[] smallMap = { 23, 24, 25, 26, 28, 29, 30 };
            int[] largeMap = { 5, 8, 16, 17, 31 };

            //set player life
            if (PLAYER_LIFE == 0) PLAYER_LIFE = 2;

            //set bot's fire distance
            if (smallMap.Contains(index))
            {
                PLAYER_LIFE += 1;
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

           // Utilities.ExecuteCommand("seta g_password \"0\""); return;

            if (TEST_)
            {
                Utilities.ExecuteCommand("seta g_password \"0\"");
            }
            else
            {
                Utilities.ExecuteCommand("seta g_password \"\"");

                string content = NEXT_MAP + ",INF,1";
                File.WriteAllText(GetPath("admin\\INF.dspl"), content);
            }

        }
        void setTeamName()
        {
            Call("setdvar", "g_TeamName_Allies", TEAMNAME_ALLIES);
            Call("setdvar", "g_TeamName_Axis", TEAMNAME_AXIS);
        }
        string GetPath(string path)
        {
            if (TEST_)
            {
                path = path.Replace("admin\\", "admin\\test\\");
            }
            return path;
        }

        #endregion

        #region client side

        /// <summary>
        /// client side dvar & set notifycommand & give weapon & set HUD & change class
        /// </summary>
        void Client_init_GAME_SET(Entity player)
        {
            #region change class
            player.Notify("menuresponse", "changeclass", "allies_recipe" + rnd.Next(1, 6));
            #endregion

            #region set
            human_List.Add(player);

            //public int PERK { get; set; }
            //public int AX_WEP { get; set; }
            //public bool BY_SUICIDE { get; set; }
            //public int LIFE { get; set; }
            //public bool RESPAWN { get; set; }
            //public bool CLASS_CHANGED { get; set; }
            //public HudElem WEAPONINFO { get; set; }

            H_SET H = H_FIELD[player.EntRef];
            H.LIFE = PLAYER_LIFE;
            H.PERK = 2;
            H.AX_WEP = 0;
            H.BY_SUICIDE = false;
            H.USE_TANK = false;

            #endregion

            #region SetClientDvar

            player.SetClientDvar("lowAmmoWarningNoAmmoColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoAmmoColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningNoReloadColor1", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor2", "0 0 0 0");
            player.SetClientDvar("lowAmmoWarningColor1", "0 0 0 0");
            player.SetClientDvar("cg_drawBreathHint", "0");
            player.SetClientDvar("cg_scoreboardpingtext", "1");
            player.SetClientDvar("cg_brass", "1");
            player.SetClientDvar("waypointIconHeight", "13");
            player.SetClientDvar("waypointIconWidth", "13");

            //player.SetClientDvar("cg_fov", "75");
            //player.SetClientDvar("clientsideeffects", "1");
            //player.SetClientDvar("cl_maxpackets", "60");
            //player.SetClientDvar("com_maxfps", "91");
            //player.SetClientDvar("r_fog", "1");
            //player.SetClientDvar("r_distortion", "1");
            //player.SetClientDvar("r_dlightlimit", "4");
            //player.SetClientDvar("fx_drawclouds", "1");
            //player.SetClientDvar("snaps", "20");

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
                if (!isSurvivor(player)) return;
                var weapon = player.CurrentWeapon;
                if (weapon.Length > 3 && weapon[2] == '5')
                {
                    player.Call("givemaxammo", weapon);
                }


            });

            player.OnNotify("HOLD_CROUCH", ent =>//view scope
            {
                if (!isSurvivor(player))
                {
                    giveOffhandWeapon(player, "throwingknife");
                    return;
                }

                giveAttachScope(player);

            });

            player.OnNotify("HOLD_PRONE", ent =>//attachment silencer heartbeat,
            {
                if (!isSurvivor(player))
                {
                    giveOffhandWeapon(player, "bouncingbetty_mp");
                    return;
                }
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
                if (!isSurvivor(player))
                {
                    giveOffhandWeapon(player, "claymore_mp");
                    return;
                }
                giveOffhandWeapon(player, offhand);
            });

            #region TANK

            Entity TANK = null;
            player.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                if (H.USE_TANK) return;


                string weap = newWeap.ToString();

                //print(weap);
                if (weap == "killstreak_remote_tank_remote_mp")
                {
                    H.USE_TANK = true;
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

                    //print("들어왔다 "+TANK.Name);
                    human_List.Add(TANK);
                    player.Call(32936);//thermalvisionfofoverlayon

                }
            });
            player.OnNotify("end_remote", (Entity ent) =>
            {
                if (H.USE_TANK)
                {
                    H.USE_TANK = false;
                    human_List.Remove(TANK);
                    player.Call(32937);//thermalvisionfofoverlayoff
                }
            });

            #endregion

            #endregion

            #region AlliesHud
            AlliesHud(player, offhand.Replace("_mp", "").ToUpper());
            #endregion

            #region giveweapon
            giveWeaponTo(player, getRandomWeapon());
            AfterDelay(500, () => giveOffhandWeapon(player, offhand));
            #endregion
        }
        #endregion

    }
}
