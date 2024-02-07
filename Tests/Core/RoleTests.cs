using Common.Data;
using Common.Exceptions;
using Core.Users.Domain;
using Core.Users.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Core
{
    public class RoleTests : IAsyncLifetime
    {
        private readonly IServiceProvider _provider;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAccessGuard _accessGuard;
        private readonly IRoleService _roleService;

        public RoleTests()
        {
            _provider = IoC.GetServiceProvider();
            _dbContext = _provider.GetService<IApplicationDbContext>();
            _accessGuard = _provider.GetService<IAccessGuard>();
            _roleService = _provider.GetService<IRoleService>();
        }

        public async Task DisposeAsync()
        {
            await (_dbContext as ApplicationDbContext).Database.EnsureDeletedAsync();
        }

        public async Task InitializeAsync()
        {
            var role = new Role
            {
                Id = 1,
                Name = "admin"
            };

            var permission = new Permission
            {
                Id = 1,
                Name = "Product",
            };

            var permission2 = new Permission
            {
                Id = 2,
                Name = "Catalog",
            };

            var rolePermission = new RolePermission
            {
                PermissionId = 1,
                RoleId = 1,
            };

            var user = new User
            {
                Email = "email@ecom.com",
                FirstName = "",
                LastName = "",
                Password = "",
                RoleId = 1,
            };

            await _dbContext.AddAsync(role);
            await _dbContext.AddAsync(permission);
            await _dbContext.AddAsync(permission2);
            await _dbContext.AddAsync(rolePermission);
            await _dbContext.AddAsync(user);

            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task UserHasPermission()
        {
            var user = await _dbContext.User
                .FirstAsync(o => o.Email == "email@ecom.com");

            Assert.True(await _accessGuard.HasPermitTo(user.Uuid, PermissionEnum.Product));
        }

        [Fact]
        public async Task UserHasNoPermission()
        {
            var user = await _dbContext.User
               .FirstAsync(o => o.Email == "email@ecom.com");

            Assert.False(await _accessGuard.HasPermitTo(user.Uuid, PermissionEnum.Category));
        }

        [Fact]
        public async Task UserHasPermissionThrow()
        {
            var user = await _dbContext.User
              .FirstAsync(o => o.Email == "email@ecom.com");

            await _accessGuard.ThrowIfHasNoPermitTo(user.Uuid, PermissionEnum.Product);
        }

        [Fact]
        public async Task UserHasNoPermissionThrow()
        {
            var user = await _dbContext.User
            .FirstAsync(o => o.Email == "email@ecom.com");

            await Assert.ThrowsAsync<DomainException>(
                async () => await _accessGuard.ThrowIfHasNoPermitTo(user.Uuid, PermissionEnum.Category)
            );
        }


        [Fact]
        public async Task CreateNewRole()
        {
            var roleName = "Manager";

            await _roleService.CreateRole(roleName);

            Assert.NotNull(await _dbContext.Role.FirstOrDefaultAsync(o => o.Name == roleName));

            await Assert.ThrowsAsync<DomainException>(async () => await _roleService.CreateRole(roleName));
        }

        [Fact]
        public async Task AddPermissionForRole()
        {
            int roleId = 1;
            int productPermissionId = 1;
            int catalogPermissionId = 2;

            await _roleService.AddPermit(roleId, catalogPermissionId);

            Assert.NotNull(
                await _dbContext.RolePermission
                    .Where(o => o.RoleId == roleId && o.PermissionId == catalogPermissionId)
                    .FirstOrDefaultAsync()
            );

            await Assert.ThrowsAsync<DomainException>(async () => await _roleService.AddPermit(roleId, productPermissionId));
        }

        [Fact]
        public async Task RemovePermission()
        {
            int roleId = 1;
            int productPermissionId = 1;
            int catalogPermissionId = 2;

            await _roleService.RemovePermit(roleId, productPermissionId);

            Assert.Null(
                await _dbContext.RolePermission
                    .Where(o => o.RoleId == roleId && o.PermissionId == productPermissionId)
                    .FirstOrDefaultAsync()
            );

            await Assert.ThrowsAsync<DomainException>(async () => await _roleService.RemovePermit(roleId, catalogPermissionId));
        }
    }
}
