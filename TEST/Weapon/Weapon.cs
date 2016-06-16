using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Timers;

namespace TEST
{
    public partial class test
    {
        void magicBullet()
        {
            ADMIN.OnNotify("weapon_fired", (p, weaponName) =>
            {
                Call("magicbullet", "ac130_25mm_mp", // bullet name
                    new Parameter(ADMIN.Call<Vector3>("getTagOrigin", "tag_weapon_left")), // start point
                    new Parameter(Call<Vector3>("anglestoforward", ADMIN.Call<Vector3>("getPlayerAngles")) * 1000000), // end point
                    new Parameter(ADMIN)); // ignore entity
            });
        }

    }
}
