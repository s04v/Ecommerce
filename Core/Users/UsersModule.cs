using Core.Auth.Domain;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core.Users
{
    public static class UsersModule
    {
        public static void AddUsersModule(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
