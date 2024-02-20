using Core.Activities.Domain;
using Core.AdminActivities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Activities
{
    public interface IAdminActivityService
    {
        Task InsertActivity(AdminActivityAreaEnum area, string log);

        Task<IEnumerable<AdminActivity>> GetAllActivities();
    }
}
