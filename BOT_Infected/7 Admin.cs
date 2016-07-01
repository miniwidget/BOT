using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    class Admin : Inf
    {
        Entity admin;
        public Admin(Entity adm)
        {
            admin = adm;
        }

        internal bool KickBOTsAll()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);

                if (ent == null) continue;
                if (ent.Call<string>(33350).Contains("bot"))//"getguid"
                {
                    Call(286, i);
                }
            }
            SayToAdmin("^2Kickbots ^7executed");
            return false;
        }

        internal bool moveBot(string name)
        {
            Vector3 o = admin.Origin;

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
            return false;
        }

        internal bool Die(string message)
        {
            if (message == null) SayToAdmin("die [player's name]");

            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Entity ent = Entity.GetEntity(i);

                    if (ent == null) continue;
                    if (ent.Name.Contains(message))
                    {
                        ent.AfterDelay(100, x => ent.Call(33341));//"suicide"
                    }
                }
            }
            return false;
        }

        internal bool Kick(string message)
        {
            if (message == null)
            {
                SayToAdmin("kick [player's name]");
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Entity ent = Entity.GetEntity(i);

                    if (ent == null) continue;
                    if (ent.Name.Contains(message))
                    {
                        ent.AfterDelay(100, x => Utilities.ExecuteCommand("dropclient " + ent.EntRef));
                    }
                }
                SayToAdmin("^2Kick ^7Executed");
            }
            return false;
        }

        internal bool Script(string str, bool restart)
        {
            Utilities.ExecuteCommand(str);
            Utilities.ExecuteCommand("fast_restart");

            return false;
        }

        internal bool Status()
        {
            string s = null;

            for (int i = 0; i < 18; i++)
            {
                Entity p = Entity.GetEntity(i);

                if (p == null) continue;
                string name = p.Name;
                if (name.StartsWith("bot"))
                {
                    s += "◎" + p.EntRef + p.GetField<string>("sessionteam").Substring(0, 2);
                }
                else s += " ◐" + p.EntRef + p.GetField<string>("sessionteam").Substring(0, 2);
            }
            Print(s);
            return false;
        }

        void SayToAdmin(string message)
        {
            Utilities.RawSayTo(admin, message);
        }

    }

    public partial class Infected
    {
        //string GetSO(string idx, Vector3 o)
        //{
        //    int x = (int)o.X;
        //    int y = (int)o.Y;
        //    int z = (int)o.Z;

        //    int diff = (int)o.DistanceTo(ADMIN.Origin);
        //    return idx + "(" + x + "," + y + "," + z + ")[" + diff + "] ";
        //}

        Admin AD;

        bool AdminCommand(string text)
        {
            if (AD == null) AD = new Admin(ADMIN);

            var texts = text.Split(' ');
            string value = null;
            if (texts.Length > 1)
            {
                text = texts[0];
                value = texts[1];
            }

            switch (text)
            {
                case "tank": TK.SetTank(ADMIN); return false;
                case "130":
                    {
                        if (ac130 == null) ac130 = new AC130();
                        ac130.start(ADMIN);
                    }
                    return false;
                case "heli":
                    {
                        H_SET H = H_FIELD[ADMIN.EntRef];
                        H.PERK = 12;
                        H.USE_HELI = 2;
                        HCT.HeliCall(ADMIN, true);

                        ADMIN.Call("setorigin", Helicopter.HELI_WAY_POINT);
                        BotDoAttack(false);
                    }
                    return false;
                case "safe":
                    {
                        USE_ADMIN_SAFE_ = !USE_ADMIN_SAFE_;
                        Utilities.RawSayTo(ADMIN, "ADMIN SAFE : " + USE_ADMIN_SAFE_);
                    }
                    return false;

                case "ultest": return AD.Script("unloadscript test.dll", true);
                case "ltest": return AD.Script("loadscript test.dll", true);
                case "fr": return AD.Script("fast_restart", false);
                case "mr": return AD.Script("map_rotate", false);
                case "status": return AD.Status();

                case "attack": return BotDoAttack(!SET.StringToBool(Call<string>("getdvar", "testClients_doAttack")));
                case "kb": return AD.KickBOTsAll();
                case "k": return AD.Kick(text);
                case "pos": return AD.moveBot(value);
                case "die": return AD.Die(value);

                case "1": ADMIN.Call(32936); return false;
                case "2": ADMIN.Call(32937); return false;
            }

            return true;
        }

    }
}
