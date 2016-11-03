using System;
using System.Threading.Tasks;
using Colors.Net;
using WebJobs.Script.Cli.Arm;

namespace WebJobs.Script.Cli.Actions.AzureActions
{
    [Action(Name = "list", Context = Context.Azure, SubContext = Context.Account)]
    [Action(Name = "list", Context = Context.Azure, SubContext = Context.Subscriptions)]
    class ListAzureAccountsAction : BaseAction
    {
        private readonly IArmManager _armManager;

        public ListAzureAccountsAction(IArmManager armManager)
        {
            _armManager = armManager;
        }

        public override async Task RunAsync()
        {
            var subscriptions = await _armManager.GetSubscriptionsAsync();
            foreach (var subscription in subscriptions)
            {
                ColoredConsole.WriteLine($"{subscription.SubscriptionId} ({subscription.DisplayName})");
            }
        }
    }
}
