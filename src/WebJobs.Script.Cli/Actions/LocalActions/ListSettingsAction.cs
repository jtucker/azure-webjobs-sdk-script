using System;
using System.Threading.Tasks;

namespace WebJobs.Script.Cli.Actions.LocalActions
{
    [Action(Name = "list", Context = Context.Settings)]
    class ListSettingsAction : BaseAction
    {
        public override Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
