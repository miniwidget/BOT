using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace TEST
{
    public partial class test
    {
        Entity ADMIN;
        Random rnd = new Random();
        Entity VEHICLE;
        Vector3[] TAG_ORIGIN = new Vector3[] { new Vector3(0, 0, 200), new Vector3(0, 0, 0) };


        void execute(string command, bool restart)
        {
            ClearBOTsInPlayers();
            AfterDelay(1000, () =>
            {
                Utilities.ExecuteCommand(command);
                if (restart)
                {
                    Utilities.ExecuteCommand("fast_restart");
                }
            });
     
        }
        void print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
        void setNotify()
        {
            HudElem INFO1 = HudElem.CreateServerFontString("hudbig", 0.8f);
            INFO1.X = 240;
            INFO1.Y = 15;
            INFO1.Alpha = 1f;
            INFO1.HideWhenInMenu = true;
            INFO1.SetText("TEST SCRIPT LOADED");
        }

    }
}
