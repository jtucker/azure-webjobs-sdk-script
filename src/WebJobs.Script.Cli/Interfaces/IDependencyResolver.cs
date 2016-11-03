using System;

namespace WebJobs.Script.Cli.Interfaces
{
    interface IDependencyResolver
    {
        T GetService<T>();
        object GetService(Type type);
        void RegisterService<T>(object obj);
    }
}
