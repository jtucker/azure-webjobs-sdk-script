using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebJobs.Script.Cli.Interfaces;

namespace WebJobs.Script.Cli
{
    class ConsoleApp
    {
        //private readonly IDependencyResolver _dependencyResolver;
        private readonly string[] _args;
        private readonly IEnumerable<ActionType> _actionTypes;
        //private readonly string _cliName;
        //private bool _isFaulted = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Task RunAsync<T>(string[] args, IDependencyResolver dependencyResolver = null)
        {
            var app = new ConsoleApp(args, typeof(T).Assembly, dependencyResolver);
            app.Parse();
            return Task.CompletedTask;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Run<T>(string[] args, IDependencyResolver dependencyResolver = null)
        {
            Task.Run(() => RunAsync<T>(args, dependencyResolver)).Wait();
        }

        internal ConsoleApp(string[] args, Assembly assembly, IDependencyResolver dependencyResolver)
        {
            _args = args;
            //_dependencyResolver = dependencyResolver;
            dependencyResolver.ToString();
            //_cliName = Process.GetCurrentProcess().ProcessName.ToLowerInvariant();
            _actionTypes = assembly
                .GetTypes()
                .Where(t => typeof(IAction).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(type => 
                {
                    var attributes = type.GetCustomAttributes<ActionAttribute>();
                    return new ActionType
                    {
                        Type = type,
                        Contexts = attributes.Select(a => a.Context),
                        SubContexts = attributes.Select(a => a.SubContext),
                        Names = attributes.Select(a => a.Name)
                    };
                });
        }

        internal void Parse()
        {
#if DEBUG
            //ConsoleAppUtilities.ValidateVerbs(_verbTypes);
#endif
            if (_args.Length == 0)
            {
                // this will be help
            }

            var argsStack = new Stack<string>(_args.Reverse());
            var actions = Enumerable.Empty<ActionType>();
            var arg = argsStack.Peek();
            var context = Context.None;
            var subContext = Context.None;
            var action = string.Empty;

            if (Enum.TryParse(arg, true, out context))
            {
                argsStack.Pop();
                if (argsStack.Any())
                {
                    arg = argsStack.Peek();
                    if (Enum.TryParse(arg, true, out subContext))
                    {
                        argsStack.Pop();
                    }
                }
            }

            if (argsStack.Any())
            {
                action = argsStack.Pop();
            }

            if (context == Context.None && string.IsNullOrEmpty(action))
            {
                // error case 
            }

            actions = _actionTypes
                .Where(a => a.Names.Any(n => n.Equals(action, StringComparison.OrdinalIgnoreCase)))
                .Where(c => c.Contexts.Contains(context))
                .Where(c => c.SubContexts.Contains(subContext));

            foreach(var a in actions)
            {
                Console.WriteLine(a.Type.ToString());
            }
        }
    }
}
