using Core.Activities;
using Core.Users.Domain;
using Microsoft.AspNetCore.Mvc;
using Web.Middleware;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/activity")]
    public class AdminActivityController : Controller
    {
        private readonly IAdminActivityService _adminActivityService;

        public AdminActivityController(IAdminActivityService adminActivityService)
        {
            _adminActivityService = adminActivityService;
        }

        [PermissionRequired(PermissionEnum.Log)]
        public async Task<IActionResult> Index()
        {
            var activities = await _adminActivityService.GetAllActivities();

            return View(activities);
        }
    }
}
