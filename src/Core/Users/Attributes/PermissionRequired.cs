using Core.Users.Domain;

namespace Web.Middleware
{
    public class PermissionRequired : Attribute
    {
        public PermissionEnum Permission { get; set; }

        public PermissionRequired(PermissionEnum permission)
        {
            Permission = permission;
        }
    }
}
