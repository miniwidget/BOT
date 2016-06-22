using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    class PerkList  //PerkLIST
    {
        internal readonly int
             Y1 = 65, k = 120, j = 40,
             X2 = -120, X2_ = -100, X2__ = -50, Y2 = 250, Y2_ = 250, Y2__ = 250, jm = 80, jm_ = 80, jm__ = 120;
        internal readonly float
            alp_ = 0.8f, alp0 = 0.1f, alp = 0.1f, alp_2 = 0.2f, alp__ = 0.5f, f1 = 0.25f, f2 = 0.25f;

        internal readonly string[] PL =
        {
            "specialty_fastreload",
            "specialty_quickdraw",
            "specialty_longersprint",
            "specialty_stalker",
            "specialty_scavenger",
            "specialty_paint",
            "specialty_quieter",
            "specialty_blindeye",
            "specialty_coldblooded",
            "specialty_blastshield",
            "specialty_hardline"
        };

        internal readonly string[] DL =
        {

            "specialty_ironlungs",
            "specialty_steadyaim",
            "specialty_bombsquad",
            "specialty_twoprimaries",

        };
        internal readonly string[] CL =
        {
            "specialty_longerrange",
            "specialty_reducedsway",
            "specialty_lightweight",
            "specialty_sharp_focus",
            "specialty_bulletpenetration",
            "specialty_moredamage"
         };

    }

}
