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

            if (i > 10 || i<0) return;
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

            player.AfterDelay(t1, p =>
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
            player.AfterDelay(t2, p =>
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
            player.AfterDelay(t3, p =>
            {
                PH.Call(32897);
                CH.Call(32897);
            });

            //player.OnNotify("CLOSE_perk", entity => PH.Call(32897));

            if (i == 0)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_quickswap", true, false);
                player.SetPerk("specialty_twoprimaries", true, false); player.SetPerk("specialty_overkillpro", true, false);
                player.SetPerk(P.CL[i], true, false);
                player.AfterDelay(100,p=>Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1SLEIGHT_OF_HAND PRO"));
            }
            else if (i == 1)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_fastoffhand", true, false);
                player.SetPerk("specialty_autospot", true, false); player.SetPerk("specialty_holdbreathwhileads", true, false);
                player.SetPerk(P.CL[i], true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1QUICKDRAW PRO"));

            }
            else if (i == 2)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_fastmantle", true, false);
                player.SetPerk(P.CL[i], true, false); player.SetPerk("specialty_bulletaccuracy", true, false); player.SetPerk("specialty_steadyaimpro", true, false); player.SetPerk("specialty_fastsprintrecovery", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1LONGERSPRINT PRO"));
            }
            else if (i == 3)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_delaymine", true, false); player.SetPerk("specialty_marksman", true, false);
                player.SetPerk(P.CL[i], true, false); player.SetPerk("specialty_fastermelee", true, false); player.SetPerk("specialty_ironlungs", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1STALKER PRO"));

            }
            else if (i == 4)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_extraammo", true, false);
                player.SetPerk(P.CL[i], true, false); player.SetPerk("specialty_detectexplosive", true, false); player.SetPerk("specialty_selectivehearing", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1SCAVENGER PRO"));

            }
            else if (i == 5)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_paint_pro", true, false);
                player.SetPerk(P.CL[i], true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1PAINT PRO"));
            }
            else if (i == 6)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_bulletdamage", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1DEADSILENCE PRO"));
            }
            else if (i == 7)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_fasterlockon", true, false); player.SetPerk("specialty_armorpiercing", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1BLINDEYE PRO"));
            }
            else if (i == 8)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_heartbreaker", true, false); player.SetPerk("specialty_spygame", true, false); player.SetPerk("specialty_empimmune", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1ASSASSIN PRO"));
            }
            else if (i == 9)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_stun_resistance", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1BLASTSHIELD PRO"));
            }
            else if (i == 10)
            {
                player.SetPerk(P.PL[i], true, false); player.SetPerk("specialty_rollover", true, false); player.SetPerk("specialty_assists", true, false);
                player.AfterDelay(100, p => Utilities.RawSayTo(player, "^2[^7 " + player.Name + " ^2] ^1HARDLINE PRO"));
            }

        }
        #endregion

    }

}
