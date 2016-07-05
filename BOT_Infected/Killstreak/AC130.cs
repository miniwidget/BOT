using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    class AC130 : Inf
    {
        //private string getKillstreakWeapon(string streakName)
        //{
        //    string empty = string.Empty;
        //    Parameter[] parameterArray = new Parameter[] { "mp/killstreakTable.csv", 1, streakName, 12 };
        //    empty = base.Call<string>("tableLookup", parameterArray);
        //    Log.Write(LogLevel.Info, string.Concat("Killstreak weapon: ", empty));
        //    return empty;
        //}
        Vector3 GetMinimapOrigin()
        {
            return Call<Entity>(365, "minimap_corner", "targetname").Origin;//getEnt
        }
        int OWNER_ENTREF;
        internal void start(Entity player)
        {
            if (USE_AC130)
            {
                if (OWNER_ENTREF == player.EntRef) return;

                player.Call(33344, "ANOTHER USER IS USING AC130. WAIT");
                return;
            }
            OWNER_ENTREF = player.EntRef;
            player.Call(33503);//disableoffhandweapons
            string curr = player.CurrentWeapon;
            player.GiveWeapon("killstreak_ac130_mp");
            player.SwitchToWeapon("killstreak_ac130_mp");
            //player.Notify("using_remote");

            player.AfterDelay(2000, p => initAC130(p, curr));
        }
        bool USE_AC130;
        int AC_COUNT;
        void initAC130(Entity player, string lastWP)
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

            //float rampupDegrees = 10;
            //float rotateTime = (70 / 360) * rampupDegrees;
            //level_ac130.Call(33408, level_ac130.GetField<Vector3>("angles").Y + rampupDegrees, rotateTime, rotateTime, 0);//rotateyaw

            Infected.H_SET H = Infected.H_FIELD[player.EntRef];

            player.Call(33306, "ac130Ammo105mm", 1);//SetPlayerData
            player.Call(33306, "ac130Ammo40mm", 4);
            player.Call(33306, "ac130Ammo25mm", 20);

            H.AC130_ON_USE = true;
            USE_AC130 = true;
            Common.StartOrEndThermal(player, true);
            if (!H.AC130_NOTIFIED)
            {
                H.AC130_NOTIFIED = true;

                player.OnNotify("weapon_fired", (p, weapon) =>
                {
                    if (!USE_AC130) return;
                    if (!H.AC130_ON_USE) return;

                    string w = weapon.ToString();
                    if (!w.StartsWith("ac")) return;

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
            }

            AC_COUNT++;
            int ac = AC_COUNT;
            level_ac130.OnInterval(70, ll =>
            {
                if (!USE_AC130|| ac!=AC_COUNT) return false;
                level_ac130.Call(33408, 360, 70);//rotateyaw
                return true;
            });
            int cloud = Call<int>(303, "misc/ac130_cloud");//loadfx
            level_ac130.OnInterval(6000, ll =>
            {
                AC_COUNT++;
                if (!USE_AC130 || ac != AC_COUNT) return false;
                Call(310, cloud, level_ac130, "tag_player", player);//playfxontagforclients
                return true;
            });
            player.AfterDelay(60000, p =>
            {
                OWNER_ENTREF = -1;
                USE_AC130 = false;
                H.AC130_ON_USE = false;

                player.Call("unlink");//unlink
                player.Call("setplayerangles", Common.ZERO);//setplayerangles

                player.TakeWeapon("killstreak_ac130_mp");
                player.TakeWeapon("ac130_105mm_mp");
                player.TakeWeapon("ac130_40mm_mp");
                player.TakeWeapon("ac130_25mm_mp");

                ac130model.Call("delete");//delete
                objModel.Call("delete");
                level_ac130.Call("delete");

                player.Call("enableOffhandWeapons");//enableOffhandWeapons
                player.SwitchToWeapon(lastWP);
                player.Call("freezeControls", false);//freezeControls
                player.Notify("stopped_using_remote");
                Common.StartOrEndThermal(player, false);
            });
        }
        void thermal()
        {

        }
    }
}
