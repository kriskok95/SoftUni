using System;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core;
using MyApp.Core.Contracts;
using MyApp.Data;


namespace MyApp
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            IServiceProvider services = ConfigureServices();
            IEngine engine = new Engine(services);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<MyAppContext>(db => db.UseSqlServer(@"Server=DESKTOP-MKU8P03\SQLEXPRESS;Database=MySpecialAppDb;Integrated Security=true"));

            serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>();
            serviceCollection.AddTransient<Mapper>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
 