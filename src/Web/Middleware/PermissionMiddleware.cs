using Common.Data;
using Common.Exceptions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace Web.Middleware
{
    public class PermissionMiddleware
    {
        private RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IApplicationDbContext dbContext)
        {
            var userId = context.User?.Claims?.FirstOrDefault(o => o.Type == "id")?.Value;
            var userUuid = userId != null ? new Guid(userId) : Guid.Empty;

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<PermissionRequired>();

            if (attribute != null)
            {
                var role = await dbContext.User
                    .Where(o => o.Uuid == userUuid)
                    .Include(o => o.Role)
                    .ThenInclude(o => o.Permissions)
                    .Select(o => o.Role)
                    .FirstOrDefaultAsync();

                if (role == null || !role.Permissions.Any(o => o.Name == attribute.Permission.ToString())) 
                {
                    throw new DomainException("Access Denied");
                }
            }
            
            await _next(context); 
        }
    }
}
