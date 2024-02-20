using Core.Activities.Domain;
using Core.Catalog.Domain;
using Core.Catalog.Interfaces;
using Core.Users.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Middleware;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [PermissionRequired(PermissionEnum.Category)]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();
            
            return View(categories);
        }

        [PermissionRequired(PermissionEnum.Category)]
        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View(model: "");
        }


        [PermissionRequired(PermissionEnum.Category)]
        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> Create(string categoryName)
        {
            await _categoryService.CreateCategory(categoryName);

            return RedirectToAction(nameof(Index));
        }

        [PermissionRequired(PermissionEnum.Category)]
        [Route("{id}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            ViewData["IsEdit"] = true;

            var category = await _categoryService.GetCategory(id);
            ViewData["Attributes"] = category.Attributes;

            return View("Edit", category.Name);
        }

        [PermissionRequired(PermissionEnum.Category)]
        [Route("{id}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, string categoryName)
        {
            await _categoryService.UpdateCategory(id, categoryName);

            return RedirectToAction(nameof(Index));
        }

        [PermissionRequired(PermissionEnum.Category)]
        [Route("{id}/attribute")]
        [HttpPost]
        public async Task<JsonResult> AddAttribute([FromRoute] int id, [FromForm]string attributeName)
        {
            var attribute = await _categoryService.AddAttribute(id, attributeName);
            
            return Json(attribute);
        }

        [PermissionRequired(PermissionEnum.Category)]
        [Route("{id}/attribute")]
        [HttpDelete]
        public async Task<IActionResult> RemoveAttribute(
            [FromRoute] int id, 
            [FromForm] int attributeId)
        {
            await _categoryService.RemoveAttribute(id, attributeId);

            return Ok();
        }
    }
}
