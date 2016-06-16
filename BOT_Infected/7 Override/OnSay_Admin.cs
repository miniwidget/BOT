using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    public partial class Infected
    {
        #region related with BOT
        void KickBOTsAll()
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
        }

        void DeployBOTsByNUM(int num)
        {
            for (int i = 0; i < num; i++)
            {
                addBot();
            };

            Utilities.RawSayAll(num + " ^2BOTs ^7deployed");
        }

        void addBot()
        {
            var bot = Utilities.AddTestClient();
            if (bot == null) return;
            bot.TakeAllWeapons();
            var weapon = SN();
            bot.GiveWeapon(weapon);
            bot.SwitchToWeapon(weapon);
            bot.SwitchToWeaponImmediate(weapon);
        }
        void moveBot(string name)
        {
            if (name == null) name = "bot";
            foreach (var bot in BOTs_List)
            {
                if (bot.Name.Contains(name))
                {
                    bot.Call("setorigin", ADMIN.Origin);
                    break;
                }
            }
        }


        #endregion

        #region related with KILL or KICK
        void Die(string message)
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
                            AfterDelay(100, () => p.Call("suicide"));
                        }
                    }
                }
            }
        }

        void Magic(string message)
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

        void Kick(string message)
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
                            AfterDelay(100, () => Utilities.RawSayAll("Kick ^2" + player.Name + " ^7executed"));
                            AfterDelay(100, () => Utilities.ExecuteCommand("dropclient " + player.EntRef));
                        }

                    }

                }
            }
        }

        #endregion

        #region 관리자 커맨드 메서드

        void script(string name, bool load)
        {
            if (load)
            {
                Utilities.ExecuteCommand("loadScript " + name + ".dll");
            }
            else
            {
                Utilities.ExecuteCommand("unloadScript " + name + ".dll");
            }
        }
        void print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
        void sayToAdmin(string m)
        {
            AfterDelay(t0, () => Utilities.RawSayTo(ADMIN, m));
        }

        #endregion

        void ResetGame(string command)
        {
            if (!TEST_) command = command.Replace("test\\", "");
            KickBOTsAll();
            AfterDelay(t1, () => Utilities.ExecuteCommand(command));
        }

        bool AdminCommand(string text)
        {

            switch (text)
            {
                case "so":
                    

                    break;
                case "pos": moveBot(null); return false;
                case "kb": Utilities.RawSayAll("^2Kickbots ^7executed"); KickBOTsAll(); return false;
                case "1": ADMIN.Call("thermalvisionfofoverlayon"); return false;
                case "2": ADMIN.Call("thermalvisionfofoverlayoff"); return false;
                case "safe": USE_ADMIN_SAFE_ = !USE_ADMIN_SAFE_; sayToAdmin("ADMIN SAFE : " + USE_ADMIN_SAFE_); return false;
            }

            var t = text.Split(' ');
            if (t.Length > 1)
            {
                var txt = t[0];
                var value = t[1];
                switch (txt)
                {
                    case "so":ADMIN.Call("playlocalsound", value);return false;
                    case "pos": moveBot(value); break;

                    case "bot": DeployBOTsByNUM(int.Parse(value)); return false;
                    case "die": Die(text); return false;
                    case "k": Kick(text); return false;
                    case "magic": Magic(text); return false;
                    case "map": ResetGame("map " + value); return false;
                }
            }

            return true;
        }

    }
}
