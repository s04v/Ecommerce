using Core.Users.Domain;
using Core.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Middleware;
using Web.Areas.Admin.Models;

namespace Web.Controllers.Admin
{
    [Area("Admin")]
    [Route("admin/admin-list")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AdminController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<ActionResult> List()
        {
            var users = await _userService.GetUsers();

            return View(new AdminListVm { Users = users });
        }
    }
}
