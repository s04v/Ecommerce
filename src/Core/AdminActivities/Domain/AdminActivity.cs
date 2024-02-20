using Core.AdminActivities.Domain;
using Core.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Activities.Domain
{
    public class AdminActivity
    {
        public int Id { get; set; }
        public string Log { get; set; }
        public AdminActivityAreaEnum Area { get; set; }
        public Guid AdminUuid { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public User Admin { get; set; }
    }
}
