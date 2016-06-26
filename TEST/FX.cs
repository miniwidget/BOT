using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class FX
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
        #region FX
        internal void GetLoadFXInt(string s)
        {
        }
        internal void triggerfx(string s)
        {
            int x = Call<int>("loadfx", s);
            if (x > 0)
            {
                Entity Effect = Call<Entity>("spawnFx", x, test.ADMIN.Origin + new Vector3(0, 0, 40));
                Call("triggerfx", Effect);
                test.ADMIN.AfterDelay(4000, p => Effect.Call("delete"));
            }
        }
        internal void PlayFX(string s)
        {

            test.ADMIN.AfterDelay(500, e =>
            {
                int i = Call<int>("LoadFX", "smoke/signal_smoke_airdrop_30sec");
                Call("PlayFX", i, test.ADMIN.Origin + new Vector3(0, 0, 50));
                test.ADMIN.Call(33344, "OK");
            });

        }
        internal void LoadFX(string s)
        {
            try
            {
                int i = int.Parse(s);

                test.ADMIN.AfterDelay(500, e =>
                {
                    Call("PlayFX", i, test.ADMIN.Origin + new Vector3(0, 0, 50));

                });

            }
            catch
            {
                test.Print("???");
            }

        }
        #endregion

    }
}
