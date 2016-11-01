using System.Collections.Generic;
using System.Threading.Tasks;
using Fclp;

namespace WebJobs.Script.Cli.Interfaces
{
    internal interface IAction
    {
        ICommandLineParserResult ParseArgs(IEnumerable<string> args);
        Task RunAsync();
    }
}
