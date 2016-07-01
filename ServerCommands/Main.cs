using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommands
{
    public partial class sc : BaseScript
    {
        /// <summary>
        /// Console Commands
        /// If type these commands at Console, commands will be executed
        /// </summary>
        /// 
        public sc()
        {
            //CheckTEST();
            OnServerCommand("/", (string[] txts) =>
            {
                int lenth = txts.Length;

                if (lenth > 1)
                {
                    if (txts[1] == "say")
                    {
                        SayToAll(txts);
                        return;
                    }

                    if (lenth == 2)
                    {
                        Commands(txts[1]);
                    }
                    else
                    {
                        Commands(txts);
                    }
                }

            });

        }

        //bool TEST_;
        //bool CheckTEST()
        //{
        //    var assembly = System.Reflection.Assembly.GetExecutingAssembly();

        //    if (assembly.Location.Contains("test"))
        //    {
        //        return TEST_ = true;
        //    }
        //    return TEST_ = false;
        //}

    }
}
