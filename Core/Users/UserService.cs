using Common.Data;
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
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _dbContext;

        public UserService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _dbContext.User
                .Include(o => o.LoginHistory.OrderByDescending(o => o.Date).Take(1))
                .ToListAsync();
        }
    }
}
