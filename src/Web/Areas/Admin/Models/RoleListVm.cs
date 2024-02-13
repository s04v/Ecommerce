using Core.Users.Domain;

namespace Web.Areas.Admin.Models
{
    public class RoleListVm
    {
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Permission> AllPermissions { get; set; }
    }
}
