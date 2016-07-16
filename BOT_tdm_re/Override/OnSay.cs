﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace Tdm
{
    public partial class Tdm
    {

        public override void OnSay(Entity player, string name, string text)
        {
            if (name == ADMIN_NAME)
            {
                if(text[0]=='/') if (!AdminCommand(text.Remove(0,1))) return;
            }

            if (GAME_ENDED_) return;

            if (VEHICLE_CODE != null)
            {
                if(text == VEHICLE_CODE)
                {
                    VHC.VehicleStartRemote(player, VHC.VEHICLE, null);
                    return;
                }
            }

            string[] texts = text.ToLower().Split(' ');
            string text0 = texts[0];

            bool Axis = H_FIELD[player.EntRef].AXIS;

            int length = texts.Length;

            if (length == 1)
            {
                switch (text0)
                {
                    case "infok": INFO.MessageInfoK(player, Axis);return;
                    case "infow": INFO.MessageInfoW(player, Axis); return;

                    case "sc":  AfterDelay(100, () => player.Call(33341)); return;//"suicide"

                    case "riot": WP.GiveWeaponTo(player, "riotshield_mp"); return;
                    case "javelin": WP.GiveWeaponTo(player, "javelin_mp"); return;
                    case "stinger": WP.GiveWeaponTo(player, "stinger_mp"); return;

                    case "ap": WP.GiveWeaponTo(player, 0); return;
                    case "ag": WP.GiveWeaponTo(player, 1); return;
                    case "ar": WP.GiveWeaponTo(player, 2); return;
                    case "sm": WP.GiveWeaponTo(player, 3); return;
                    case "lm": WP.GiveWeaponTo(player, 4); return;
                    case "sg": WP.GiveWeaponTo(player, 5); return;
                    case "sn": WP.GiveWeaponTo(player, 6); return;

                    case "scope": WP.GiveAttachScope(player); return;
                    case "ammo": 
                        player.Call(33523, player.CurrentWeapon);//givemaxammo = full stock & not clip
                        player.Call(33468, player.CurrentWeapon, 100);//setweaponammoclip
                        return;
                    case "rpg": WP.GiveWeaponTo(player, "rpg_mp"); return;
                    case "smaw": WP.GiveWeaponTo(player, "iw5_smaw_mp"); return;
                    case "m320": WP.GiveWeaponTo(player, "m320_mp"); return;
                    case "xm25": WP.GiveWeaponTo(player, "xm25_mp"); return;

                }

            }
            else if (length == 2 )
            {
                int i;
                if (!int.TryParse(texts[1], out i)) i = 0; 

                switch (text0)
                {
                    case "ap": WP.GiveWeaponTo(player, 0,i); return;
                    case "ag": WP.GiveWeaponTo(player, 1, i); return;
                    case "ar": WP.GiveWeaponTo(player, 2, i); return;
                    case "sm": WP.GiveWeaponTo(player, 3, i); return;
                    case "lm": WP.GiveWeaponTo(player, 4, i); return;
                    case "sg": WP.GiveWeaponTo(player, 5, i); return;
                    case "sn": WP.GiveWeaponTo(player, 6, i); return;
                }
            }
        }
    }
}
