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

            //player.Notify("using_remote");
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

            Entity killCamEnt = Call<Entity>("Spawn", "script_model", airShip.Origin+ new Vector3(0, 0, -200f));
            killCamEnt.Call("LinkTo", airShip, "tag_light_belly", new Vector3(0,0,-200f));

            //player.GiveWeapon("heli_remote_mp");
            //player.SwitchToWeapon("heli_remote_mp");

            //player.Call("CameraLinkTo", new Parameter[] { killCamEnt,"tag_origin"});
            //player.Call("PlayerLinkWeaponViewToDelta", airShip, "tag_player", 1.0f, 90, 90, 90, 90);
            //player.Call("setPlayerAngles", airShip.Call<Vector3>("getTagAngles", "tag_player"));
            //player.Call("unlink");
            //player.GiveWeapon("ac130_105mm_mp");
            //player.GiveWeapon("ac130_40mm_mp");
            //player.GiveWeapon("ac130_25mm_mp");
            //player.SwitchToWeapon("ac130_105mm_mp");
            //player.Call("playerlinktodelta", airShip, "tag_player", 1.0f, 90, 90, 90, 90);
            //player.Call("linkto", airShip);
            //player.Call("linkto", killCamEnt);
            player.Call("remotecontrolvehicle", airShip);
            //player.Call("drivevehicleandcontrolturret", airShip);


        }


    }
}
