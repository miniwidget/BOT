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
                //case "gametype": print(Call<string>("getdvar","g_gametype")); break;
                case "fr": ExecuteCommand("fast_restart"); break;
                case "mr": ExecuteCommand("map_rotate"); break;

                case "linf": ExecuteCommand("loadscript inf.dll", "fast_restart"); break;
                case "lsc": ExecuteCommand("loadscript sc.dll", "fast_restart"); break;
                case "ltest": ExecuteCommand("loadscript test.dll", "fast_restart"); break;
                case "ltdm": ExecuteCommand("loadscript tdm.dll", "fast_restart"); break;

                case "ulinf": ExecuteCommand("unloadscript inf.dll", "fast_restart"); break;
                case "ulsc": ExecuteCommand("unloadscript sc.dll", "fast_restart"); break;
                case "ultest": ExecuteCommand("unloadscript test.dll", "fast_restart"); break;
                case "ultdm": ExecuteCommand("unloadscript tdm.dll", "fast_restart"); break;
                case "status": Status(); break;

                case "3rd":  break;

            }
        }
        void Commands(string[] ss)
        {
            string s = ss[0];

            switch (s)
            {
                case "so": break;
            }
        }
    }
}
