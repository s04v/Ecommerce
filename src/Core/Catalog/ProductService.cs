using Common;
using Common.Data;
using Common.Services;
using Core.Catalog.Domain;
using Core.Catalog.Dtos;
using Core.Catalog.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Catalog
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IStorageService _storageService;

        public ProductService(IApplicationDbContext dbContext, IStorageService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task<Product> GetProduct(Guid uuid)
        {
            return await _dbContext.Product
                .Where(o => o.Uuid == uuid)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Product
                .ToListAsync();
        }

        public async Task CreateProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Slug = CreateProductSlug(productDto.Name),
                CategoryId = productDto.CategoryId,
                ManufacturerId = productDto.ManufacturerId,
                Description = productDto.Description,
                Price = productDto.Price,
                IsPublished = productDto.IsPublished,
                CreatedDate = DateTime.Now,
            };

            if (productDto.Thumbnail != null)
            {
                string fileExt = MimeTypeHelper.MimeToExtension(productDto.ThumbnailMimeType);
                string fileName = $@"{Guid.NewGuid()}.{fileExt}";

                await _storageService.SaveFileAsync(productDto.Thumbnail, fileName);
                product.Thumbnail = fileName;

            }

            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(ProductDto productDto)
        {
            var product = await _dbContext.Product
                .Where(o => o.Uuid == productDto.Uuid)
                .FirstOrDefaultAsync();

            product.Name = productDto.Name;
            product.Slug = CreateProductSlug(productDto.Name);
            product.CategoryId = productDto.CategoryId;
            product.ManufacturerId = productDto.ManufacturerId;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.IsPublished = productDto.IsPublished;

            if (productDto.Thumbnail != null)
            {
                string fileExt = MimeTypeHelper.MimeToExtension(productDto.ThumbnailMimeType);
                string fileName = $@"{Guid.NewGuid()}.{fileExt}";

                await _storageService.SaveFileAsync(productDto.Thumbnail, fileName);
                product.Thumbnail = fileName;
            }

            await _dbContext.SaveChangesAsync();
        }


        private string CreateProductSlug(string name)
        {
            string slug = name.ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");       
            slug = Regex.Replace(slug, @"\s+", " ").Trim();  
            slug = Regex.Replace(slug, @"\s", "-");   

            return slug;
        }

    }
}
