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
        int obj_idx=25;
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
            HudElem hud = HUD.elem;
            hud.X = float.Parse(value);
            return false;
        }
        bool hudelemY(string value)
        {
            HudElem hud = HUD.elem;
            hud.Y = float.Parse(value);
            return false;
        }
        bool hudelemVA(string value)
        {
            HudElem hud = HUD.elem;
            hud.VertAlign = value;
            return false;
        }
        bool hudelemHA(string value)
        {
            HudElem hud = HUD.elem;
            hud.HorzAlign = value;
            return false;
        }
        bool hudelemTXT(string value)
        {
            HudElem hud = HUD.elem;
            hud.SetText(value);
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
            capsule.AfterDelay(200,c=>
            {
                Call("playFXOnTag", fx, capsule, "tag_light_tail1");

                Call("playFXOnTag", fx, capsule, "tag_light_nose");
            });

            player.Call("setorigin", player.Origin + Common.GetVector(0, 0, 140));
            return false;
        }

        bool Sentry(Entity player)
        {
            if (SENTRY_GUN != null)
            {
                SENTRY_GUN.Call("delete");
                SENTRY_GUN = null;
            }

            string
                weaponInfo = "sentry_minigun_mp",
                modelBase = "sentry_minigun_weak";

            SENTRY_GUN = Call<Entity>(19, "misc_turret", player.Origin, weaponInfo);//spawnTurret
            SENTRY_GUN.Call(32929, modelBase);//setModel
            SENTRY_GUN.Call(32864, "sentry_offline");//setmode : sentry sentry_offline
            SENTRY_GUN.Call(33122, true);//setturretmodechangewait
            //SENTRY_GUN.Call("setcontents", 0);
            SENTRY_GUN.Call(33006, player);//setsentryowner
                                           //SENTRY_GUN.Call(33051, "axis");//setturretteam
                                           //SENTRY_GUN.SetField("sessionteam", "axis");
                                           //SENTRY_GUN.Call(33007, player);//setsentrycarrier

            SENTRY_GUN.Call(33083, 0f);//left arc
            SENTRY_GUN.Call(33084, 0f);// right arc

            float fireTime = 0;// weaponFireTime(level.sentrySettings[self.sentryType].weaponInfo);
                int
                    minShots = 20,
                    maxShots = 120,
                    minPause = 150,
                    maxPause = 350;

                //for (;;)
                //{
                //    numShots = randomIntRange(minShots, maxShots + 1);

                //    for (i = 0; i < numShots && !self.overheated; i++)
                //    {
                //        self shootTurret();
                //        self.heatLevel += fireTime;
                //        wait(fireTime);
                //    }

                //    wait(randomFloatRange(minPause, maxPause));
                //}
            
            return true;
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
                case "ag": Print(SENTRY_GUN.GetField<Vector3>("angles"));return false;
                case "b4":
                    {
                        ADMIN.Call("setorigin", BOTs_List[3].Origin);
                    }return false;
                case "y": return hudelemY(value);//9
                case "x":return hudelemX(value);//50
                case "va": return hudelemVA(value);//bottom
                case "ha": return hudelemHA(value);//left
                case "txt": return hudelemTXT(value);
                case "com":return compass(value);

                case "capsule": return SolidCapsule(ADMIN);
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
