using Common.Data;
using Common.Services;
using Core.Activities.Domain;
using Core.AdminActivities.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Activities
{
    public class AdminActivityService : IAdminActivityService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IWorkContext _workContext;

        public AdminActivityService(IApplicationDbContext dbContext, IWorkContext workContext)
        {
            _dbContext = dbContext;
            _workContext = workContext;
        }

        public async Task InsertActivity(AdminActivityAreaEnum area, string log)
        {
            var userUuid = _workContext.GetCurrentUserUuid();

            var activity = new AdminActivity
            {
                Log = log,
                Area = area,
                AdminUuid = userUuid,
                CreatedDate = DateTime.Now,
            };

            await _dbContext.AddAsync(activity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AdminActivity>> GetAllActivities()
        {
            return await _dbContext.Activity
                .Include(o => o.Admin)
                .ToListAsync();
        }
    }
}
