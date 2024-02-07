using Core.Auth.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Users.Domain
{
    public class User
    {
        public Guid Uuid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual IEnumerable<LoginHistory> LoginHistory { get; set; }
    }
}
