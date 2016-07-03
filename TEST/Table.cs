using InfinityScript;
using System;
using System.Collections.Generic;
using System.IO;
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
            Print("typed: " + i);
            string value = null;
            value = Call<string>("tableLookup", "mp/killstreakTable.csv", 0, i, 9);
            Print("result" + value);
        }
        internal void GetTeamName(string teamRef)
        {
            /*
            game[teamref]

            "delta_multicam": //US_
            "sas_urban": //UK_
            "gign_paris": //FR_
            "pmc_africa": //PC_
            "opforce_air":// RU_
            "opforce_snow":// RU_
            "opforce_urban":// RU_
            "opforce_woodland":// RU_
            "opforce_africa":// AF_
            "opforce_henchmen": // IC_

            */
            string value = Call<string>("tableLookup", "mp/factionTable.csv", 0, teamRef, 7);
            Print("value: " + value);
        }
        internal void GetModel()
        {
            //getviewmodel
            /*
            getweaponmodel
            getweaponslistall
            getattachmodelname
            getplayerweaponmodel


            */
            Entity player = test.ADMIN;
            string getviewmodel = player.Call<string>("getviewmodel");
            string getweaposlistall = player.Call<string>("getweaposlistall");
            string getplayerweaponmodel = player.Call<string>("getplayerweaponmodel");
            Print(getviewmodel + "\n" + getweaposlistall + "\n" + getplayerweaponmodel);
            Print("here");
            List<string> modelList = new List<string>();
            for (int i = 0; i < 2048; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;
                var model = ent.GetField<string>("model");
                modelList.Add(model);
            }
            File.WriteAllLines(@"z:\model.txt", modelList.ToArray());
        }

        //void ViewModel(string teamRef)
        //{
        //    foreach (Entity p in Players)
        //    {
        //        var s = p.GetField<string>("model");
        //        Print(s);
        //    }
        //}

    }
}
