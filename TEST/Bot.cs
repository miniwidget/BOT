using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class Bot : InfinityBase
    {

        void spawndBot()
        {
            Entity bot = Utilities.AddTestClient();
            if (bot == null) return;
            Print(bot.Name + " Connected");
            bot.SpawnedPlayer += () =>
            {
                bot.Call("setorigin", test.ADMIN.Origin);
            };


        }

        void KickBOTsAll()
        {
            for (int i = 0; i < 18; i++)
            {
                Entity ent = Entity.GetEntity(i);

                if (ent == null) continue;
                if (ent.Call<string>("getguid").Contains("bot"))
                {
                    Call("kick", i);
                }
            }
        }
    }
}
