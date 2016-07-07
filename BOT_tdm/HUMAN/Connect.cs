using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {

        void Human_Connected(Entity player)
        {
            string name = player.Name;

            if (player.Name == ADMIN_NAME)
            {
                ADMIN = player;
                setADMIN();
            }

            if (!HUMAN_CONNECTED_) HUMAN_CONNECTED_ = true;
            Print(name + " connected ♥");
            Client_init_GAME_SET(player);
            
        }
        void Tdm_PlayerDisConnected(Entity player)
        {
            H_ALLIES_LIST.Remove(player);
            H_AXIS_LIST.Remove(player);
            human_List.Remove(player);
            if (human_List.Count == 0 ) HUMAN_CONNECTED_ = false;
        }

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

    }
}
