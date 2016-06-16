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

            allies_weap_hud.SetText(ALLIES_HUD_TEXTS + offhand);

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

        void AxisHud(Entity player)
        {
            player.Notify("CLOSE");
            //player.Notify("CLOSE_perk");
            //human_List.Remove(player);
            

            HudElem axis_weap_hud = HudElem.CreateFontString(player, "hudbig", 0.5f);
            axis_weap_hud.X = 740;
            axis_weap_hud.Y = 150;
            axis_weap_hud.AlignX = "right";
            axis_weap_hud.HideWhenInMenu = true;
            axis_weap_hud.Foreground = false;
            axis_weap_hud.Alpha = 0f;
            axis_weap_hud.SetText("^7type following\n\n^2infow ^7weapon info\n^2sc ^7 suicide\n^2riot ^7 riotshield\n^2stinger ^7stinger\n\n^7bind key at option\n\n^2[{+movedown}] ^7 throwingknife\n^2[{+prone}] ^7 bouncingbetty\n^2[{+stance}] ^7 claymore");

            player.OnNotify("open_", entity => axis_weap_hud.Alpha = 0.6f);
            player.OnNotify("close_", entity => axis_weap_hud.Alpha = 0f);
            player.OnNotify("CLOSE_", entity => axis_weap_hud.Call(32897));

            AfterDelay(t3, () => player.Notify("open_"));
        }
        
    }
}
