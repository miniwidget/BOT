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

        void SayToAdmin(string message)
        {
            Utilities.RawSayTo(admin, message);
        }

    }

    public partial class Infected
    {

        Admin AD;
#if DEBUG

        bool stop;
        bool BotToMe()
        {
            if (SET.TEST_)
            {
                OnInterval(5000, () =>
                {
                    if (stop) return false;
                    foreach (Entity bot in BOTs_List)
                    {
                        if (IsBOT[bot.EntRef] == null) continue;
                        bot.Call("setorigin", ADMIN.Origin);
                    }
                    return true;
                });
            }
            return false;
        }
        void Viewchange(Entity player, bool _3rd)
        {
            if (_3rd)
            {
                player.SetClientDvar("camera_thirdPerson", "1");
                player.SetClientDvar("camera_thirdPersonOffset", "-200");//default -120커지면확대 0-좌+우 14커지면 위에서, 작아지면 밑에서 봄
            }else
            {
                player.SetClientDvar("camera_thirdPerson", "0");
            }
        }
        string GetSO(string idx, Vector3 o)
        {
            int x = (int)o.X;
            int y = (int)o.Y;
            int z = (int)o.Z;

            int diff = (int)o.DistanceTo(ADMIN.Origin);
            return idx + "(" + x + "," + y + "," + z + ")[" + diff + "] ";
        }
        string GetSO(Vector3 o)
        {
            int x = (int)o.X;
            int y = (int)o.Y;
            int z = (int)o.Z;

            return "(" + x + "," + y + "," + z + ") ";
        }

        int obj_idx = 25;
        bool compass(string s)
        {
            Print(s);
            Call(431, obj_idx, "inactive"); // objective_add

            obj_idx++;
            Call(431, obj_idx, "active"); // objective_add
            Call(435, obj_idx, ADMIN.Origin); // objective_position
            Call(434, obj_idx, s);//objective_icon

            /*
            compass_objpoint_ac130_friendly 동그라미  
            compass_waypoint_bomb 사다리꼴

            compass_waypoint_captureneutral
            compass_waypoint_capture
            compass_waypoint_defend

            compass_objpoint_reaper_friendly

            */
            return false;

        }
        bool hudelemX(string value)
        {
            //HudElem hud = HUD.elem;
            //hud.X = float.Parse(value);
            return false;
        }
        bool hudelemY(string value)
        {
            //HudElem hud = HUD.elem;
            //hud.Y = float.Parse(value);
            return false;
        }
        bool hudelemVA(string value)
        {
            //HudElem hud = HUD.elem;
            //hud.VertAlign = value;
            return false;
        }
        bool hudelemHA(string value)
        {
            //HudElem hud = HUD.elem;
            //hud.HorzAlign = value;
            return false;
        }
        bool hudelemTXT(string value)
        {
            //HudElem hud = HUD.elem;
            //hud.SetText(value);
            return false;
        }
        bool SolidCapsule(Entity player)
        {

            Vector3 origin = player.Origin + Common.GetVector(0, 0, 30);
            Entity capsule = Call<Entity>("spawn", "script_model", origin);
            //capsule.SetField("angles", new Vector3(0f, 90f, 0f));
            capsule.Call("setmodel", "vehicle_remote_uav");
            int UAV_REMOTE_COLLISION_RADIUS = 30;
            int UAV_REMOTE_Z_OFFSET = 0;
            capsule.Call("MakeVehicleSolidCapsule", UAV_REMOTE_COLLISION_RADIUS, UAV_REMOTE_Z_OFFSET, UAV_REMOTE_COLLISION_RADIUS);

            Call(431, 20, "active"); // objective_add
            Call(435, 20, origin); // objective_position
            Call(434, 20, "compass_waypoint_bomb"); //compass_objpoint_ac130_friendly compass_waypoint_bomb objective_icon

            //compass_waypoint_capture 동그라미

            //setminimap
            //compass_objpoint_reaper_friendly

            //precacheShader("compass_waypoint_captureneutral");
            //precacheShader("compass_waypoint_capture");
            //precacheShader("compass_waypoint_defend");

            //precacheShader("waypoint_captureneutral");
            //precacheShader("waypoint_capture");
            //precacheShader("waypoint_defend");

            //*PrecacheMiniMapIcon( "compass_objpoint_reaper_friendly" );
            //*PrecacheMiniMapIcon( "compass_objpoint_reaper_enemy" );


            int fx = Call<int>("loadfx", "misc/aircraft_light_wingtip_green");
            capsule.AfterDelay(200, c =>
             {
                 Call("playFXOnTag", fx, capsule, "tag_light_tail1");

                 Call("playFXOnTag", fx, capsule, "tag_light_nose");
             });

            player.Call("setorigin", player.Origin + Common.GetVector(0, 0, 140));
            return false;
        }

        bool AddHuman()
        {
            Entity jugg = null;
            int i = 0;
            foreach (Entity bot in BOTs_List)
            {
                if (i == BOT_LUCKY_IDX) continue;
                i++;
                if (bot.EntRef == BOT_JUGG_ENTREF)
                {
                    jugg = bot;
                    Print(bot.Name);
                    continue;
                }
                human_List.Add(bot);
                bot.Call("setorigin", ADMIN.Origin);
            }
            Utilities.RawSayAll("executed");
            Call(42, "testClients_doMove", 1);
            Call(42, "testClients_doAttack", 0);

            jugg.Call("setorigin", ADMIN.Origin);
            return false;
        }
        bool ComeToMe()
        {
            foreach(Entity bot in BOTs_List)
            {
                if(bot.CurrentWeapon=="riotshield_mp")continue;
                bot.Call("setorigin", ADMIN.Origin);
            }
            return false;
        }
#endif

        bool AdminCommand(string text)
        {
            if (AD == null) AD = new Admin(ADMIN);

            var texts = text.Split(' '); string value = null;
            if (texts.Length > 1) { text = texts[0]; value = texts[1]; }

            switch (text)
            {
                case "end": BotHeli.BOT_HELI.Call("delete");return false;
#if DEBUG
                case "tome": return BotToMe();
                case "stop": stop = !stop; return false;
                case "rm": VehicleTest(ADMIN); return false;
                case "plane": PredatorStart(ADMIN,H_FIELD[ADMIN.EntRef]); return false;
                case "3rdon": Viewchange(ADMIN, true);return false;
                case "3rdoff": Viewchange(ADMIN, false);return false;
                case "pp": PK.Perk_Hud(ADMIN, int.Parse(value));return false;

                case "y": return hudelemY(value);//9
                case "x": return hudelemX(value);//50
                case "va": return hudelemVA(value);//bottom
                case "ha": return hudelemHA(value);//left
                case "txt": return hudelemTXT(value);
                case "com": return compass(value);

                case "capsule": return SolidCapsule(ADMIN);

                case "tank": TK.SetTank(ADMIN); return false;
                case "to": return TurretOrigin(ADMIN, value);
                case "toff": teston = false; return false;
                case "ton": teston = true; return false;

                //case "130":
                //    {
                //        if (ac130 == null) ac130 = new AC130();
                //        ac130.start(ADMIN);
                //    }
                //    return false;


                case "test": AddHuman(); return false;
            
                case "go":
                    float a = float.Parse(value.Split(',')[0]);
                    float b = float.Parse(value.Split(',')[1]);
                    float c = float.Parse(value.Split(',')[2]);
                    ADMIN.Call("setorigin", new Vector3(a, b, c));return false;
                case "attack": return BotDoAttack(!SET.StringToBool(Call<string>("getdvar", "testClients_doAttack")));
                case "p": Print((int)ADMIN.Origin.X + "," + (int)ADMIN.Origin.Y + "," + (int)ADMIN.Origin.Z); return false;
#endif
                case "safe": SET.USE_ADMIN_SAFE_ = !SET.USE_ADMIN_SAFE_; Utilities.RawSayTo(ADMIN, "ADMIN SAFE : " + SET.USE_ADMIN_SAFE_); return false;
                case "heli":CP.Marker(ADMIN, H_FIELD[ADMIN.EntRef], 2);return false;

                case "ultest": return AD.Script("unloadscript test.dll", true);
                case "ltest": return AD.Script("loadscript test.dll", true);
                case "fr": return AD.Script("fast_restart", false);
                case "mr": return AD.Script("map_rotate", false);

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
