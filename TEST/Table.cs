﻿using InfinityScript;
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
            test.Print(getviewmodel + "\n" + getweaposlistall + "\n" + getplayerweaponmodel);
            test.Print("here");
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
    }
}
