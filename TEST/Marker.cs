using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    internal class Marker
    {
        TReturn Call<TReturn>(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }

        void Call(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        void Call(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        #region Marker
        internal void uavStrikerMarker()
        {
            Entity player = test.ADMIN;
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
                    Vector3
                    startPos = missile.Origin;
                    startPos.Z += 6000;

                    Call("magicbullet", "ac130_105mm_mp", missile.Origin + startPos, missile.Origin, player);


                    int loadfx = Call<int>("loadfx", "misc/laser_glow");
                    Entity Effect = Call<Entity>("spawnFx", loadfx, missile.Origin);

                    Call("triggerfx", Effect);
                    Effect.AfterDelay(500, h => Effect.Call("delete"));
                    //missile = null;
                });
            });
        }
        internal void airdropMarker()
        {
            Entity player = test.ADMIN;
            player.GiveWeapon("airdrop_marker_mp");
            player.SwitchToWeaponImmediate("airdrop_marker_mp");
            player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
            {
                if (weaponName.ToString() != "airdrop_marker_mp") return;
                test.Print("weap : " + weaponName.ToString());
                Entity e = entity.As<Entity>();
                e.AfterDelay(3000, ee =>
                {
                    Entity Effect = Call<Entity>("spawnFx", 131, e.Origin);
                    Call("triggerfx", Effect);
                    ee.AfterDelay(10000, eee =>
                    {
                        Effect.Call("delete");
                    });
                });
            });

        }
        internal void test_(string ss)
        {
            //smoke/signal_smoke_airdrop_30sec
            //misc/laser_glow
            Entity player = test.ADMIN;
            player.AfterDelay(200, p =>//
            {
                int loadfx = Call<int>("loadfx", ss);
                Entity Effect = Call<Entity>("spawnFx", loadfx, player.Origin+ new Vector3(0,0,60));
                Call("triggerfx", Effect);
            });
            
        }
        internal void TI()
        {
            Entity player = test.ADMIN;
            player.GiveWeapon("flare_mp");
            player.SwitchToWeaponImmediate("flare_mp");
            player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
            {

                Call(431, 1, "active"); // objective_add
                Call(435, 1, player.Origin); // objective_position
                Call(434, 1, "compass_waypoint_bomb"); // objective_icon

                Entity Effect = Call<Entity>("spawnFx", 131, player.Origin);
                Call("triggerfx", Effect);
                player.AfterDelay(10000, p =>
                {
                    Effect.Call("delete");
                    Call(431, 1, "inactive");
                });
            });
        }

        bool NOTIFIED = false;
        internal void _beginLocationSelection(Entity player, string streakName, string selectorType, bool directionality, int size)
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
                    test.Print(loc.ToString());
                    player.Call("endlocationselection", true);
                    player.Call("setblurforplayer", 0f, 0f);
                });
            }
            catch
            {

            }

        }

        #endregion

    }
}
