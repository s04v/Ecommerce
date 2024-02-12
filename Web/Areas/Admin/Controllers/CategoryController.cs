using Core.Catalog.Interfaces;
using Core.Users.Domain;
using Microsoft.AspNetCore.Mvc;
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
            var category = await _categoryService.GetCategory(id);
            
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
    }
}
