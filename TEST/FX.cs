using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class FX : InfinityBase
    {
        #region FX
 

        internal void triggerfx(string s)
        {
            int i = Call<int>("loadfx", s);
            if (i <= 0) return;

            Entity Effect = Call<Entity>("spawnFx", i, test.ADMIN.Origin + new Vector3(0, 0, 40));
            Call("triggerfx", Effect);

            test.ADMIN.AfterDelay(4000, p => Effect.Call("delete"));

        }
        internal void PlayFX(string s)
        {
            int i = Call<int>("LoadFX", s);//"smoke/signal_smoke_airdrop_30sec"
            if (i <= 0) return;

            Call("PlayFX", i, test.ADMIN.Origin + new Vector3(0, 0, 50));

        }

        #endregion

    }
}
