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

        HudElem SERVER_HUD;
        void Server_Hud()
        {
            SERVER_HUD = HudElem.CreateServerFontString("hudbig", 0.8f);
            SERVER_HUD.X = 240;
            SERVER_HUD.Y = 3;
            SERVER_HUD.Alpha = 0.7f;
            SERVER_HUD.HideWhenInMenu = true;
            SERVER_HUD.SetText(SERVER_NAME);
        }
        readonly string ALLIES_TOP_HUD_TEXTS = "ATTACHMENT *INFOA\n^7WEAPONINFO *INFOW";
        readonly string ALLIES_RIGHT_TEXTS =
@"^7TYPE FOLLOWING
^2AP ^74 AKIMBO PISTOL
^2AG ^76 AKIMBO GUN
^2AR ^79 ASSAU RIFFLE
^2SM ^76 SUB M_GUN
^2LM ^75 LIGHT M_GUN
^2SG ^74 SHOT GUN
^2SN ^76 SNIPE GUN

^7PRESS KEY
^2[{+strafe}] ^7AMMO
^2[{+movedown}] ^7VIEWSCOPE
^2[{+prone}] ^7ATTATCHMENT";


        internal void AlliesHud(Entity player, bool show)
        {
            var H = Tdm.H_FIELD[player.EntRef];

            H.HUD_SERVER = HudElem.CreateFontString(player, "hudbig", 0.8f);
            H.HUD_SERVER.X = 240;
            H.HUD_SERVER.Y = 3;
            if (show) H.HUD_SERVER.Alpha = 0.7f;
            else H.HUD_SERVER.Alpha = 0;
            H.HUD_SERVER.HideWhenInMenu = true;
            H.HUD_SERVER.SetText(Info.GetStr(SERVER_NAME, false));

            H.HUD_TOP_INFO = HudElem.CreateFontString(player, "hudbig", 0.4f);
            H.HUD_TOP_INFO.Alpha = 0.8f;
            H.HUD_TOP_INFO.X = -40;
            H.HUD_TOP_INFO.Y = -10;
            H.HUD_TOP_INFO.HorzAlign = "right";
            H.HUD_TOP_INFO.HideWhenInMenu = true;
            H.HUD_TOP_INFO.SetText(Info.GetStr(ALLIES_TOP_HUD_TEXTS, false));
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
            H.HUD_RIGHT_INFO.SetText(Info.GetStr(ALLIES_RIGHT_TEXTS, false));

            H.HUD_PERK_COUNT = HudElem.CreateFontString(player, "hudbig", 0.65f);

            H.HUD_PERK_COUNT.X = 0;
            H.HUD_PERK_COUNT.Y = 9;

            H.HUD_PERK_COUNT.Foreground = false;
            H.HUD_PERK_COUNT.VertAlign = "bottom";
            H.HUD_PERK_COUNT.HorzAlign = "left";

            H.HUD_PERK_COUNT.HideWhenInMenu = true;
            H.HUD_PERK_COUNT.Alpha = 1f;
            H.HUD_PERK_COUNT.SetText("");

        }


    }
}
