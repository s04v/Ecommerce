using Common.Services;

namespace Web
{
    public class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _contextAccessor;


        public WorkContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetCurrentUserUuid()
        {
            var id = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(o => o.Type == "id")?.Value;

            if (id != null)
            {
                return new Guid(id);
            }

            return Guid.Empty;
        }
    }
}
