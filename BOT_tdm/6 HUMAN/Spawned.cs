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

        void human_spawned(Entity player)//LIFE 1 or 2
        {
            H_SET H = H_FIELD[player.EntRef];
            H.PERK = 2;
            H.USE_TANK = false;

            string team = player.GetField<string>("sessionteam");

            if (H.TEAM != null)
            {
                if (H.TEAM != team)
                {
                    if (team == "axis")
                    {
                        H_ALLIES_LIST.Remove(player);
                        H_AXIS_LIST.Add(player);
                    }
                    else
                    {
                        H_ALLIES_LIST.Add(player);
                        H_AXIS_LIST.Remove(player);
                    }
                }
            }
            else
            {
                H.TEAM = team;
                if (team == "axis") H_AXIS_LIST.Add(player);
                else H_ALLIES_LIST.Add(player);
            }
        }
    }
}
