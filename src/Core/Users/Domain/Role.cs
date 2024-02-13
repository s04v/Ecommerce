using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Users.Domain
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSystem { get; set; } = false;

        public IEnumerable<Permission> Permissions { get; set; }
    }
}
