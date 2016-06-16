using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommands
{
    public partial class sc
    {

        void Commands(string s)
        {
            switch (s)
            {
                case "gametype": print(Call<string>("getdvar","g_gametype")); break;
                case "fr": ExecuteCommand("fast_restart"); break;
                case "mr": ExecuteCommand("map_rotate"); break;

                case "linf": ExecuteCommand("loadscript test\\inf.dll", "fast_restart"); break;
                case "lsc": ExecuteCommand("loadscript test\\sc.dll", "fast_restart"); break;
                case "ltest": ExecuteCommand("loadscript test\\test.dll", "fast_restart"); break;
                case "ltdm": ExecuteCommand("loadscript test\\tdm.dll", "fast_restart"); break;

                case "ulinf": ExecuteCommand("unloadscript test\\inf.dll", "fast_restart"); break;
                case "ulsc": ExecuteCommand("unloadscript test\\sc.dll", "fast_restart"); break;
                case "ultest": ExecuteCommand("unloadscript test\\test.dll", "fast_restart"); break;
                case "ultdm": ExecuteCommand("unloadscript test\\tdm.dll", "fast_restart"); break;
                case "status": Status(); break;
            }
        }
        void Commands(string[] s)
        {
        }
    }
}
