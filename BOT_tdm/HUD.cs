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
        string ALLIES_HUD_TEXTS =
            @"
^7TYPE FOLLOWING
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
^2[{+prone}] ^7ATTATCHMENT
^2[{+stance}] ^7";

     
        void AlliesHud(Entity player,string offhand)
        {
            HudElem allies_info_hud = HudElem.CreateFontString(player, "hudbig", 0.4f);
            allies_info_hud.Alpha = 0.8f;
            allies_info_hud.X = -40;
            allies_info_hud.Y = -10;
            allies_info_hud.HorzAlign = "right";
            allies_info_hud.HideWhenInMenu = true;
            allies_info_hud.SetText("ATTACHMENT ^2INFOA\n^7WEAPONINFO ^2INFOW");
            allies_info_hud.Call(32895, 2f);// "moveovertime"
            allies_info_hud.Y = 5;

            HudElem allies_weap_hud = HudElem.CreateFontString(player, "hudbig", 0.5f);
            allies_weap_hud.HorzAlign = "right";
            allies_weap_hud.Alpha = 0.6f;
            allies_weap_hud.X = -40;
            allies_weap_hud.Y = 150;
            allies_weap_hud.HideWhenInMenu = true;
            allies_weap_hud.Foreground = false;
            allies_weap_hud.SetText(ALLIES_HUD_TEXTS);
            HudElem hud = HudElem.CreateFontString(player, "hudbig", 0.8f);

            hud.X = 0;
            hud.Y = 9;

            hud.Foreground = false;
            hud.VertAlign = "bottom";
            hud.HorzAlign = "left";

            hud.HideWhenInMenu = true;
            hud.Alpha = 1f;
            hud.SetText("");
            Tdm.H_FIELD[player.EntRef].PERK_COUNT_HUD = hud;
        }


    }
}
