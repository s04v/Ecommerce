using Common.Data;
using Core.Auth;
using Core.Auth.Dtos;
using Core.Users.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Core
{
    public class AuthTests : IAsyncLifetime
    {
        private readonly IServiceProvider _provider;
        private readonly IApplicationDbContext _dbContext;

        public AuthTests()
        {
            _provider = IoC.GetServiceProvider();
            _dbContext = _provider.GetService<IApplicationDbContext>();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            var guestRole = new Role
            {
                Name = "Guest",
                IsSystem = true
            };

            await _dbContext.Role.AddAsync(guestRole);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task RegisterAndLogin()
        {
            var authService = _provider.GetService<IAuthService>();

            // Add user 
            await authService.RegisterAsync(new RegisterDto
            {
                Email = "email@ecom.com",
                Password = "Password@1",
                FirstName = "",
                LastName = "",
            });

            var user = await _dbContext.User
                .Where(o => o.Email == "email@ecom.com")
                .FirstOrDefaultAsync();

            Assert.NotNull(user);

            var token = await authService.LoginAsync(new LoginDto
            {
                Email = "email@ecom.com",
                Password = "Password@1",
                IpAddress = "0.0.0.0",
                UserDevice = "Windows"
            });

            Assert.NotEmpty(token);

            var loginHistory = await _dbContext.LoginHistory
                .Where(o => o.UserUuid == user.Uuid)
                .FirstOrDefaultAsync();

            Assert.NotNull(loginHistory);
        }
    }
}