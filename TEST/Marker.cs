﻿using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    internal class Marker : InfinityBase
    {
        #region Marker
        internal void uavStrikerMarker(Entity player)
        {
            player.GiveWeapon("uav_strike_marker_mp");
            player.SwitchToWeaponImmediate("uav_strike_marker_mp");

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
        internal void airdropMarker(Entity player)
        {
            string marker = "airdrop_sentry_marker_mp";
            player.GiveWeapon(marker);
            player.SwitchToWeaponImmediate(marker);
            player.OnNotify("grenade_fire", (Entity owner, Parameter entity, Parameter weaponName) =>
            {
                if (weaponName.ToString() != marker) return;
                Print("weap : " + weaponName.ToString());
                Entity e = entity.As<Entity>();
                int i = Call<int>(303, "misc/flare_ambient");//loadfx
                if (i < 1) return;
                e.AfterDelay(3000, ee =>
                {
                    Entity Effect = Call<Entity>("spawnFx", i, e.Origin);
                    Call("triggerfx", Effect);
                    ee.AfterDelay(10000, eee =>
                    {
                        Effect.Call("delete");
                    });
                });
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

                int i = Call<int>(303, "misc/flare_ambient");//loadfx
                if (i < 1) return;

                Entity Effect = Call<Entity>("spawnFx", i, player.Origin);
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
                    Print(loc.ToString());
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
