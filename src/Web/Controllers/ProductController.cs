using Core.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("{slug}")]
        public async Task<ActionResult> ProductPreview([FromRoute] string slug)
        {
            var product = await _productService.GetProductBySlug(slug);

            return View(product);
        }
    }
}
