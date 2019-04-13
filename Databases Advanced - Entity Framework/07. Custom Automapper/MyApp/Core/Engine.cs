using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Contracts;

namespace MyApp.Core
{
    public class Engine : IEngine
    {
        private readonly IServiceProvider provider;

        public Engine(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Run()
        {
            while (true)
            { 
                string[] inputArgs = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                if (inputArgs[0] == "Exit")
                {
                    return;
                }

                var commandInterpreter = this.provider.GetService<ICommandInterpreter>();

                string result = commandInterpreter.Read(inputArgs);

                Console.WriteLine(result);
            }
        }      
    }
}
