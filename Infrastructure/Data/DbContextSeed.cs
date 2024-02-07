using Common.Data;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Infrastructure.Data
{
    public static class DbContextSeed
    {
        public static async Task Seed(IApplicationDbContext dbContext)
        {
            await SeedPermissions(dbContext);
            await SeedSuperAdmin(dbContext);
        }

        private static async Task SeedPermissions(IApplicationDbContext dbContext)
        {
            if (dbContext.RolePermission.Any())
            {
                return;
            }

            var permissionToInsert = new List<Permission>();

            foreach (string name in Enum.GetNames(typeof(PermissionEnum)))
            {
                permissionToInsert.Add(new Permission { Name = name });
            }

            var permissions = await dbContext.Permission.ToListAsync();

            foreach(var permission in permissionToInsert)
            {
                if (!permissions.Any(o => o.Name == permission.Name))
                {
                    await dbContext.Permission.AddAsync(permission);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedSuperAdmin(IApplicationDbContext dbContext)
        {
            if(dbContext.User.Any() || dbContext.Role.Any())
            {
                return;
            }

            var role = new Role
            {
                Name = "SuperAdmin",
            };

            await dbContext.Role.AddAsync(role);
            await dbContext.SaveChangesAsync();

            string passwordHash = BCryptNet.HashPassword("admin", 10);

            var superAdmin = new User
            {
                FirstName = "Super",
                LastName = "Admin",
                Email = "admin@ecom.com",
                Password = passwordHash,
                RoleId = role.Id,
            };

            await dbContext.User.AddAsync(superAdmin);
            await dbContext.SaveChangesAsync();

            var permissions = await dbContext.Permission
                .ToListAsync();


            foreach (var permission in permissions)
            {
                await dbContext.AddAsync(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id
                });
                await dbContext.SaveChangesAsync();

            }
        }

    }
}
