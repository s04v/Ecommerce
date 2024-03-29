﻿using Common.Data;
using Common.Exceptions;
using Core.Activities;
using Core.AdminActivities.Domain;
using Core.Users.Domain;
using Core.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Users
{
    public class RoleService : IRoleService
    {
        private readonly ClaimsPrincipal _user;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAdminActivityService _activityService;
        public RoleService(IApplicationDbContext dbContext, IAdminActivityService activityService)
        {
            _dbContext = dbContext;
            _activityService = activityService;
        }

        public async Task<Role> CreateRole(string name)
        {
            if (await _dbContext.Role.AnyAsync(o => o.Name == name))
            {
                throw new DomainException($"Role with name {name} exists");
            }

            var role = new Role
            {
                Name = name,
            };

            await _dbContext.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Role,
                $"Created \"{name}\" role");

            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            var roles = await _dbContext.Role
                .Include(o => o.Permissions)
                .ToListAsync();

            return roles;
        }

        public async Task AddPermit(int roleId, int permissionId)
        {
            var role = await _dbContext.Role
                .Where(o => o.Id == roleId)
                .Include(o => o.Permissions)
                .FirstOrDefaultAsync()
            ?? throw new DomainException("Role not found");

            if (role.IsSystem)
            {
                throw new DomainException("Permissions cannot be added for this role");
            }

            var permission = await _dbContext.Permission
                 .Where(o => o.Id == permissionId)
                 .FirstOrDefaultAsync()
             ?? throw new DomainException("Permission not found");

            if (role.Permissions.Any(o => o.Id == permissionId))
            {
                throw new DomainException("Role already has that permit");
            }

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            await _dbContext.AddAsync(rolePermission);
            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Role,
                $"Add \"{permission.Name}\" permission for \"{role.Name}\" role");
        }

        public async Task RemovePermit(int roleId, int permissionId)
        {
            var rolePermission = await _dbContext.RolePermission
                .Where(o => o.RoleId == roleId && o.PermissionId == permissionId)
                .FirstOrDefaultAsync()
            ?? throw new DomainException("Not found");

            var role = await _dbContext.Role
                .Where(o => o.Id == roleId)
                .FirstAsync();

            if (role.IsSystem)
            {
                throw new DomainException("Permissions cannot be removed from this role");
            }

            _dbContext.RolePermission.Remove(rolePermission);

            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Role,
                $"Remove \"{rolePermission.Permission.Name}\" permission from \"{role.Name}\" role");
        }

        public async Task<IEnumerable<Permission>> GetAllPermissions()
        {
            var permissions = await _dbContext.Permission
                .ToListAsync();

            return permissions;
        }

        public async Task<Role> GetRole(int id)
        {
            var role = await _dbContext.Role
                .Where(o => o.Id == id)
                .Include(o => o.Permissions)
                .FirstOrDefaultAsync();

            return role;
        }
    }
}

