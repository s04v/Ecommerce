using Core.Users.Interfaces;
using Core.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Catalog.Interfaces;

namespace Core.Catalog
{
    public static class CatalogModule
    {
        public static void AddCatalogModule(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
