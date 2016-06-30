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
        public Admin(Entity adm)
        {
            admin = adm;
        }
        Entity admin;

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
            Utilities.RawSayTo(admin, message);
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
        void testAC130()
        {
            if (ac130 == null) ac130 = new AC130();
            ac130.start(ADMIN);
        }
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
                case "130": testAC130(); return false;
                //case "o": ADMIN.Call("setorigin", TK.REMOTETANK.Origin); return false;
                //case "3rd": AD.Viewchange(ADMIN); return false;
                case "attack": BotDoAttack(!SET.StringToBool(Call<string>("getdvar", "testClients_doAttack"))); return false;
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
                //script
                case "ulsc": AD.Script("unloadscript sc.dll", true); return false;
                case "lsc": AD.Script("loadscript sc.dll", true); return false;
                case "fr": AD.Script("fast_restart", false); return false;
                case "mr": AD.Script("map_rotate", false); return false;

                case "kb": AD.KickBOTsAll(); return false;
                case "k": AD.Kick(text); return false;
                case "1": ADMIN.Call(32936); return false;
                case "2": ADMIN.Call(32937); return false;
                case "safe": USE_ADMIN_SAFE_ = !USE_ADMIN_SAFE_; Utilities.RawSayTo(ADMIN, "ADMIN SAFE : " + USE_ADMIN_SAFE_); return false;
                
                case "pos": AD.moveBot(value); return false;
                case "die": AD.Die(text); return false;
                case "posb": BOTs_List[3].Call("setorigin", new Vector3(-994.454f, 1227.692f, 1572.443f));return false;
                   
            }


            return true;
        }

    }
}
