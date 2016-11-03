using System;
using System.Threading.Tasks;

namespace WebJobs.Script.Cli.Actions.LocalActions
{
    [Action(Name = "run", Context = Context.Function)]
    class RunFunctionAction : BaseAction
    {
        public override Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
