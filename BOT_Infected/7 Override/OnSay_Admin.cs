using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    class KwnavCommand : BaseScript
    {
        internal Entity ADMIN;
        internal void KickBOTsAll()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);

                if (ent == null) continue;
                if (ent.Call<string>("getguid").Contains("bot"))
                {
                    Call("kick", i);
                }
            }
            Utilities.RawSayAll("^2Kickbots ^7executed");
        }

        internal void DeployBOTsByNUM(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Utilities.AddTestClient();
            };
        }

        internal void Die(string message)
        {
            String[] split = message.Split(' ');
            if (split.Length == 1) sayToAdmin("die [player's name]");

            else if (split.Length > 1)
            {
                foreach (Entity p in Players)
                {
                    if (p != null && p.IsPlayer)
                    {
                        if (p.Name.Contains(split[1]))
                        {
                            p.AfterDelay(100, x => p.Call("suicide"));
                        }
                    }
                }
            }
        }

        internal void Magic(string message)
        {
            String[] split = message.Split(' ');

            if (split.Length == 1) sayToAdmin("magic [player's name]");

            else if (split.Length > 1)
            {
                foreach (Entity player in Players)
                {
                    if (player != null && player.IsPlayer)
                    {
                        if (player.Name.Contains(split[1]))
                        {
                            var targetPos = player.Origin;
                            var startPos = ADMIN.Origin;

                            startPos.Z = startPos.Z + 1000;
                            Entity rocket = Call<Entity>("magicbullet", "uav_strike_projectile_mp", startPos, targetPos, ADMIN);
                        }
                    }
                }

            }
        }

        internal void Kick(string message)
        {
            Char[] delimit = { ' ' };
            String[] split = message.Split(delimit);
            if (split.Length == 1)
            {
                sayToAdmin("warn [player's name]");
            }

            else if (split.Length > 1)
            {
                for (int i = 0; i < 18; i++)
                {
                    Entity player = Call<Entity>("getEntByNum", i);
                    if (player != null && player.IsPlayer)
                    {
                        if (player.Name.Contains(message.Split(' ')[1]))
                        {
                            player.AfterDelay(100, x => Utilities.RawSayAll("Kick ^2" + player.Name + " ^7executed"));
                            player.AfterDelay(100, x => Utilities.ExecuteCommand("dropclient " + player.EntRef));
                        }

                    }

                }
            }
        }
        internal void sayToAdmin(string m)
        {
            AfterDelay(100, () => Utilities.RawSayTo(ADMIN, m));
        }


        bool _3rd;
        internal void ChangeView()
        {
            if (_3rd) ADMIN.SetClientDvar("camera_thirdPerson", "0");
            else ADMIN.SetClientDvar("camera_thirdPerson", "1");
            _3rd = !_3rd;
        }

    }

    public partial class Infected
    {
        KwnavCommand KC;

        #region related with BOT

        void moveBot(string name)
        {

            foreach (var bot in BOTs_List)
            {
                if (name == null)
                {
                    bot.Call("setorigin", ADMIN.Origin);
                }
                else if (bot.Name.Contains(name))
                {
                    bot.Call("setorigin", ADMIN.Origin);
                    break;
                }
            }
        }
        #endregion

        #region 관리자 커맨드 메서드

        void print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }



        #endregion

        string getprintPos(string o)
        {
            o = o.Replace(", ", "f, ").Replace(")", "f)");
            return o;
        }
        void printPos()
        {
            var o = getprintPos(ADMIN.Origin.ToString());
            //case MAP.mp_interchange: HELI_WAY_POINT = new Vector3(2535, -573, 100); break;
            string s = "case " + MAP_INDEX + ": HELI_WAY_POINT = new Vector3 " + o + "; break;";
            print(s);
        }


        bool AdminCommand(string text)
        {
            if (KC == null)
            {
                KC = new KwnavCommand();
                KC.ADMIN = this.ADMIN;
            }

            switch (text)
            {
                    //testset();
                case "test": testset();  return false;
                case "3rd": KC.ChangeView(); return false;
                case "m2": ADMIN.Call("setorigin", HELI_WAY_POINT); return false;
                case "p": printPos(); return false;
                case "1": ADMIN.Call("thermalvisionfofoverlayon"); return false;
                case "2": ADMIN.Call("thermalvisionfofoverlayoff"); return false;
                case "pos": moveBot(null); return false;
                case "kb":  KC.KickBOTsAll(); return false;
                
                //case "safe": USE_ADMIN_SAFE_ = !USE_ADMIN_SAFE_; print("ADMIN SAFE : " + USE_ADMIN_SAFE_); return false;
            }

            var t = text.Split(' ');
            if (t.Length > 1)
            {
                var txt = t[0];
                var value = t[1];
                switch (txt)
                {
                    case "pos": moveBot(value); break;
                    case "bot": KC.DeployBOTsByNUM(int.Parse(value)); return false;
                    case "die": KC.Die(text); return false;
                    case "k": KC.Kick(text); return false;
                    case "magic": KC.Magic(text); return false;
                }
            }

            return true;
        }

    }
}
