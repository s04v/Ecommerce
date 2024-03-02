using Common;
using Common.Data;
using Common.Services;
using Core.Activities;
using Core.AdminActivities.Domain;
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
        private readonly IAdminActivityService _activityService;

        public ProductService(IApplicationDbContext dbContext, IStorageService storageService, IAdminActivityService activityService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
            _activityService = activityService;
        }

        public async Task<Product> GetProduct(Guid uuid)
        {
            return await _dbContext.Product
                .Where(o => o.Uuid == uuid)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductBySlug(string slug)
        {
            return await _dbContext.Product
                .Where(o => o.Slug == slug)
                .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Product
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategoryId(int categoryId)
        {
            return await _dbContext.Product
                .Where(o => o.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductAttribute>> GetProductAttributes(Guid productUuid)
        {
            var productCategoryId = await _dbContext.Product
                .Where(o => o.Uuid == productUuid)
                .Select(o => o.CategoryId)
                .FirstOrDefaultAsync();

            var values = await _dbContext.ProductAttributeValue
                .ToListAsync(); 

            return await _dbContext.ProductAttribute
                .Include(o => o.Values)
                .Where(o => o.CategoryId == productCategoryId)
                .ToListAsync();
        }

        public async Task AddAttributeValue(ProductAttributeValueDto dto)
        {
            var attributeValue = new ProductAttributeValue
            {
                Value = dto.Value,
                AdditionalPrice = dto.AdditionalPrice,
                AttributeId = dto.AttributeId,
                ProductUuid = dto.ProductUuid,
            };

            await _dbContext.AddAsync(attributeValue);
            await _dbContext.SaveChangesAsync();
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

            await _activityService.InsertActivity(AdminActivityAreaEnum.Product,
                $"Created \"{product.Name}\" product.");
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

            await _activityService.InsertActivity(AdminActivityAreaEnum.Product,
                $"Updated \"{product.Name}\" product.");
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
