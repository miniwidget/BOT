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
                    Script("unloadscript inf.dll", true);
                }
                else if (key == "linf")
                {
                    Script("loadscript inf.dll", true);
                }
                else if(key == "fr")
                {
                    Utilities.ExecuteCommand("fast_Restart");
                }
                else if (key == "ultdm")
                {
                    Script("unloadscript tdm.dll", true);
                }
                else if (key == "ltdm")
                {
                    Script("loadscript tdm.dll", true);
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
