using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    public partial class test
    {
        internal static void Print(object s)
        {
            Log.Write(LogLevel.None, "{0}", s.ToString());
        }
        bool GetADMIN()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity p = Entity.GetEntity(i);

                if (p == null) continue;
                if (p.Name == "kwnav")
                {
                   test.ADMIN = p;
                    break;
                }
            }
            return true;
        }
        void ViewModel(string teamRef)
        {
            foreach (Entity p in Players)
            {
                var s = p.GetField<string>("model");
                test.Print(s);
            }
        }
        void Script(string command, bool restart)
        {
            AfterDelay(1000, () =>
            {
                Utilities.ExecuteCommand(command);
                if (restart)
                {
                    Utilities.ExecuteCommand("fast_restart");
                }
            });

        }

        bool VIEWCHANGED_;
        void Viewchange()
        {
            Entity player = ADMIN;
            if (!VIEWCHANGED_)
            {
                player.SetClientDvar("cg_thirdperson", "1");
                player.SetClientDvar("camera_thirdPerson", "1");
                player.SetClientDvar("camera_thirdPersonCrosshairOffset", "0.35");//default 0.35 //0
                player.SetClientDvar("camera_thirdPersonFovScale ", "0.9");//default 0.35 //0
                player.SetClientDvar("camera_thirdPersonOffsetAds", "-60 -20 4");//default 2
                player.SetClientDvar("camera_thirdPersonOffset", "-120 0 14");//default -120커지면확대 0-좌+우 14커지면 위에서, 작아지면 밑에서 봄

            }else
            {
                player.SetClientDvar("cg_thirdperson", "0");
                player.SetClientDvar("camera_thirdPerson", "0");
            }
            VIEWCHANGED_ = !VIEWCHANGED_;
            
        }

    }
}
