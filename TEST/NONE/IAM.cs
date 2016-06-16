// Type: IAM.IAM
// Assembly: IAM, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6E6BAA6-E916-4B15-A81B-51B7312C39BA
// Assembly location: C:\Users\szczur\Desktop\IAM.dll

using InfinityScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;

namespace IAM
{
    public class IAM : BaseScript
    {
        public int screamsec = 9;
        private Timer saver = new Timer();
        public int votetime = -5;
        private Dictionary<string, string> xlrstats = new Dictionary<string, string>();
        private Dictionary<string, int> spams = new Dictionary<string, int>();
        private Timer delay = new Timer();
        private Timer rules = new Timer();
        private List<Entity> Entitys = new List<Entity>();
        private List<Entity> BadPings = new List<Entity>();
        private Dictionary<string, string> gCommands = new Dictionary<string, string>();
        private Dictionary<string, string> Groups = new Dictionary<string, string>();
        private List<string> ImmunePlayer = new List<string>();
        private int intervalMesages = 10000;
        private Timer map1 = new Timer();
        private Dictionary<int, string> Maps = new Dictionary<int, string>();
        private Dictionary<int, string> Rules = new Dictionary<int, string>();
        private Dictionary<int, string> timedmessages = new Dictionary<int, string>();
        private Dictionary<string, string> UserGroup = new Dictionary<string, string>();
        private Dictionary<string, int> warnings = new Dictionary<string, int>();
        public ArrayList maplist = new ArrayList();
        private List<Entity> allies = new List<Entity>();
        private List<Entity> axis = new List<Entity>();
        private int min = 60;
        private bool checkp = true;
        public string sa3id;
        public string dspl;
        public bool isscream;
        public bool snm;
        public string spammer;
        public string messagescr;
        public bool screamed;
        public bool hardc;
        public string password;
        public bool saeed;
        public string maxpingcfg;
        public bool fristkill;
        public int hekilled;
        public int hekilled1;
        public int hekilled2;
        public int hekilled3;
        public int hekilled4;
        public int hekilled5;
        public int hekilled6;
        public int hekilled7;
        public int hekilled8;
        public int hekilled9;
        public int hekilled10;
        public int hekilled11;
        public int hekilled12;
        public int hekilled13;
        public int hekilled14;
        public int hekilled15;
        public int hekilled16;
        public int hekilled17;
        public int hekilled18;
        public string killed;
        public string killed1;
        public string killed2;
        public string killed3;
        public string killed4;
        public string killed5;
        public string killed6;
        public string killed7;
        public string killed8;
        public string killed9;
        public string killed10;
        public string killed11;
        public string killed12;
        public string killed13;
        public string killed14;
        public string killed15;
        public string killed16;
        public string killed17;
        public string killed18;
        public string BotName;
        public string votemap;
        public string maplikename;
        public string votemode;
        public string votetype;
        public Entity vkn;
        public string plvote;
        public bool voted;
        public int yes;
        public int no;
        public int vtime;
        //private Langfilter teacher;
        //private NoKnife KnifeClass;
        //private JSG juspg;
        //private Aliaser cloaker;
        private int conter;
        private int contermessa;
        private int contmap;
        private string listG;
        private Entity mapEnt;
        public Entity PlAyEr;
        public string admins;
        public string dsrname;
        public string antihack;
        public string visitors;
        public bool map;
        public int gravil;
        public int jumpl;
        public int speel;
        public string messagepl;
        public string welcomelist;
        private int hasel;
        private int laskill;
        private Entity lastkiller;
        private string checkname;
        private string hcc;
        private string isfale;
        public string welcomer;
        public string Team1;
        public string Team2;
        public string wlpm;
        public string vmap;
        public string vmod;
        public string maxpingi;
        public string kob;
        public string promode;
        public string autoreg;
        public string num;
        public string string1;

        public IAM()
        {
            this.Loadconfig();
            this.loadplist();
            this.hardco();
            this.antihackload();
            this.loginload();
            this.maplistload();
            this.maxpingload();
            this.LoadTeamNames();
            this.modlistload();
            this.BotNameload();
            this.badwordcfg();
            this.check();
            this.nextsec();
            this.votetimecfg();
            this.LoadXlrStats();
            //this.juspg = new JSG();
            //this.cloaker = new Aliaser();
            //this.KnifeClass = new NoKnife();
            //this.teacher = new Langfilter();
            this.GetPermission();
            this.GetXlrStats();
            this.GetImmune();
            this.maxping();
            this.TimedMessages();
            //this.juspg.setDefaultGameValue();
            this.maplist.Add((object)"dome");
            this.maplist.Add((object)"arkaden");
            this.maplist.Add((object)"lockdown");
            this.maplist.Add((object)"hardhat");
            this.maplist.Add((object)"bootleg");
            this.maplist.Add((object)"interchange");
            this.maplist.Add((object)"carbon");
            this.maplist.Add((object)"downturn");
            this.maplist.Add((object)"outpost");
            this.maplist.Add((object)"gateway");
            this.maplist.Add((object)"lookout");
            this.maplist.Add((object)"overwatch");
            this.maplist.Add((object)"fallen");
            this.maplist.Add((object)"terminal");
            this.maplist.Add((object)"underground");
            this.maplist.Add((object)"village");
            this.maplist.Add((object)"mission");
            this.maplist.Add((object)"resistance");
            this.maplist.Add((object)"bakaraa");
            this.maplist.Add((object)"seatown");
            this.maplist.Add((object)"liberation");
            this.maplist.Add((object)"pizza");
            this.maplist.Add((object)"blackbox");
            this.maplist.Add((object)"sanctuary");
            this.maplist.Add((object)"foundation");
            this.maplist.Add((object)"oasis");
            this.maplist.Add((object)"aground");
            this.maplist.Add((object)"erosion");
            this.maplist.Add((object)"uturn");
            this.maplist.Add((object)"intersection");
            this.maplist.Add((object)"vortex");
            this.maplist.Add((object)"decommission");
            this.maplist.Add((object)"offshore");
            this.maplist.Add((object)"gulch");
            this.maplist.Add((object)"boardwalk");
            this.maplist.Add((object)"parish");
            this.Maps.Add(0, "^3[MapName] [ShortName]");
            this.Maps.Add(1, "^3Bootleg  boo");
            this.Maps.Add(2, "^3Hardhat  har");
            this.Maps.Add(3, "^3Carbon  car");
            this.Maps.Add(4, "^3Downturn  dow");
            this.Maps.Add(5, "^3Outpost out");
            this.Maps.Add(6, "^3Dome = dom");
            this.Maps.Add(7, "^3Getaway get");
            this.Maps.Add(8, "^3Lookout loo");
            this.Maps.Add(9, "^3Overwatch ove");
            this.Maps.Add(10, "^3Fallen fal");
            this.Maps.Add(11, "^3Terminal ter");
            this.Maps.Add(12, "^3Underground und");
            this.Maps.Add(13, "^3Arkaden ark");
            this.Maps.Add(14, "^3Decommission dec");
            this.Maps.Add(15, "^3Parish par");
            this.Maps.Add(16, "^3[MapName] [ShortName]");
            this.Maps.Add(17, "^3Off Shore off");
            this.Maps.Add(18, "^3Boardwalk bok");
            this.Maps.Add(19, "^3Pizza piz");
            this.Maps.Add(20, "^3Gulch gul");
            this.Maps.Add(21, "^3Foundation fou");
            this.Maps.Add(22, "^3BlackBox bla");
            this.Maps.Add(23, "^3Sanctuary san");
            this.Maps.Add(24, "^3Aground agr");
            this.Maps.Add(25, "^3Vortex vor");
            this.Maps.Add(26, "^3Erosion ero");
            this.Maps.Add(27, "^3Liberation lib");
            this.Maps.Add(28, "^3Oasis oas");
            this.Maps.Add(29, "^3U-Turn utu");
            this.Maps.Add(30, "^3Lockdown loc");
            this.Maps.Add(31, "^3Intersection ins");
            this.string1 = "hide";
            this.Maps.Add(32, "^3Mission mis");
            this.Maps.Add(33, "^3Interchange int");
            this.Maps.Add(34, "^3Baakara baa");
            this.Maps.Add(35, "^3Ressistance res");
            this.Maps.Add(36, "^3Seatown sea");
            this.Maps.Add(37, "^3Village vil");
            this.PlayerConnected += new Action<Entity>(this.playerConnected);
            this.PlayerConnecting += new Action<Entity>(this.playerConnecting);
            this.PlayerDisconnected += new Action<Entity>(this.playerDisconnected);
            this.mptimer();
            this.fristkill = false;
            this.hekilled = 0;
            this.hekilled1 = 0;
            this.hekilled2 = 0;
            this.hekilled3 = 0;
            this.hekilled4 = 0;
            this.hekilled5 = 0;
            this.hekilled6 = 0;
            this.hekilled7 = 0;
            this.hekilled8 = 0;
            this.hekilled9 = 0;
            this.hekilled10 = 0;
            this.hekilled11 = 0;
            this.hekilled12 = 0;
            this.hekilled13 = 0;
            this.hekilled14 = 0;
            this.hekilled15 = 0;
            this.hekilled16 = 0;
            this.hekilled17 = 0;
            this.hekilled18 = 0;
            this.zerokill();
            if (this.hardc)
                Utilities.ExecuteCommand("g_hardcore 1");
           // this.OnNotify("game_ended", (Action<Parameter>)(level => this.KnifeClass.EnableKnife()));
        }

        private void testLang(string message, Entity target)
        {
            //if (!this.teacher.issueWarning(message))
            //    return;
            //this.warnlang(target);
        }

        private void maxpingload()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("pingcensor"))
                    this.maxpingcfg = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void RemoveToAdmins(Entity player)
        {
            if (!this.antihack.Contains(player.Name + ","))
                return;
            string str1 = this.antihack;
            this.antihack = this.antihack.Replace(player.Name + ",", "");
            string str2 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
            File.Delete("scripts\\\\IAM\\\\IAM.cfg");
            using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
            {
                string str3 = str2.Replace("admins=" + str1, "admins=" + this.antihack);
                streamWriter.WriteLine(str3);
            }
        }

        private void sayrule()
        {
            if (this.PlAyEr == null)
                return;
            string message = "";
            Entity player = this.PlAyEr;
            this.Rules.TryGetValue(this.conter, out message);
            this.TellClient(player, message);
            ++this.conter;
            if (this.conter >= this.Rules.Count)
            {
                this.PlAyEr = (Entity)null;
                this.conter = 0;
                this.rules.Dispose();
                this.rules.Enabled = false;
                this.Rules.Clear();
            }
            else
                this.AfterDelay(1000, new Action(this.sayrule));
        }

        private void AddToAdmins(Entity player)
        {
            if (this.antihack.Contains(player.Name + ","))
                return;
            string str1 = this.antihack;
            IAM iam = this;
            string str2 = iam.antihack + player.Name + ",";
            iam.antihack = str2;
            string str3 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
            File.Delete("scripts\\\\IAM\\\\IAM.cfg");
            using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
            {
                string str4 = str3.Replace("admins=" + str1, "admins=" + this.antihack);
                streamWriter.WriteLine(str4);
            }
        }

        private void AddToPlayers(Entity player)
        {
            IAM iam = this;
            string str = iam.welcomelist + player.Name + ",";
            iam.welcomelist = str;
            File.Delete("scripts\\\\IAM\\\\playerslist");
            using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\playerslist", true))
                streamWriter.WriteLine(this.welcomelist);
        }

        private void RemoveFromPlayers(Entity player)
        {
            string oldValue = this.welcomelist;
            this.welcomelist = this.welcomelist.Replace(player.Name + ",", "");
            string str1 = File.ReadAllText("scripts\\\\IAM\\\\playerslist");
            File.Delete("scripts\\\\IAM\\\\playerslist");
            using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\playerslist", true))
            {
                string str2 = str1.Replace(oldValue, this.welcomelist);
                streamWriter.WriteLine(str2);
            }
        }

