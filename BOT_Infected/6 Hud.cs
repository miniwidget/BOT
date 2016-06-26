using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infected
{
    internal class Hud
    {
        internal HudElem SERVER;
        internal static string SERVER_NAME_;
        readonly string ALLIES_HUD_TEXTS =
@"^7TYPE FOLLOWING
^2AP ^74 AKIMBO PISTOL
^2AG ^76 AKIMBO GUN
^2AR ^79 ASSAU RIFFLE
^2SM ^76 SUB M_GUN
^2LM ^75 LIGHT M_GUN
^2SG ^74 SHOT GUN
^2SN ^76 SNIPE GUN
^2LOC ^7RE LOCATION
^2T I ^7SAVE LOCATION

^7PRESS KEY
^2[{+strafe}] ^7AMMO
^2[{+movedown}] ^7VIEWSCOPE";
        //^2[{+prone}] ^7ATTATCHMENT

        internal void AlliesHud(Entity player)
        {
            HudElem allies_info_hud = HudElem.CreateFontString(player, "hudbig", 0.4f);
            allies_info_hud.X = 740;
            allies_info_hud.Y = -10;
            allies_info_hud.AlignX = "right";
            allies_info_hud.HideWhenInMenu = true;
            allies_info_hud.Alpha = 0f;
            allies_info_hud.SetText("ATTACHMENT ^2INFOA\n^7WEAPONINFO ^2INFOW");

            HudElem allies_weap_hud = HudElem.CreateFontString(player, "hudbig", 0.5f);
            allies_weap_hud.X = 700;
            allies_weap_hud.Y = 150;
            allies_weap_hud.AlignX = "right";
            allies_weap_hud.HideWhenInMenu = true;
            allies_weap_hud.Foreground = false;
            allies_weap_hud.Alpha = 0f;

            allies_weap_hud.SetText(ALLIES_HUD_TEXTS);
            allies_info_hud.Alpha = 0.8f; allies_weap_hud.Alpha = 0.6f;
            allies_weap_hud.Call("moveovertime", 2f);
            allies_weap_hud.X = 740;

            allies_info_hud.Call("moveovertime", 2f);
            allies_info_hud.Y = 5;

            player.OnNotify("CLOSE", e =>
            {
                allies_info_hud.Call(32897); allies_weap_hud.Call(32897);
            });
        }
        readonly string AXIS_HUD_TEXTS =
@"^7type following
^2infow ^7weapon info
^2sc ^7 suicide
^2riot ^7 riotshield
^2stinger ^7stinger
^2LOC ^7RE LOCATION";

        internal void AxisHud(Entity player)
        {
            player.Notify("CLOSE");

            HudElem axis_weap_hud = HudElem.CreateFontString(player, "hudbig", 0.5f);
            axis_weap_hud.X = 740;
            axis_weap_hud.Y = 150;
            axis_weap_hud.AlignX = "right";
            axis_weap_hud.HideWhenInMenu = true;
            axis_weap_hud.Foreground = false;
            axis_weap_hud.Alpha = 0f;
            axis_weap_hud.SetText(AXIS_HUD_TEXTS);

            player.OnNotify("open_", entity => axis_weap_hud.Alpha = 0.6f);
            player.OnNotify("close_", entity => axis_weap_hud.Alpha = 0f);
            player.OnNotify("CLOSE_", entity => axis_weap_hud.Call(32897));

            player.AfterDelay(3000, x => player.Notify("open_"));
        }
        internal void ServerHud()
        {
            SERVER = HudElem.CreateServerFontString("hudbig", 0.8f);
            SERVER.X = 240;
            SERVER.Y = 3;
            SERVER.Alpha = 0.7f;
            SERVER.HideWhenInMenu = true;

            SERVER.SetText(SERVER_NAME_);
        }

    }

}
