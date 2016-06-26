using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Infected
{
    internal class Perk
    {
        #region Perk

        int
            X2 = -120, X2_ = -100, X2__ = -50, Y2 = 250, Y2_ = 250, Y2__ = 250, jm = 80, jm_ = 80, jm__ = 120;
        float alp_ = 0.8f, alp0 = 0.1f, alp = 0.1f, alp_2 = 0.2f, alp__ = 0.5f;

        internal void Perk_Hud(Entity player, int i)
        {
            if (i > 10 || i < 0) return;
            i -= 1;

            HudElem PH = HudElem.NewClientHudElem(player);
            PH.Foreground = true;
            PH.X = X2_;
            PH.Y = Y2_;
            PH.Alpha = alp_;
            PH.SetShader(PL[i], jm_, jm_);
            PH.Call("moveovertime", 0.25f); PH.X = X2__;

            HudElem CH = HudElem.NewClientHudElem(player);
            CH.Parent = HudElem.UIParent;
            CH.X = X2;
            CH.Y = Y2;
            CH.Alpha = alp;
            CH.Foreground = true;
            CH.SetShader(PL[i] + "_upgrade", jm, jm);

            PH.X = X2__;
            PH.Y = Y2__;
            PH.Alpha = alp__;
            PH.SetShader(PL[i], jm__, jm__);
            PH.Call("moveovertime", 0.25f); PH.X = X2_;

            CH.X = X2;
            CH.Y = Y2_;
            CH.Alpha = alp_2;
            CH.SetShader(PL[i] + "_upgrade", jm_, jm_);
            CH.Call("moveovertime", 0.25f); CH.X = X2_;

            player.AfterDelay(1000, p =>
            {
                PH.Call(32897);

                CH.X = X2_;
                CH.Y = Y2__;
                CH.Alpha = alp__;
                CH.SetShader(PL[i] + "_upgrade", jm__, jm__);
                CH.Call("moveovertime", 0.25f); CH.X = X2;

                player.AfterDelay(1000, pp => CH.Call(32897));
            });
          

            string
                say = null,
                perk1 = null,
                perk2 = null,
                perk3 = null,
                perk4 = null,
                perk5 = null;

            if (i == 0)
            {
                perk1 = "specialty_quickswap";
                perk2 = "specialty_twoprimaries";
                perk3 = "specialty_overkillpro";
                perk4 = CL[i];
                say = " ^2] SLEIGHT_OF_HAND PRO";
            }
            else if (i == 1)
            {
                perk1 = "specialty_fastoffhand";
                perk2 = "specialty_autospot";
                perk3 = "specialty_holdbreathwhileads";
                perk4 = CL[i];
                say = " ^2] QUICKDRAW PRO";
            }
            else if (i == 2)
            {
                perk1 = "specialty_fastmantle";
                perk2 = CL[i];
                perk3 = "specialty_bulletaccuracy";
                perk4 = "specialty_steadyaimpro";
                perk5 = "specialty_fastsprintrecovery";
                say = " ^2] LONGERSPRINT PRO";
            }
            else if (i == 3)
            {
                perk1 = "specialty_delaymine";
                perk2 = "specialty_marksman";
                perk3 = CL[i];
                perk4 = "specialty_fastermelee";
                perk5 = "specialty_ironlungs";
                say = " ^2] STALKER PRO";
            }
            else if (i == 4)
            {
                perk1 = "specialty_extraammo";
                perk2 = CL[i];
                perk3 = "specialty_detectexplosive";
                perk4 = "specialty_selectivehearing";
                say = " ^2] SCAVENGER PRO";
            }
            else if (i == 5)
            {
                perk1 = "specialty_paint_pro";
                perk2 = CL[i];
                say = " ^2] PAINT PRO";
            }
            else if (i == 6)
            {
                perk1 = "specialty_bulletdamage";
                say = " ^2] DEADSILENCE PRO";
            }
            else if (i == 7)
            {
                perk1 = "specialty_fasterlockon";
                perk2 = "specialty_armorpiercing";
                say = " ^2] BLINDEYE PRO";
            }
            else if (i == 8)
            {
                perk1 = "specialty_heartbreaker";
                perk2 = "specialty_spygame";
                perk3 = "specialty_empimmune";
                say = " ^2] ASSASSIN PRO";
            }
            else if (i == 9)
            {
                perk1 = "specialty_stun_resistance";
                say = " ^2] BLASTSHIELD PRO";
            }
            else if (i == 10)
            {
                perk1 = "specialty_rollover";
                perk2 = "specialty_assists";
                say = " ^2] 1HARDLINE PRO";
            }

            Utilities.RawSayTo(player, "^2[^7 " + player.Name + say);

            player.SetPerk(PL[i], true, false);

            if (perk1 == null) return; player.SetPerk(perk1, true, false);
            if (perk2 == null) return; player.SetPerk(perk2, true, false);
            if (perk3 == null) return; player.SetPerk(perk3, true, false);
            if (perk4 == null) return; player.SetPerk(perk4, true, false);
            if (perk5 == null) return; player.SetPerk(perk5, true, false);
        }
        #endregion

         readonly string[] PL =
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

        readonly string[] DL =
        {
            "specialty_ironlungs",
            "specialty_steadyaim",
            "specialty_bombsquad",
            "specialty_twoprimaries",

        };
        readonly string[] CL =
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
