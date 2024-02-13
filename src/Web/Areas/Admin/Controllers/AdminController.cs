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
            var users = await _userService.GetAllUsers();

            return View(new AdminListVm { Users = users });
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("new")]
        public async Task<ActionResult> AddNewAdmin()
        {
            var users = await _userService.GetAllUsers();
            
            ViewData["AllRoles"] = await _roleService.GetAllRoles();

            return View(users);
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("{userUuid}/set-role")]
        public async Task<ActionResult> SetUserRole([FromRoute] Guid userUuid)
        {
            var user = await _userService.GetUserById(userUuid);

            ViewData["AllRoles"] = await _roleService.GetAllRoles();

            return View(user);
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("{userUuid}/set-role")]
        [HttpPost]
        public async Task<ActionResult> SetUserRole([FromRoute] Guid userUuid, [FromForm] int roleId)
        {
            if (roleId < 1)
            {
                return RedirectToAction(nameof(SetUserRole), new { userUuid = userUuid });
            }

            await _userService.SetRole(userUuid, roleId);

            return RedirectToAction(nameof(AddNewAdmin));
        }
    }
}
