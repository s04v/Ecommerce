using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;

namespace Web.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetUserAgent(this Controller controller)
        {
            return controller.Request.Headers["User-Agent"].ToString() ?? "Unknown Device";
        }

        public static IPAddress? GetClientIpAddress(this Controller controller)
        {
            return controller.HttpContext.Connection.RemoteIpAddress;
        }

        public static Guid GetUserId(this Controller controller)
        {
            var id = controller.HttpContext.User?.Claims?.FirstOrDefault(o => o.Type == "id")?.Value;

            if (id != null)
            {
                return new Guid(id);
            }

            return Guid.Empty;
        }
    }
}
