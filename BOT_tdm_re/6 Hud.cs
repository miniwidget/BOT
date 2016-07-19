using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tdm
{
    class Hud
    {
        internal void ChangeHud(Entity player,bool show)
        {
            H_SET H = Tdm.H_FIELD[player.EntRef];

            H.HUD_TOP_INFO.SetText(Info.GetStr(PLAYERS_TOP_HUD_TEXTS, H.AXIS));
            H.HUD_RIGHT_INFO.SetText(Info.GetStr(PLAYERS_RIGHT_TEXTS, H.AXIS));

            if (H.AXIS) H.HUD_SERVER.SetText(SERVER_NAME_.Replace("^2", "^1")); else H.HUD_SERVER.SetText(SERVER_NAME_);
        }
        internal void PlayersHud(Entity player, H_SET H , bool show)
        {
            H.HUD_SERVER = HudElem.CreateFontString(player, "hudbig", 0.8f);
            H.HUD_SERVER.X = 240;
            H.HUD_SERVER.Y = 3;
            if (show) H.HUD_SERVER.Alpha = 0.7f;
            else H.HUD_SERVER.Alpha = 0;
            H.HUD_SERVER.HideWhenInMenu = true;
            if (Tdm.TEST_) SERVER_NAME_ = "^2TEST ^7SERVER";
            if (H.AXIS) H.HUD_SERVER.SetText(SERVER_NAME_.Replace("^2", "^1")); else H.HUD_SERVER.SetText(SERVER_NAME_);

            H.HUD_TOP_INFO = HudElem.CreateFontString(player, "hudbig", 0.4f);
            H.HUD_TOP_INFO.Alpha = 0.8f;
            H.HUD_TOP_INFO.X = -40;
            H.HUD_TOP_INFO.Y = -10;
            H.HUD_TOP_INFO.HorzAlign = "right";
            H.HUD_TOP_INFO.HideWhenInMenu = true;
            H.HUD_TOP_INFO.SetText(Info.GetStr(PLAYERS_TOP_HUD_TEXTS, H.AXIS));
            H.HUD_TOP_INFO.Call(32895, 2f);// "moveovertime"
            H.HUD_TOP_INFO.Y = 5;

            H.HUD_RIGHT_INFO = HudElem.CreateFontString(player, "hudbig", 0.5f);
            H.HUD_RIGHT_INFO.HorzAlign = "right";
            if (show) H.HUD_RIGHT_INFO.Alpha = 0.7f;
            else H.HUD_RIGHT_INFO.Alpha = 0;
            H.HUD_RIGHT_INFO.X = -40;
            H.HUD_RIGHT_INFO.Y = 150;
            H.HUD_RIGHT_INFO.HideWhenInMenu = true;
            H.HUD_RIGHT_INFO.Foreground = false;
            H.HUD_RIGHT_INFO.SetText(Info.GetStr(PLAYERS_RIGHT_TEXTS, H.AXIS));

            H.HUD_PERK_COUNT = HudElem.CreateFontString(player, "hudbig", 0.65f);
            H.HUD_PERK_COUNT.X = 0;
            H.HUD_PERK_COUNT.Y = 9;
            H.HUD_PERK_COUNT.Foreground = false;
            H.HUD_PERK_COUNT.VertAlign = "bottom";
            H.HUD_PERK_COUNT.HorzAlign = "left";
            H.HUD_PERK_COUNT.HideWhenInMenu = true;
            H.HUD_PERK_COUNT.Alpha = 1f;
            H.HUD_PERK_COUNT.SetText(H.PERK_TXT);
        }


        readonly string PLAYERS_TOP_HUD_TEXTS = "WEAPONINFO *INFOW\n^7KILLSTERAKS *INFOK";
        internal static string SERVER_NAME_;
        readonly string PLAYERS_RIGHT_TEXTS =
@"TYPE FOLLOWING

*AP ^74 AKIMBO PISTOL
*AG ^76 AKIMBO GUN
*AR ^79 ASSAU RIFFLE
*SM ^76 SUB M_GUN
*LM ^75 LIGHT M_GUN
*SG ^74 SHOT GUN
*SN ^76 SNIPE GUN

*SC ^7 SUICIDE
*VS ^7VIEW SCOPE
*AMMO ^7GET AMMO";

    }
}
