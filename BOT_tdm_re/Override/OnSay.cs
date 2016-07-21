using System;
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

                    case "riot": GUN.GiveWeaponTo(player, "riotshield_mp"); return;
                    case "javelin": GUN.GiveWeaponTo(player, "javelin_mp"); return;
                    case "stinger": GUN.GiveWeaponTo(player, "stinger_mp"); return;

                    case "ap":
                    case "ag":
                    case "ar":
                    case "sm":
                    case "lm":
                    case "sg":
                    case "sn": GUN.GiveWeaponByType(player, text0); return;

                    case "vs":
                    case "at":
                    case "sl": GUN.GiveOtherTypes(player, text0, H_FIELD[player.EntRef].GUN); return;

                    case "am":
                        {
                            player.Call(33523, player.CurrentWeapon);//givemaxammo = full stock & not clip
                            player.Call(33468, player.CurrentWeapon, 100);//setweaponammoclip
                        }
                        return;
                    case "rpg": GUN.GiveWeaponTo(player, "rpg_mp"); return;
                    case "smaw": GUN.GiveWeaponTo(player, "iw5_smaw_mp"); return;
                    case "m320": GUN.GiveWeaponTo(player, "m320_mp"); return;
                    case "xm25": GUN.GiveWeaponTo(player, "xm25_mp"); return;

                }

            }
            else if (length == 2 )
            {
                int i;
                if (!int.TryParse(texts[1], out i)) i = 0; 

                switch (text0)
                {
                    case "ap":
                    case "ag":
                    case "ar":
                    case "sm":
                    case "lm":
                    case "sg":
                    case "sn": GUN.GiveWeaponByNum(player, text0, i); return;
                }
            }
        }
    }
}
