using System;
using System.Linq;
using System.Threading.Tasks;
using Colors.Net;
using WebJobs.Script.Cli.Arm;

namespace WebJobs.Script.Cli.Actions.AzureActions
{
    [Action(Name = "login", Context = Context.Azure)]
    class LoginAction : BaseAction
    {
        private readonly IArmManager _armManager;

        public LoginAction(IArmManager armManager)
        {
            _armManager = armManager;
        }

        public override async Task RunAsync()
        {
            await _armManager.LoginAsync();
            _armManager.DumpTokenCache().ToList().ForEach(l => ColoredConsole.WriteLine(l));
        }
    }
}
