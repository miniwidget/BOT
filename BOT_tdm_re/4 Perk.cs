using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Tdm
{
    public partial class Tdm
    {
        void GivePerkToHumanKiller(int entref)
        {
            int killer_idx = KILLER_ENTREF[entref];
            if (killer_idx == -1) return;

            GivePerkTo(killer_idx);
            KILLER_ENTREF[entref] = -1;
        }

        void GivePerkTo(int k)
        {
            if (k >= human_List.Count) return;

            Entity killer = human_List[k];
            if (killer.EntRef > 17) return;//deny tank

            H_SET H = H_FIELD[killer.EntRef];
            if (H.PERK > 34) return;

            var i = H.PERK += 1;
            if (i == 9) H.PERK_TXT = H.PERK_TXT.Replace("^1PRDT", "HELI");

            if (H.PERK_TXT.Length != 17) H.HUD_PERK_COUNT.SetText(H.PERK_TXT += "*");

            if (i > 2 && i % 3 == 0)//give perk & show hud
            {
                i = i / 3; if (i > 10) return;
                PK.Perk_Hud(killer, i,H.AXIS);
                killer.Call(33466, "mp_killstreak_radar");
            }
            else if (i == 8)//give predator
            {
                H.CAN_USE_PREDATOR = true;

                if (CP.CARE_PACKAGE != null) killer.Call(33344, Info.GetStr("PRESS *[ [{+activate}] ] ^7AT THE CARE PACKAGE", H.AXIS));
                else killer.Call(33344, Info.GetStr("PRESS *[ [{+activate}] ] ^7TO CALL PREDATOR", H.AXIS));

                string txt = H.PERK_TXT;
                H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "^1" + txt);
            }
            else if (i == 11)//give helicopter
            {
                H.HUD_PERK_COUNT.SetText(H.PERK_TXT = "^1HELI **********");

                H.CAN_USE_HELI = true;
                HCT.HeliAttachFlagTag(killer);
            }
        }

    }

    class Perk
    {
        #region Perk

        int
            X2 = -100, X2_ = -80, X2__ = -30,
            Y2 = 250,
            jm = 80, jm_ = 120;

        float
            alp = 0.1f,
            alp_2 = 0.2f,
            alp_ = 0.8f,
            alp__ = 0.5f;

        internal void Perk_Hud(Entity player, int i,bool Axis)
        {
            i -= 1;

            HudElem PH = HudElem.NewClientHudElem(player);
            PH.HorzAlign = "left";
            PH.Foreground = true;
            PH.X = X2_;
            PH.Y = Y2;
            PH.Alpha = alp_;
            PH.SetShader(PL[i], jm, jm);
            PH.Call(32895, 0.25f); PH.X = X2__;//"moveovertime"

            HudElem CH = HudElem.NewClientHudElem(player);
            CH.HorzAlign = "left";
            //CH.Parent = HudElem.UIParent;
            CH.X = X2;
            CH.Y = Y2;
            CH.Alpha = alp;
            CH.Foreground = true;
            CH.SetShader(PL[i] + "_upgrade", jm, jm);

            PH.X = X2__;
            PH.Alpha = alp__;
            PH.SetShader(PL[i], jm_, jm_);
            PH.Call(32895, 0.25f); PH.X = X2_;

            CH.X = X2;
            CH.Alpha = alp_2;
            CH.SetShader(PL[i] + "_upgrade", jm, jm);
            CH.Call(32895, 0.25f); CH.X = X2_;

            string
                
                perk1 = null,
                perk2 = null,
                perk3 = null,
                perk4 = null,
                perk5 = null,
                say = null;

            player.AfterDelay(1000, p =>
            {
                PH.Call(32897);

                CH.X = X2_;
                CH.Alpha = alp__;
                CH.SetShader(PL[i] + "_upgrade", jm_, jm_);
                CH.Call(32895, 0.25f); CH.X = X2;

                player.AfterDelay(1000, pp => CH.Call(32897));
                Tdm.PlayDialog(player, Axis, i);
            });

            if (i == 0)
            {
                //specialty_fastreload
                perk1 = "specialty_quickswap";
                perk2 = "specialty_longerrange";
                perk3 = "specialty_twoprimaries";
                perk4 = "specialty_overkillpro";
                say = " ^2] SLEIGHT_OF_HAND PRO";
            }
            else if (i == 1)
            {
                //specialty_quickdraw
                perk1 = "specialty_fastoffhand";
                perk2 = "specialty_reducedsway";
                say = " ^2] QUICKDRAW PRO";
            }
            else if (i == 2)
            {
                //specialty_longersprint
                perk1 = "specialty_fastmantle";
                perk2 = "specialty_lightweight";
                perk3 = "specialty_fastsprintrecovery";
                say = " ^2] LONGERSPRINT PRO";
            }
            else if (i == 3)
            {
                //specialty_stalker
                perk1 = "specialty_delaymine";
                perk2 = "specialty_marksman";
                perk3 = "specialty_ironlungs";
                say = " ^2] STALKER PRO";
            }
            else if (i == 4)
            {
                //specialty_scavenger
                perk1 = "specialty_extraammo";
                perk2 = "specialty_bulletpenetration";
                perk3 = "specialty_moredamage";
                perk4 = "specialty_bulletaccuracy";

                say = " ^2] SCAVENGER PRO";
            }
            else if (i == 5)
            {
                //specialty_paint
                perk1 = "specialty_paint_pro";
                perk2 = "specialty_sharp_focus";
                perk3 = "specialty_steadyaimpro";
                perk4 = "specialty_holdbreathwhileads";

                say = " ^2] STEADYAIM PRO";
            }
            else if (i == 6)
            {
                //specialty_quieter
                perk1 = "specialty_bulletdamage";
                perk2 = "specialty_detectexplosive";
                perk3 = "specialty_selectivehearing";

                say = " ^2] DEADSILENCE PRO";
            }
            else if (i == 7)
            {
                //specialty_blindeye
                perk1 = "specialty_fasterlockon";
                perk2 = "specialty_armorpiercing";
                perk3 = "specialty_autospot";

                say = " ^2] BLINDEYE PRO";
            }
            else if (i == 8)
            {
                //specialty_coldblooded
                perk1 = "specialty_heartbreaker";
                perk2 = "specialty_spygame";
                perk3 = "specialty_empimmune";

                say = " ^2] ASSASSIN PRO";
            }
            else if (i == 9)
            {
                //specialty_blastshield
                perk1 = "specialty_stun_resistance";

                say = " ^2] BLASTSHIELD PRO";
            }
            else if (i == 10)
            {
                //specialty_hardline
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
    }

}
