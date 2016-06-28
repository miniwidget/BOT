using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{

    class Table : InfinityBase
    {
        #region infinityscript
        
        #endregion

        internal void tableValue(string i)
        {
            test.Print("typed: " + i);
            string value = null;
            value = Call<string>("tableLookup", "mp/killstreakTable.csv", 0, i, 9);
            test.Print("result" + value);
        }
        internal void GetTeamName(string teamRef)
        {
            //pmc_africa

            string value = Call<string>("tableLookup", "mp/factionTable.csv", 0, teamRef, 7);
            test.Print("value: " + value);
        }
    }
}
