using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST.NONE
{
    class ToDo
    {
        #region todo
        //Entity JJUG_BOT,HELLI_BOT;

        /*
        1번 저거넛 봇t
        2번 RPG 봇
        3번 리엇 봇
        4번 인간


        1. OK!!! session team / isAlive 상태 확인

        2. OK 사람이 죽었을 때, 탄알 3개만 줌

        3. 봇 클래스 변경 가능한지 확인
        
        4. OK survivor 킬스트렉 어설트 드론 을 공격용 헬기로 변경

        5. dead man 에게 헬기런쳐 지급

        6. OK 메뉴 총기 리스트 변경
        gameOpt commonOption.allowCustomClasses "1"
        gameOpt commonOption.forceRespawn "0"

                bool firstSpawn_HelliBOT = true;
        bool secondSpawn;

                void spawnd_Helli_BOT()
        {
            if (ADMIN == null) return;
            if (HELLI_BOT == null) return;
            if (!secondSpawn)
            {
                secondSpawn = true;
                return;
            }
            HELLI_BOT.Call("setmovespeedscale", 0);
            if (VEHICLE != null) VEHICLE.Call(32928);
            var o = HELLI_BOT.Origin; o.Z += 1000;
            string minimap_model = "attack_littlebird_mp";
            string realModel = "vehicle_little_bird_armed";
            VEHICLE = Call<Entity>("spawnhelicopter", HELLI_BOT, o, HELLI_BOT.GetField<Vector3>("angles"), minimap_model, realModel);
            //HELLI_BOT.Call("linkto", VEHICLE, "tag_origin", TAG_ORIGIN[0], TAG_ORIGIN[1]);

            Entity turret = Call<Entity>(19, "misc_turret", HELLI_BOT.Origin, "littlebird_guard_minigun_mp", false);
            turret.Call("linkto", VEHICLE, "tag_origin", new Vector3(30, 30, 0), new Vector3(0, 0, 0));
            turret.Call(32929, "mp_remote_turret");//setmodel mp_remote_turret
            turret.Call(32942);//MakeUnusable
            turret.Call(33052);//maketurretsolid
            turret.Call(33417, true);//setcandamage

            HELLI_BOT.Call("remotecontrolvehicle", VEHICLE);//remotecontrolvehicle
           AfterDelay(1000,()=> HELLI_BOT.Call("remotecontrolvehicle", turret));//remotecontrolvehicle

            if (firstSpawn_HelliBOT)
            {
                firstSpawn_HelliBOT = false;
                //bool control = true;
                OnInterval(250, () =>
                {
                    var ho = ADMIN.Origin; ho.Z -= 50;

                    Vector3 temp_ = Call<Vector3>(247, ho - HELLI_BOT.Origin);//vectortoangles
                    HELLI_BOT.Call(33531, temp_);//SetPlayerAngles

                    return true;
                });
            }
            //player.AfterDelay(60000, e =>
            //{
            //    player.Call(32843);//unlink
            //    player.Call(33257);//remotecontrolvehicleoff
            //    player.Call(32980);//remotecontrolturretoff
            //    turret.Call(32928);//delete
            //    lbExplode(LB);
            //    Restore(player, false);
            //});
        }
                Entity bot = null;
            bot.SpawnedPlayer += () => spawned_bot(bot);
            if (TEST) return;
            bot.AfterDelay(10000, p =>
            {
                if (bot.GetField<string>("sessionteam") == "axis")
                {
                    p.AfterDelay(5000, x => bot.Call("suicide"));
                }
                else
                {
                    if (!FINAL_BOT_FOUND && !HUMAN_CONNECTED_)
                    {
                        FINAL_BOT_FOUND = true;
                        return;
                    }
                    bot.Call("suicide");
                }

            });
                            //b.Call("setorigin", o);
                //b.Call("setmovespeedscale", 1);//setmovespeedscale


                    string ENTIRE_MAPLIST = "mp_aground_ss|mp_alpha|mp_boardwalk|mp_bootleg|mp_bravo|mp_burn_ss|mp_carbon|mp_cement|mp_courtyard_ss|mp_crosswalk_ss|mp_dome|mp_exchange|mp_hardhat|mp_hillside_ss|mp_interchange|mp_italy|mp_lambeth|mp_meteora|mp_moab|mp_mogadishu|mp_morningwood|mp_nola|mp_overwatch|mp_paris|mp_park|mp_plaza2|mp_qadeem|mp_radar|mp_restrepo_ss|mp_roughneck|mp_seatown|mp_shipbreaker|mp_six_ss|mp_terminal_cls|mp_underground|mp_village";

            MAP_NAME = Call<string>("getdvar", "mapname");
            var map_list = ENTIRE_MAPLIST.Split('|').ToList();
            int index = map_list.IndexOf(MAP_NAME);
            if (index == 35) index = 0;
            NEXT_MAP = map_list[index+1];
            Call("setdvar", "sv_nextmap", NEXT_MAP);
            print("맵 인덱스 : " + index + " 다음 맵 : " + NEXT_MAP);

        */
        #endregion

    }
}
