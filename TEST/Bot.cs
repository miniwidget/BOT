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
            Entity b = Utilities.AddTestClient();
            if (b == null) return;
            b.SpawnedPlayer += () => testClientSpawned(b);

        }
        void testClientSpawned(Entity bot)
        {
            test.Print(bot.Name + " Connected");
            bot.Call("setorigin",test.ADMIN.Origin);
        }

        List<Entity> BOTs_List = new List<Entity>();
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
