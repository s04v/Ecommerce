using Common.Data;
using Common.Exceptions;
using Core.Users.Domain;
using Core.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Users
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _dbContext;

        public UserService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dbContext.User
                .Include(o => o.Role)
                .ToListAsync();
        }

        public async Task<User> GetUserById(Guid uuid)
        {
            return await _dbContext.User
                .Where(o => o.Uuid == uuid)
                .FirstAsync();
        }

        public async Task SetRole(Guid userUuid, int roleId)
        {
            var user = await _dbContext.User
                .Where(o => o.Uuid == userUuid)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new DomainException("User not found");
            }

            user.RoleId = roleId;

            await _dbContext.SaveChangesAsync();
        }
    }
}
