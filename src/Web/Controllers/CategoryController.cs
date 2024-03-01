using Core.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Route("category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [Route("{slug}")]
        public async Task<ActionResult> Products([FromRoute] string slug)
        {
            var category = await _categoryService.GetBySlug(slug);
            var products = await _productService.GetAllProductsByCategoryId(category.Id);

            var vm = new CategoryProductsVm
            {
                Category = category,
                Products = products
            };

            return View(vm);
        }
    }
}
