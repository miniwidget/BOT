using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class AC130 : InfinityBase
    {
        public AC130()
        {
            Entity player = test.ADMIN;
            initAC130(player);
        }

        Vector3 GetMinimapOrigin()
        {
            return Call<Entity>(365, "minimap_corner", "targetname").Origin;//getEnt
        }

        bool USE_AC130;
        void initAC130(Entity player)
        {
            /*실제 플레이어가 총쏘는 포인트*/
            Entity level_ac130 = Call<Entity>(85, "script_model", GetMinimapOrigin());//spawn
            level_ac130.Call(32929, "c130_zoomrig");//setModel
            level_ac130.SetField("angles", new Vector3(0, 115, 0));//angles horizontal 115 degree
            //level_ac130.owner = undefined;
            //level_ac130.thermal_vision = "ac130_thermal_mp";
            //level_ac130.enhanced_vision = "ac130_enhanced_mp";

            /*스크립트 모델*/
            Entity ac130model = Call<Entity>(85, "script_model", level_ac130.Call<Vector3>("getTagOrigin", "tag_player"));//spawn
            ac130model.Call(32929, "vehicle_ac130_coop");//setModel
            ac130model.Call(32841, level_ac130, "tag_player", new Vector3(0, 80, 32), new Vector3(-25, 0, 0));//linkTo

            /*미니맵 모델*/
            Entity objModel = Call<Entity>(367, player, "script_model", player.Origin + new Vector3(0, 0, 30000), "compass_objpoint_ac130_friendly", "compass_objpoint_ac130_enemy");//spawnPlane
            objModel.Call(33416);//notSolid
            objModel.Call(32841, level_ac130, "tag_player", new Vector3(0, 80, 32), new Vector3(0, -90, 0));//linkTo

            /*if ( getDvarInt( "camera_thirdPerson" ) ) player setThirdPersonDOF( false );*/

            player.GiveWeapon("ac130_105mm_mp");
            player.GiveWeapon("ac130_40mm_mp");
            player.GiveWeapon("ac130_25mm_mp");
            player.SwitchToWeapon("ac130_105mm_mp");

            /*player thread overlay( player );*/

            /*player thread attachPlayer( player );*/
            player.Call(32887, level_ac130, "tag_player", 1, 90, 90, 90, 90);//PlayerLinkWeaponviewToDelta
            player.Call(33531, level_ac130.Call<Vector3>("getTagAngles", "tag_player"));//setPlayerAngles

            /* player thread changeWeapons(); */
            /* player thread weaponFiredThread();*/
            /* player thread context_Sensative_Dialog();*/
            /* player thread shotFired();*/
            /* player thread clouds();*/

            //float rampupDegrees = 10;
            //float rotateTime = (70 / 360) * rampupDegrees;
            //level_ac130.Call(33408, level_ac130.GetField<Vector3>("angles").Y + rampupDegrees, rotateTime, rotateTime, 0);//rotateyaw

            bool useAC130 = false;
            USE_AC130 = true;

            player.Call(33306, "ac130Ammo105mm", 1);//SetPlayerData
            player.Call(33306, "ac130Ammo40mm", 4);
            player.Call(33306, "ac130Ammo25mm", 20);

            player.OnNotify("weapon_fired", (p, weapon) =>
            {
                if (!USE_AC130) return;
                string w = weapon.ToString();
                if (w.StartsWith("ac")) useAC130 = true;
                else if (useAC130) useAC130 = false;

                if (!useAC130) return;

                int ammoCount = 0;
                if (w == "ac130_105mm_mp")
                {
                    Call("earthquake", 0.2f, 1, ac130model.Origin, 1000);
                    ammoCount = player.Call<int>(33470, w);//"GetWeaponAmmoClip"
                    player.Call(33306, "ac130Ammo105mm", ammoCount);
                }
                else if (w == "ac130_40mm_mp")
                {
                    Call("earthquake", 0.1f, 0.5f, ac130model.Origin, 1000);
                    ammoCount = player.Call<int>(33470, w);
                    player.Call(33306, "ac130Ammo40mm", ammoCount);
                }
                else if (w == "ac130_25mm_mp")
                {
                    ammoCount = player.Call<int>(33470, w);
                    player.Call(33306, "ac130Ammo25mm", ammoCount);
                }

                if (ammoCount == 0)
                {
                    if (w == "ac130_105mm_mp")
                    {
                        p.AfterDelay(5000, x =>
                        {
                            player.Call(33468, w, 1);//"setweaponAmmoClip"
                            player.Call(33306, "ac130Ammo105mm", 1);
                        });
                    }
                    else if (w == "ac130_40mm_mp")
                    {
                        p.AfterDelay(3000, x =>
                        {
                            player.Call(33468, w, 4);
                            player.Call(33306, "ac130Ammo40mm", 4);
                        });
                    }
                    else
                    {
                        p.AfterDelay(1500, x =>
                        {
                            player.Call(33468, w, 20);
                            player.Call(33306, "ac130Ammo25mm", 20);
                        });
                    }

                }

            });
            level_ac130.OnInterval(70, ll =>
            {
                if (!USE_AC130) return false;
                level_ac130.Call(33408, 360, 70);//rotateyaw
                return true;
            });
            int cloud = Call<int>(303, "misc/ac130_cloud");//loadfx
            level_ac130.OnInterval(6000, ll =>
            {
                if (!USE_AC130) return false;
                Call(310 ,cloud, level_ac130, "tag_player", player);//playfxontagforclients
                return true;
            });

        }

    }
}
