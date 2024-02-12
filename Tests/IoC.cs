using Common.Data;
using Core.Auth;
using Core.Catalog;
using Core.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class IoC
    {
        public static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAuthModule();
            serviceCollection.AddUsersModule();
            serviceCollection.AddCatalogModule();

            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            serviceCollection.AddTransient<IConfiguration>(opt =>
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("secrets.json");
                return configurationBuilder.Build();
            });

            serviceCollection.AddDbContext<IApplicationDbContext,ApplicationDbContext>(opt =>
            {
                opt.UseInMemoryDatabase("ecommerce");
            });

            return serviceCollection.BuildServiceProvider();
        }
    }
}
