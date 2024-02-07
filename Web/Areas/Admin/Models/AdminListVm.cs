using Core.Users.Domain;

namespace Web.Areas.Admin.Models
{
    public class AdminListVm
    {
        public IEnumerable<User> Users { get; set; }
    }
}
