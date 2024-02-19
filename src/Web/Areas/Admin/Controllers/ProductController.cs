using Core.Catalog.Domain;
using Core.Catalog.Dtos;
using Core.Catalog.Interfaces;
using Core.Users.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using Web.Middleware;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;

        public ProductController(IProductService productService, ICategoryService categoryService, IManufacturerService manufacturerService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
        }

        [PermissionRequired(PermissionEnum.Product)]
        public async Task<ActionResult> Index()
        {
            var products = await _productService.GetAllProducts();
            
            return View(products);
        }

        [PermissionRequired(PermissionEnum.Product)]
        [HttpGet]
        [Route("new")]
        public async Task<ActionResult> Create()
        {
                var categories = await _categoryService.GetAllCategories();
                var manufacturers = await _manufacturerService.GetAll();

                ViewData["Categories"] = categories;
                ViewData["Manufacturers"] = manufacturers;

                return View(new ProductDto());
        }

        [PermissionRequired(PermissionEnum.Product)]
        [HttpPost]
        [Route("new")]
        public async Task<ActionResult> Create([FromForm]ProductDto productDto, [FromForm] IFormFile thumbnail)
        {

            if (thumbnail != null)
            {
                var ms = new MemoryStream();

                await thumbnail.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                productDto.Thumbnail = ms;
                productDto.ThumbnailMimeType = thumbnail.ContentType;

            }

            await _productService.CreateProduct(productDto);

            return RedirectToAction(nameof(Index));
        }

        [PermissionRequired(PermissionEnum.Product)]
        [HttpGet]
        [Route("{id}/edit")]
        public async Task<ActionResult> Edit([FromRoute] Guid id)
        {
            var product = await _productService.GetProduct(id);

            var vm = new ProductDto
            {
                Name = product.Name,
                CategoryId = product.CategoryId,
                ManufacturerId = product.ManufacturerId,
                Description = product.Description,
                Price = product.Price,
                IsPublished = product.IsPublished,
                ThumbnailPath = product.Thumbnail
            };

            var categories = await _categoryService.GetAllCategories();
            var manufacturers = await _manufacturerService.GetAll();

            ViewData["Categories"] = categories;
            ViewData["Manufacturers"] = manufacturers;

            return View(vm);
        }

        [PermissionRequired(PermissionEnum.Product)]
        [HttpPost]
        [Route("{id}/edit")]
        public async Task<ActionResult> Edit(
            [FromRoute] Guid id,
            [FromForm] ProductDto productDto, 
            [FromForm] IFormFile thumbnail)
        {
            productDto.Uuid = id;

            if (thumbnail != null)
            {
                var ms = new MemoryStream();

                await thumbnail.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                productDto.Thumbnail = ms;
                productDto.ThumbnailMimeType = thumbnail.ContentType;
            }

            await _productService.UpdateProduct(productDto);

            return RedirectToAction(nameof(Index));
        }

        [PermissionRequired(PermissionEnum.Product)]
        [HttpGet]
        [Route("{id}/edit/attributes")]
        public async Task<ActionResult> EditAttributes([FromRoute] Guid id)
        {
            var product = await _productService.GetProduct(id);

            var vm = new ProductDto
            {
                Name = product.Name,
                CategoryId = product.CategoryId,
                ManufacturerId = product.ManufacturerId,
                Description = product.Description,
                Price = product.Price,
                IsPublished = product.IsPublished,
                ThumbnailPath = product.Thumbnail
            };

            var categories = await _categoryService.GetAllCategories();
            var manufacturers = await _manufacturerService.GetAll();

            ViewData["Categories"] = categories;
            ViewData["Manufacturers"] = manufacturers;

            return View(vm);
        }
    }
}