        private void loadplist()
        {
            this.welcomelist = "";
            StreamReader streamReader = new StreamReader("scripts\\\\IAM\\\\playerslist");
            this.welcomelist = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private bool searchinlist(Entity player)
        {
            return this.welcomelist.Contains(player.Name);
        }

        private void loginload()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("password"))
                    this.password = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void hardco()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("hardcore"))
                    this.hcc = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
            if (this.hcc == "true")
            {
                this.hardc = true;
            }
            else
            {
                if (!(this.hcc == "true"))
                    return;
                this.hardc = false;
            }
        }

        private void antihackload()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("admins"))
                    this.antihack = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        public void mptimer()
        {
            if (this.maxpingcfg == "true")
            {
                if (this.min == 0)
                {
                    this.checkp = true;
                    this.min = 60;
                }
                else if (this.checkp)
                {
                    int num = int.Parse(this.maxpingi);
                    foreach (Entity player in this.Entitys)
                    {
                        if (!this.IsImmune(player) && player.Ping > num && !this.BadPings.Contains(player))
                            this.BadPings.Add(player);
                    }
                    foreach (Entity player in this.BadPings)
                        this.warnping(player);
                    this.checkp = false;
                }
            }
            --this.min;
            this.AfterDelay(1000, new Action(this.mptimer));
        }

        public void nextscr()
        {
            if (this.screamsec <= 0)
            {
                this.screamsec = 9;
                this.messagescr = "";
                this.isscream = false;
            }
            else
            {
                this.ServerSay("^" + this.screamsec.ToString() + this.messagescr);
                --this.screamsec;
                this.AfterDelay(1000, new Action(this.nextscr));
            }
        }

        public void check()
        {
            if (this.voted)
            {
                if (this.votetime == 0 | this.votetime == -1 | this.votetime == -2)
                {
                    this.voted = false;
                    this.votetimecfg();
                    if (this.yes > this.no)
                    {
                        if (this.votetype == "Restart")
                        {
                            this.votetype = "";
                            this.plvote = "";
                            this.yes = 0;
                            this.no = 0;
                            this.voted = false;
                            this.ServerSay("^3Vote Restart Accepted!");
                            Utilities.ExecuteCommand("fast_restart");
                        }
                        else if (this.votetype == "Nextmap")
                        {
                            this.votetype = "";
                            this.plvote = "";
                            this.yes = 0;
                            this.no = 0;
                            this.voted = false;
                            this.ServerSay("^3Vote Nextmap Accepted!");
                            Utilities.ExecuteCommand("map_rotate");
                        }
                        else if (this.votetype == "Map")
                        {
                            this.votetype = "";
                            this.plvote = "";
                            this.yes = 0;
                            this.no = 0;
                            this.voted = false;
                            this.ServerSay("^3Vote Map Accepted!");
                            Utilities.ExecuteCommand("map " + this.votemap);
                        }
                        else if (this.votetype == "Gametype")
                        {
                            this.votetype = "";
                            this.plvote = "";
                            this.yes = 0;
                            this.no = 0;
                            this.voted = false;
                            this.ServerSay("^3Vote Mode Accepted! and Mod will be changed 3sec later");
                            Utilities.ExecuteCommand("sv_maprotation " + this.votemode);
                            this.AfterDelay(3000, new Action(this.Maprotate));
                        }
                        else if (this.votetype == "Kick")
                        {
                            this.votetype = "";
                            this.plvote = "";
                            this.yes = 0;
                            this.no = 0;
                            this.voted = false;
                            this.ServerSay("^3Vote Mode Accepted! and ^2" + this.vkn.Name + "^3 Got kicked");
                            Utilities.ExecuteCommand("dropclient " + (object)this.vkn.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : You Kicked by Vote\"");
                        }
                        else
                        {
                            this.plvote = "";
                            this.yes = 0;
                            this.no = 0;
                            this.voted = false;
                            this.ServerSay("^1VoteType Error Syntax");
                        }
                    }
                    else if (this.yes < this.no)
                    {
                        this.plvote = "";
                        this.yes = 0;
                        this.no = 0;
                        this.voted = false;
                        this.ServerSay("^3Vote Failed. ^2Yes : ^5" + (object)this.yes + " ^1NO : ^5" + (string)(object)this.no + " ^2Yes must bigger than no");
                    }
                    else if (this.yes == this.no)
                    {
                        this.plvote = "";
                        this.yes = 0;
                        this.no = 0;
                        this.voted = false;
                        this.ServerSay("^3Vote Failed. ^2Yes : ^5" + (object)this.yes + " ^1NO : ^5" + (string)(object)this.no + " ^2Yes must bigger than no");
                    }
                }
                else if (this.votetime == 45)
                {
                    if (this.votetype == "Map")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Map : ^5" + this.maplikename.ToUpper() + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Gametype")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3DSPL : ^5" + this.votemode + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Kick")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3To Kick : ^5" + this.vkn.Name + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                }
                else if (this.votetime == 30)
                {
                    if (this.votetype == "Map")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Map : ^5" + this.maplikename.ToUpper() + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Gametype")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3DSPL : ^5" + this.votemode + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Kick")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3To Kick : ^5" + this.vkn.Name + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                }
                else if (this.votetime == 15)
                {
                    if (this.votetype == "Map")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Map : ^5" + this.maplikename.ToUpper() + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Gametype")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3DSPL : ^5" + this.votemode + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Kick")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3To Kick : ^5" + this.vkn.Name + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                }
                else if (this.votetime == 5)
                {
                    if (this.votetype == "Map")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Map : ^5" + this.maplikename.ToUpper() + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Gametype")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3DSPL : ^5" + this.votemode + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else if (this.votetype == "Kick")
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3To Kick : ^5" + this.vkn.Name + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                    else
                        this.ServerSay("^3Vote^5" + (object)this.votetype + " ^3Yes : ^5" + (string)(object)this.yes + " ^3No : ^5" + (string)(object)this.no + " ^7(^2!y ^5for ^3<Yes> ^7and ^2!n ^5for ^3<No>^7)");
                }
                this.AfterDelay(1000, new Action(this.check));
            }
            else
                IAM.Print("Vote Ended.", new object[0]);
        }

        private void gametype()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("dspl"))
                    this.dspl = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
            if (File.Exists("admin\\\\" + this.dspl + ".dspl") || File.Exists("players\\\\" + this.dspl + ".dspl"))
                Utilities.ExecuteCommand("sv_maprotation " + this.dspl);
            else
                IAM.Print("Warning : Map rotate on IAM.dspl and map or gametype may stuck so go to IAM.cfg and set a dspl on dspl=xxx to set that dspl after IAM.dspl", new object[0]);
        }

        private void nextsec()
        {
            if (this.voted)
            {
                --this.votetime;
                this.AfterDelay(1000, new Action(this.nextsec));
            }
            else
                IAM.Print("Vote Time Disabled!", new object[0]);
        }

        private void warnlang(Entity player)
        {
            string str1 = "^1Bad ^1language";
            int num1 = 0;
            if (!this.warnings.TryGetValue(player.GUID.ToString(), out num1))
                return;
            int num2 = num1 + 1;
            if (num2 >= 3)
            {
                this.badwordcfg();
                if (this.kob == "kick")
                {
                    ("^1<playername> ^3has been kicked ^3for" + str1).Replace("<playername>", player.Name);
                    this.warnings.Remove(player.GUID.ToString());
                    Utilities.ExecuteCommand("dropclient " + (object)player.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : " + str1 + "\"");
                }
                else
                {
                    if (!(this.kob == "tmpban"))
                        return;
                    ("^1<playername> ^3has been tempbanned ^3for" + str1).Replace("<playername>", player.Name);
                    this.warnings.Remove(player.GUID.ToString());
                    Utilities.ExecuteCommand("tempbanclient " + (object)player.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : TempBanned for badword\"");
                }
            }
            else
            {
                if (num2 >= 3)
                    return;
                this.warnings.Remove(player.GUID.ToString());
                this.warnings.Add(player.GUID.ToString(), num2);
                string str2 = "^1<playername> ^7: ^3Warned for " + str1 + " ^1warning : ^3<warncount> of 3";
                this.ServerSay(str2.Replace("<playername>", player.Name).Replace("<warncount>", num2.ToString()));
                player.Call("iprintlnbold", new Parameter[1]
                {
          (Parameter) str2.Replace("<playername>", player.Name).Replace("<warncount>", num2.ToString())
                });
            }
        }

        private void addToLangFilter(string message, Entity issuer)
        {
            char[] chArray = new char[1]
            {
        ' '
            };
            string[] strArray = message.Split(chArray);
            if (strArray.Length <= 1)
            {
                this.TellClient(issuer, "Please enter a word or words");
            }
            else
            {
                //for (int index = 1; index < strArray.Length; ++index)
                //    this.teacher.addWord(strArray[index]);
                this.TellClient(issuer, "Word(s) Added!");
            }
        }

        private void addplayerTowarn(Entity player)
        {
            if (this.warnings.ContainsKey(player.GUID.ToString()))
                return;
            this.warnings.Add(player.GUID.ToString(), 0);
        }

        private void addplayerToantispammer(Entity player)
        {
            if (this.spams.ContainsKey(player.GUID.ToString()))
                return;
            this.spams.Add(player.GUID.ToString(), 0);
        }

        private void AddImmunet(Entity player, Entity entity)
        {
            if (this.IsImmune(entity))
            {
                player.Call("iprintlnbold", new Parameter[1]
                {
          (Parameter) "^1This Player is already immune."
                });
            }
            else
            {
                this.ImmunePlayer.Add(entity.GUID.ToString());
                this.UpdateImmune();
            }
        }

        private void AddImmune(Entity player, Entity entity)
        {
            if (this.IsImmune(entity))
            {
                player.Call("iprintlnbold", new Parameter[1]
                {
          (Parameter) "^1This Player is already immune."
                });
            }
            else
            {
                this.ImmunePlayer.Add(entity.GUID.ToString());
                this.iprintlnbold(entity, "^2you^1 are ^6now ^1immune.");
                this.ServerSay(" ^3" + entity.Name + " ^1is now immune.");
                this.UpdateImmune();
            }
        }

        private void ban(string message, Entity player)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string newValue = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (this.IsImmune(byName))
                    this.TellClient(player, "^3" + byName.Name + " ^1is immune to that command.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        newValue = newValue + " " + strArray[index];
                    Utilities.ExecuteCommand("banclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]));
                    string str1 = "^2<playername> ^3has been banned ^7for ^1<reason> ^7by ^1<kicker>";
                    foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                    {
                        if (str2.StartsWith("banmessage"))
                            str1 = str2.Split(new char[1]
                            {
                '='
                            })[1];
                    }
                    this.ServerSay(str1.Replace("<playername>", byName.Name).Replace("<reason>", newValue).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    Utilities.ExecuteCommand("banclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]));
                    this.ServerSay("^2<playername> ^3has been banned ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                }
            }
        }

        private void bant(string message, Entity player)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (this.IsImmune(byName))
                    this.TellClient(player, "^3" + byName.Name + " ^1is immune to that command.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    Utilities.ExecuteCommand("banclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]));
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    Utilities.ExecuteCommand("banclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]));
                }
            }
        }

        public void loadlogin()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("login"))
                    this.isfale = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        public bool heisadmin(Entity player)
        {
            this.loadlogin();
            return this.isfale == "false" || this.antihack.Contains(player.Name + ",");
        }

        public bool canUseCommand(Entity player, string command)
        {
            string str1;
            if (this.gCommands.TryGetValue(this.GetGroup(player.GUID.ToString()), out str1))
            {
                if (str1.Equals("*ALL*"))
                    return true;
                string str2 = str1;
                char[] chArray = new char[1]
                {
          ','
                };
                foreach (string str3 in str2.Split(chArray))
                {
                    if (str3.ToLower().Equals(command.ToLower()))
                        return true;
                }
            }
            return false;
        }

        private void Loadconfig()
        {
            if (!File.Exists("scripts\\\\IAM\\\\IAM.cfg"))
            {
                File.Create("scripts\\\\IAM\\\\IAM.cfg").Close();
                File.WriteAllLines("scripts\\\\IAM\\\\IAM.cfg", new string[35]
                {
          "groups=User,Member,Admin,MasterAdmin",
          "Member_xuids=,",
          "Admin_xuids=,,",
          "MasterAdmin_xuids=76561197960594356,,,",
          "User_xuids=*EVERYONE*",
          "Admin_commands=",
          "MasterAdmin_commands=*ALL*",
          "User_commands=!help,!rules,!ver,!guid",
          "",
          "timedmessages=true",
          "connectmessage=^2Welcome ^5<rank> ^7<playername>.",
          "MasterAdmin_message=^2Welcome ^1MasterAdmin ^5<playername>",
          "Admin_message=^2Welcome ^1Admin ^5<playername>",
          "Member_message=^2Welcome ^1Member ^5<playername>",
          "User_message=^2Doorood Bar to ^5<playername>",
          "kickmessage=^2<playername> ^3has been kicked ^7for ^1<reason> ^7by ^1<kicker>",
          "banmessage=^2<playername> ^3has been banned ^7for ^1<reason> ^7by ^1<kicker>",
          "tempbanmessage=^2<playername> ^3has been temp banned ^7for ^1<reason> ^7by ^1<kicker>",
          "warnmessage=^1<playername> ^3has been warned ^7for ^1<reason> ^7warning: ^2<warncount> of 3",
          "unwarnmessage=^1<playername> ^3was unwarned",
          "immuneplayers=76561197960594356,",
          "rules=^2Dont Cheat",
          "blockchat=false",
          "promod=icon",
          "welcomer=true",
          "maxping=200",
          "pm=true",
          "votetime=60",
          "vmaps=mp_hardhat mp_dome ",
          "vmod=dsplname,",
          "badword=kick",
          "autoreg=false",
          "Team1=^1IAM",
          "Team2=^2Guest",
          "botname=^0(^2IAM^0) ^7: "
                });
                if (!File.Exists("scripts\\\\IAM\\\\timedmessages.txt"))
                {
                    File.Create("scripts\\\\IAM\\\\timedmessages.txt").Close();
                    File.WriteAllLines("scripts\\\\IAM\\\\timedmessages.txt", new string[6]
                    {
            "30000",
            "^5respect to ^1rules",
            "^6respect the ^2admins",
            "@admins",
            "@time",
            "@nextmap"
                    });
                }
            }
            if (File.Exists("scripts\\\\IAM\\\\playerslist"))
                return;
            File.Create("scripts\\\\IAM\\\\playerslist");
        }

        private void UnAddToGroup(string group, Entity player, Entity entity)
        {
            string str1 = (string)null;
            string str2 = (string)null;
            foreach (string str3 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str3.StartsWith("groups"))
                    str1 = str3.Split(new char[1]
                    {
            '='
                    })[1];
            }
            foreach (string str3 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str3.StartsWith(group + "_xuids"))
                    str2 = str3.Split(new char[1]
                    {
            '='
                    })[1];
            }
            if (!str1.Contains(group))
                this.TellClient(player, "^2That group does not exist!");
            else if (!str2.Contains(entity.GUID.ToString()))
            {
                this.TellClient(player, "^2Couldn't find the use in this group!");
            }
            else
            {
                string str3 = str2;
                string str4 = str2.Replace(entity.GUID.ToString(), "");
                string str5 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                {
                    string str6 = str5.Replace(group + "_xuids=" + str3, group + "_xuids=" + str4);
                    streamWriter.WriteLine(str6);
                    this.ServerSay(group + " ^1" + entity.Name + "^3 has been Removed from ^1" + group + "^7 group.");
                }
            }
        }

        private void AddToGroup(string group, Entity player, Entity entity)
        {
            string str1 = (string)null;
            string str2 = (string)null;
            foreach (string str3 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str3.StartsWith("groups"))
                    str1 = str3.Split(new char[1]
                    {
            '='
                    })[1];
            }
            foreach (string str3 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str3.StartsWith(group + "_xuids"))
                    str2 = str3.Split(new char[1]
                    {
            '='
                    })[1];
            }
            if (!str1.Contains(group))
                this.TellClient(player, "^2That group does not exist!");
            else if (str2.Contains(entity.GUID.ToString()))
            {
                this.TellClient(player, "^2That User is already in that group!");
            }
            else
            {
                string str3 = str2;
                string str4 = str2 + "," + entity.GUID.ToString();
                string str5 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                {
                    string str6 = str5.Replace(group + "_xuids=" + str3, group + "_xuids=" + str4);
                    streamWriter.WriteLine(str6);
                    this.ServerSay(" User ^1" + entity.Name + "^3 has been added to ^1" + group + "^7 group.");
                }
            }
        }

        private void mpr()
        {
            Utilities.ExecuteCommand("map_rotate");
        }

        private void Maprotate()
        {
            //this.KnifeClass.EnableKnife();
            Utilities.ExecuteCommand("start_map_rotate Default");
        }

        public void ServerSay(string message)
        {
            Utilities.RawSayAll(this.BotName + message);
        }

        private Entity FindByName(string name)
        {
            int num = 0;
            Entity entity1 = (Entity)null;
            foreach (Entity entity2 in this.Entitys)
            {
                if (0 <= entity2.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    entity1 = entity2;
                    ++num;
                }
            }
            if (num <= 1 && num == 1)
                return entity1;
            else
                return (Entity)null;
        }

        private string FindMapByName(string name)
        {
            int num = 0;
            string str1 = (string)null;
            foreach (string str2 in this.maplist)
            {
                if (0 <= str2.IndexOf(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    str1 = str2;
                    ++num;
                }
            }
            if (num <= 1 && num == 1)
                return str1;
            else
                return (string)null;
        }

        private string WelcomeMessage(Entity player)
        {
            string str1 = "^2Welcome ^5<rank> ^7<playername> Player Number ^5<playernumber>.";
            foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str2.StartsWith(this.GetRankByName(player.Name) + "_message"))
                    str1 = str2.Split(new char[1]
                    {
            '='
                    })[1];
                else if (str2.StartsWith("connectmessage"))
                    str1 = str2.Split(new char[1]
                    {
            '='
                    })[1];
            }
            return str1;
        }

        public string GetGroup(string xuid)
        {
            string str = "User";
            if (this.UserGroup.ContainsKey(xuid))
                this.UserGroup.TryGetValue(xuid, out str);
            return str;
        }

        private void getteam()
        {
            this.axis.Clear();
            this.allies.Clear();
            foreach (Entity entity in this.Entitys)
            {
                if (entity.GetField<string>("sessionteam") == "allies")
                    this.allies.Add(entity);
                else if (entity.GetField<string>("sessionteam") == "axis")
                    this.axis.Add(entity);
            }
            this.hasel = this.allies.Count - this.axis.Count;
        }

        private void Default()
        {
            Utilities.ExecuteCommand("sv_maprotation default");
        }

        private bool GetImmune()
        {
            string[] strArray = File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg");
            string str1 = "";
            foreach (string str2 in strArray)
            {
                if (str2.StartsWith("immuneplayers"))
                    str1 = str2.Split(new char[1]
                    {
            '='
                    })[1];
            }
            string str3 = str1;
            char[] chArray = new char[1]
            {
        ','
            };
            foreach (string str2 in str3.Split(chArray))
                this.ImmunePlayer.Add(str2);
            return true;
        }

        private bool GetPermission()
        {
            int num = 0;
            StreamReader streamReader1 = new StreamReader("scripts\\\\IAM\\\\IAM.cfg");
            string str1 = streamReader1.ReadLine();
            if (str1 == null)
                return false;
            streamReader1.Close();
            string str2 = str1.Split(new char[1]
            {
        '='
            })[1];
            this.listG = str2;
            string str3 = str2;
            char[] chArray1 = new char[1]
            {
        ','
            };
            foreach (string key in str3.Split(chArray1))
            {
                StreamReader streamReader2 = new StreamReader("scripts\\\\IAM\\\\IAM.cfg");
                string str4;
                while ((str4 = streamReader2.ReadLine()) != null)
                {
                    if (str4.StartsWith(key + "_xuids"))
                        this.Groups.Add(key, str4.Split(new char[1]
                        {
              '='
                        })[1]);
                    if (str4.StartsWith(key + "_commands"))
                        this.gCommands.Add(key, str4.Split(new char[1]
                        {
              '='
                        })[1]);
                    ++num;
                }
            }
            foreach (KeyValuePair<string, string> keyValuePair in this.Groups)
            {
                string str4 = keyValuePair.Value;
                char[] chArray2 = new char[1]
                {
          ','
                };
                foreach (string key in str4.Split(chArray2))
                {
                    if (this.UserGroup.ContainsKey(key))
                        IAM.Print("The XUID: " + key + " is in multiple groups.", new object[0]);
                    else
                        this.UserGroup.Add(key, keyValuePair.Key);
                }
            }
            return true;
        }

        private string GetRankByName(string name)
        {
            string str = "User";
            Entity byName = this.FindByName(name);
            if (byName != null)
                str = this.GetGroup(byName.GUID.ToString());
            return str;
        }

        private void iprintlnbold(Entity player, string message)
        {
            player.Call("iprintlnbold", new Parameter[1]
            {
        (Parameter) message
            });
        }

        private bool IsImmune(Entity player)
        {
            return this.ImmunePlayer.Contains(player.GUID.ToString());
        }

        private void getslot(string message, Entity player)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (this.IsImmune(byName))
                    this.TellClient(player, "^3" + byName.Name + " ^1is immune to that command.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    Utilities.ExecuteCommand("dropclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Sorry Slot\"");
                    this.ServerSay("^2<playername> ^3has been kicked ^7for ^1Slot ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    Utilities.ExecuteCommand("dropclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Sorry Slot\"");
                    this.ServerSay("^2<playername> ^3has been kicked ^7for ^1Slot ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                }
            }
        }

        private void changemap(string message, Entity player)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a map name");
            }
            else
            {
                string mapByName = this.FindMapByName(strArray[1]);
                if (mapByName == null)
                {
                    this.TellClient(player, "^1That map wasn't found or multiple were found.");
                    this.sa3id = "";
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    this.sa3id = "";
                    this.sa3id = mapByName;
                }
            }
        }

        private void kick(string message, Entity player)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string newValue = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (this.IsImmune(byName))
                    this.TellClient(player, "^3" + byName.Name + " ^1is immune to that command.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        newValue = newValue + " " + strArray[index];
                    Utilities.ExecuteCommand("dropclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]) + " \"" + newValue + "\"");
                    string str1 = "^2<playername> ^3has been kicked ^7for ^1<reason> ^7by ^1<kicker>";
                    foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                    {
                        if (str2.StartsWith("kickmessage"))
                            str1 = str2.Split(new char[1]
                            {
                '='
                            })[1];
                    }
                    this.ServerSay(str1.Replace("<playername>", byName.Name).Replace("<reason>", newValue).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    Utilities.ExecuteCommand("dropclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]) + " \"^2Shoma kick shodid\"");
                    this.ServerSay("^2<playername> ^3has been kicked ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                }
            }
        }

        private void NextMap(object source, ElapsedEventArgs e)
        {
            string message = "";
            this.Maps.TryGetValue(this.contmap, out message);
            this.TellClient(this.mapEnt, message);
            ++this.contmap;
            if (this.contmap < this.Maps.Count)
                return;
            this.contmap = 0;
            this.map1.Dispose();
        }

        private void welcome()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("welcomer"))
                    this.welcomer = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void LoadTeamNames()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("Team1"))
                    this.Team1 = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("Team2"))
                    this.Team2 = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void wlpmer()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("pm"))
                    this.wlpm = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void maplistload()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("vmaps"))
                    this.vmap = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void modlistload()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("vmod"))
                    this.vmod = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void maxping()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("maxping"))
                    this.maxpingi = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void votetimecfg()
        {
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("votetime"))
                    this.vtime = int.Parse(str.Split(new char[1]
                    {
            '='
                    })[1]);
                else
                    this.vtime = 60;
            }
        }

        private void badwordcfg()
        {
            this.kob = "";
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("badword"))
                    this.kob = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void promod()
        {
            this.promode = "";
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("promode"))
                    this.promode = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void BotNameload()
        {
            this.BotName = "";
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("botname"))
                    this.BotName = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private void autoregister()
        {
            this.autoreg = "";
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith("autoreg"))
                    this.autoreg = str.Split(new char[1]
                    {
            '='
                    })[1];
            }
        }

        private bool XlrCheck(Entity player)
        {
            foreach (KeyValuePair<string, string> keyValuePair in this.xlrstats)
            {
                if (keyValuePair.Key == player.Name)
                    return false;
            }
            this.AddToXlrStats(player);
            return true;
        }

        private void addbot()
        {
            Utilities.AddTestClient();
        }

        private void playerConnecting(Entity player)
        {
            if (this.welcomelist.Contains(player.Name))
                return;
            IAM.Print("try connect " + player.Name + " GUID : " + player.GUID.ToString(), new object[0]);
            this.loadplist();
        }

        private void playerConnected(Entity player)
        {
            this.Entitys.Add(player);
            if (!this.welcomelist.Contains(player.Name))
            {
                this.AddToPlayers(player);
                int num = 0;
                StreamReader streamReader = new StreamReader("scripts\\\\IAM\\\\xlrstats");
                string str1;
                while ((str1 = streamReader.ReadLine()) != null)
                {
                    if (str1.Contains(player.Name))
                        this.num = "#" + num.ToString();
                    ++num;
                }
                streamReader.Close();
                this.welcome();
                this.wlpmer();
                string str2 = this.WelcomeMessage(player).Replace("<playername>", player.Name).Replace("<rank>", this.GetRankByName(player.Name));
                string message = !(this.num == "") ? str2.Replace("<playernumber>", this.num) : str2.Replace("<playernumber>", "Not Registered");
                if (this.welcomer == "true")
                {
                    if (this.wlpm == "true")
                        this.TellClient(player, message);
                    else
                        this.ServerSay(message);
                }
                this.num = "";
            }
            this.promod();
            if (this.promode == "true")
            {
                player.SetClientDvar("cg_fov", "80");
                player.SetClientDvar("cg_scoreboardpingtext", "1");
                player.SetClientDvar("waypointIconHeight", "13");
                player.SetClientDvar("waypointIconWidth", "13");
                player.SetClientDvar("cl_maxpackets", "60");
                player.SetClientDvar("r_fog", "1");
                player.SetClientDvar("fx_drawclouds", "1");
                player.SetClientDvar("r_distortion", "1");
                player.SetClientDvar("r_dlightlimit", "4");
                player.SetClientDvar("cg_brass", "1");
                player.SetClientDvar("snaps", "20");
                player.SetClientDvar("com_maxfps", "91");
                player.SetClientDvar("clientsideeffects", "1");
            }
            else if (this.promode == "false")
            {
                player.SetClientDvar("r_filmtweakdarktint", "0.7 0.85 1");
                player.SetClientDvar("r_filmtweakcontrast", "1.4");
                player.SetClientDvar("r_filmtweakdesaturation", "0.2");
                player.SetClientDvar("r_filmusetweaks", "0");
                player.SetClientDvar("r_filmtweaklighttint", "1.1 1.05 0.85");
                player.SetClientDvar("cg_fov", "66");
                player.SetClientDvar("cg_scoreboardpingtext", "1");
                player.SetClientDvar("waypointIconHeight", "13");
                player.SetClientDvar("waypointIconWidth", "13");
                player.SetClientDvar("cl_maxpackets", "100");
                player.SetClientDvar("r_fog", "0");
                player.SetClientDvar("fx_drawclouds", "0");
                player.SetClientDvar("r_distortion", "0");
                player.SetClientDvar("r_dlightlimit", "0");
                player.SetClientDvar("cg_brass", "0");
                player.SetClientDvar("snaps", "30");
                player.SetClientDvar("com_maxfps", "100");
                player.SetClientDvar("clientsideeffects", "0");
                player.SetClientDvar("r_filmTweakBrightness", "0.2");
            }
            else if (this.promode == "icon")
            {
                player.SetClientDvar("waypointIconHeight", "13");
                player.SetClientDvar("waypointIconWidth", "13");
                player.SetClientDvar("cg_scoreboardpingtext", "1");
            }
            if (this.GetRankByName(player.Name) == "MasterAdmin")
            {
                IAM iam = this;
                string str = iam.admins + player.Name + "^7[^5MA^7]^3,^7";
                iam.admins = str;
            }
            else if (this.GetRankByName(player.Name) == "Admin")
            {
                IAM iam = this;
                string str = iam.admins + player.Name + "^7[^2A^7]^3,^7";
                iam.admins = str;
            }
            else if (this.GetRankByName(player.Name) == "Member")
            {
                IAM iam = this;
                string str = iam.admins + player.Name + "^7[^3M^7]^3,^7";
                iam.admins = str;
            }
            else if (!(this.GetRankByName(player.Name) == "User"))
            {
                IAM iam = this;
                string str = iam.admins + player.Name + "^7[^1X^7]^3,^7";
                iam.admins = str;
            }
            player.SetClientDvar("g_TeamName_Allies", this.Team2);
            player.SetClientDvar("g_TeamName_Axis", this.Team1);
            this.autoregister();
            if (this.autoreg == "true")
            {
                this.XlrCheck(player);
                this.UpdateXlrStats();
            }
            this.addplayerTowarn(player);
            this.addplayerToantispammer(player);
        }

        private void playerDisconnected(Entity player)
        {
            this.loadplist();
            this.RemoveFromPlayers(player);
            this.RemoveToAdmins(player);
            if (this.GetRankByName(player.Name) == "MasterAdmin")
                this.admins = this.admins.Replace(player.Name + "^7[^5MA^7]^3,^7", "");
            else if (this.GetRankByName(player.Name) == "Admin")
                this.admins = this.admins.Replace(player.Name + "^7[^2A^7]^3,^7", "");
            else if (this.GetRankByName(player.Name) == "Member")
                this.admins = this.admins.Replace(player.Name + "^7[^3M^7]^3,^7", "");
            else if (!(this.GetRankByName(player.Name) == "User"))
                this.admins = this.admins.Replace(player.Name + "^7[^1X^7]^3,^7", "");
            this.BadPings.Remove(player);
            IAM.Print(string.Concat(new object[4]
            {
        (object) "left ",
        (object) player.Name,
        (object) " GUID : ",
        (object) player.GUID
            }), new object[0]);
            this.Entitys.Remove(player);
            this.warnings.Remove(player.GUID.ToString());
            if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "0")
                this.killed1 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "1")
                this.killed2 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "2")
                this.killed3 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "3")
                this.killed4 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "4")
                this.killed5 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "5")
                this.killed6 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "6")
                this.killed7 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "7")
                this.killed8 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "8")
                this.killed9 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "9")
                this.killed10 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "10")
                this.killed11 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "11")
                this.killed12 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "12")
                this.killed13 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "13")
                this.killed14 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "14")
                this.killed15 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "15")
                this.killed16 = "";
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "16")
            {
                this.killed17 = "";
            }
            else
            {
                if (!(player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "17"))
                    return;
                this.killed1 = "";
            }
        }

        private static void Print(string format, params object[] p)
        {
            Log.Write(InfinityScript.LogLevel.All, format, p);
        }

        private void slot(Entity player)
        {
            int num = 0;
            Entity entity1 = (Entity)null;
            foreach (Entity entity2 in this.Entitys)
            {
                if (entity2.Ping > num)
                {
                    entity1 = entity2;
                    num = entity2.Ping;
                }
            }
            if (entity1 != null)
                Utilities.ExecuteCommand("dropclient " + (object)entity1.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Your Ping is too high and we need slot :)\"");
            else
                this.TellClient(player, "^1Could not find suitable client to kick.");
        }

        private void TellClient(Entity player, string message)
        {
            Utilities.RawSayTo(player, this.BotName + "^3[PM] ^7: " + message);
        }

        private void inf(Entity player, string message)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    this.TellClient(player, "^5" + (object)byName.Name + "^7: ^2GUID : ^1" + (string)(object)byName.GUID + " ^2Ping : ^1" + (string)(object)byName.Ping + " ^2Life : ^1" );
                    this.TellClient(player, "--> ^2Health : ^1" + (object)byName.Health + " ^2Ip : ^1" + (string)(object)byName.IP + " ^2Weapon : ^1" + byName.CurrentWeapon);
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    this.TellClient(player, "^5" + (object)byName.Name + "^7: ^2GUID : ^1" + (string)(object)byName.GUID + " ^2Ping : ^1" + (string)(object)byName.Ping + " ^2Life : ^1" );
                    this.TellClient(player, "--> ^2Health : ^1" + (object)byName.Health + " ^2Ip : ^1" + (string)(object)byName.IP + " ^2Weapon : ^1" + byName.CurrentWeapon);
                }
            }
        }

        private void Hud(Entity player, string message)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    HudElem fontString = HudElem.CreateFontString(byName, "hudsmall", 1f);
                    fontString.SetPoint("TOP", "TOP", 0, 400);
                    fontString.SetText(message.Replace(strArray[0], "").Replace(strArray[1], ""));
                    fontString.SetField("glowcolor", (Parameter)new Vector3(1f, 0.4f, 0.0f));
                    fontString.Alpha = 1f;
                    fontString.GlowAlpha = 0.6f;
                    fontString.Call("SetPulseFX", (Parameter)100, (Parameter)7000, (Parameter)600);
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    HudElem fontString = HudElem.CreateFontString(byName, "hudsmall", 1f);
                    fontString.SetPoint("TOP", "TOP", 0, 400);
                    fontString.SetText(message.Replace(strArray[0], "").Replace(strArray[1], ""));
                    fontString.SetField("glowcolor", (Parameter)new Vector3(1f, 0.4f, 0.0f));
                    fontString.Alpha = 1f;
                    fontString.GlowAlpha = 0.6f;
                    fontString.Call("SetPulseFX", (Parameter)100, (Parameter)7000, (Parameter)600);
                }
            }
        }

        private void block(Entity player, string message)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str1 = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str1 = str1 + " " + strArray[index];
                    IAM iam = this;
                    string str2 = iam.visitors + byName.Name + ",";
                    iam.visitors = str2;
                    this.ServerSay("^2Chat Blocked for ^1" + byName.Name);
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    IAM iam = this;
                    string str2 = iam.visitors + byName.Name + ",";
                    iam.visitors = str2;
                    this.ServerSay("^2Chat Blocked for ^1" + byName.Name);
                }
            }
        }

        private void unblock(Entity player, string message)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    this.visitors = this.visitors.Replace(byName.Name + ",", "");
                    this.ServerSay("^2Chat Unblocked for ^1" + byName.Name);
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    this.visitors = this.visitors.Replace(byName.Name + ",", "");
                    this.ServerSay("^2Chat Unblocked for ^1" + byName.Name);
                }
            }
        }

        private void setteam(Entity player, string message)
        {
            string str = "axis,allies,spectator";
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (!str.Contains(strArray[2]))
                {
                    this.TellClient(player, "^1That team name is wrong enter true name | Team Names : " + str);
                }
                else
                {
                    byName.SetField("team", (Parameter)strArray[2]);
                    byName.SetField("sessionteam", (Parameter)strArray[2]);
                    this.TellClient(player, "^2His team changed to ^7" + strArray[2]);
                    this.TellClient(byName, "^2Your team changed to ^7" + strArray[2]);
                    byName.Notify("menuresponse", (Parameter)"team_marinesopfor", (Parameter)strArray[2]);
                }
            }
        }

        private void PM(Entity player, string message)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    Utilities.RawSayTo(byName, this.BotName + "^3[PM]^7:^2 " + player.Name + " : ^1" + message.Replace(strArray[0], "").Replace(strArray[1], ""));
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    Utilities.RawSayTo(byName, this.BotName + "^3[PM]^7:^2 " + player.Name + " : ^1" + message.Replace(strArray[0], "").Replace(strArray[1], ""));
                }
            }
        }

        private void TimedMessages()
        {
            string str = "";
            foreach (string str1 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str1.StartsWith("timedmessages"))
                    str = str1.Split(new char[1]
                    {
            '='
                    })[1];
            }
            if (!(str == "true"))
                return;
            string[] strArray = File.ReadAllLines("scripts\\\\IAM\\\\timedmessages.txt");
            this.intervalMesages = Convert.ToInt32(strArray[0]);
            for (int index = 1; index < strArray.Length; ++index)
                this.timedmessages.Add(index - 1, strArray[index]);
            this.OnInterval(this.intervalMesages, (Func<bool>)(() =>
            {
                str = "";
                this.timedmessages.TryGetValue(this.contermessa, out str);
                if (str == "@nextmap")
                {
                    string local_0_37 = this.Call<string>("getdvar", new Parameter[1]
                    {
            (Parameter) "nextmap"
                    }).Replace("*", "(Random)").Replace("_default", "").Replace("mp_dome", "Dome").Replace("mp_paris", "Resistance").Replace("mp_village", "Village").Replace("mp_bootleg", "Bootleg").Replace("mp_carbon", "Carbon").Replace("mp_interchange", "Interchange").Replace("mp_hardhat", "Hardhat").Replace("mp_exchange", "Downturn").Replace("mp_radar", "Outpost").Replace("mp_hillside_ss", "Gateway").Replace("mp_restrepo_ss", "Lookout").Replace("mp_overwatch", "Overwatch").Replace("mp_lambeth", "Fallen").Replace("mp_terminal_cls", "Terminal").Replace("mp_underground", "Underground").Replace("mp_plaza2", "Arkaden").Replace("mp_shipbreaker", "Decommision").Replace("mp_nola", "Parish").Replace("mp_roughneck", "Off Shore").Replace("mp_boardwalk", "Boardwalk").Replace("mp_italy", "Pizza").Replace("mp_moab", "Gulch").Replace("mp_cement", "Foundation").Replace("mp_morningwood", "Black Box").Replace("mp_meteora", "Sanctuary").Replace("mp_aground_ss", "Aground").Replace("mp_burn_ss", "U-Turn").Replace("mp_courtyard_ss", "Erosion").Replace("mp_park", "Liberation").Replace("mp_qadeem", "Oasis").Replace("mp_six_ss", "Vortex").Replace("mp_alpha", "Lockdown").Replace("mp_bravo", "Mission").Replace("mp_mogadishu", "Bakaara").Replace("mp_seatown", "Seatown");
                    if (this.snm)
                    {
                        char[] local_1 = new char[1]
                        {
              ' '
                        };
                        string[] local_2 = local_0_37.Split(local_1);
                        local_0_37 = local_0_37.Replace(local_2[0], this.sa3id);
                    }
                    char[] local_3 = new char[1]
                    {
            ' '
                    };
                    string[] local_4 = local_0_37.Split(local_3);
                    this.ServerSay("^3NextMap ^7: ^5" + local_0_37.Replace(local_4[1], "(" + local_4[1] + ")"));
                }
                else if (str == "@time")
                    this.ServerSay("^3Time ^7: ^5" + DateTime.Now.ToLongTimeString());
                else if (str == "@admins")
                {
                    if (this.admins == "")
                        this.ServerSay("^1There are no online admins");
                    else
                        this.ServerSay("^3Online ^3Admins ^7: " + this.admins);
                }
                else
                    this.ServerSay(str);
                if (this.contermessa >= this.timedmessages.Count)
                    this.contermessa = 0;
                ++this.contermessa;
                return true;
            }));
        }

        private void tmpban(string message, Entity player)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string newValue = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (this.IsImmune(byName))
                    this.TellClient(player, "^3" + byName.Name + " ^1is immune to that command.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        newValue = newValue + " " + strArray[index];
                    Utilities.ExecuteCommand("tempbanclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]) + " \"" + newValue + "\"");
                    string str1 = "^2<playername> ^3has been temp banned ^7for ^1<reason> ^7by ^1<kicker>";
                    foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                    {
                        if (str2.StartsWith("tempbanmessage"))
                            str1 = str2.Split(new char[1]
                            {
                '='
                            })[1];
                    }
                    this.ServerSay(str1.Replace("<playername>", byName.Name).Replace("<reason>", newValue).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    Utilities.ExecuteCommand("tempbanclient " + (object)byName.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Player TempBanned!\"");
                    this.ServerSay("^2<playername> ^3has been temp banned ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                }
            }
        }

        private void unwarn(Entity player, Entity target)
        {
            int num1 = 0;
            if (!this.warnings.TryGetValue(target.GUID.ToString(), out num1))
                return;
            if (num1 == 0)
            {
                this.TellClient(player, "^1This player doesn't have any warnings.");
            }
            else
            {
                int num2 = num1 - 1;
                this.warnings.Remove(target.GUID.ToString());
                this.warnings.Add(target.GUID.ToString(), num2);
                string str1 = "^1<playername> ^3was unwarned ^2now he have ^1<warncount> ^3of ^23";
                foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                {
                    if (str2.StartsWith("unwarnmessage"))
                        str1 = str2.Split(new char[1]
                        {
              '='
                        })[1];
                }
                this.ServerSay(str1.Replace("<playername>", target.Name).Replace("<warncount>", num2.ToString()));
            }
        }

        public void UpdateGroups()
        {
            this.Groups.Clear();
            foreach (KeyValuePair<string, string> keyValuePair in this.UserGroup)
            {
                string str;
                if (this.Groups.TryGetValue(keyValuePair.Value, out str))
                {
                    str = !(str == "") ? str + (object)',' + keyValuePair.Key : keyValuePair.Key;
                    this.Groups.Remove(keyValuePair.Value);
                    this.Groups.Add(keyValuePair.Value, str);
                }
                else
                    this.Groups.Add(keyValuePair.Value, keyValuePair.Key);
            }
            int index = 0;
            string[] contents = File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg");
            foreach (KeyValuePair<string, string> keyValuePair in this.Groups)
            {
                foreach (string str in contents)
                {
                    if (str.StartsWith(keyValuePair.Key + "_xuids"))
                    {
                        contents[index] = keyValuePair.Key + "_xuids=" + keyValuePair.Value;
                        break;
                    }
                }
                ++index;
            }
            contents[0] = "groups=" + this.listG;
            File.WriteAllLines("scripts\\\\IAM\\\\IAM.cfg", contents);
        }

        private void UpdateImmune()
        {
            int index = 0;
            string str1 = "";
            foreach (object obj in this.ImmunePlayer)
                str1 = (string)obj + (object)',';
            string[] contents = File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg");
            foreach (string str2 in contents)
            {
                if (str2.StartsWith("immuneplayers"))
                {
                    contents[index] = "immuneplayers=" + str1;
                    File.WriteAllLines("scripts\\\\IAM\\\\IAM.cfg", contents);
                    break;
                }
                else
                    ++index;
            }
        }

        private void warnspam(Entity player)
        {
            int num1 = 0;
            if (!this.warnings.TryGetValue(player.GUID.ToString(), out num1))
                return;
            int num2 = num1 + 1;
            if (num2 >= 3)
            {
                "^1<playername> ^3has been kicked ^7for ^2spam.".Replace("<playername>", player.Name);
                this.warnings.Remove(player.GUID.ToString());
                Utilities.ExecuteCommand("dropclient " + (object)player.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Don't Spam\"");
            }
            else
            {
                if (num2 >= 3)
                    return;
                this.warnings.Remove(player.GUID.ToString());
                this.warnings.Add(player.GUID.ToString(), num2);
                string str1 = "^3<playername> ^7: ^7Don't Spam , Shut Up.";
                foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                {
                    if (str2.StartsWith("warnspammsg"))
                        str1 = str2.Split(new char[1]
                        {
              '='
                        })[1];
                }
                this.ServerSay(str1.Replace("<playername>", player.Name));
            }
        }

        private void warnfake(Entity player)
        {
            int num1 = 0;
            if (!this.warnings.TryGetValue(player.GUID.ToString(), out num1))
                return;
            int num2 = num1 + 1;
            if (num2 >= 3)
            {
                "^1<playername> ^3has been kicked ^7for ^2spam by fake commands.".Replace("<playername>", player.Name);
                this.warnings.Remove(player.GUID.ToString());
                Utilities.ExecuteCommand("dropclient " + (object)player.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Don't Spam\"");
            }
            else
            {
                if (num2 >= 3)
                    return;
                this.warnings.Remove(player.GUID.ToString());
                this.warnings.Add(player.GUID.ToString(), num2);
                string str1 = "^3<playername> ^7: ^7Don't use fake commands.";
                foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                {
                    if (str2.StartsWith("warnfakemsg"))
                        str1 = str2.Split(new char[1]
                        {
              '='
                        })[1];
                }
                this.ServerSay(str1.Replace("<playername>", player.Name));
            }
        }

        private void warnping(Entity player)
        {
            int num1 = 0;
            if (!this.warnings.TryGetValue(player.GUID.ToString(), out num1))
                return;
            int num2 = num1 + 1;
            if (num2 >= 3)
            {
                "^1<playername> ^3has been kicked ^7beacuse ^2his ping is too high.".Replace("<playername>", player.Name);
                this.warnings.Remove(player.GUID.ToString());
                Utilities.ExecuteCommand("dropclient " + (object)player.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Your ping is too high for this server\"");
            }
            else
            {
                if (num2 >= 3)
                    return;
                this.warnings.Remove(player.GUID.ToString());
                this.warnings.Add(player.GUID.ToString(), num2);
                string str1 = "3<playername> ^7: ^7Your Ping is too high if you can fix it or you will be kicked.";
                foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                {
                    if (str2.StartsWith("maxpingmsg"))
                        str1 = str2.Split(new char[1]
                        {
              '='
                        })[1];
                }
                this.ServerSay(str1.Replace("<playername>", player.Name));
            }
        }

        private void warnlng(Entity player)
        {
            int num1 = 0;
            if (!this.warnings.TryGetValue(player.GUID.ToString(), out num1))
                return;
            int num2 = num1 + 1;
            if (num2 >= 3)
            {
                "^1<playername> ^3has been kicked ^7for ^2Bad Language.".Replace("<playername>", player.Name);
                this.warnings.Remove(player.GUID.ToString());
                Utilities.ExecuteCommand("dropclient " + (object)player.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : Bad Language\"");
            }
            else if (this.IsImmune(player))
            {
                this.TellClient(player, "^3" + player.Name + " ^1is immune to that command.");
            }
            else
            {
                if (num2 >= 3)
                    return;
                this.warnings.Remove(player.GUID.ToString());
                this.warnings.Add(player.GUID.ToString(), num2);
                string str1 = "2<playername> ^7: ^1Warned For Bad Language.";
                foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                {
                    if (str2.StartsWith("maxpingmsg"))
                        str1 = str2.Split(new char[1]
                        {
              '='
                        })[1];
                }
                this.ServerSay(str1.Replace("<playername>", player.Name));
            }
        }

        private void warn(Entity player, string reason)
        {
            int num1 = 0;
            if (!this.warnings.TryGetValue(player.GUID.ToString(), out num1))
                return;
            int num2 = num1 + 1;
            if (num2 >= 3)
            {
                "^1<playername> ^3has been kicked ^7beacuse ^2he has too many warnings.".Replace("<playername>", player.Name);
                this.warnings.Remove(player.GUID.ToString());
                Utilities.ExecuteCommand("dropclient " + (object)player.Call<int>("getentitynumber", new Parameter[0]) + " \"IAM : you have too many warnings\"");
            }
            else if (this.IsImmune(player))
            {
                this.TellClient(player, "^3" + player.Name + " ^1is immune to that command.");
            }
            else
            {
                if (num2 >= 3)
                    return;
                this.warnings.Remove(player.GUID.ToString());
                this.warnings.Add(player.GUID.ToString(), num2);
                string str1 = "<playername> ^3has been warned ^7for ^1<reason> ^7warning: ^2<warncount> of 3";
                foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                {
                    if (str2.StartsWith("warnmessage"))
                        str1 = str2.Split(new char[1]
                        {
              '='
                        })[1];
                }
                this.ServerSay(str1.Replace("<playername>", player.Name).Replace("<reason>", reason).Replace("<warncount>", num2.ToString()));
                this.iprintlnbold(player, string.Concat(new object[4]
                {
          (object) "You have been ^3Warned ^7for ^1",
          (object) reason,
          (object) " ^7Warning number:^1 ",
          (object) num2
                }));
            }
        }

        private void LoadXlrStats()
        {
            if (File.Exists("scripts\\\\IAM\\\\xlrstats"))
                return;
            File.Create("scripts\\\\IAM\\\\xlrstats").Close();
            File.WriteAllLines("scripts\\\\IAM\\\\xlrstats", new string[2]
            {
        "Users=nottouch",
        "nottouch=1500,1"
            });
        }

        private void GetXlrStats()
        {
            string[] strArray = File.ReadAllLines("scripts\\\\IAM\\\\xlrstats");
            string str1 = strArray[0].Split(new char[1]
            {
        '='
            })[1];
            char[] chArray = new char[1]
            {
        ','
            };
            foreach (string key in str1.Split(chArray))
            {
                if (key == "")
                    break;
                string str2 = "";
                foreach (string str3 in strArray)
                {
                    if (str3.StartsWith(key))
                        str2 = str3.Split(new char[1]
                        {
              '='
                        })[1];
                }
                this.xlrstats.Add(key, str2);
            }
        }

        private void UpdateXlrStats()
        {
            int index = 1;
            string str1 = "";
            foreach (KeyValuePair<string, string> keyValuePair in this.xlrstats)
                str1 = str1 + keyValuePair.Key + ",";
            string[] contents = new string[this.xlrstats.Count + 3];
            string[] strArray = str1.Split(new char[1]
            {
        ','
            });
            contents[0] = "Users=" + str1;
            foreach (string key in strArray)
            {
                string str2 = "";
                this.xlrstats.TryGetValue(key, out str2);
                contents[index] = key + "=" + str2;
                ++index;
            }
            File.WriteAllLines("scripts\\IAM\\xlrstats", contents);
        }

        private void AddToXlrStats(Entity player)
        {
            this.xlrstats.Add(player.Name, "1,1");
        }

        private bool Protected(Entity player)
        {
            string str1 = (string)null;
            foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str2.StartsWith("protect"))
                    str1 = str2.Split(new char[1]
                    {
            '='
                    })[1];
            }
            return str1.Contains(player.GUID.ToString());
        }

        private bool isenable(string type)
        {
            string str1 = (string)null;
            foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str2.StartsWith("spree"))
                    str1 = str2.Split(new char[1]
                    {
            '='
                    })[1];
            }
            return str1.Contains(type);
        }

        public bool antispamer()
        {
            string str1 = (string)null;
            foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str2.StartsWith("antispam"))
                    str1 = str2.Split(new char[1]
                    {
            '='
                    })[1];
            }
            return str1 == null || str1 == "true";
        }

        public int GetKillValue(string name)
        {
            int num = 5;
            foreach (string str in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str.StartsWith(name + "_value"))
                    num = Convert.ToInt32(str.Split(new char[1]
                    {
            '='
                    })[1]);
            }
            return num;
        }

        private string GetMessege(string name)
        {
            string str1 = (string)null;
            foreach (string str2 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str2.StartsWith(name + "_messege"))
                    str1 = str2.Split(new char[1]
                    {
            '='
                    })[1];
            }
            return str1;
        }

        private void zerokill()
        {
            this.laskill = 0;
            this.AfterDelay(3000, new Action(this.zerokill));
        }

        private void GetPlayerStats(Entity player, string message)
        {
            string[] strArray = message.Split(new char[1]
            {
        ' '
            });
            string str = "";
            if (strArray.Length <= 1)
            {
                this.TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = this.FindByName(strArray[1]);
                if (byName == null)
                    this.TellClient(player, "^1This Player Not Registred on xlrstats.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    foreach (KeyValuePair<string, string> keyValuePair in this.xlrstats)
                    {
                        if (keyValuePair.Key.Equals(byName.Name))
                        {
                            double num1 = Convert.ToDouble(keyValuePair.Value.Split(new char[1]
                            {
                ','
                            })[0]);
                            double num2 = Convert.ToDouble(keyValuePair.Value.Split(new char[1]
                            {
                ','
                            })[1]);
                            double num3 = num1 / num2;
                            this.TellClient(player, "^2" + byName.Name + " stats: ^7K ^1" + num1.ToString() + "^7, D ^1" + num2.ToString() + "^7, K/D ^1" + num1.ToString());
                        }
                    }
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    foreach (KeyValuePair<string, string> keyValuePair in this.xlrstats)
                    {
                        if (keyValuePair.Key.Equals(byName.Name))
                        {
                            double num1 = Convert.ToDouble(keyValuePair.Value.Split(new char[1]
                            {
                ','
                            })[0]);
                            double num2 = Convert.ToDouble(keyValuePair.Value.Split(new char[1]
                            {
                ','
                            })[1]);
                            double num3 = num1 / num2;
                            this.TellClient(player, "^2" + byName.Name + " stats: ^7K ^1" + num1.ToString() + "^7, D ^1" + num2.ToString() + "^7, K/D ^1" + num1.ToString());
                        }
                    }
                }
            }
        }

        public override void OnExitLevel()
        {
            //this.KnifeClass.EnableKnife();
            if (!this.snm)
                return;
            if (this.sa3id == "dome")
                Utilities.ExecuteCommand("map mp_dome");
            else if (this.sa3id == "bootleg")
                Utilities.ExecuteCommand("map mp_bootleg");
            else if (this.sa3id == "hardhat")
                Utilities.ExecuteCommand("map mp_hardhat");
            else if (this.sa3id == "lockdown")
                Utilities.ExecuteCommand("map mp_alpha");
            else if (this.sa3id == "mission")
                Utilities.ExecuteCommand("map mp_bravo");
            else if (this.sa3id == "carbon")
                Utilities.ExecuteCommand("map mp_carbon");
            else if (this.sa3id == "downturn")
                Utilities.ExecuteCommand("map mp_exchange");
            else if (this.sa3id == "interchange")
                Utilities.ExecuteCommand("map mp_interchange");
            else if (this.sa3id == "fallen")
                Utilities.ExecuteCommand("map mp_lambeth");
            else if (this.sa3id == "bakaraa")
                Utilities.ExecuteCommand("map mp_mogadishu");
            else if (this.sa3id == "resistance")
                Utilities.ExecuteCommand("map mp_paris");
            else if (this.sa3id == "arkaden")
                Utilities.ExecuteCommand("map mp_plaza2");
            else if (this.sa3id == "outpost")
                Utilities.ExecuteCommand("map mp_radar");
            else if (this.sa3id == "seatown")
                Utilities.ExecuteCommand("map mp_seatown");
            else if (this.sa3id == "underground")
                Utilities.ExecuteCommand("map mp_underground");
            else if (this.sa3id == "village")
                Utilities.ExecuteCommand("map mp_village");
            else if (this.sa3id == "terminal")
                Utilities.ExecuteCommand("map mp_terminal_cls");
            else if (this.sa3id == "overwatch")
                Utilities.ExecuteCommand("map mp_overwatch");
            else if (this.sa3id == "liberation")
                Utilities.ExecuteCommand("map mp_park");
            else if (this.sa3id == "pizza")
                Utilities.ExecuteCommand("map mp_italy");
            else if (this.sa3id == "blackbox")
                Utilities.ExecuteCommand("map mp_morningwood");
            else if (this.sa3id == "sanctuary")
                Utilities.ExecuteCommand("map mp_meteora");
            else if (this.sa3id == "foundation")
                Utilities.ExecuteCommand("map mp_cement");
            else if (this.sa3id == "oasis")
                Utilities.ExecuteCommand("map mp_qadeem");
            else if (this.sa3id == "aground")
                Utilities.ExecuteCommand("map mp_aground_ss");
            else if (this.sa3id == "erosion")
                Utilities.ExecuteCommand("map mp_courtyard_ss");
            else if (this.sa3id == "gateway")
                Utilities.ExecuteCommand("map mp_hillside_ss");
            else if (this.sa3id == "lookout")
                Utilities.ExecuteCommand("map mp_restrepo_ss");
            else if (this.sa3id == "uturn")
                Utilities.ExecuteCommand("map mp_burn_ss");
            else if (this.sa3id == "intersection")
                Utilities.ExecuteCommand("map mp_crosswalk_ss");
            else if (this.sa3id == "vortex")
                Utilities.ExecuteCommand("map mp_six_ss");
            else if (this.sa3id == "decommission")
                Utilities.ExecuteCommand("map mp_shipbreaker");
            else if (this.sa3id == "offshore")
                Utilities.ExecuteCommand("map mp_roughneck");
            else if (this.sa3id == "gulch")
                Utilities.ExecuteCommand("map mp_moab");
            else if (this.sa3id == "boardwalk")
                Utilities.ExecuteCommand("map mp_boardwalk");
            else if (this.sa3id == "parish")
                Utilities.ExecuteCommand("map mp_nola");
            this.snm = false;
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            string str1 = "";
            if (this.xlrstats.TryGetValue(player.Name, out str1))
            {
                int num = Convert.ToInt32(str1.Split(new char[1]
                {
          ','
                })[1]);
                string str2 = Convert.ToInt32(str1.Split(new char[1]
                {
          ','
                })[0]).ToString() + "," + (num + 1).ToString();
                this.xlrstats.Remove(player.Name);
                this.xlrstats.Add(player.Name, str2);
            }
            if (this.xlrstats.TryGetValue(attacker.Name, out str1))
            {
                string str2 = (Convert.ToInt32(str1.Split(new char[1]
                {
          ','
                })[0]) + 1).ToString() + "," + Convert.ToInt32(str1.Split(new char[1]
                {
          ','
                })[1]).ToString();
                this.xlrstats.Remove(attacker.Name);
                this.xlrstats.Add(attacker.Name, str2);
            }
            this.UpdateXlrStats();
            string messege1 = this.GetMessege("endspree");
            int num1 = player.Call<int>("getentitynumber", new Parameter[0]);
            if (num1.ToString() == "0")
            {
                if (this.hekilled >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled = 0;
                this.killed1 = "";
                this.killed1 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "1")
            {
                if (this.hekilled1 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled1 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled1.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled1 = 0;
                this.killed2 = "";
                this.killed2 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "2")
            {
                if (this.hekilled2 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled2 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled2.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled2 = 0;
                this.killed3 = "";
                this.killed3 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "3")
            {
                if (this.hekilled3 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled3 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled3.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled3 = 0;
                this.killed4 = "";
                this.killed4 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "4")
            {
                if (this.hekilled4 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled4 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled4.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled4 = 0;
                this.killed5 = "";
                this.killed5 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "5")
            {
                if (this.hekilled5 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled5 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled5.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled5 = 0;
                this.killed6 = "";
                this.killed6 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "6")
            {
                if (this.hekilled6 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled6 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled6.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled6 = 0;
                this.killed7 = "";
                this.killed7 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "7")
            {
                if (this.hekilled7 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled7 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled7.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled7 = 0;
                this.killed8 = "";
                this.killed8 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "8")
            {
                if (this.hekilled8 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled8 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled8.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled8 = 0;
                this.killed9 = "";
                this.killed9 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "9")
            {
                if (this.hekilled9 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled9 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled9.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled9 = 0;
                this.killed10 = "";
                this.killed10 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "10")
            {
                if (this.hekilled10 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled10 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled10.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled10 = 0;
                this.killed11 = "";
                this.killed11 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "11")
            {
                if (this.hekilled11 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled11 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled11.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled11 = 0;
                this.killed12 = "";
                this.killed12 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "12")
            {
                if (this.hekilled12 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled12 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled12.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled12 = 0;
                this.killed13 = "";
                this.killed13 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "13")
            {
                if (this.hekilled13 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled13 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled13.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled13 = 0;
                this.killed14 = "";
                this.killed14 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "14")
            {
                if (this.hekilled14 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled14 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled14.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled14 = 0;
                this.killed15 = "";
                this.killed15 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "15")
            {
                if (this.hekilled15 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled15 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled15.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled15 = 0;
                this.killed16 = "";
                this.killed16 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "16")
            {
                if (this.hekilled16 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled16 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled16.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled16 = 0;
                this.killed17 = "";
                this.killed17 = "^2" + attacker.Name + " ^1Killed You";
            }
            else if (player.Call<int>("getentitynumber", new Parameter[0]).ToString() == "17")
            {
                if (this.hekilled17 >= this.GetKillValue("endspree"))
                {
                    if (messege1 == "")
                        this.ServerSay("^2" + (object)player.Name + "^1's killing spree ended (^2" + (string)(object)this.hekilled17 + " ^1kills). He was killed 8by ^3" + attacker.Name + "^1!");
                    else
                        this.ServerSay(messege1.Replace("<playername>", player.Name).Replace("<numkills>", this.hekilled17.ToString()).Replace("<killer>", attacker.Name));
                }
                this.hekilled17 = 0;
                this.killed18 = "";
                this.killed18 = "^2" + attacker.Name + " ^1Killed You";
            }
            if (this.isenable("multikill"))
            {
                this.lastkiller = attacker;
                ++this.laskill;
                if (this.checkname == attacker.Name)
                {
                    if (this.laskill == this.GetKillValue("multikill"))
                    {
                        string messege2 = this.GetMessege("multikill");
                        if (messege2 == "")
                            this.ServerSay("^3" + attacker.Name + " ^7He Gets Multi kill!");
                        else
                            this.ServerSay(messege2.Replace("<playername>", attacker.Name));
                    }
                    else if (this.laskill >= this.GetKillValue("monsterkill"))
                    {
                        string messege2 = this.GetMessege("monsterkill");
                        if (messege2 == "")
                            this.ServerSay("^3" + attacker.Name + " ^7He Gets Monster kill!");
                        else
                            this.ServerSay(messege2.Replace("<playername>", attacker.Name));
                    }
                }
                else
                    this.laskill = 0;
                this.checkname = attacker.Name;
            }
            if (this.isenable("fristkill"))
            {
                string messege2 = this.GetMessege("fristkill");
                if (!this.fristkill)
                {
                    if (messege2 == "")
                    {
                        this.ServerSay("^3" + attacker.Name + " ^1drew first blood!");
                        this.fristkill = true;
                    }
                    else
                    {
                        this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled.ToString()));
                        this.fristkill = true;
                    }
                }
            }
            if (this.isenable("rowkill"))
            {
                string messege2 = this.GetMessege("rowkill5");
                string messege3 = this.GetMessege("rowkill10");
                if (attacker.Call<int>("getentitynumber", new Parameter[0]).ToString() == "0")
                {
                    ++this.hekilled;
                    if (this.hekilled == this.GetKillValue("firstrow"))
                    {
                        if (messege2 == "")
                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled.ToString() + " Kills in Row^3)");
                        else
                            this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled.ToString()));
                    }
                    if (this.hekilled == this.GetKillValue("secondrow"))
                    {
                        if (messege3 == "")
                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled.ToString() + " Kills in Row^3)");
                        else
                            this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled.ToString()));
                    }
                }
                else if (attacker.Call<int>("getentitynumber", new Parameter[0]).ToString() == "1")
                {
                    ++this.hekilled1;
                    if (this.hekilled1 == this.GetKillValue("firstrow"))
                    {
                        if (messege2 == "")
                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled1.ToString() + " Kills in Row^3)");
                        else
                            this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled1.ToString()));
                    }
                    if (this.hekilled == this.GetKillValue("secondrow"))
                    {
                        if (messege3 == "")
                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled1.ToString() + " Kills in Row^3)");
                        else
                            this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled1.ToString()));
                    }
                }
                else
                {
                    num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                    if (num1.ToString() == "2")
                    {
                        ++this.hekilled2;
                        if (this.hekilled2 == this.GetKillValue("firstrow"))
                        {
                            if (messege2 == "")
                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled2.ToString() + " Kills in Row^3)");
                            else
                                this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled2.ToString()));
                        }
                        if (this.hekilled == this.GetKillValue("secondrow"))
                        {
                            if (messege3 == "")
                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled2.ToString() + " Kills in Row^3)");
                            else
                                this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled2.ToString()));
                        }
                    }
                    else
                    {
                        num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                        if (num1.ToString() == "3")
                        {
                            ++this.hekilled3;
                            if (this.hekilled3 == this.GetKillValue("firstrow"))
                            {
                                if (messege2 == "")
                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled3.ToString() + " Kills in Row^3)");
                                else
                                    this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled3.ToString()));
                            }
                            if (this.hekilled == this.GetKillValue("secondrow"))
                            {
                                if (messege3 == "")
                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled3.ToString() + " Kills in Row^3)");
                                else
                                    this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled3.ToString()));
                            }
                        }
                        else
                        {
                            num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                            if (num1.ToString() == "4")
                            {
                                ++this.hekilled4;
                                if (this.hekilled4 == this.GetKillValue("firstrow"))
                                {
                                    if (messege2 == "")
                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled4.ToString() + " Kills in Row^3)");
                                    else
                                        this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled4.ToString()));
                                }
                                if (this.hekilled == this.GetKillValue("secondrow"))
                                {
                                    if (messege3 == "")
                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled4.ToString() + " Kills in Row^3)");
                                    else
                                        this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled4.ToString()));
                                }
                            }
                            else
                            {
                                num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                if (num1.ToString() == "5")
                                {
                                    ++this.hekilled5;
                                    if (this.hekilled5 == this.GetKillValue("firstrow"))
                                    {
                                        if (messege2 == "")
                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled5.ToString() + " Kills in Row^3)");
                                        else
                                            this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled5.ToString()));
                                    }
                                    if (this.hekilled == this.GetKillValue("secondrow"))
                                    {
                                        if (messege3 == "")
                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled5.ToString() + " Kills in Row^3)");
                                        else
                                            this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled5.ToString()));
                                    }
                                }
                                else
                                {
                                    num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                    if (num1.ToString() == "6")
                                    {
                                        ++this.hekilled6;
                                        if (this.hekilled6 == this.GetKillValue("firstrow"))
                                        {
                                            if (messege2 == "")
                                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled6.ToString() + " Kills in Row^3)");
                                            else
                                                this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled6.ToString()));
                                        }
                                        if (this.hekilled == this.GetKillValue("secondrow"))
                                        {
                                            if (messege3 == "")
                                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled6.ToString() + " Kills in Row^3)");
                                            else
                                                this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled6.ToString()));
                                        }
                                    }
                                    else
                                    {
                                        num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                        if (num1.ToString() == "7")
                                        {
                                            ++this.hekilled7;
                                            if (this.hekilled7 == this.GetKillValue("firstrow"))
                                            {
                                                if (messege2 == "")
                                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled7.ToString() + " Kills in Row^3)");
                                                else
                                                    this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled7.ToString()));
                                            }
                                            if (this.hekilled == this.GetKillValue("secondrow"))
                                            {
                                                if (messege3 == "")
                                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled7.ToString() + " Kills in Row^3)");
                                                else
                                                    this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled7.ToString()));
                                            }
                                        }
                                        else
                                        {
                                            num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                            if (num1.ToString() == "8")
                                            {
                                                ++this.hekilled8;
                                                if (this.hekilled8 == this.GetKillValue("firstrow"))
                                                {
                                                    if (messege2 == "")
                                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled8.ToString() + " Kills in Row^3)");
                                                    else
                                                        this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled8.ToString()));
                                                }
                                                if (this.hekilled == this.GetKillValue("secondrow"))
                                                {
                                                    if (messege3 == "")
                                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled8.ToString() + " Kills in Row^3)");
                                                    else
                                                        this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled8.ToString()));
                                                }
                                            }
                                            else
                                            {
                                                num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                if (num1.ToString() == "9")
                                                {
                                                    ++this.hekilled9;
                                                    if (this.hekilled9 == this.GetKillValue("firstrow"))
                                                    {
                                                        if (messege2 == "")
                                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled9.ToString() + " Kills in Row^3)");
                                                        else
                                                            this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled9.ToString()));
                                                    }
                                                    if (this.hekilled == this.GetKillValue("secondrow"))
                                                    {
                                                        if (messege3 == "")
                                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled9.ToString() + " Kills in Row^3)");
                                                        else
                                                            this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled9.ToString()));
                                                    }
                                                }
                                                else
                                                {
                                                    num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                    if (num1.ToString() == "10")
                                                    {
                                                        ++this.hekilled10;
                                                        if (this.hekilled10 == this.GetKillValue("firstrow"))
                                                        {
                                                            if (messege2 == "")
                                                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled10.ToString() + " Kills in Row^3)");
                                                            else
                                                                this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled10.ToString()));
                                                        }
                                                        if (this.hekilled == this.GetKillValue("secondrow"))
                                                        {
                                                            if (messege3 == "")
                                                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled10.ToString() + " Kills in Row^3)");
                                                            else
                                                                this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled10.ToString()));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                        if (num1.ToString() == "11")
                                                        {
                                                            ++this.hekilled11;
                                                            if (this.hekilled11 == this.GetKillValue("firstrow"))
                                                            {
                                                                if (messege2 == "")
                                                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled11.ToString() + " Kills in Row^3)");
                                                                else
                                                                    this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled11.ToString()));
                                                            }
                                                            if (this.hekilled == this.GetKillValue("secondrow"))
                                                            {
                                                                if (messege3 == "")
                                                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled11.ToString() + " Kills in Row^3)");
                                                                else
                                                                    this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled11.ToString()));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                            if (num1.ToString() == "12")
                                                            {
                                                                ++this.hekilled12;
                                                                if (this.hekilled12 == this.GetKillValue("firstrow"))
                                                                {
                                                                    if (messege2 == "")
                                                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled12.ToString() + " Kills in Row^3)");
                                                                    else
                                                                        this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled12.ToString()));
                                                                }
                                                                if (this.hekilled == this.GetKillValue("secondrow"))
                                                                {
                                                                    if (messege3 == "")
                                                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled12.ToString() + " Kills in Row^3)");
                                                                    else
                                                                        this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled12.ToString()));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                                if (num1.ToString() == "13")
                                                                {
                                                                    ++this.hekilled13;
                                                                    if (this.hekilled13 == this.GetKillValue("firstrow"))
                                                                    {
                                                                        if (messege2 == "")
                                                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled13.ToString() + " Kills in Row^3)");
                                                                        else
                                                                            this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled13.ToString()));
                                                                    }
                                                                    if (this.hekilled == this.GetKillValue("secondrow"))
                                                                    {
                                                                        if (messege3 == "")
                                                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled13.ToString() + " Kills in Row^3)");
                                                                        else
                                                                            this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled13.ToString()));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                                    if (num1.ToString() == "14")
                                                                    {
                                                                        ++this.hekilled14;
                                                                        if (this.hekilled14 == this.GetKillValue("firstrow"))
                                                                        {
                                                                            if (messege2 == "")
                                                                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled14.ToString() + " Kills in Row^3)");
                                                                            else
                                                                                this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled14.ToString()));
                                                                        }
                                                                        if (this.hekilled == this.GetKillValue("secondrow"))
                                                                        {
                                                                            if (messege3 == "")
                                                                                this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled14.ToString() + " Kills in Row^3)");
                                                                            else
                                                                                this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled14.ToString()));
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                                        if (num1.ToString() == "15")
                                                                        {
                                                                            ++this.hekilled15;
                                                                            if (this.hekilled15 == this.GetKillValue("firstrow"))
                                                                            {
                                                                                if (messege2 == "")
                                                                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled15.ToString() + " Kills in Row^3)");
                                                                                else
                                                                                    this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled15.ToString()));
                                                                            }
                                                                            if (this.hekilled == this.GetKillValue("secondrow"))
                                                                            {
                                                                                if (messege3 == "")
                                                                                    this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled15.ToString() + " Kills in Row^3)");
                                                                                else
                                                                                    this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled15.ToString()));
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                                            if (num1.ToString() == "16")
                                                                            {
                                                                                ++this.hekilled16;
                                                                                if (this.hekilled16 == this.GetKillValue("firstrow"))
                                                                                {
                                                                                    if (messege2 == "")
                                                                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled16.ToString() + " Kills in Row^3)");
                                                                                    else
                                                                                        this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled16.ToString()));
                                                                                }
                                                                                if (this.hekilled == this.GetKillValue("secondrow"))
                                                                                {
                                                                                    if (messege3 == "")
                                                                                        this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled16.ToString() + " Kills in Row^3)");
                                                                                    else
                                                                                        this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled16.ToString()));
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                num1 = attacker.Call<int>("getentitynumber", new Parameter[0]);
                                                                                if (num1.ToString() == "17")
                                                                                {
                                                                                    ++this.hekilled17;
                                                                                    if (this.hekilled17 == this.GetKillValue("firstrow"))
                                                                                    {
                                                                                        if (messege2 == "")
                                                                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled17.ToString() + " Kills in Row^3)");
                                                                                        else
                                                                                            this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled17.ToString()));
                                                                                    }
                                                                                    if (this.hekilled == this.GetKillValue("secondrow"))
                                                                                    {
                                                                                        if (messege3 == "")
                                                                                            this.ServerSay("^3" + attacker.Name + " ^7Gets Spree Kills!^3(" + this.hekilled17.ToString() + " Kills in Row^3)");
                                                                                        else
                                                                                            this.ServerSay(messege3.Replace("<playername>", attacker.Name).Replace("<numkills>", this.hekilled17.ToString()));
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (this.isenable("headshot"))
            {
                string messege2 = this.GetMessege("headshot");
                if (mod == "MOD_HEAD_SHOT")
                {
                    if (messege2 == "")
                        this.ServerSay("^3" + attacker.Name + " ^7Killed ^1" + player.Name + " ^7By HeadShot");
                    else
                        this.ServerSay(messege2.Replace("<playername>", attacker.Name).Replace("<target>", player.Name));
                }
            }
            if (this.isenable("suicide"))
            {
                string messege2 = this.GetMessege("suicide");
                if (mod == "MOD_SUICIDE")
                {
                    if (messege2 == "")
                        this.ServerSay("^3" + attacker.Name + " ^7Was Depressed");
                    else
                        this.ServerSay(messege2.Replace("<playername>", attacker.Name));
                }
            }
            if (!this.isenable("explode") || !(mod == "MOD_EXPLOSIVE"))
                return;
            string messege4 = this.GetMessege("explode");
            if (messege4 == "")
                this.ServerSay("^3" + attacker.Name + " ^7His Ass Exploded!");
            else
                this.ServerSay(messege4.Replace("<playername>", attacker.Name));
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (this.isenable("falling"))
            {
                string messege = this.GetMessege("fall");
                if (mod == "MOD_FALLING" && damage >= 100)
                {
                    if (messege == "")
                        this.ServerSay("^3" + player.Name + " ^7Was Spider Man.");
                    else
                        this.ServerSay(messege.Replace("<playername>", player.Name));
                }
            }
            if (!this.isenable("knife"))
                return;
            string messege1 = this.GetMessege("knife");
            if (!(mod == "MOD_MELEE"))
                return;
            if (messege1 == "")
                this.ServerSay("^3" + attacker.Name + " ^7Humiliated ^1" + player.Name);
            else
                this.ServerSay(messege1.Replace("<playername>", attacker.Name).Replace("<target>", player.Name));
        }

        public override void OnStartGameType()
        {
        }

        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
            char[] chArray1 = new char[1]
            {
        ' '
            };
            string[] strArray1 = message.Split(chArray1);
            if (message.StartsWith("!") || message.StartsWith("@"))
            {
                string alias = "";// this.cloaker.getAlias(strArray1[0].ToLower());
                message = message.Replace(strArray1[0], alias);
            }
            string str1 = this.Call<string>("getdvar", new Parameter[1]
            {
        (Parameter) "mapname"
            });
            this.Call<string>("getdvar", new Parameter[1]
            {
        (Parameter) "sv_maprotation"
            });
            string[] strArray2 = message.Split(new char[1]
            {
        ' '
            });
            string str2 = "";
            foreach (string str3 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
            {
                if (str3.StartsWith("blockchat"))
                    str2 = str3.Split(new char[1]
                    {
            '='
                    })[1];
            }
            if (!this.IsImmune(player))
                this.testLang(message, player);
            long guid;
            if (this.antispamer())
            {
                if (this.GetRankByName(player.Name) == "User" && message.StartsWith("!"))
                {
                    int num1 = 0;
                    this.spams.TryGetValue(player.GUID.ToString(), out num1);
                    this.spams.Remove(player.GUID.ToString());
                    int num2 = num1 + 1;
                    this.spams.Add(player.GUID.ToString(), num2);
                    int num3 = 0;
                    Dictionary<string, int> dictionary1 = this.spams;
                    guid = player.GUID;
                    string key1 = guid.ToString();
                    // ISSUE: explicit reference operation
                    // ISSUE: variable of a reference type
                    int local = 0;
                    dictionary1.TryGetValue(key1, out local);
                    if (num3 / 4 >= 1)
                    {
                        this.warnfake(player);
                        Dictionary<string, int> dictionary2 = this.spams;
                        guid = player.GUID;
                        string key2 = guid.ToString();
                        dictionary2.Remove(key2);
                        Dictionary<string, int> dictionary3 = this.spams;
                        guid = player.GUID;
                        string key3 = guid.ToString();
                        int num4 = 0;
                        dictionary3.Add(key3, num4);
                    }
                }
                if (this.spammer == player.Name && this.GetRankByName(player.Name) == "User" && !message.StartsWith("!"))
                {
                    int num1 = 0;
                    Dictionary<string, int> dictionary1 = this.spams;
                    guid = player.GUID;
                    string key1 = guid.ToString();
                    // ISSUE: explicit reference operation
                    // ISSUE: variable of a reference type
                    int local1 = 0;
                    dictionary1.TryGetValue(key1, out local1);
                    Dictionary<string, int> dictionary2 = this.spams;
                    guid = player.GUID;
                    string key2 = guid.ToString();
                    dictionary2.Remove(key2);
                    int num2 = num1 + 1;
                    Dictionary<string, int> dictionary3 = this.spams;
                    guid = player.GUID;
                    string key3 = guid.ToString();
                    int num3 = num2;
                    dictionary3.Add(key3, num3);
                    int num4 = 0;
                    Dictionary<string, int> dictionary4 = this.spams;
                    guid = player.GUID;
                    string key4 = guid.ToString();
                    // ISSUE: explicit reference operation
                    // ISSUE: variable of a reference type
                    int local2 = @num4;
                    dictionary4.TryGetValue(key4, out local2);
                    if (num4 / 5 >= 1)
                    {
                        this.warnspam(player);
                        Dictionary<string, int> dictionary5 = this.spams;
                        guid = player.GUID;
                        string key5 = guid.ToString();
                        dictionary5.Remove(key5);
                        Dictionary<string, int> dictionary6 = this.spams;
                        guid = player.GUID;
                        string key6 = guid.ToString();
                        int num5 = 0;
                        dictionary6.Add(key6, num5);
                    }
                }
                this.spammer = player.Name;
            }
            if (strArray2[0] == "!ver")
            {
                this.TellClient(player, "^2IAM ^5Created By Sa3id ^1V^03.2 ^3Build 80 ^7 Check fourdeltaone.net for new updates and report bugs");
                return BaseScript.EventEat.EatGame;
            }
            else if (strArray2[0].Equals("!admintest"))
            {
                this.TellClient(player, "^2" + player.Name + " is ^1" + this.GetRankByName(player.Name));
                return BaseScript.EventEat.EatGame;
            }
            else
            {
                if (!message.StartsWith("!") && !message.StartsWith("@"))
                {
                    if (type == BaseScript.ChatType.All && str2 == "team")
                    {
                        this.TellClient(player, "^1Public Chat Blocked Use Team Chat");
                        return BaseScript.EventEat.EatGame;
                    }
                    else if (str2 == "true")
                    {
                        this.TellClient(player, "^1Chat is Blocked!");
                        return BaseScript.EventEat.EatGame;
                    }
                    else if (this.visitors.Contains(player.Name))
                    {
                        this.TellClient(player, "^1Your Chat is Blocked");
                        return BaseScript.EventEat.EatGame;
                    }
                    else if (str2 == "false")
                        return BaseScript.EventEat.EatNone;
                }
                if (message.StartsWith("!") && !this.canUseCommand(player, strArray2[0]) || message.StartsWith("@") && !this.canUseCommand(player, strArray2[0]))
                {
                    player.Call("iprintlnbold", new Parameter[1]
                    {
            (Parameter) "^1You don't have permission to use that command."
                    });
                    return BaseScript.EventEat.EatGame;
                }
                else if (this.heisadmin(player) || this.GetRankByName(player.Name) == "User")
                {
                    if (strArray2[0].Equals("!add"))
                    {
                        if (strArray2.Length <= 2)
                        {
                            this.TellClient(player, "^1enter a ^5group ^1and ^5playername");
                            return BaseScript.EventEat.EatGame;
                        }
                        else
                        {
                            Entity byName = this.FindByName(strArray2[2]);
                            if (byName == null)
                                this.TellClient(player, "^1That user wasn't found or multiple were found.");
                            else
                                this.AddToGroup(strArray2[1], player, byName);
                            return BaseScript.EventEat.EatGame;
                        }
                    }
                    else if (strArray2[0].Equals("!remove"))
                    {
                        if (strArray2.Length <= 2)
                        {
                            this.TellClient(player, "^1enter a ^5group ^1and ^5playername");
                            return BaseScript.EventEat.EatGame;
                        }
                        else
                        {
                            Entity byName = this.FindByName(strArray2[2]);
                            if (byName == null)
                                this.TellClient(player, "^1That user wasn't found or multiple were found.");
                            else
                                this.UnAddToGroup(strArray2[1], player, byName);
                            return BaseScript.EventEat.EatGame;
                        }
                    }
                    else if (strArray2[0] == "!setnextmap")
                    {
                        string mapByName = this.FindMapByName(strArray2[1]);
                        if (strArray2[1].StartsWith("mis"))
                        {
                            this.snm = true;
                            this.sa3id = "";
                            this.sa3id = "mission";
                            this.ServerSay("^2NextMap Set To ^7^7Mission");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (mapByName == null)
                        {
                            this.TellClient(player, "^1That map wasn't found or multiple were found.");
                            this.sa3id = (string)null;
                            this.snm = false;
                            return BaseScript.EventEat.EatGame;
                        }
                        else
                        {
                            if (strArray2.Length > 2)
                                return BaseScript.EventEat.EatGame;
                            this.snm = true;
                            this.sa3id = "";
                            this.sa3id = mapByName;
                            this.ServerSay("^2NextMap Set To ^7" + this.sa3id);
                            return BaseScript.EventEat.EatGame;
                        }
                    }
                    else if (strArray2[0] == "!load")
                    {
                        if (strArray2.Length < 2)
                        {
                            this.TellClient(player, "^1Enter the name of script ex !load IAM.dll");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (!File.Exists("scripts\\" + strArray2[1]))
                        {
                            this.TellClient(player, "^1Can't find the script");
                            return BaseScript.EventEat.EatGame;
                        }
                        else
                        {
                            Utilities.ExecuteCommand("loadscript " + strArray2[1]);
                            Utilities.ExecuteCommand("fast_restart");
                            this.ServerSay("^1" + strArray2[1] + " ^2Loaded and fastrestarted!");
                            return BaseScript.EventEat.EatGame;
                        }
                    }
                    else if (strArray2[0] == "!unload")
                    {
                        if (strArray2.Length < 2)
                        {
                            this.TellClient(player, "^1Enter the name of script ex !load IAM.dll");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (!File.Exists("scripts\\" + strArray2[1]))
                        {
                            this.TellClient(player, "^1Can't find the script");
                            return BaseScript.EventEat.EatGame;
                        }
                        else
                        {
                            Utilities.ExecuteCommand("unloadscript " + strArray2[1]);
                            Utilities.ExecuteCommand("fast_restart");
                            this.ServerSay("^1" + strArray2[1] + " ^2Unloaded and fastrestarted!");
                            return BaseScript.EventEat.EatGame;
                        }
                    }
                    else if (strArray2[0] == "!killed")
                    {
                        int num = player.Call<int>("getentitynumber", new Parameter[0]);
                        if (num.ToString() == "0")
                        {
                            this.TellClient(player, this.killed1);
                            return BaseScript.EventEat.EatGame;
                        }
                        else
                        {
                            num = player.Call<int>("getentitynumber", new Parameter[0]);
                            if (num.ToString() == "1")
                            {
                                this.TellClient(player, this.killed2);
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                num = player.Call<int>("getentitynumber", new Parameter[0]);
                                if (num.ToString() == "2")
                                {
                                    this.TellClient(player, this.killed3);
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    num = player.Call<int>("getentitynumber", new Parameter[0]);
                                    if (num.ToString() == "3")
                                    {
                                        this.TellClient(player, this.killed4);
                                        return BaseScript.EventEat.EatGame;
                                    }
                                    else
                                    {
                                        num = player.Call<int>("getentitynumber", new Parameter[0]);
                                        if (num.ToString() == "4")
                                        {
                                            this.TellClient(player, this.killed5);
                                            return BaseScript.EventEat.EatGame;
                                        }
                                        else
                                        {
                                            num = player.Call<int>("getentitynumber", new Parameter[0]);
                                            if (num.ToString() == "5")
                                            {
                                                this.TellClient(player, this.killed6);
                                                return BaseScript.EventEat.EatGame;
                                            }
                                            else
                                            {
                                                num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                if (num.ToString() == "6")
                                                {
                                                    this.TellClient(player, this.killed7);
                                                    return BaseScript.EventEat.EatGame;
                                                }
                                                else
                                                {
                                                    num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                    if (num.ToString() == "7")
                                                    {
                                                        this.TellClient(player, this.killed8);
                                                        return BaseScript.EventEat.EatGame;
                                                    }
                                                    else
                                                    {
                                                        num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                        if (num.ToString() == "8")
                                                        {
                                                            this.TellClient(player, this.killed9);
                                                            return BaseScript.EventEat.EatGame;
                                                        }
                                                        else
                                                        {
                                                            num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                            if (num.ToString() == "9")
                                                            {
                                                                this.TellClient(player, this.killed10);
                                                                return BaseScript.EventEat.EatGame;
                                                            }
                                                            else
                                                            {
                                                                num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                if (num.ToString() == "10")
                                                                {
                                                                    this.TellClient(player, this.killed11);
                                                                    return BaseScript.EventEat.EatGame;
                                                                }
                                                                else
                                                                {
                                                                    num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                    if (num.ToString() == "11")
                                                                    {
                                                                        this.TellClient(player, this.killed12);
                                                                        return BaseScript.EventEat.EatGame;
                                                                    }
                                                                    else
                                                                    {
                                                                        num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                        if (num.ToString() == "12")
                                                                        {
                                                                            this.TellClient(player, this.killed13);
                                                                            return BaseScript.EventEat.EatGame;
                                                                        }
                                                                        else
                                                                        {
                                                                            num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                            if (num.ToString() == "13")
                                                                            {
                                                                                this.TellClient(player, this.killed14);
                                                                                return BaseScript.EventEat.EatGame;
                                                                            }
                                                                            else
                                                                            {
                                                                                num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                                if (num.ToString() == "14")
                                                                                {
                                                                                    this.TellClient(player, this.killed15);
                                                                                    return BaseScript.EventEat.EatGame;
                                                                                }
                                                                                else
                                                                                {
                                                                                    num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                                    if (num.ToString() == "15")
                                                                                    {
                                                                                        this.TellClient(player, this.killed16);
                                                                                        return BaseScript.EventEat.EatGame;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                                        if (num.ToString() == "16")
                                                                                        {
                                                                                            this.TellClient(player, this.killed17);
                                                                                            return BaseScript.EventEat.EatGame;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            num = player.Call<int>("getentitynumber", new Parameter[0]);
                                                                                            if (!(num.ToString() == "17"))
                                                                                                return BaseScript.EventEat.EatGame;
                                                                                            this.TellClient(player, this.killed18);
                                                                                            return BaseScript.EventEat.EatGame;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (strArray2[0] == "!addword")
                    {
                        this.addToLangFilter(message, player);
                        return BaseScript.EventEat.EatGame;
                    }
                    else if (strArray2[0] == "!vgl")
                    {
                        this.TellClient(player, this.vmod);
                        return BaseScript.EventEat.EatGame;
                    }
                    else if (strArray2[0] == "!vml")
                    {
                        this.TellClient(player, this.vmap);
                        return BaseScript.EventEat.EatGame;
                    }
                    else
                    {
                        if (strArray2[0] == "!kc")
                        {
                            if (strArray2[1] == "on")
                            {
                                Utilities.ExecuteCommand("scr_game_allowkillcam 1");
                                this.TellClient(player, "^2KillCam ^1Enabled!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1] == "off")
                            {
                                Utilities.ExecuteCommand("scr_game_allowkillcam 0");
                                this.TellClient(player, "^2KillCam ^1Disabled!");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!votekick")
                        {
                            if (this.Entitys.Count >= 4)
                            {
                                if (this.votetime > 0)
                                {
                                    this.TellClient(player, "^3You can't start a vote now! wait " + this.votetime.ToString() + " seconds");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (this.voted)
                                {
                                    this.TellClient(player, "^3There's already a vote in progress!");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    Entity byName = this.FindByName(strArray2[1]);
                                    if (byName == null)
                                    {
                                        this.TellClient(player, "^1That user wasn't found or multiple were found.");
                                        return BaseScript.EventEat.EatGame;
                                    }
                                    else if (this.IsImmune(byName))
                                    {
                                        this.TellClient(player, "^3" + byName.Name + " ^1is immune to that command.");
                                        return BaseScript.EventEat.EatGame;
                                    }
                                    else
                                    {
                                        this.votetype = "Kick";
                                        IAM iam = this;
                                        string str3 = iam.plvote;
                                        guid = player.GUID;
                                        string str4 = guid.ToString();
                                        string str5 = ",";
                                        string str6 = str3 + str4 + str5;
                                        iam.plvote = str6;
                                        this.voted = true;
                                        this.votetime = 60;
                                        this.check();
                                        this.nextsec();
                                        ++this.yes;
                                        this.ServerSay("^3Vote Kick is Started by^2 " + player.Name + " ^2for kick : ^3" + byName.Name);
                                        this.vkn = byName;
                                        return BaseScript.EventEat.EatGame;
                                    }
                                }
                            }
                            else
                            {
                                this.TellClient(player, "^1Players Not Enough for Vote!");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!vc")
                        {
                            this.yes = 0;
                            this.no = 0;
                            this.voted = false;
                            this.votetime = -5;
                            this.ServerSay("^3Vote Canceled by ^2Admin");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!vnm")
                        {
                            if (this.Entitys.Count >= 4)
                            {
                                if (this.votetime > 0)
                                {
                                    this.TellClient(player, "^3You can't start a vote now! wait " + this.votetime.ToString() + " seconds");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (this.voted)
                                {
                                    this.TellClient(player, "^3There's already a vote in progress!");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.votetype = "Nextmap";
                                    IAM iam = this;
                                    string str3 = iam.plvote;
                                    guid = player.GUID;
                                    string str4 = guid.ToString();
                                    string str5 = ",";
                                    string str6 = str3 + str4 + str5;
                                    iam.plvote = str6;
                                    this.voted = true;
                                    this.votetime = 60;
                                    this.check();
                                    this.nextsec();
                                    this.yes = 1;
                                    this.ServerSay("^3Vote Nextmap is Started by^2 " + player.Name);
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                            else
                            {
                                this.TellClient(player, "^1Players Not Enough for Vote!");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!vfr")
                        {
                            if (this.Entitys.Count >= 4)
                            {
                                if (this.votetime > 0)
                                {
                                    this.TellClient(player, "^3You can't start a vote now! wait " + this.votetime.ToString() + " seconds");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (this.voted)
                                {
                                    this.TellClient(player, "^3There's already a vote in progress!");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.votetype = "Restart";
                                    this.voted = true;
                                    this.votetime = 60;
                                    this.check();
                                    this.nextsec();
                                    IAM iam = this;
                                    string str3 = iam.plvote;
                                    guid = player.GUID;
                                    string str4 = guid.ToString();
                                    string str5 = ",";
                                    string str6 = str3 + str4 + str5;
                                    iam.plvote = str6;
                                    this.yes = 1;
                                    this.ServerSay("^3Vote Restart is Started by^2 " + player.Name);
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                            else
                            {
                                this.TellClient(player, "^1Players Not Enough for Vote!");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!vg")
                        {
                            if (this.Entitys.Count >= 4)
                            {
                                if (this.votetime > 0)
                                {
                                    this.TellClient(player, "^3You can't start a vote now! wait " + this.votetime.ToString() + " seconds");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (this.voted)
                                {
                                    this.TellClient(player, "^3There's already a vote in progress!");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (strArray2[1] == "")
                                {
                                    this.TellClient(player, "^3Type a dspl from the list type !vgl for list");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (File.Exists("admin\\" + strArray2[1] + ".dspl") || File.Exists("players2\\" + strArray2[1] + ".dspl"))
                                {
                                    this.votemode = strArray2[1];
                                    this.votetype = "Gametype";
                                    IAM iam = this;
                                    string str3 = iam.plvote;
                                    guid = player.GUID;
                                    string str4 = guid.ToString();
                                    string str5 = ",";
                                    string str6 = str3 + str4 + str5;
                                    iam.plvote = str6;
                                    this.voted = true;
                                    this.votetime = 60;
                                    this.check();
                                    this.nextsec();
                                    this.yes = 1;
                                    this.ServerSay("^3VoteMode Started ^7- ^2Mode : ^3" + strArray2[1] + " ^3Started by ^2" + player.Name);
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.TellClient(player, "^1vote can't start");
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                            else
                            {
                                this.TellClient(player, "^1Players Not Enough for Vote!");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!vm")
                        {
                            if (this.Entitys.Count >= 4)
                            {
                                if (this.votetime > 0)
                                {
                                    this.TellClient(player, "^3You can't start a vote now! wait " + this.votetime.ToString() + " seconds");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (this.voted)
                                {
                                    this.TellClient(player, "^3There's already a vote in progress!");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else if (strArray2[1] == "")
                                {
                                    this.TellClient(player, "^1Type a map name from the list type !vml for list");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.changemap(message, player);
                                    strArray2[1] = this.sa3id;
                                    strArray2[1] = strArray2[1].Replace("dome", "mp_dome");
                                    strArray2[1] = strArray2[1].Replace("resistance", "mp_paris");
                                    strArray2[1] = strArray2[1].Replace("village", "mp_village");
                                    strArray2[1] = strArray2[1].Replace("bootleg", "mp_bootleg");
                                    strArray2[1] = strArray2[1].Replace("carbon", "mp_carbon");
                                    strArray2[1] = strArray2[1].Replace("interchange", "mp_interchange");
                                    strArray2[1] = strArray2[1].Replace("hardhat", "mp_hardhat");
                                    strArray2[1] = strArray2[1].Replace("downturn", "mp_exchange");
                                    strArray2[1] = strArray2[1].Replace("outpost", "mp_radar");
                                    strArray2[1] = strArray2[1].Replace("gateway", "mp_hillside_ss");
                                    strArray2[1] = strArray2[1].Replace("lookout", "mp_restrepo_ss");
                                    strArray2[1] = strArray2[1].Replace("overwatch", "mp_overwatch");
                                    strArray2[1] = strArray2[1].Replace("fallen", "mp_lambeth");
                                    strArray2[1] = strArray2[1].Replace("terminal", "mp_terminal_cls");
                                    strArray2[1] = strArray2[1].Replace("underground", "mp_underground");
                                    strArray2[1] = strArray2[1].Replace("arkaden", "mp_plaza2");
                                    strArray2[1] = strArray2[1].Replace("decommision", "mp_shipbreaker");
                                    strArray2[1] = strArray2[1].Replace("parish", "mp_nola");
                                    strArray2[1] = strArray2[1].Replace("off shore", "mp_roughneck");
                                    strArray2[1] = strArray2[1].Replace("boardwalk", "mp_boardwalk");
                                    strArray2[1] = strArray2[1].Replace("pizza", "mp_italy");
                                    strArray2[1] = strArray2[1].Replace("gulch", "mp_moab");
                                    strArray2[1] = strArray2[1].Replace("foundation", "mp_cement");
                                    strArray2[1] = strArray2[1].Replace("black box", "mp_morningwood");
                                    strArray2[1] = strArray2[1].Replace("Sanctuary", "mp_meteora");
                                    strArray2[1] = strArray2[1].Replace("aground", "mp_aground_ss");
                                    strArray2[1] = strArray2[1].Replace("uturn", "mp_burn_ss");
                                    strArray2[1] = strArray2[1].Replace("erosion", "mp_courtyard_ss");
                                    strArray2[1] = strArray2[1].Replace("liberation", "mp_park");
                                    strArray2[1] = strArray2[1].Replace("oasis", "mp_qadeem");
                                    strArray2[1] = strArray2[1].Replace("vortex", "mp_six_ss");
                                    strArray2[1] = strArray2[1].Replace("lockdown", "mp_alpha");
                                    strArray2[1] = strArray2[1].Replace("mission", "mp_bravo");
                                    strArray2[1] = strArray2[1].Replace("bakaara", "mp_mogadishu");
                                    strArray2[1] = strArray2[1].Replace("seatown", "mp_seatown");
                                    if (File.Exists("zone\\english\\" + strArray2[1] + ".ff") || File.Exists("zone\\dlc\\" + strArray2[1] + ".ff"))
                                    {
                                        this.votemap = strArray2[1];
                                        this.votetype = "Map";
                                        this.maplikename = this.sa3id;
                                        IAM iam = this;
                                        string str3 = iam.plvote;
                                        guid = player.GUID;
                                        string str4 = guid.ToString();
                                        string str5 = ",";
                                        string str6 = str3 + str4 + str5;
                                        iam.plvote = str6;
                                        this.voted = true;
                                        this.votetime = 60;
                                        this.check();
                                        this.nextsec();
                                        this.yes = 1;
                                        this.ServerSay("^3VoteMap Started ^7- ^2Map : ^3" + this.sa3id.ToUpper() + " ^3Started by ^2" + player.Name);
                                        return BaseScript.EventEat.EatGame;
                                    }
                                    else
                                    {
                                        this.TellClient(player, "^1vote can't start");
                                        return BaseScript.EventEat.EatGame;
                                    }
                                }
                            }
                            else
                            {
                                this.TellClient(player, "^1Players Not Enough for Vote!");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!vy" || strArray2[0] == "!y")
                        {
                            if (!this.voted)
                            {
                                this.TellClient(player, "^3There is currently no vote in progress! To start a vote, type ^2!vn (for nextmap) ^3or ^2!vm(map from maplist) or !vg(mode from modelist) ^3in chat.");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                string str3 = this.plvote;
                                guid = player.GUID;
                                string str4 = guid.ToString() + ",";
                                if (str3.Contains(str4))
                                {
                                    this.TellClient(player, "^3You already voted!");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    string str5 = this.plvote;
                                    guid = player.GUID;
                                    string str6 = guid.ToString() + ",";
                                    if (!str5.Contains(str6))
                                    {
                                        IAM iam = this;
                                        string str7 = iam.plvote;
                                        guid = player.GUID;
                                        string str8 = guid.ToString();
                                        string str9 = ",";
                                        string str10 = str7 + str8 + str9;
                                        iam.plvote = str10;
                                        this.yes = this.yes + 1;
                                        this.ServerSay("^3" + player.Name + " ^2Voted ^1<YES>");
                                        return BaseScript.EventEat.EatGame;
                                    }
                                }
                            }
                        }
                        else if (strArray2[0] == "!vn" || strArray2[0] == "!n")
                        {
                            if (!this.voted)
                            {
                                this.TellClient(player, "^3There is currently no vote in progress! To start a vote, type ^2!vn (for nextmap) ^3or ^2!vm(map from maplist) or !vg(mode from modelist) ^3in chat.");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                string str3 = this.plvote;
                                guid = player.GUID;
                                string str4 = guid.ToString() + ",";
                                if (str3.Contains(str4))
                                {
                                    this.TellClient(player, "^3You already voted!");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    string str5 = this.plvote;
                                    guid = player.GUID;
                                    string str6 = guid.ToString() + ",";
                                    if (!str5.Contains(str6))
                                    {
                                        IAM iam = this;
                                        string str7 = iam.plvote;
                                        guid = player.GUID;
                                        string str8 = guid.ToString();
                                        string str9 = ",";
                                        string str10 = str7 + str8 + str9;
                                        iam.plvote = str10;
                                        this.no = this.no + 1;
                                        this.ServerSay("^3" + player.Name + " ^2Voted ^1<NO>");
                                        return BaseScript.EventEat.EatGame;
                                    }
                                }
                            }
                        }
                        else if (strArray2[0] == "!hc")
                        {
                            if (strArray2[1] == "on")
                            {
                                Utilities.ExecuteCommand("g_hardcore 1");
                                this.hardc = true;
                                this.ServerSay("^2Hardcore ^1Enabled!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                if (!(strArray2[1] == "off"))
                                    return BaseScript.EventEat.EatGame;
                                Utilities.ExecuteCommand("g_hardcore 0");
                                this.hardc = false;
                                this.ServerSay("^2Hardcore ^1Disabled!");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!addbot")
                        {
                            this.AfterDelay(2000, new Action(this.addbot));
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!badword")
                        {
                            if (strArray2[1] == "on")
                            {
                                //this.teacher.useLangFilter = true;
                                this.TellClient(player, "^1BadWord Censor Is ^3On");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                if (!(strArray2[1] == "off"))
                                    return BaseScript.EventEat.EatGame;
                               // this.teacher.useLangFilter = false;
                                this.TellClient(player, "^1BadWord Censor Is ^3Off");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!scream")
                        {
                            if (this.isscream)
                            {
                                this.TellClient(player, "^1There are alreday scream wait to end last scream");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                this.messagescr = message.Replace(strArray2[0], "");
                                this.screamsec = 9;
                                this.nextscr();
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!fov")
                        {
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1type a Fov between 75 to 90");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                player.SetClientDvar("cg_fov", strArray2[1]);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!blocklist")
                        {
                            this.TellClient(player, "^3" + this.visitors);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!block")
                        {
                            this.block(player, message);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!unblock")
                        {
                            this.unblock(player, message);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!blockchat"))
                        {
                            if (strArray2[1].Equals("on"))
                            {
                                string str3 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                                File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                                using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                                {
                                    string str4 = str3.Replace("blockchat=false", "blockchat=true").Replace("blockchat=team", "blockchat=true");
                                    streamWriter.WriteLine(str4);
                                }
                                this.ServerSay("^2Chat ^1Blocked!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1].Equals("off"))
                            {
                                string str3 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                                File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                                using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                                {
                                    string str4 = str3.Replace("blockchat=true", "blockchat=false").Replace("blockchat=team", "blockchat=false");
                                    streamWriter.WriteLine(str4);
                                }
                                this.ServerSay("^2Chat ^1Unblocked!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1].Equals("team"))
                            {
                                string str3 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                                File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                                using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                                {
                                    string str4 = str3.Replace("blockchat=true", "blockchat=team").Replace("blockchat=false", "blockchat=team");
                                    streamWriter.WriteLine(str4);
                                }
                                this.ServerSay("^2Public ^1Blocked! ^3Use Team Chat!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                this.TellClient(player, "^1!blockchat on | off | team |");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!addimmune"))
                        {
                            if (strArray2.Length <= 1)
                            {
                                this.TellClient(player, "^1enter a playername.");
                            }
                            else
                            {
                                Entity byName = this.FindByName(strArray2[1]);
                                this.AddImmune(player, byName);
                            }
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!yell"))
                        {
                            string message1 = "";
                            if (strArray2.Length <= 1)
                            {
                                this.TellClient(player, "^1write a message");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                for (int index = 2; index < strArray2.Length; ++index)
                                    message1 = message1 + " " + strArray2[index];
                                if (strArray2[1].Equals("all"))
                                {
                                    foreach (Entity player1 in this.Entitys)
                                        this.iprintlnbold(player1, message1);
                                }
                                else
                                {
                                    Entity byName = this.FindByName(strArray2[1]);
                                    if (byName == null)
                                        this.TellClient(player, "^1That user wasn't found or multiple were found.");
                                    else
                                        this.iprintlnbold(byName, message1);
                                }
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!rules"))
                        {
                            this.conter = 0;
                            this.PlAyEr = player;
                            foreach (string str3 in File.ReadAllLines("scripts\\\\IAM\\\\IAM.cfg"))
                            {
                                if (str3.StartsWith("rules"))
                                {
                                    string str4 = str3.Split(new char[1]
                                    {
                    '='
                                    })[1];
                                    int key = 0;
                                    string str5 = str4;
                                    char[] chArray2 = new char[1]
                                    {
                    ','
                                    };
                                    foreach (string str6 in str5.Split(chArray2))
                                    {
                                        this.Rules.Add(key, str6);
                                        ++key;
                                    }
                                }
                            }
                            this.sayrule();
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!kick"))
                        {
                            if (strArray2[1] == "all")
                            {
                                foreach (Entity player1 in this.Entitys)
                                {
                                    if (!this.IsImmune(player1))
                                        Utilities.ExecuteCommand("dropclient " + (object)player1.Call<int>("getentitynumber", new Parameter[0]) + " \"All players kicked\"");
                                }
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                this.kick(message, player);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!ban"))
                        {
                            this.ban(message, player);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!tmpban"))
                        {
                            this.tmpban(message, player);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!slot"))
                        {
                            if (strArray2.Length == 1)
                            {
                                this.slot(player);
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2.Length == 2)
                            {
                                this.getslot(message, player);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!warn"))
                        {
                            if (strArray2.Length <= 1)
                                this.TellClient(player, "^1Enter a playername");
                            string reason = "";
                            Entity byName = this.FindByName(strArray2[1]);
                            if (byName == null)
                            {
                                this.TellClient(player, "^1That user wasn't found or multiple were found.");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                if (strArray2.Length > 2)
                                {
                                    for (int index = 2; index < strArray2.Length; ++index)
                                        reason = reason + " " + strArray2[index];
                                    this.warn(byName, reason);
                                }
                                else
                                    this.warn(byName, "no reason");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!unwarn"))
                        {
                            if (strArray2.Length <= 1)
                                this.TellClient(player, "^1Enter a playername");
                            Entity byName = this.FindByName(strArray2[1]);
                            this.unwarn(player, byName);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!maprotate"))
                        {
                          //  this.KnifeClass.EnableKnife();
                            Utilities.ExecuteCommand("map_rotate");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!rcon")
                        {
                            Utilities.ExecuteCommand(message.Replace(strArray2[0], ""));
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!time")
                        {
                            string str3 = DateTime.Now.ToLongTimeString();
                            this.TellClient(player, "^2It's ^7: ^1" + str3);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "@time")
                        {
                            this.ServerSay("^2It's ^7: ^1" + DateTime.Now.ToLongTimeString());
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!myteam")
                        {
                            this.TellClient(player, player.GetField<string>("sessionteam"));
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!setteam")
                        {
                            this.setteam(player, message);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!balance")
                        {
                            this.getteam();
                            if (this.allies.Count < this.axis.Count)
                            {
                                if (this.hasel != -1 && this.hasel != 1 && this.hasel != 0)
                                {
                                    foreach (Entity entity in this.axis)
                                    {
                                        if (this.hasel != -1 && this.hasel != 1 && this.hasel != 0)
                                        {
                                            entity.SetField("team", (Parameter)"allies");
                                            entity.SetField("sessionteam", (Parameter)"allies");
                                            entity.Notify("menuresponse", (Parameter)"team_marinesopfor", (Parameter)"allies");
                                            this.getteam();
                                        }
                                    }
                                }
                                this.ServerSay("^1Teams Balanced!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (this.allies.Count > this.axis.Count)
                            {
                                this.hasel = this.allies.Count - this.axis.Count;
                                if (this.hasel != 1 && this.hasel != -1 && this.hasel != 0)
                                {
                                    foreach (Entity entity in this.allies)
                                    {
                                        if (this.hasel != -1 && this.hasel != 1 && this.hasel != 0)
                                        {
                                            entity.SetField("team", (Parameter)"axis");
                                            entity.SetField("sessionteam", (Parameter)"axis");
                                            entity.Notify("menuresponse", (Parameter)"team_marinesopfor", (Parameter)"axis");
                                            this.getteam();
                                        }
                                    }
                                }
                                this.ServerSay("^1Teams Balanced!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                if (this.allies.Count != this.axis.Count)
                                    return BaseScript.EventEat.EatGame;
                                this.ServerSay("^1Teams are Balanced");
                                IAM.Print("Teams are Balanced", new object[0]);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!abalance")
                        {
                            Utilities.ExecuteCommand("scr_teambalance 1");
                            Utilities.ExecuteCommand("set scr_teambalance \"1\"");
                            this.ServerSay("^3Balance ^1Activaded!");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!dbalance")
                        {
                            Utilities.ExecuteCommand("scr_teambalance 0");
                            Utilities.ExecuteCommand("set scr_teambalance \"0\"");
                            this.ServerSay("^3Balance ^1Deactivaded!");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!jump")
                        {
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1Type a Integer Value");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1] == "def")
                            {
                            //    this.juspg.Jump(strArray2[1]);
                                this.jumpl = 39;
                                this.ServerSay("^2Jump Height Value Set to ^1Default");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                               // this.juspg.Jump(strArray2[1]);
                                this.jumpl = int.Parse(strArray2[1]);
                                this.ServerSay("^2Jump Height Value Set to ^1" + strArray2[1]);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!speed")
                        {
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1Type a Integer Value");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1] == "def")
                            {
                              //  this.juspg.speed(strArray2[1]);
                                this.ServerSay("^2Speed Value Set to ^1Default");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                               // this.juspg.speed(strArray2[1]);
                                this.ServerSay("^2Speed Value Set to ^1" + strArray2[1]);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!gravity")
                        {
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1Type a Integer Value");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1] == "def")
                            {
                              //  this.juspg.gravity(strArray2[1]);
                                this.ServerSay("^2Gravity Value Set to ^1Default");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                              //  this.juspg.gravity(strArray2[1]);
                                this.ServerSay("^2Gravity Value Set to ^1" + strArray2[1]);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!jsg")
                        {
                         //   this.juspg.setDefaultGameValue();
                            this.ServerSay("^1Jump & Speed & Gravity Set to ^2Default");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!knife")
                        {
                            if (strArray2[1] == "on")
                            {
                              //  this.KnifeClass.EnableKnife();
                                this.ServerSay("^2Knife ^1Enabled");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1] == "off")
                            {
                              //  this.KnifeClass.DisableKnife();
                                this.ServerSay("^2Knife ^1Disabled");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                this.TellClient(player, "^2Knife ^7: ^1On Or Off");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!ngt")
                        {
                            if (strArray2[1] == "")
                                this.TellClient(player, "^1Type a true dspl");
                            else if (File.Exists("admin\\" + strArray2[1] + ".dspl") || File.Exists("players2\\" + strArray2[1] + ".dspl"))
                            {
                                Utilities.ExecuteCommand("sv_maprotation " + strArray2[1]);
                                this.TellClient(player, "^1Warning : ^3Enable Knife Before Map Rotating");
                                this.ServerSay("^3Changing DSPL To ^2" + strArray2[1].ToUpper() + " ^1Without Rotation");
                            }
                            else
                                this.TellClient(player, "^1can't t that dspl");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!get")
                        {
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1Type a true dspl");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (File.Exists("admin\\" + strArray2[1] + ".dspl") || File.Exists("players2\\" + strArray2[1] + ".dspl"))
                            {
                                Utilities.ExecuteCommand("sv_maprotation " + strArray2[1]);
                                this.ServerSay("^3Changing DSPL To ^2" + strArray2[1].ToUpper() + " ^1With Rotation");
                            //    this.KnifeClass.EnableKnife();
                                this.AfterDelay(2000, new Action(this.Maprotate));
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                this.TellClient(player, "^1can't find that dspl");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!mod")
                        {
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1enter dsr name");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2.Length > 1)
                            {
                                if (File.Exists("admin\\" + strArray2[1] + ".dsr") || File.Exists("players2\\" + strArray2[1] + ".dsr"))
                                {
                                    File.Delete("admin\\IAM.dspl");
                                    File.Delete("players2\\IAM.dspl");
                                    str1.Replace("default:", "");
                                    using (StreamWriter streamWriter = new StreamWriter("players2\\IAM.dspl", true))
                                        streamWriter.WriteLine(str1 + "," + strArray2[1] + ",1000");
                                    using (StreamWriter streamWriter = new StreamWriter("admin\\IAM.dspl", true))
                                        streamWriter.WriteLine(str1 + "," + strArray2[1] + ",1000");
                                  //  this.KnifeClass.EnableKnife();
                                    Utilities.ExecuteCommand("sv_maprotation IAM");
                                    this.ServerSay("^3Changing Gametype To ^2" + strArray2[1]);
                                    this.AfterDelay(2000, new Action(this.mpr));
                                    this.AfterDelay(2000, new Action(this.gametype));
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.TellClient(player, "^1could'nt find dsr file");
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                        }
                        else if (strArray2[0] == "!gametype")
                        {
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1type a map name");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[2] == "")
                            {
                                this.TellClient(player, "^1type a dsr name");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2.Length > 3)
                            {
                                this.TellClient(player, "^1WTF? Just write mapname and dsrname!");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2.Length > 2)
                            {
                                strArray2[1] = strArray2[1].Replace("dome", "mp_dome");
                                strArray2[1] = strArray2[1].Replace("resistance", "mp_paris");
                                strArray2[1] = strArray2[1].Replace("village", "mp_village");
                                strArray2[1] = strArray2[1].Replace("bootleg", "mp_bootleg");
                                strArray2[1] = strArray2[1].Replace("carbon", "mp_carbon");
                                strArray2[1] = strArray2[1].Replace("interchange", "mp_interchange");
                                strArray2[1] = strArray2[1].Replace("hardhat", "mp_hardhat");
                                strArray2[1] = strArray2[1].Replace("downturn", "mp_exchange");
                                strArray2[1] = strArray2[1].Replace("outpost", "mp_radar");
                                strArray2[1] = strArray2[1].Replace("gateway", "mp_hillside_ss");
                                strArray2[1] = strArray2[1].Replace("lookout", "mp_restrepo_ss");
                                strArray2[1] = strArray2[1].Replace("overwatch", "mp_overwatch");
                                strArray2[1] = strArray2[1].Replace("fallen", "mp_lambeth");
                                strArray2[1] = strArray2[1].Replace("terminal", "mp_terminal_cls");
                                strArray2[1] = strArray2[1].Replace("underground", "mp_underground");
                                strArray2[1] = strArray2[1].Replace("arkaden", "mp_plaza2");
                                strArray2[1] = strArray2[1].Replace("decommision", "mp_shipbreaker");
                                strArray2[1] = strArray2[1].Replace("parish", "mp_nola");
                                strArray2[1] = strArray2[1].Replace("off shore", "mp_roughneck");
                                strArray2[1] = strArray2[1].Replace("boardwalk", "mp_boardwalk");
                                strArray2[1] = strArray2[1].Replace("pizza", "mp_italy");
                                strArray2[1] = strArray2[1].Replace("gulch", "mp_moab");
                                strArray2[1] = strArray2[1].Replace("foundation", "mp_cement");
                                strArray2[1] = strArray2[1].Replace("black box", "mp_morningwood");
                                strArray2[1] = strArray2[1].Replace("Sanctuary", "mp_meteora");
                                strArray2[1] = strArray2[1].Replace("aground", "mp_aground_ss");
                                strArray2[1] = strArray2[1].Replace("uturn", "mp_burn_ss");
                                strArray2[1] = strArray2[1].Replace("erosion", "mp_courtyard_ss");
                                strArray2[1] = strArray2[1].Replace("liberation", "mp_park");
                                strArray2[1] = strArray2[1].Replace("oasis", "mp_qadeem");
                                strArray2[1] = strArray2[1].Replace("vortex", "mp_six_ss");
                                strArray2[1] = strArray2[1].Replace("lockdown", "mp_alpha");
                                strArray2[1] = strArray2[1].Replace("mission", "mp_bravo");
                                strArray2[1] = strArray2[1].Replace("bakaara", "mp_mogadishu");
                                strArray2[1] = strArray2[1].Replace("seatown", "mp_seatown");
                                if (File.Exists("admin\\" + strArray2[2] + ".dsr") && File.Exists("zone\\english\\" + strArray2[1] + ".ff") || File.Exists("admin\\" + strArray2[2] + ".dsr") && File.Exists("zone\\dlc\\" + strArray2[1] + ".ff") || (File.Exists("players2\\" + strArray2[2] + ".dsr") && File.Exists("zone\\english\\" + strArray2[1] + ".ff") || File.Exists("players2\\" + strArray2[2] + ".dsr") && File.Exists("zone\\dlc\\" + strArray2[1] + ".ff")))
                                {
                                    File.Delete("admin\\IAM.dspl");
                                    File.Delete("players2\\IAM.dspl");
                                    using (StreamWriter streamWriter = new StreamWriter("players2\\IAM.dspl", true))
                                        streamWriter.WriteLine(strArray2[1] + "," + strArray2[2] + ",1000");
                                    using (StreamWriter streamWriter = new StreamWriter("admin\\IAM.dspl", true))
                                        streamWriter.WriteLine(strArray2[1] + "," + strArray2[2] + ",1000");
                                    Utilities.ExecuteCommand("sv_maprotation IAM");
                                    this.ServerSay("^3Changing Gametype To ^2" + strArray2[2] + " ^3& ^3Map ^7: ^2" + strArray2[1]);
                                //    this.KnifeClass.EnableKnife();
                                    this.AfterDelay(2000, new Action(this.mpr));
                                    this.AfterDelay(2000, new Action(this.gametype));
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.TellClient(player, "^1could'nt find map or dsr");
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                        }
                        else if (strArray2[0].Equals("!admins"))
                        {
                            if (this.admins == "")
                            {
                                this.TellClient(player, "^1Online admins not in server");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                this.TellClient(player, "^3Online ^2Admins ^7: " + this.admins);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!pm"))
                        {
                            this.PM(player, message);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("@admins"))
                        {
                            this.ServerSay("^3Online ^2Admins ^7: " + this.admins);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!nextmap"))
                        {
                            string str3 = this.Call<string>("getdvar", new Parameter[1]
                            {
                (Parameter) "nextmap"
                            }).Replace("*", "(Random)").Replace("_default", "").Replace("mp_dome", "Dome").Replace("mp_paris", "Resistance").Replace("mp_village", "Village").Replace("mp_bootleg", "Bootleg").Replace("mp_carbon", "Carbon").Replace("mp_interchange", "Interchange").Replace("mp_hardhat", "Hardhat").Replace("mp_exchange", "Downturn").Replace("mp_radar", "Outpost").Replace("mp_hillside_ss", "Gateway").Replace("mp_restrepo_ss", "Lookout").Replace("mp_overwatch", "Overwatch").Replace("mp_lambeth", "Fallen").Replace("mp_terminal_cls", "Terminal").Replace("mp_underground", "Underground").Replace("mp_plaza2", "Arkaden").Replace("mp_shipbreaker", "Decommision").Replace("mp_nola", "Parish").Replace("mp_roughneck", "Off Shore").Replace("mp_boardwalk", "Boardwalk").Replace("mp_italy", "Pizza").Replace("mp_moab", "Gulch").Replace("mp_cement", "Foundation").Replace("mp_morningwood", "Black Box").Replace("mp_meteora", "Sanctuary").Replace("mp_aground_ss", "Aground").Replace("mp_burn_ss", "U-Turn").Replace("mp_courtyard_ss", "Erosion").Replace("mp_park", "Liberation").Replace("mp_qadeem", "Oasis").Replace("mp_six_ss", "Vortex").Replace("mp_alpha", "Lockdown").Replace("mp_bravo", "Mission").Replace("mp_mogadishu", "Bakaara").Replace("mp_seatown", "Seatown");
                            if (this.snm)
                            {
                                char[] chArray2 = new char[1]
                                {
                  ' '
                                };
                                string[] strArray3 = str3.Split(chArray2);
                                str3 = str3.Replace(strArray3[0], this.sa3id);
                            }
                            char[] chArray3 = new char[1]
                            {
                ' '
                            };
                            string[] strArray4 = str3.Split(chArray3);
                            string str4 = str3.Replace(strArray4[1], "(" + strArray4[1] + ")");
                            this.TellClient(player, "^3NextMap ^7: ^5" + str4);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("@nextmap"))
                        {
                            string str3 = this.Call<string>("getdvar", new Parameter[1]
                            {
                (Parameter) "nextmap"
                            }).Replace("*", "(Random)").Replace("_default", "").Replace("mp_dome", "Dome").Replace("mp_paris", "Resistance").Replace("mp_village", "Village").Replace("mp_bootleg", "Bootleg").Replace("mp_carbon", "Carbon").Replace("mp_interchange", "Interchange").Replace("mp_hardhat", "Hardhat").Replace("mp_exchange", "Downturn").Replace("mp_radar", "Outpost").Replace("mp_hillside_ss", "Gateway").Replace("mp_restrepo_ss", "Lookout").Replace("mp_overwatch", "Overwatch").Replace("mp_lambeth", "Fallen").Replace("mp_terminal_cls", "Terminal").Replace("mp_underground", "Underground").Replace("mp_plaza2", "Arkaden").Replace("mp_shipbreaker", "Decommision").Replace("mp_nola", "Parish").Replace("mp_roughneck", "Off Shore").Replace("mp_boardwalk", "Boardwalk").Replace("mp_italy", "Pizza").Replace("mp_moab", "Gulch").Replace("mp_cement", "Foundation").Replace("mp_morningwood", "Black Box").Replace("mp_meteora", "Sanctuary").Replace("mp_aground_ss", "Aground").Replace("mp_burn_ss", "U-Turn").Replace("mp_courtyard_ss", "Erosion").Replace("mp_park", "Liberation").Replace("mp_qadeem", "Oasis").Replace("mp_six_ss", "Vortex").Replace("mp_alpha", "Lockdown").Replace("mp_bravo", "Mission").Replace("mp_mogadishu", "Bakaara").Replace("mp_seatown", "Seatown");
                            if (this.snm)
                            {
                                char[] chArray2 = new char[1]
                                {
                  ' '
                                };
                                string[] strArray3 = str3.Split(chArray2);
                                str3 = str3.Replace(strArray3[0], this.sa3id);
                            }
                            char[] chArray3 = new char[1]
                            {
                ' '
                            };
                            string[] strArray4 = str3.Split(chArray3);
                            this.ServerSay("^3NextMap ^7: ^5" + str3.Replace(strArray4[1], "(" + strArray4[1] + ")"));
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!kill"))
                        {
                            if (strArray2.Length <= 1)
                            {
                                this.TellClient(player, "^1Enter a playername format: !kill [playername or all]");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1].Equals("all"))
                            {
                                using (List<Entity>.Enumerator enumerator = this.Entitys.GetEnumerator())
                                {
                                    if (enumerator.MoveNext())
                                    {
                                        Entity target = enumerator.Current;
                                        Vector3 origin1 = target.Origin;
                                        Vector3 origin2 = target.Origin;
                                        origin1.Z -= 1000f;
                                        origin2.Z += 6000f;
                                        this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)origin2, (Parameter)origin1, (Parameter)target);
                                        origin2.X += 2000f;
                                        this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)origin2, (Parameter)origin1, (Parameter)target);
                                        origin2.X -= 4000f;
                                        this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)origin2, (Parameter)origin1, (Parameter)target);
                                        player.AfterDelay(3000, (Action<Entity>)(entity =>
                                        {
                                            Vector3 local_0 = target.Origin;
                                            Vector3 local_1 = target.Origin;
                                            local_0.Z -= 1000f;
                                            local_1.Z += 6000f;
                                            this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)local_1, (Parameter)local_0, (Parameter)target);
                                            local_1.X += 2000f;
                                            local_1.Y += 3000f;
                                            this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)local_1, (Parameter)local_0, (Parameter)target);
                                            local_1.X -= 4000f;
                                            local_1.Y -= 6000f;
                                            this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)local_1, (Parameter)local_0, (Parameter)target);
                                        }));
                                        player.AfterDelay(5000, (Action<Entity>)(entity =>
                                        {
                                            Vector3 local_0 = target.Origin;
                                            Vector3 local_1 = target.Origin;
                                            local_0.Z -= 1000f;
                                            local_1.Z += 6000f;
                                            this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)local_1, (Parameter)local_0, (Parameter)target);
                                            local_1.Y += 5000f;
                                            this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)local_1, (Parameter)local_0, (Parameter)target);
                                            local_1.Y -= 10000f;
                                            this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)local_1, (Parameter)local_0, (Parameter)target);
                                            local_1.Y -= 2000f;
                                            local_1.X += 4000f;
                                            this.Call("magicbullet", (Parameter)"ac130_105mm_mp", (Parameter)local_1, (Parameter)local_0, (Parameter)target);
                                        }));
                                        return BaseScript.EventEat.EatGame;
                                    }
                                }
                            }
                            else
                            {
                                Entity byName = this.FindByName(strArray2[1]);
                                if (byName == null)
                                {
                                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    Vector3 origin1 = byName.Origin;
                                    Vector3 origin2 = byName.Origin;
                                    origin1.Z -= 1000f;
                                    origin2.Z += 1000f;
                                    this.Call("magicbullet", (Parameter)"uav_strike_projectile_mp", (Parameter)origin2, (Parameter)origin1, (Parameter)byName);
                                    player.Call("visionsetthermal", new Parameter[0]);
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                        }
                        else if (strArray2[0] == "!clearadmins")
                        {
                            this.admins = "";
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!maplist"))
                        {
                            this.mapEnt = player;
                            this.map1.Interval = 2500.0;
                            this.map1.Enabled = true;
                            this.map1.Elapsed += new ElapsedEventHandler(this.NextMap);
                            this.map1.Start();
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!setpw")
                        {
                            if (strArray2[1] == "n")
                            {
                                Utilities.ExecuteCommand("g_password \"\"");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                Utilities.ExecuteCommand("g_password " + strArray2[1]);
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!map")
                        {
                       //     this.KnifeClass.EnableKnife();
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1please enter a map name");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                if (strArray2[1] == "dow")
                                    Utilities.ExecuteCommand("map mp_exchange");
                                else if (strArray2[1] == "dom")
                                    Utilities.ExecuteCommand("map mp_dome");
                                else if (strArray2[1] == "boo")
                                    Utilities.ExecuteCommand("map mp_bootleg");
                                else if (strArray2[1] == "har")
                                    Utilities.ExecuteCommand("map mp_hardhat");
                                else if (strArray2[1] == "loc")
                                    Utilities.ExecuteCommand("map mp_alpha");
                                else if (strArray2[1] == "mis")
                                    Utilities.ExecuteCommand("map mp_bravo");
                                else if (strArray2[1] == "car")
                                    Utilities.ExecuteCommand("map mp_carbon");
                                else if (strArray2[1] == "int")
                                    Utilities.ExecuteCommand("map mp_interchange");
                                else if (strArray2[1] == "fal")
                                    Utilities.ExecuteCommand("map mp_lambeth");
                                else if (strArray2[1] == "bak")
                                    Utilities.ExecuteCommand("map mp_mogadishu");
                                else if (strArray2[1] == "res")
                                    Utilities.ExecuteCommand("map mp_paris");
                                else if (strArray2[1] == "ark")
                                    Utilities.ExecuteCommand("map mp_plaza2");
                                else if (strArray2[1] == "out")
                                    Utilities.ExecuteCommand("map mp_radar");
                                else if (strArray2[1] == "sea")
                                    Utilities.ExecuteCommand("map mp_seatown");
                                else if (strArray2[1] == "und")
                                    Utilities.ExecuteCommand("map mp_underground");
                                else if (strArray2[1] == "vil")
                                    Utilities.ExecuteCommand("map mp_village");
                                else if (strArray2[1] == "ter")
                                    Utilities.ExecuteCommand("map mp_terminal_cls");
                                else if (strArray2[1] == "ove")
                                    Utilities.ExecuteCommand("map mp_overwatch");
                                else if (strArray2[1] == "lib")
                                    Utilities.ExecuteCommand("map mp_overwatch");
                                else if (strArray2[1] == "piz")
                                    Utilities.ExecuteCommand("map mp_italy");
                                else if (strArray2[1] == "bla")
                                    Utilities.ExecuteCommand("map mp_morningwood");
                                else if (strArray2[1] == "san")
                                    Utilities.ExecuteCommand("map mp_meteora");
                                else if (strArray2[1] == "fou")
                                    Utilities.ExecuteCommand("map mp_cement");
                                else if (strArray2[1] == "oas")
                                    Utilities.ExecuteCommand("map mp_qadeem");
                                else if (strArray2[1] == "agr")
                                    Utilities.ExecuteCommand("map mp_aground_ss");
                                else if (strArray2[1] == "ero")
                                    Utilities.ExecuteCommand("map mp_courtyard_ss");
                                else if (strArray2[1] == "gat")
                                    Utilities.ExecuteCommand("map mp_hillside_ss");
                                else if (strArray2[1] == "get")
                                    Utilities.ExecuteCommand("map mp_hillside_ss");
                                else if (strArray2[1] == "loo")
                                    Utilities.ExecuteCommand("map mp_restrepo_ss");
                                else if (strArray2[1] == "utu")
                                    Utilities.ExecuteCommand("map mp_burn_ss");
                                else if (strArray2[1] == "ins")
                                    Utilities.ExecuteCommand("map mp_crosswalk_ss");
                                else if (strArray2[1] == "vor")
                                    Utilities.ExecuteCommand("map mp_six_ss");
                                else if (strArray2[1] == "dec")
                                    Utilities.ExecuteCommand("map mp_shipbreaker");
                                else if (strArray2[1] == "off")
                                    Utilities.ExecuteCommand("map mp_roughneck");
                                else if (strArray2[1] == "gul")
                                    Utilities.ExecuteCommand("map mp_moab");
                                else if (strArray2[1] == "boa")
                                    Utilities.ExecuteCommand("map mp_boardwalk");
                                else if (strArray2[1] == "par")
                                {
                                    Utilities.ExecuteCommand("map mp_nola");
                                }
                                else
                                {
                                    this.changemap(message, player);
                                    if (this.sa3id == "dome")
                                        Utilities.ExecuteCommand("map mp_dome");
                                    else if (this.sa3id == "bootleg")
                                        Utilities.ExecuteCommand("map mp_bootleg");
                                    else if (this.sa3id == "hardhat")
                                        Utilities.ExecuteCommand("map mp_hardhat");
                                    else if (this.sa3id == "lockdown")
                                        Utilities.ExecuteCommand("map mp_alpha");
                                    else if (this.sa3id == "mission")
                                        Utilities.ExecuteCommand("map mp_bravo");
                                    else if (this.sa3id == "carbon")
                                        Utilities.ExecuteCommand("map mp_carbon");
                                    else if (this.sa3id == "downturn")
                                        Utilities.ExecuteCommand("map mp_exchange");
                                    else if (this.sa3id == "interchange")
                                        Utilities.ExecuteCommand("map mp_interchange");
                                    else if (this.sa3id == "fallen")
                                        Utilities.ExecuteCommand("map mp_lambeth");
                                    else if (this.sa3id == "bakaraa")
                                        Utilities.ExecuteCommand("map mp_mogadishu");
                                    else if (this.sa3id == "resistance")
                                        Utilities.ExecuteCommand("map mp_paris");
                                    else if (this.sa3id == "arkaden")
                                        Utilities.ExecuteCommand("map mp_plaza2");
                                    else if (this.sa3id == "outpost")
                                        Utilities.ExecuteCommand("map mp_radar");
                                    else if (this.sa3id == "seatown")
                                        Utilities.ExecuteCommand("map mp_seatown");
                                    else if (this.sa3id == "underground")
                                        Utilities.ExecuteCommand("map mp_underground");
                                    else if (this.sa3id == "village")
                                        Utilities.ExecuteCommand("map mp_village");
                                    else if (this.sa3id == "terminal")
                                        Utilities.ExecuteCommand("map mp_terminal_cls");
                                    else if (this.sa3id == "overwatch")
                                        Utilities.ExecuteCommand("map mp_overwatch");
                                    else if (this.sa3id == "liberation")
                                        Utilities.ExecuteCommand("map mp_park");
                                    else if (this.sa3id == "pizza")
                                        Utilities.ExecuteCommand("map mp_italy");
                                    else if (this.sa3id == "blackbox")
                                        Utilities.ExecuteCommand("map mp_morningwood");
                                    else if (this.sa3id == "sanctuary")
                                        Utilities.ExecuteCommand("map mp_meteora");
                                    else if (this.sa3id == "foundation")
                                        Utilities.ExecuteCommand("map mp_cement");
                                    else if (this.sa3id == "oasis")
                                        Utilities.ExecuteCommand("map mp_qadeem");
                                    else if (this.sa3id == "aground")
                                        Utilities.ExecuteCommand("map mp_aground_ss");
                                    else if (this.sa3id == "erosion")
                                        Utilities.ExecuteCommand("map mp_courtyard_ss");
                                    else if (this.sa3id == "gateway")
                                        Utilities.ExecuteCommand("map mp_hillside_ss");
                                    else if (this.sa3id == "lookout")
                                        Utilities.ExecuteCommand("map mp_restrepo_ss");
                                    else if (this.sa3id == "uturn")
                                        Utilities.ExecuteCommand("map mp_burn_ss");
                                    else if (this.sa3id == "intersection")
                                        Utilities.ExecuteCommand("map mp_crosswalk_ss");
                                    else if (this.sa3id == "vortex")
                                        Utilities.ExecuteCommand("map mp_six_ss");
                                    else if (this.sa3id == "decommission")
                                        Utilities.ExecuteCommand("map mp_shipbreaker");
                                    else if (this.sa3id == "offshore")
                                        Utilities.ExecuteCommand("map mp_roughneck");
                                    else if (this.sa3id == "gulch")
                                        Utilities.ExecuteCommand("map mp_moab");
                                    else if (this.sa3id == "boardwalk")
                                        Utilities.ExecuteCommand("map mp_boardwalk");
                                    else if (this.sa3id == "parish")
                                        Utilities.ExecuteCommand("map mp_nola");
                               //     this.KnifeClass.EnableKnife();
                                }
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!restart")
                        {
                         //   this.KnifeClass.EnableKnife();
                            Utilities.ExecuteCommand("map_restart");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!res")
                        {
                      //      this.KnifeClass.EnableKnife();
                            Utilities.ExecuteCommand("fast_restart");
                            this.ServerSay(" ^2Fast Restarting the map...");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!inf")
                        {
                            this.inf(player, message);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!sxlr")
                        {
                            this.GetPlayerStats(player, message);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!unban"))
                        {
                            if (strArray2.Length <= 1)
                            {
                                this.TellClient(player, "^1Enter a playername");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                Entity byName = this.FindByName(strArray2[1]);
                                if (byName == null)
                                {
                                    this.TellClient(player, "^1That user wasn't found or multiple were found.");
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    Utilities.ExecuteCommand("unban " + byName.Name);
                                    this.ServerSay(" ^1" + byName.Name + " ^3has been unbanned.");
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                        }
                        else if (strArray2[0].Equals("!unimmune"))
                        {
                            if (strArray2.Length <= 1)
                            {
                                this.TellClient(player, "^1Enter a playername");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                Entity byName = this.FindByName(strArray2[1]);
                                foreach (string str3 in this.ImmunePlayer)
                                {
                                    guid = byName.GUID;
                                    if (guid.ToString() == str3)
                                    {
                                        List<string> list = this.ImmunePlayer;
                                        guid = byName.GUID;
                                        string str4 = guid.ToString();
                                        list.Remove(str4);
                                        this.UpdateImmune();
                                        this.ServerSay(" ^1" + byName.Name + " ^3isn't immune ^2now");
                                        return BaseScript.EventEat.EatGame;
                                    }
                                }
                                this.TellClient(player, "^1" + byName.Name + " ^3isn't already immune.");
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!guid"))
                        {
                            IAM iam = this;
                            Entity player1 = player;
                            string str3 = "^2Your GUID is: ^5";
                            guid = player.GUID;
                            string str4 = guid.ToString();
                            string message1 = str3 + str4;
                            iam.TellClient(player1, message1);
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!say"))
                        {
                            if (strArray2.Length <= 1)
                            {
                                this.TellClient(player, "^1Enter a message");
                                return BaseScript.EventEat.EatGame;
                            }
                            else
                            {
                                this.ServerSay(message.Replace(strArray2[0], ""));
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0].Equals("!help"))
                        {
                            foreach (KeyValuePair<string, string> keyValuePair in this.gCommands)
                            {
                                if (keyValuePair.Key == this.GetRankByName(player.Name))
                                    this.TellClient(player, "^1You can use: ^7" + keyValuePair.Value.Replace(",", "^1,^7"));
                            }
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!xlrstats"))
                        {
                            this.num = "";
                            int num1 = 0;
                            StreamReader streamReader = new StreamReader("scripts\\\\IAM\\\\xlrstats");
                            string str3;
                            while ((str3 = streamReader.ReadLine()) != null)
                            {
                                if (str3.Contains(player.Name))
                                    this.num = "#" + num1.ToString();
                                ++num1;
                            }
                            streamReader.Close();
                            foreach (KeyValuePair<string, string> keyValuePair in this.xlrstats)
                            {
                                if (keyValuePair.Key.Equals(player.Name))
                                {
                                    double num2 = Convert.ToDouble(keyValuePair.Value.Split(new char[1]
                                    {
                    ','
                                    })[0]);
                                    double num3 = Convert.ToDouble(keyValuePair.Value.Split(new char[1]
                                    {
                    ','
                                    })[1]);
                                    double num4 = num2 / num3;
                                    if (num2 > 0.0)
                                    {
                                        HudElem fontString = HudElem.CreateFontString(player, "hudsmall", 1f);
                                        fontString.SetPoint("TOP", "TOP", 0, 400);
                                        fontString.SetText("^2Your stats: ^7K ^1" + num2.ToString() + "^7, D ^1" + num3.ToString() + "^7, K/D ^1" + num4.ToString());
                                        fontString.SetField("glowcolor", (Parameter)new Vector3(1f, 0.4f, 0.0f));
                                        fontString.Alpha = 1f;
                                        fontString.GlowAlpha = 0.6f;
                                        fontString.Call("SetPulseFX", (Parameter)100, (Parameter)7000, (Parameter)600);
                                        return BaseScript.EventEat.EatGame;
                                    }
                                    else if (num2 == 0.0 && num3 == 0.0)
                                    {
                                        HudElem fontString = HudElem.CreateFontString(player, "hudsmall", 1f);
                                        fontString.SetPoint("TOP", "TOP", 0, 400);
                                        fontString.SetText("^2Your stats: ^7K ^1" + num2.ToString() + "^7, D ^1" + num3.ToString() + "^7, K/D^1 0");
                                        fontString.SetField("glowcolor", (Parameter)new Vector3(1f, 0.4f, 0.0f));
                                        fontString.Alpha = 1f;
                                        fontString.GlowAlpha = 0.6f;
                                        fontString.Call("SetPulseFX", (Parameter)100, (Parameter)7000, (Parameter)600);
                                        return BaseScript.EventEat.EatGame;
                                    }
                                    else
                                    {
                                        HudElem fontString = HudElem.CreateFontString(player, "hudsmall", 1f);
                                        fontString.SetPoint("TOP", "TOP", 0, 400);
                                        fontString.SetText("^2Your stats: ^7K ^1" + num2.ToString() + "^7, D ^1" + num3.ToString() + "^7, K/D ^1" + num2.ToString());
                                        fontString.SetField("glowcolor", (Parameter)new Vector3(1f, 0.4f, 0.0f));
                                        fontString.Alpha = 1f;
                                        fontString.GlowAlpha = 0.6f;
                                        fontString.Call("SetPulseFX", (Parameter)100, (Parameter)7000, (Parameter)600);
                                        return BaseScript.EventEat.EatGame;
                                    }
                                }
                            }
                            this.TellClient(player, "^1Could not find you in the stats database, type !register to save your stats.");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0].Equals("!register"))
                        {
                            foreach (KeyValuePair<string, string> keyValuePair in this.xlrstats)
                            {
                                if (keyValuePair.Key == player.Name)
                                {
                                    this.TellClient(player, "^2You Are Already Registered.");
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                            this.AddToXlrStats(player);
                            this.TellClient(player, "^2Successfully Registered on XlrStats");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!maxping")
                        {
                            Entity entity1 = player;
                            if (strArray2[1] == "")
                            {
                                this.TellClient(player, "^1Type ^2on ^1Or ^2off");
                                return BaseScript.EventEat.EatGame;
                            }
                            else if (strArray2[1] == "off")
                            {
                                if (this.maxpingcfg == "true")
                                {
                                    string str3 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                                    File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                                    using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                                    {
                                        string str4 = str3.Replace("pingcensor=true", "pingcensor=false");
                                        streamWriter.WriteLine(str4);
                                    }
                                    this.ServerSay("^2MaxPing Detector ^1Disabled");
                                    this.maxpingcfg = "false";
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.TellClient(player, "^2MaxPing Censor is already off");
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                            else if (strArray2[1] == "on")
                            {
                                if (this.maxpingcfg == "false")
                                {
                                    string str3 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                                    File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                                    using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                                    {
                                        string str4 = str3.Replace("pingcensor=false", "pingcensor=true");
                                        streamWriter.WriteLine(str4);
                                    }
                                    this.ServerSay("^2MaxPing Detector ^1Enabled");
                                    this.maxpingcfg = "true";
                                    return BaseScript.EventEat.EatGame;
                                }
                                else
                                {
                                    this.TellClient(player, "^2MaxPing Censor is already on");
                                    return BaseScript.EventEat.EatGame;
                                }
                            }
                            else if (strArray2[1] == "p")
                            {
                                foreach (Entity entity2 in this.Entitys)
                                {
                                    if (entity2.Ping > entity1.Ping)
                                        entity1 = entity2;
                                }
                                this.TellClient(player, "^2" + entity1.Name + " Ping : ^1" + entity1.Ping.ToString());
                                return BaseScript.EventEat.EatGame;
                            }
                        }
                        else if (strArray2[0] == "!login")
                        {
                            this.TellClient(player, "^2You Already Logged");
                            return BaseScript.EventEat.EatGame;
                        }
                        else if (strArray2[0] == "!setmaxping")
                        {
                            this.maxping();
                            string str3 = File.ReadAllText("scripts\\\\IAM\\\\IAM.cfg");
                            File.Delete("scripts\\\\IAM\\\\IAM.cfg");
                            using (StreamWriter streamWriter = new StreamWriter("scripts\\\\IAM\\\\IAM.cfg", true))
                            {
                                string str4 = str3.Replace("maxping=" + this.maxpingi, "maxping=" + strArray2[1]);
                                streamWriter.WriteLine(str4);
                            }
                            this.maxpingi = strArray2[1];
                            this.ServerSay("^2Max Ping Set to ^1" + strArray2[1]);
                            return BaseScript.EventEat.EatGame;
                        }
                        else
                        {
                            player.Call("iprintln", new Parameter[1]
                            {
                (Parameter) ("IAM : Unknown cmd" + strArray2[0].Replace("!", ""))
                            });
                            return BaseScript.EventEat.EatNone;
                        }
                        return BaseScript.EventEat.EatNone;
                    }
                }
                else if (strArray2[0] == "!login")
                {
                    if (strArray2[1] == this.password)
                    {
                        this.AddToAdmins(player);
                        this.antihackload();
                        this.TellClient(player, "^2Logged Sucessfully!");
                        return BaseScript.EventEat.EatGame;
                    }
                    else
                    {
                        this.TellClient(player, "^1Wrong Password");
                        return BaseScript.EventEat.EatGame;
                    }
                }
                else
                {
                    this.TellClient(player, "^1You Must To Login with !login <password>");
                    return BaseScript.EventEat.EatGame;
                }
            }
        }
    }
}