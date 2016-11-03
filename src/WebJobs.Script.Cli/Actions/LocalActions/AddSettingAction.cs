using System;
using System.Threading.Tasks;

namespace WebJobs.Script.Cli.Actions.LocalActions
{
    [Action(Name = "add", Context = Context.Settings)]
    class AddSettingAction : BaseAction
    {
        public override Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
