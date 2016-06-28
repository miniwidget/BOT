using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace TEST
{
    class Weapon : InfinityBase
    {
        void magicBullet(Entity player)
        {
            player.OnNotify("weapon_fired", (p, weaponName) =>
            {
                Call("magicbullet", "ac130_25mm_mp", // bullet name
                    new Parameter(player.Call<Vector3>("getTagOrigin", "tag_weapon_left")), // start point
                    new Parameter(Call<Vector3>("anglestoforward", player.Call<Vector3>("getPlayerAngles")) * 1000000), // end point
                    new Parameter(player)); // ignore entity
            });
        }

        void giveWeapon(Entity player)
        {
            player.Call("freezecontrols", true);
            player.AfterDelay(500, p=>
            {
                //player.GiveWeapon("iw5_fmg9_mp_akimbo");
                //player.SwitchToWeaponImmediate("iw5_fmg9_mp_akimbo");
            });

        }


    }
}
