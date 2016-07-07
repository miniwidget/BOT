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

            if (H.Axis)
            {
                if (H_ALLIES_LIST.Contains(player)) H_ALLIES_LIST.Remove(player);
                if (!H_AXIS_LIST.Contains(player)) H_AXIS_LIST.Add(player);
            }
            else
            {
                if (!H_ALLIES_LIST.Contains(player)) H_ALLIES_LIST.Add(player);
                if (H_AXIS_LIST.Contains(player)) H_AXIS_LIST.Remove(player);
            }
        }
    }
}
