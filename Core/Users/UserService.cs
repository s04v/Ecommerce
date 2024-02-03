using Common.Data;
using Core.Users.Domain;
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
    }s
}
