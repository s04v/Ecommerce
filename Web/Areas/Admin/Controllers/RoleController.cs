using Core.Users.Domain;
using Core.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Web.Areas.Admin.Models;
using Web.Middleware;
using Web.Areas.Admin.Models;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/role")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("role-list")]
        public async Task<ActionResult> List()
        {
            var roles = await _roleService.GetAllRoles();
            var permissions = await _roleService.GetAllPermissions();

            return View(new RoleListVm { Roles = roles, AllPermissions = permissions });
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("{id}/edit")]
        public async Task<ActionResult> Edit(int id)
        {
            var role = await _roleService.GetRole(id);
            var permissions = await _roleService.GetAllPermissions();

            ViewData["Role"] = role;
            ViewData["AllPermissions"] = permissions;

            var vm = new RoleCreateOrEditVm();
            vm.RoleId = role.Id;
            vm.RoleName = role.Name;

            return View(vm);
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("{id}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(int id, RoleCreateOrEditVm form)
        {
            var role = await _roleService.GetRole(id);

            var permissionsToRemove = new List<int>();

            foreach (var permission in role.Permissions)
            {
                if (!form.SelectedPermissions.Contains(permission.Id))
                {
                    await _roleService.RemovePermit(role.Id, permission.Id);
                }
            }

            var permissionsToAdd = new List<int>();

            foreach (var permissionId in form.SelectedPermissions)
            {
                if (!role.Permissions.Any(o => o.Id == permissionId))
                {
                    await _roleService.AddPermit(role.Id, permissionId);
                }
            }

            return RedirectToAction(nameof(List));
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("new")]
        public async Task<ActionResult> Create()
        {
            var permissions = await _roleService.GetAllPermissions();

            ViewData["AllPermissions"] = permissions;

            return View(new RoleCreateOrEditVm());
        }

        [PermissionRequired(PermissionEnum.Role)]
        [Route("new")]
        [HttpPost]
        public async Task<ActionResult> Create(RoleCreateOrEditVm form)
        {
            if (!ModelState.IsValid)
            {
                var permissions = await _roleService.GetAllPermissions();

                ViewData["AllPermissions"] = permissions;

                return View(new RoleCreateOrEditVm());
            }

            var role = await _roleService.CreateRole(form.RoleName);

            if (form.SelectedPermissions.Count != 0)
            {
                foreach(var permissionId in form.SelectedPermissions)
                {
                    await _roleService.AddPermit(role.Id, permissionId);
                }
            }

            return RedirectToAction(nameof(List));
        }
    }
}
