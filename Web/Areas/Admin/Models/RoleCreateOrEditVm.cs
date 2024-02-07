using Core.Users.Domain;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models
{
    public class RoleCreateOrEditVm
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }
        public ICollection<int> SelectedPermissions { get; set; } = new List<int>();
    }
}
