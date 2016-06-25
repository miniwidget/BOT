using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class Bot
    {
        #region infinityscript
        TReturn Call<TReturn>(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            return Function.Call<TReturn>(func, parameters);
        }

        void Call(string func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        void Call(int func, params Parameter[] parameters)
        {
            Function.SetEntRef(-1);
            Function.Call(func, parameters);
        }
        #endregion

        void spawndBot()
        {
            Entity b = Utilities.AddTestClient();
            if (b == null) return;
            b.SpawnedPlayer += () => testClientSpawned(b);

        }
        void testClientSpawned(Entity bot)
        {
            test.Print(bot.Name + " Connected");
            //giveWeapon(bot);
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
