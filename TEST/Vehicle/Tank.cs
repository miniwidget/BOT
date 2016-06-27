using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class Tank
    {
        #region infinity script
        TReturn Call<TReturn>(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }
        TReturn Call<TReturn>(int func, params Parameter[] parameters)
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
        #endregion

        Vector3 ZERO = new Vector3();
        Entity remoteTank;


        internal void SetTank()
        {
            Entity player = test.ADMIN;

            remoteTank = Call<Entity>("SpawnVehicle", "vehicle_ugv_talon_mp", "remote_tank", "remote_ugv_mp", player.Origin, ZERO, player);
            Vector3 turretAttachTagOrigin = remoteTank.Call<Vector3>("GetTagOrigin", "tag_turret_attach");
            Entity rmt1 = Call<Entity>("SpawnTurret", "misc_turret", turretAttachTagOrigin, "remote_turret_mp", false);//ugv_turret_mp
            rmt1.Call("SetModel", "mp_remote_turret");//vehicle_ugv_talon_gun_mp
            rmt1.Call(32841, remoteTank, "tag_turret_attach", new Vector3(-50, 0, 75f), ZERO);
            rmt1.Call(33084, 180f);//SetLeftArc
            rmt1.Call(33083, 180f);//SetRightArc
            rmt1.Call(33086, 180f);//SetBottomArc

            Entity rmt2 = Call<Entity>("SpawnTurret", "misc_turret", turretAttachTagOrigin, "remote_turret_mp",false);
            rmt2.Call("SetModel", "mp_remote_turret");
            rmt2.Call(32841, remoteTank, "tag_turret_attach", new Vector3(50, 0, 75f), ZERO);
            rmt2.Call(33084, 180f);//SetLeftArc
            rmt2.Call(33083, 180f);//SetRightArc
            rmt2.Call(33086, 180f);//SetBottomArc

            setNotify();

        }
        void setNotify()
        {
            Entity player = test.ADMIN;
            player.Call(33445, "ACTIVATE", "+activate");//"notifyonplayercommand"
            player.OnNotify("ACTIVATE", ent =>
            {
          
                ent.AfterDelay(250, e =>
                {
                    if (ent.Call<int>(33539) == 1)//isusingturret
                    {
                        //player.Call("giveweapon", "killstreak_remote_turret_remote_mp");// mortar_remote_zoom_mp
                        //player.SwitchToWeaponImmediate("killstreak_remote_turret_remote_mp");
                        //player.Call("remotecontrolturret", rmTurret);
                        player.Call(33256, remoteTank);//remotecontrolvehicle  
                       
                        //player.Call(32792, "prop_flag_neutral", "tag_shield_back", true);//attachshieldmodel
                        //player.Call(32792, "weapon_riot_shield_mp", "tag_shield_back", true);//attachshieldmodel
                        //player.Call(32792, "weapon_riot_shield_mp", "tag_player", true);//attachshieldmodel
                    }
                    else
                    {
                        player.Call(32843);//unlink
                        player.Call(33257);//remotecontrolvehicleoff
                        //player.Call(32980);//remotecontrolturretoff
                        player.Call("setorigin", remoteTank.Origin);
                        player.Call(33531, ZERO);
                    }
                });


            });

            Entity t = Call<Entity>("getent", "vehicle_ugv_talon_mp", "model");
            if (t != null) player.Call(33344, "FOUND " + t.EntRef);

        }

        /*
         
        tank_FireMissiles( remoteTank ) // self == owner
        {
	        self endon ( "disconnect" );
	        self endon ( "end_remote" );
	        level endon ( "game_ended" );
	        remoteTank endon ( "death" );
		
	        rocketNum = 0;

	        while ( true )
	        {
		        if ( self FragButtonPressed() )
		        {
			        tagOrigin = remoteTank.mgTurret.origin;
			        tagAngles = remoteTank.mgTurret.angles;
			        switch( rocketNum )
			        {
			        case 0:
				        tagOrigin = remoteTank.mgTurret GetTagOrigin( "tag_missile1" );
				        tagAngles = remoteTank.mgTurret GetTagAngles( "tag_player" );
				        break;
			        case 1:
				        tagOrigin = remoteTank.mgTurret GetTagOrigin( "tag_missile2" );
				        tagAngles = remoteTank.mgTurret GetTagAngles( "tag_player" );
				        break;
			        }
			
			        remoteTank PlaySound( "talon_missile_fire" );
			        self PlayLocalSound( "talon_missile_fire_plr" );
			
			        destPoint = tagOrigin + ( AnglesToForward( tagAngles ) * 100 );
			        rocket = MagicBullet( level.tankSettings[ remoteTank.tankType ].missileInfo, tagOrigin, destPoint, self );
			        //rocket Missile_SetTargetEnt( remoteTank.samTargetEnt );
			        //rocket Missile_SetFlightmodeDirect();
			
			        rocketNum = ( rocketNum + 1 ) % 2;
			
			        self SetPlayerData( "ugvMissile", 0 );

			        wait( 5.0 );

			        remoteTank PlaySound( "talon_rocket_reload" );
			        self PlayLocalSound( "talon_rocket_reload_plr" );
			
			        self SetPlayerData( "ugvMissile", 1 );
		        }	
		        else
			        wait( 0.05 );							
	        }
        }
         tank_blinkyLightAntenna() // self == tank
        {
	        self endon( "death" );

	        while ( true )
	        {
		        PlayFXOnTag( getfx( "remote_tank_antenna_light_mp" ), self.mgTurret, "tag_headlight_right" );
		        wait( 1.0 );
		        StopFXOnTag( getfx( "remote_tank_antenna_light_mp" ), self.mgTurret, "tag_headlight_right" );
	        }
        }

        tank_blinkyLightCamera() // self == tank
        {
	        self endon( "death" );

	        while ( true )
	        {
		        PlayFXOnTag( getfx( "remote_tank_camera_light_mp" ), self.mgTurret, "tag_tail_light_right" );
		        wait( 2.0 );
		        StopFXOnTag( getfx( "remote_tank_camera_light_mp" ), self.mgTurret, "tag_tail_light_right" );
	        }
        }
         */
    }
}
