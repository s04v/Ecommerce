using Core.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Components
{
    public class MenuComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public MenuComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllCategories();

            return View(categories);
        }
    }
}
