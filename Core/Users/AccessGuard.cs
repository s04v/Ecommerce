using Common.Data;
using Common.Exceptions;
using Core.Users.Domain;
using Core.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Users
{
    public class AccessGuard : IAccessGuard
    {
        private readonly IApplicationDbContext _dbContext;

        public AccessGuard(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasPermitTo(Guid userUuid, PermissionEnum permission)
        {
            var role = await _dbContext.User
                .Include(o => o.Role)
                .Select(o => o.Role)
                .FirstOrDefaultAsync();

            if (role?.Permissions.ToList().Find(o => o.Name == permission.ToString()) != null)
            {
                return true; 
            } 
            else
            {
                return false;
            }
        }

        public async Task ThrowIfHasNoPermitTo(Guid userUuid, PermissionEnum permission)
        {
            if (!(await HasPermitTo(userUuid, permission)))
            {
                throw new DomainException("access denied");
            }
        }
    }
}
