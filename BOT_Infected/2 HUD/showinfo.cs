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
        void ShowInfoA(ref Entity ent)
        {
            if (!isSurvivor(ent))
            {
                ent.Call(33344, "NO FUNCTION. BYE");
                return;
            }
            roopMessage(ent, 0, MT.MESSAGES_ALLIES_INFO_A);
        }
        void ShowInfoW(ref Entity ent)
        {
            if (isSurvivor(ent))

                roopMessage(ent, 0, MT.MESSAGES_ALLIES_INFO_W);
            else
                roopMessage(ent, 0, MT.MESSAGES_AXIS_INFO_W);
        }

        void roopMessage(Entity e, int i, string[] lists)
        {
            if (e == null) return;

            var entref = e.EntRef;
            if (i == 0)
            {
                if (FL[entref].ON_MESSAGE) return;
            }
            FL[entref].ON_MESSAGE = true;

            e.Call(33344, lists[i]);
            i++;
            if (i == lists.Length)
            {
                FL[entref].ON_MESSAGE = false;
                return;
            }
            e.AfterDelay(4000, x =>
            {
                roopMessage(e, i, lists);
            });
        }
        void RM(Entity e, int i, string[] lists)
        {
            if (e == null) return;

            var entref = e.EntRef;
            if (i == 0)
            {
                if (FL[entref].ON_MESSAGE) return;
            }
            FL[entref].ON_MESSAGE = true;

            e.Call(33344, lists[i]);
            i++;
            if (i == lists.Length)
            {
                FL[entref].ON_MESSAGE = false;
                return;
            }
            e.AfterDelay(2500, x =>
            {
                roopMessage(e, i, lists);
            });
        }

        void showMessage(Entity e, string message)
        {
            if (FL[e.EntRef].ON_MESSAGE) return;
            e.Call(33344, message);
        }
    }
}

