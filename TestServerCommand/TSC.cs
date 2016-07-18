using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServerCommand
{
    public class TSC : BaseScript
    {
        public TSC()
        {
            OnServerCommand("/", (string[] texts) =>
            {
                string key = texts[1].ToLower();
                switch (key)
                {
                    case "fr": Script("fast_Restart inf.dll", false); return;
                    case "mr": Script("map_rotate inf.dll", false); return;

                    case "ulinf": Script("unloadscript inf.dll", true); return;
                    case "linf": Script("loadscript inf.dll", true); return;
                    case "ultdm": Script("unloadscript tdm.dll", true); return;
                    case "ltdm": Script("loadscript tdm.dll", true); return;

                    //case "dialog":
                    //    {
                    //        string res = null;
                    //        for (int i = 0; i < 1024; i++)
                    //        {
                    //            res += " " + Call<string>("tableLookup", "mp/killstreakTable.csv", 0, i, 8);
                    //        }

                    //        Log.Write(LogLevel.None, res);
                    //    }
                    //    return;
                }
            });

        }
        void Script(string command1, bool fr)
        {
            Utilities.ExecuteCommand(command1);
            if (fr) Utilities.ExecuteCommand("fast_restart");
        }
    }
}
