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
        #region Perk

        int Y1 = 65, k = 120, j = 40;
        int X2 = -120, X2_ = -100, X2__ = -50, Y2 = 250, Y2_ = 250, Y2__ = 250, jm = 80, jm_ = 80, jm__ = 120;
        float alp_ = 0.8f, alp0 = 0.1f, alp = 0.1f, alp_2 = 0.2f, alp__ = 0.5f, f1 = 0.25f, f2 = 0.25f;

        void Perk_Hud(Entity player, int i)
        {
            if (GAME_ENDED_) return;

            if (i > 10 || i < 0) return;
            i -= 1;
            //print(player.Name + " perk count : " + i);

            HudElem PH = HudElem.NewClientHudElem(player);
            PH.Foreground = true;
            player.OnNotify("+TAB", entity => PH.Alpha = 0.5f);
            player.OnNotify("-TAB", entity => PH.Alpha = 0f);
            PH.X = X2_;
            PH.Y = Y2_;
            PH.Alpha = alp_;
            PH.SetShader(P.PL[i], jm_, jm_);
            PH.Call("moveovertime", f2); PH.X = X2__;

            HudElem CH = HudElem.NewClientHudElem(player);
            CH.Parent = HudElem.UIParent;
            CH.X = X2;
            CH.Y = Y2;
            CH.Alpha = alp;
            CH.Foreground = true;
            CH.SetShader(P.PL[i] + "_upgrade", jm, jm);

            AfterDelay(t1, () =>
            {
                PH.X = X2__;
                PH.Y = Y2__;
                PH.Alpha = alp__;
                PH.SetShader(P.PL[i], jm__, jm__);
                PH.Call("moveovertime", f1); PH.X = X2_;

                CH.X = X2;
                CH.Y = Y2_;
                CH.Alpha = alp_2;
                CH.SetShader(P.PL[i] + "_upgrade", jm_, jm_);
                CH.Call("moveovertime", f1); CH.X = X2_;
            });
            AfterDelay(t2, () =>
            {
                PH.X = k + (j * i);
                PH.Y = Y1;
                PH.Alpha = alp0;
                PH.SetShader(P.PL[i], j, j);

                CH.X = X2_;
                CH.Y = Y2__;
                CH.Alpha = alp__;
                CH.SetShader(P.PL[i] + "_upgrade", jm__, jm__);
                CH.Call("moveovertime", f1); CH.X = X2;
            });
            AfterDelay(t3, () =>
            {
                PH.Call(32897);
                CH.Call(32897);
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
                perk4 = P.CL[i];
                say = "^2[^7 " + player.Name + " ^2] ^1SLEIGHT_OF_HAND PRO";
            }
            else if (i == 1)
            {
                perk1 = "specialty_fastoffhand";
                perk2 = "specialty_autospot";
                perk3 = "specialty_holdbreathwhileads";
                perk4 = P.CL[i];
                say = "^2[^7 " + player.Name + " ^2] ^1QUICKDRAW PRO";
            }
            else if (i == 2)
            {
                perk1 = "specialty_fastmantle";
                perk2 = P.CL[i];
                perk3 = "specialty_bulletaccuracy";
                perk4 = "specialty_steadyaimpro";
                perk5 = "specialty_fastsprintrecovery";
                say = "^2[^7 " + player.Name + " ^2] ^1LONGERSPRINT PRO";
            }
            else if (i == 3)
            {
                perk1 = "specialty_delaymine";
                perk2 = "specialty_marksman";
                perk3 = P.CL[i];
                perk4 = "specialty_fastermelee";
                perk5 = "specialty_ironlungs";
                say = "^2[^7 " + player.Name + " ^2] ^1STALKER PRO";
            }
            else if (i == 4)
            {
                perk1 = "specialty_extraammo";
                perk2 = P.CL[i];
                perk3 ="specialty_detectexplosive";
                perk4 ="specialty_selectivehearing";
                say = "^2[^7 " + player.Name + " ^2] ^1SCAVENGER PRO";
            }
            else if (i == 5)
            {
                perk1 ="specialty_paint_pro";
                perk2 = P.CL[i];
                say = "^2[^7 " + player.Name + " ^2] ^1PAINT PRO";
            }
            else if (i == 6)
            {
                perk1 ="specialty_bulletdamage";
                say = "^2[^7 " + player.Name + " ^2] ^1DEADSILENCE PRO";
            }
            else if (i == 7)
            {
                perk1 ="specialty_fasterlockon";
                perk2 ="specialty_armorpiercing";
                say = "^2[^7 " + player.Name + " ^2] ^1BLINDEYE PRO";
            }
            else if (i == 8)
            {
                perk1 ="specialty_heartbreaker";
                perk2 ="specialty_spygame";
                perk3 ="specialty_empimmune";
                say = "^2[^7 " + player.Name + " ^2] ^1ASSASSIN PRO";
            }
            else if (i == 9)
            {
                perk1 ="specialty_stun_resistance";
                say = "^2[^7 " + player.Name + " ^2] ^1BLASTSHIELD PRO";
            }
            else if (i == 10)
            {
                perk1 ="specialty_rollover";
                perk2 ="specialty_assists";
                say = "^2[^7 " + player.Name + " ^2] ^1HARDLINE PRO";
            }

            AfterDelay(100, () => Utilities.RawSayTo(player, say));

            player.SetPerk(P.PL[i], true, false);

            if (perk1 == null) return; player.SetPerk(perk1, true, false);
            if (perk2 == null) return; player.SetPerk(perk2, true, false);
            if (perk3 == null) return; player.SetPerk(perk3, true, false);
            if (perk4 == null) return; player.SetPerk(perk4, true, false);
            if (perk5 == null) return; player.SetPerk(perk5, true, false);


        }
        #endregion

    }

}
