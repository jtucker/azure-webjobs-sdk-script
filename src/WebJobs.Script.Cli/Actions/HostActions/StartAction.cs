using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fclp;
using WebJobs.Script.Cli.Interfaces;

namespace WebJobs.Script.Cli.Actions.HostActions
{
    [Action(Name = "start", Context = Context.Host)]
    class StartAction : IAction
    {
        public ICommandLineParserResult ParseArgs(IEnumerable<string> args)
        {
            throw new NotImplementedException();
        }

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
