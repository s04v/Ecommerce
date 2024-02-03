using Core.Auth.Domain;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> User { get; set; }
        DbSet<LoginHistory> LoginHistory { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
