using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    internal class Admin
    {
        internal void KickBOTsAll()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);

                if (ent == null) continue;
                if (ent.Call<string>(33350).Contains("bot"))//"getguid"
                {
                    Function.SetEntRef(-1);
                    Function.Call(286, i);
                }
            }
            SayToAdmin("^2Kickbots ^7executed");

        }
        internal void moveBot(string name)
        {
            Vector3 o = Infected.ADMIN.Origin;

            if (name == null) name = "bot";
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);

                if (ent == null) continue;
                if (ent.Name.Contains(name))
                {
                    ent.Call(33529, o);//"setorigin"
                }
            }
            SayToAdmin("^2moveBot ^7executed");
        }
        internal void Die(string message)
        {
            string[] split = message.Split(' ');
            if (split.Length == 1) SayToAdmin("die [player's name]");

            else if (split.Length > 1)
            {
                for (int i = 0; i < 18; i++)
                {
                    Entity ent = Entity.GetEntity(i);

                    if (ent == null) continue;
                    if (ent.Name.Contains(split[1]))
                    {
                        ent.AfterDelay(100, x => ent.Call(33341));//"suicide"
                    }
                }
            }
        }

        internal void Kick(string message)
        {
            string[] split = message.Split(' ');
            if (split.Length == 1)
            {
                SayToAdmin("warn [player's name]");
            }
            else if (split.Length > 1)
            {
                for (int i = 0; i < 18; i++)
                {
                    Entity ent = Entity.GetEntity(i);

                    if (ent == null) continue;
                    if (ent.Name.Contains(message.Split(' ')[1]))
                    {
                        ent.AfterDelay(100, x => Utilities.ExecuteCommand("dropclient " + ent.EntRef));
                    }
                }
                SayToAdmin("^2Kick ^7Executed");
            }
        }

        void SayToAdmin(string message)
        {
            Utilities.RawSayTo(Infected.ADMIN, message);
        }
        internal void Script(string str, bool restart)
        {
            Utilities.ExecuteCommand(str);
            Utilities.ExecuteCommand("fast_restart");
        }
        
    }
    public partial class Infected
    {
        Admin AD;

        bool AdminCommand(string text)
        {
            if (AD == null) AD = new Admin();

            switch (text)
            {
                case "heli":HCT.HeliCall(ADMIN);return false; 
                case "ulsc": AD.Script("unloadscript sc.dll",true); return false;
                case "lsc": AD.Script("loadscript sc.dll",true); return false;
                case "fr": AD.Script("fast_restart",false); return false;
                case "mr": AD.Script("map_rotate",false); return false;
                case "pos": AD.moveBot(null); return false;
                case "kb": AD.KickBOTsAll(); return false;
                case "1": ADMIN.Call(32936); return false;
                case "2": ADMIN.Call(32937); return false;
                case "safe": USE_ADMIN_SAFE_ = !USE_ADMIN_SAFE_; Utilities.RawSayTo(ADMIN, "ADMIN SAFE : " + USE_ADMIN_SAFE_); return false;
            }

            var texts = text.Split(' ');
            if (texts.Length > 1)
            {
                var txt = texts[0];
                var value = texts[1];

                switch (txt)
                {
                    case "pos": AD.moveBot(value); return false;
                    case "die": AD.Die(text); return false;
                    case "k": AD.Kick(text); return false;
                }
            }

            return true;
        }

    }
}
