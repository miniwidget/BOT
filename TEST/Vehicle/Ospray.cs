using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class Ospray : InfinityBase
    {

        public Ospray()
        {
            Call(42, "testClients_doCrouch", 1);
            Call(42, "testClients_doMove", 0);
            Call(42, "testClients_doAttack", 0);

            Entity player = test.ADMIN;

            player.Notify("using_remote");
            initOspray(player);
            //player.AfterDelay(2000, p =>
            //{
                
            //});
        }
        Entity airShip;
        void initOspray(Entity player)
        {
            Vector3 pathStart = player.Origin + new Vector3(0, 0, 500), angle = new Vector3();

             airShip = Call<Entity>("spawnHelicopter", player, pathStart, angle, "osprey_player_mp", "vehicle_v22_osprey_body_mp");
            //airShip.Call("VehicleTurretControlOn", player); // not found function 
            player.GiveWeapon("heli_remote_mp");
            player.SwitchToWeapon("heli_remote_mp");

            player.Call("PlayerLinkWeaponViewToDelta", airShip, "tag_player", 1.0f, 90, 90, 90, 90);
            player.Call("setPlayerAngles", airShip.Call<Vector3>("getTagAngles", "tag_player"));

        }


    }
}
