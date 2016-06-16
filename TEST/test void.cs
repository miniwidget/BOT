using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace TEST
{
    public partial class test
    {
        public override void OnSay(Entity player, string name, string text)
        {
            if (ADMIN == null) getADMIN();
            if (text == "tank")
            {
                setTank();
                Utilities.RawSayAll("tank");
            }
            if (text.StartsWith("fx"))
            {
                print("yes");
                LoadFX(text.Split(' ')[1]);
            }
            else if (text.StartsWith("lfx"))
            {
                getLoadFXInt(text.Split(' ')[1]);
            }
        }
        #region Marker
        void uavStrikerMarker()
        {
            Entity player = ADMIN;
            player.GiveWeapon("uav_strike_marker_mp");
            player.SwitchToWeaponImmediate("uav_strike_marker_mp");

            //self waittill( "missile_fire", missile, weaponName );

            player.OnNotify("missile_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
            {
                string wep = weaponName.As<string>();
                if (wep != "uav_strike_marker_mp") return;
                Entity missile = entity.As<Entity>();
                missile.OnNotify("death", (g) =>
                {
                    Vector3 startPos = missile.Origin; startPoint.Z += 6000;

                    Call("magicbullet", "ac130_105mm_mp", missile.Origin + startPos, missile.Origin, player);
                    int loadfx = Call<int>("loadfx", "misc/laser_glow");
                    Entity Effect = Call<Entity>("spawnFx", loadfx, missile.Origin);
                    Call("triggerfx", Effect);
                    Effect.AfterDelay(500, h => Effect.Call("delete"));
                    //missile = null;
                });
            });
        }
        void airdropMarker()
        {
            Entity player = ADMIN;
            player.GiveWeapon("airdrop_marker_mp");
            player.SwitchToWeaponImmediate("airdrop_marker_mp");
            player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
            {
                if (weaponName.ToString() != "airdrop_marker_mp") return;
                print("weap : " + weaponName.ToString());
                try
                {
                    Entity e = entity.As<Entity>();
                    AfterDelay(3000, () =>
                    {
                        Entity Effect = Call<Entity>("spawnFx", 131, e.Origin);
                        Call("triggerfx", Effect);
                        AfterDelay(10000, () =>
                        {
                            Effect.Call("delete");
                        });
                    });
                }
                catch
                {
                    print("에러");
                }
            });

        }

        void TI()
        {
            Entity player = ADMIN;
            player.GiveWeapon("flare_mp");
            player.SwitchToWeaponImmediate("flare_mp");
            player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
            {

                Call(431, 1, "active"); // objective_add
                Call(435, 1, player.Origin); // objective_position
                Call(434, 1, "compass_waypoint_bomb"); // objective_icon

                Entity Effect = Call<Entity>("spawnFx", 131, player.Origin);
                Call("triggerfx", Effect);
                AfterDelay(10000, () =>
                {
                    Effect.Call("delete");
                    Call(431, 1, "inactive");
                });
            });
        }

        bool NOTIFIED = false;
        void _beginLocationSelection(Entity player, string streakName, string selectorType, bool directionality, int size)
        {
            player.SetClientDvar("ui_selecting_location", "1");
            player.Call("beginlocationselection", selectorType, directionality, size);
            try
            {
                // player.SetField("selectingLocation", true);
                player.Call("setblurforplayer", 5f, 0f);

                //self waittill( "confirm_location", location );
                if (NOTIFIED) return;
                bool entered = false;
                player.OnNotify("confirm_location", (Entity owner, Parameter a, Parameter b) =>
                {
                    if (entered) return;
                    else entered = true;

                    Vector3 loc = a.As<Vector3>();
                    print(loc.ToString());
                    player.Call("endlocationselection", true);
                    player.Call("setblurforplayer", 0f, 0f);
                });
            }
            catch
            {

            }

        }

        #endregion

        #region FX
        void getLoadFXInt(string s)
        {
            //smoke/smoke_grenade_11sec_mp
            try
            {

                //int i = 0;
                //string a = null;
                //while (i < 31)
                //{
                //    i++;
                //    int x = Call<int>("loadfx", "smoke/signal_smoke_airdrop_" + i.ToString() + "sec");
                //    a += " " + x.ToString();
                //}
                //print(a);
                //i = 0;
                //a = null;
                //while (i < 31)
                //{
                //    i++;
                //    int x = Call<int>("loadfx", "smoke/signal_smoke_airdrop_" + i.ToString() + "sec_mp");
                //    a += " " + x.ToString();
                //}
                //print(a);
                //int loadfx = Call<int>("loadfx", s);
                //print("FX INT :  " + );
            }
            catch
            {

            }

        }
        void LoadFX(string s)
        {
            try
            {
                int i = int.Parse(s);

                AfterDelay(100, () =>
                {
                    Call("PlayFX", i, ADMIN.Origin);
                    print("dfdf");
                });

            }
            catch
            {
                print("???");
            }

        }
        #endregion

        void fc()
        {
            var o = ADMIN.Origin;
            foreach (Entity p in Players)
            {
                if (p == null) continue;
                if (ADMIN != null && p == ADMIN) continue;
                p.Call("setorigin", o);
                //p.Call("freezecontrols", true);
                p.Call("setmovespeedscale", 0f);
            }
        }
        void giveWeapon(Entity player)
        {
            player.Call("freezecontrols", true);
            AfterDelay(500, () =>
            {
                //player.GiveWeapon("iw5_fmg9_mp_akimbo");
                //player.SwitchToWeaponImmediate("iw5_fmg9_mp_akimbo");
            });

        }
        void spawndBot()
        {
            Entity b = Utilities.AddTestClient();
            if (b == null) return;
            b.SpawnedPlayer += () => testClientSpawned(b);

        }
        void testClientSpawned(Entity bot)
        {
            print(bot.Name + " Connected");
            //giveWeapon(bot);
            bot.Call("setorigin", ADMIN.Origin);
        }

        List<Entity> BOTs_List = new List<Entity>();
        void KickBOTsAll()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);

                if (ent == null) continue;
                if (ent.Call<string>("getguid").Contains("bot"))
                {
                    Call("kick", i);
                }
            }
        }
        bool ClearBOTsInPlayers()
        {
            if (Players.Count > 0)//when fast_restart executed, remove pre allocated bots in Infinityscript
            {
                foreach (Entity p in Players)
                {
                    if (p.Name == "") BOTs_List.Add(p);
                }
                foreach (Entity p in BOTs_List)
                {
                    Players.Remove(p);
                }
                BOTs_List.Clear();
            }
            return true;
        }
        void sound(string s)
        {
            print("value" + s);
            string value = null;
            value = Call<string>("tableLookup", "mp/factionTable.csv", 0, s, 7);
            print("value" + value);

            ADMIN.Call("playlocalsound", s);
        }
        void tableValue(string i)
        {
            print("typed: " + i);
            string value = null;
            value = Call<string>("tableLookup", "mp/killstreakTable.csv", 0, i, 9);
            print("result" + value);
        }

        void setTank()
        {
            ADMIN.OnNotify("weapon_change", (Entity ent, Parameter newWeap) =>
            {
                //36
                string weap = newWeap.ToString();
                print(weap);
                if(weap == "killstreak_remote_tank_remote_mp")
                {
                    bool found = false;
                    Entity Ent = null;
                    for (int i = 0; i < 2048; i++)
                    {
                        Ent = Entity.GetEntity(i);
                        if (Ent == null) continue;
                        var model = Ent.GetField<string>("model");
                        if (model=="vehicle_ugv_talon_mp")
                        {
                            print("찾았다 " + i);
                            found = true;
                            break;
                        }


                    }
                    if (!found) return;
                    Ent.OnNotify("death", (Entity tank) =>
                    {

                    });
                }
            });
            ADMIN.OnNotify("joined_team", (Entity ent) => {

                print("joined_team");
            });
        }
    }
}
