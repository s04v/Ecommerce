using Core.Catalog.Domain;
using Core.Catalog.Dtos;
using Core.Catalog.Interfaces;
using Core.Users.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Web.Middleware;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/manufacturer")]
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [PermissionRequired(PermissionEnum.Brand)]
        public async Task<ActionResult> Index()
        {
            var manufacturers = await _manufacturerService.GetAll();

            return View(manufacturers);
        }

        [PermissionRequired(PermissionEnum.Brand)]
        [HttpGet]
        [Route("new")]
        public ActionResult Create()
        {
            var manufacturer = new Manufacturer();

            return View(manufacturer);
        }

        [PermissionRequired(PermissionEnum.Brand)]
        [HttpPost]
        [Route("new")]
        public async Task<ActionResult> Create(
            [Required(ErrorMessage = "Manufacturer name is required")]
            [FromForm] 
            string manufacturerName,
            [FromForm(Name = "picture")] IFormFile picture
        )
        {
            if(!ModelState.IsValid)
            {
                return View(new Manufacturer
                { 
                    Name = manufacturerName
                });
            }

            using (var ms = new MemoryStream())
            {
                await picture.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var manufacturerDto = new ManufacturerDto
                {
                    Name = manufacturerName,
                    Picture = ms,
                    MimeType = picture.ContentType
                };

                await _manufacturerService.CreateManufacturer(manufacturerDto);   
            }

            return RedirectToAction(nameof(Index));
        }

        [PermissionRequired(PermissionEnum.Brand)]
        [HttpGet]
        [Route("{id}/edit")]
        public async Task<ActionResult> Edit([FromRoute] int id)
        {
            var manufacturer = await _manufacturerService.Get(id);

            return View(manufacturer);
        }

        [PermissionRequired(PermissionEnum.Brand)]
        [HttpPost]
        [Route("{id}/edit")]
        public async Task<ActionResult> Edit(
            [FromRoute]  int id,
            [FromForm] string manufacturerName,
            [Required(ErrorMessage = "Pitcure is required")]
            [FromForm(Name = "picture")] 
            IFormFile picture
        )
        {
            if (!ModelState.IsValid)
            {
                return View(new Manufacturer
                {
                    Name = manufacturerName
                });
            }

            var manufacturerDto = new ManufacturerDto
            {
                Id = id,
                Name = manufacturerName,
            };

            using (var ms = new MemoryStream())
            {
                if (picture != null)
                {
                    await picture.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    manufacturerDto.Picture = ms;
                    manufacturerDto.MimeType = picture.ContentType;
                }

                await _manufacturerService.UpdateManufacturer(manufacturerDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [PermissionRequired(PermissionEnum.Brand)]
        [HttpGet]
        [Route("{id}/delete")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _manufacturerService.Remove(id);

            return RedirectToAction(nameof(Index));
        } 
    }
}
