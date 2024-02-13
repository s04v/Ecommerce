using Common.Data;
using Core.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Users.Interfaces
{
    public interface IAccessGuard
    {
        Task<bool> HasPermitTo(Guid userUuid, PermissionEnum permission);

        Task ThrowIfHasNoPermitTo(Guid userUuid, PermissionEnum permission);
    }
}
