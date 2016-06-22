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
        void PerkWait(Field F, int time)
        {
            F.wait = true;
            AfterDelay(time, () => F.wait = false);

            int idx = F.killerIdx;
            if (idx == -1) return;
            F.killerIdx = -1;

            if (idx > HUMAN_LIST.Count-1) return;
            Entity e = HUMAN_LIST[idx];
            if (e.CurrentWeapon[2] != '5') return;//iw5
            idx = e.EntRef; if (idx == -1) return;
            Field H = FL[idx];
            if (H.PERK < 34 )//iw5
            {
                var i = (H.PERK += 1);

                if (i > 2 && i % 3 == 0)
                {
                    i = i / 3; if (i > 10) return;

                    PerkHud(ref e, i);
                }
                else if (i == 11)
                {
                    H.USE_HELI = 1;
                    HeliAttachFlagTag(ref e);
                }
            }
        }

        void PerkHud(ref Entity player,  int i)
        {
            player.Call(33466, "mp_killstreak_radar");//playlocalsound
            i -= 1;

            HudElem PH = HudElem.NewClientHudElem(player);
            PH.Foreground = true;
            PH.X = CPL.X2_;
            PH.Y = CPL.Y2_;
            PH.Alpha = CPL.alp_;
            PH.SetShader(CPL.PL[i], CPL.jm_, CPL.jm_);
            PH.Call(32895, CPL.f2); PH.X = CPL.X2__;

            HudElem CH = HudElem.NewClientHudElem(player);
            CH.Parent = HudElem.UIParent;
            CH.X = CPL.X2;
            CH.Y = CPL.Y2;
            CH.Alpha = CPL.alp;
            CH.Foreground = true;
            CH.SetShader(CPL.PL[i] + "_upgrade", CPL.jm, CPL.jm);

            player.AfterDelay(t1, x =>
            {
                PH.X = CPL.X2__;
                PH.Y = CPL.Y2__;
                PH.Alpha = CPL.alp__;
                PH.SetShader(CPL.PL[i], CPL.jm__, CPL.jm__);
                PH.Call(32895, CPL.f1); PH.X = CPL.X2_;

                CH.X = CPL.X2;
                CH.Y = CPL.Y2_;
                CH.Alpha = CPL.alp_2;
                CH.SetShader(CPL.PL[i] + "_upgrade", CPL.jm_, CPL.jm_);
                CH.Call(32895, CPL.f1); CH.X = CPL.X2_;

                x.AfterDelay(t1, xx =>
                {
                    PH.X = CPL.k + (CPL.j * i);
                    PH.Y = CPL.Y1;
                    PH.Alpha = CPL.alp0;
                    PH.SetShader(CPL.PL[i], CPL.j, CPL.j);

                    CH.X = CPL.X2_;
                    CH.Y = CPL.Y2__;
                    CH.Alpha = CPL.alp__;
                    CH.SetShader(CPL.PL[i] + "_upgrade", CPL.jm__, CPL.jm__);
                    CH.Call(32895, CPL.f1); CH.X = CPL.X2;

                    xx.AfterDelay(t1, xxx =>
                    {
                        PH.Call(32897);
                        CH.Call(32897);
                    });
                });
          
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
                perk4 = CPL.CL[i];
                say = " ^2] SLEIGHT_OF_HAND PRO";
            }
            else if (i == 1)
            {
                perk1 = "specialty_fastoffhand";
                perk2 = "specialty_autospot";
                perk3 = "specialty_holdbreathwhileads";
                perk4 = CPL.CL[i];
                say = " ^2] QUICKDRAW PRO";
            }
            else if (i == 2)
            {
                perk1 = "specialty_fastmantle";
                perk2 = CPL.CL[i];
                perk3 = "specialty_bulletaccuracy";
                perk4 = "specialty_steadyaimpro";
                perk5 = "specialty_fastsprintrecovery";
                say = " ^2] LONGERSPRINT PRO";
            }
            else if (i == 3)
            {
                perk1 = "specialty_delaymine";
                perk2 = "specialty_marksman";
                perk3 = CPL.CL[i];
                perk4 = "specialty_fastermelee";
                perk5 = "specialty_ironlungs";
                say = " ^2] STALKER PRO";
            }
            else if (i == 4)
            {
                perk1 = "specialty_extraammo";
                perk2 = CPL.CL[i];
                perk3 ="specialty_detectexplosive";
                perk4 ="specialty_selectivehearing";
                say = " ^2] SCAVENGER PRO";
            }
            else if (i == 5)
            {
                perk1 ="specialty_paint_pro";
                perk2 = CPL.CL[i];
                say = " ^2] PAINT PRO";
            }
            else if (i == 6)
            {
                perk1 ="specialty_bulletdamage";
                say = " ^2] DEADSILENCE PRO";
            }
            else if (i == 7)
            {
                perk1 ="specialty_fasterlockon";
                perk2 ="specialty_armorpiercing";
                say = " ^2] BLINDEYE PRO";
            }
            else if (i == 8)
            {
                perk1 ="specialty_heartbreaker";
                perk2 ="specialty_spygame";
                perk3 ="specialty_empimmune";
                say = " ^2] ASSASSIN PRO";
            }
            else if (i == 9)
            {
                perk1 ="specialty_stun_resistance";
                say = " ^2] BLASTSHIELD PRO";
            }
            else if (i == 10)
            {
                perk1 ="specialty_rollover";
                perk2 ="specialty_assists";
                say = " ^2] 1HARDLINE PRO";
            }

            Utilities.RawSayTo(player, "^2[^7 " + player.Name +say);

            player.SetPerk(CPL.PL[i], true, false);

            if (perk1 == null) return; player.SetPerk(perk1, true, false);
            if (perk2 == null) return; player.SetPerk(perk2, true, false);
            if (perk3 == null) return; player.SetPerk(perk3, true, false);
            if (perk4 == null) return; player.SetPerk(perk4, true, false);
            if (perk5 == null) return; player.SetPerk(perk5, true, false);


        }
        #endregion

    }

}
