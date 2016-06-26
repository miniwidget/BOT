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
                if (ent.Call<string>("getguid").Contains("bot"))
                {
                    Function.SetEntRef(-1);
                    Function.Call("kick", i);
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
                    ent.Call("setorigin", o);
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
                        ent.AfterDelay(100, x => ent.Call("suicide"));
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
        internal void Script(string str)
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
                case "ulsc": AD.Script("unloadscript sc.dll"); return false;
                case "lsc": AD.Script("loadscript sc.dll"); return false;
                case "fr": Utilities.ExecuteCommand("fast_restart"); return false;
                case "pos": AD.moveBot(null); return false;
                case "kb": AD.KickBOTsAll(); return false;
                case "1": ADMIN.Call("thermalvisionfofoverlayon"); return false;
                case "2": ADMIN.Call("thermalvisionfofoverlayoff"); return false;
                case "safe": USE_ADMIN_SAFE_ = !USE_ADMIN_SAFE_; Utilities.RawSayTo(ADMIN, "ADMIN SAFE : " + USE_ADMIN_SAFE_); return false;
            }

            var texts = text.Split(' ');
            if (texts.Length > 1)
            {
                var txt = texts[0];
                var value = texts[1];

                switch (txt)
                {
                    case "perk": PK.Perk_Hud(ADMIN, int.Parse(value));return false;
                    case "pos": AD.moveBot(value); return false;
                    case "die": AD.Die(text); return false;
                    case "k": AD.Kick(text); return false;
                }
            }

            return true;
        }

    }
}
