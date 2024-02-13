using Core.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Users.Interfaces
{
    public interface IRoleService
    {
        Task<Role> CreateRole(string name);
        Task AddPermit(int roleId, int permissionId);
        Task RemovePermit(int roleId, int permissionId);
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role> GetRole(int id);
        Task<IEnumerable<Permission>> GetAllPermissions();
    }
}
