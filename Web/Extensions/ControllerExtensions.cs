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
    }
}
