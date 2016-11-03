using System;
using System.Threading.Tasks;
using Colors.Net;
using WebJobs.Script.Cli.Arm;

namespace WebJobs.Script.Cli.Actions.AzureActions
{
    [Action(Name = "set", Context = Context.Azure, SubContext = Context.Account)]
    [Action(Name = "set", Context = Context.Azure, SubContext = Context.Subscriptions)]
    class SetAzureAccountAction : BaseAction
    {
        public SetAzureAccountAction()
        {
        }

        public override Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
