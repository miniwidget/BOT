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
            ADMIN.Call(33445, "SPECT", "centerview");//notifyonplayercommand
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

            if (name == ADMIN_NAME)
            {
                ADMIN = player;
                setADMIN();
            }

            var entref = player.EntRef;
            if (isSurvivor(player))
            {
                print(name + " connected ♥");
                Client_init_GAME_SET(player);
                player.SpawnedPlayer += () => human_spawned(ref player);
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
            Field H = FL[player.EntRef];
            H.LIFE = -2;
            H.AX_WEP = 1;

            player.SetField("sessionteam", "axis");
            HUMAN_LIST.Remove(player);
            HUMAN_AXIS_LIST.Add(player);

            player.AfterDelay(100, x =>
            {
                player.Call("suicide");
                player.Notify("menuresponse", "changeclass", "axis_recipe4");
            });

        }
        void Inf_PlayerDisConnected(Entity player)
        {

            if (HUMAN_LIST.Contains(player))// 봇 타겟리스트에서 접속 끊은 사람 제거
            {
                int i = HUMAN_LIST.IndexOf(player);
                foreach(Field F in FL)
                {
                    if(F.human_target_idx== i)
                    {
                        F.human_target_idx = -1;
                        break;
                    }
                }
                HUMAN_LIST.Remove(player);
            }
            else
            {
                int i = HUMAN_AXIS_LIST.IndexOf(player);
                foreach (Field F in FL)
                {
                    if (F.human_target_idx == i)
                    {
                        F.human_target_idx = -1;
                        break;
                    }
                }
                HUMAN_AXIS_LIST.Remove(player);
            }
        }
    }
}
