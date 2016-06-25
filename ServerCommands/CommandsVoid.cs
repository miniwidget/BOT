using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommands
{
    public partial class sc
    {
        void SayToAll(string[] texts)
        {
            string notice = null;

            foreach (string s in texts)
            {
                notice += s + " ";
            }
            Utilities.RawSayAll(notice);
        }
        void Status()
        {
            string s = null;

            for (int i = 0; i < 18; i++)
            {
                Entity p = Entity.GetEntity(i);

                if (p == null)
                {
                    print("NULL" + i);
                    Players.Remove(p);
                    continue;
                }
                string name = p.Name;
                if (name.StartsWith("bot"))
                {
                    s += "◎" + p.EntRef + abbrrev(p.GetField<string>("sessionteam")) ;
                }
                else if (name == "")
                {
                    Players.Remove(p);
                }
                else s += " ◐" + p.EntRef + abbrrev(p.GetField<string>("sessionteam"));
            }
            print(s);
        }
        string abbrrev(string s)
        {
            if (s == "axis") s = "D ";
            else if (s == "spectator") s = "S ";
            else s = "L ";
            return ":" +s;
        }
        void print(object o)
        {
            Log.Write(LogLevel.None, "{0}", o.ToString());
        }
        void ExecuteCommand(string a)
        {
            //if (!TEST_) a = a.Replace("test\\", "");

            Utilities.ExecuteCommand(a);
        }
        void ExecuteCommand(string a, string b)
        {
            //if (!TEST_) a = a.Replace("test\\", "");

            Utilities.ExecuteCommand(a);
            Utilities.ExecuteCommand(b);
        }

    }
}
