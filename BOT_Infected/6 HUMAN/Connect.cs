using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Infected
{
    public partial class Infected
    {
        void setADMIN()
        {
            ADMIN.Call("notifyonplayercommand", "SPECT", "centerview");
            bool spect = false;
            ADMIN.OnNotify("SPECT", a =>
            {
                if (!spect)
                {
                    ADMIN.Call("allowspectateteam", "freelook", true);
                    ADMIN.SetField("sessionstate", "spectator");
                }
                else
                {
                    ADMIN.Call("allowspectateteam", "freelook", false);
                    ADMIN.SetField("sessionstate", "playing");
                }
                spect = !spect;
            });
            if (TEST_)
            {
                ADMIN.Call("thermalvisionfofoverlayon");
                ADMIN.Call("setmovespeedscale", 1.5f);
            }
        }
        void Human_Connected(Entity player)
        {

            string name = player.Name;

            if (player.Name == ADMIN_NAME)
            {
                ADMIN = player;
                setADMIN();
            }

            if (isSurvivor(player))
            {
                if (!HUMAN_CONNECTED_)
                {
                    HUMAN_CONNECTED_ = true;
                }
                print(name + " connected ♥");
                Client_init_GAME_SET(player);
                player.SpawnedPlayer += () => human_spawned(player);
            }
            else
            {
                //Utilities.ExecuteCommand("dropclient " + player.EntRef + " \"Join Next Round please\"");
                AXIS_Connected(player);
            }

        }
        void AXIS_Connected(Entity player)
        {
            print("AXIS connected ☜");
            H_SET H = H_FIELD[player.EntRef];
            H.LIFE = -2;
            H.AX_WEP = 1;

            player.SetField("sessionteam", "axis");
            human_List.Remove(player);
            HUMAN_AXIS_LIST.Add(player);
            player.AfterDelay(100, x =>
            {
                player.Call("suicide");
                player.Notify("menuresponse", "changeclass", "axis_recipe4");
            });

        }
        void Inf_PlayerDisConnected(Entity player)
        {
            if (human_List.Contains(player))// 봇 타겟리스트에서 접속 끊은 사람 제거
            {
                human_List.Remove(player);
            }
            else
            {
                HUMAN_AXIS_LIST.Remove(player);
            }
            if (human_List.Count == 0 && !GAME_ENDED_) HUMAN_CONNECTED_ = false;
        }

    }
}
