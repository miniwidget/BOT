using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommands
{
    public partial class sc// : BaseScript
    {
        internal Dictionary<string, List<Func<string[], bool>>> _serverCommandHandlers = new Dictionary<string, List<Func<string[], bool>>>();
        internal Dictionary<string, List<Action<Entity, string[]>>> _clientCommandHandlers = new Dictionary<string, List<Action<Entity, string[]>>>();

        internal bool ProcessServerCommand(string command, string[] args)
        {
            bool eat = false;
            if (_serverCommandHandlers.ContainsKey(command))
            {
                var handles = _serverCommandHandlers[command];
                foreach (var handle in handles)
                {
                    if (handle(args))
                    {
                        eat = true;
                    }
                }
            }
            return eat;
        }

        public void OnServerCommand(string command, Func<string[], bool> func)
        {
            if (!_serverCommandHandlers.ContainsKey(command))
            {
                _serverCommandHandlers[command] = new List<Func<string[], bool>>();
            }
            _serverCommandHandlers[command].Add(func);
        }
        public void OnServerCommand(string command, Action<string[]> func)
        {
            OnServerCommand(command, args =>
            {
                func(args);
                return true;
            });
        }
        public void OnServerCommand(string command, Action func)
        {
            OnServerCommand(command, args =>
            {
                func();
                return true;
            });
        }

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
