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
            //smoke/smoke_grenade_11sec_mp
            try
            {

                //int i = 0;
                //string a = null;
                //while (i < 31)
                //{
                //    i++;
                //    int x = Call<int>("loadfx", "smoke/signal_smoke_airdrop_" + i.ToString() + "sec");
                //    a += " " + x.ToString();
                //}
                //test.Print(a);
                //i = 0;
                //a = null;
                //while (i < 31)
                //{
                //    i++;
                //    int x = Call<int>("loadfx", "smoke/signal_smoke_airdrop_" + i.ToString() + "sec_mp");
                //    a += " " + x.ToString();
                //}
                //test.Print(a);
                //int loadfx = Call<int>("loadfx", s);
                //test.Print("FX INT :  " + );
            }
            catch
            {

            }

        }
       internal void LoadFX(string s)
        {
            try
            {
                int i = int.Parse(s);

                test.ADMIN.AfterDelay(500, e =>
                {
                    Call("PlayFX", i,test.ADMIN.Origin);
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
