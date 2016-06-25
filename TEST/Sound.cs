using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class Sound
    {
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

        internal void PlayLocalSound(string s)
        {
           test.ADMIN.Call("playlocalsound", s);
        }
        internal void PlaySound(string soundname)
        {
            //string voicePrefix = Call<string>("tableLookup", "mp/factionTable.csv", 0, "allies", 7) + "0_";//US_0_
            //string bcSoounds = "rpg_incoming";
            //string soundAlias = voicePrefix + bcSoounds;//US_0_rpg_incoming
            //AF_1mc_losing_fight
           test.ADMIN.Call("playsoundtoteam", soundname, "allies");
        }
        void sound(string s)
        {
            test.Print("value" + s);
            string value = null;
            value = Call<string>("tableLookup", "mp/factionTable.csv", 0, s, 7);
            test.Print("value" + value);

           test.ADMIN.Call("playlocalsound", s);
        }

    }
}
