using Common.Data;
using Core.Users.Domain;
using Core.Users.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core
{
    public class UserTests : IAsyncLifetime
    {
        private readonly IServiceProvider _provider;
        private readonly IApplicationDbContext _dbContext;
        private readonly IUserService _userService;

        public UserTests()
        {
            _provider = IoC.GetServiceProvider();
            _dbContext = _provider.GetService<IApplicationDbContext>();
            _userService = _provider.GetService<IUserService>();
        }

        public async Task InitializeAsync()
        {
            var adminRole = new Role
            {
                Id = 1,
                Name = "admin"
            };

            var guestRole = new Role
            {
                Id = 2,
                Name = "guest"
            };


            var user = new User
            {
                Email = "email@ecom.com",
                FirstName = "",
                LastName = "",
                Password = "",
                RoleId = 2,
            };

            await _dbContext.AddAsync(adminRole);
            await _dbContext.AddAsync(guestRole);
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DisposeAsync()
        {
            await (_dbContext as ApplicationDbContext).Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task SetRole()
        {
            var roleId = 1;
            var user = await _dbContext.User
                .Where(o => o.Email == "email@ecom.com")
                .FirstAsync();

            await _userService.SetRole(user.Uuid, roleId);

            user = await _dbContext.User
                .Where(o => o.Email == "email@ecom.com")
                .FirstAsync();

            Assert.Equal(roleId, user.RoleId);
        }
       
    }
}
