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
                if (key == "ulinf")
                {
                    Utilities.ExecuteCommand("unloadscript inf.dll");
                    Utilities.ExecuteCommand("fast_Restart");
                }
                else if (key == "linf")
                {
                    Utilities.ExecuteCommand("loadscript inf.dll");
                    Utilities.ExecuteCommand("fast_Restart");
                }
                else if(key == "fr")
                {
                    Utilities.ExecuteCommand("fast_Restart");
                }
            });

        }
    }
}
