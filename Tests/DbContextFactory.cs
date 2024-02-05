using Common.Data;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class DbContextFactory
    {
        public static IApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseInMemoryDatabase("ecommerce");

            var dbContext = new ApplicationDbContext(options.Options);

            return dbContext;
        }
    }
}
