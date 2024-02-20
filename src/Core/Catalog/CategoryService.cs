using Common.Data;
using Common.Exceptions;
using Common.Services;
using Core.Activities;
using Core.Activities.Domain;
using Core.AdminActivities.Domain;
using Core.Catalog.Domain;
using Core.Catalog.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IAdminActivityService _activityService;

        public CategoryService(IApplicationDbContext dbContext, IAdminActivityService activityService)
        {
            _dbContext = dbContext;
            _activityService = activityService;
        }

        public async Task CreateCategory(string name)
        {
            if (await _dbContext.Category.AnyAsync(o => o.Name == name))
            {
                throw new DomainException("Category already exists");
            }

            var category = new Category
            {
                Name = name
            };

            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Category, 
                $"Created \"{name}\" category");
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await _dbContext.Category
                .ToListAsync();

            return categories;
        }

        public async Task UpdateCategory(int id, string name)
        {
            var category = await _dbContext.Category
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();

            var oldName = category.Name;

            if (category == null)
            {
                throw new DomainException("Category not found");
            }

            category.Name = name;

            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Category,
                $"Category updated from \"{oldName}\" to \"{name}\" ");
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _dbContext.Category
                .Where(o => o.Id == id)
                .Include(o => o.Attributes)
                .FirstOrDefaultAsync();
            
            return category;
        }
        public async Task<ProductAttribute> AddAttribute(int categoryId, string attributeName)
        {
            
            if (await _dbContext.ProductAttribute.AnyAsync(o => o.Name == attributeName))
            {
                throw new DomainException("Attribute already exists");
            }

            var attribute = new ProductAttribute
            {
                CategoryId = categoryId,
                Name = attributeName
            };

            await _dbContext.AddAsync(attribute);
            await _dbContext.SaveChangesAsync();

            var category = await _dbContext.Category
                .Where(o => o.Id == categoryId)
                .FirstOrDefaultAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Category,
                $"Attribute \"{attributeName}\" added to \"{category.Name}\" category ");

            return attribute;
        }

        public async Task RemoveAttribute(int categoryId, int attributeId)
        {
            var attribute = await _dbContext.ProductAttribute
                .Where(o => o.Id == attributeId)
                .FirstOrDefaultAsync();

            if (attribute == null)
            {
                throw new DomainException("Attribute already exists");
            }

            _dbContext.ProductAttribute.Remove(attribute);
            await _dbContext.SaveChangesAsync();

            var category = await _dbContext.Category
               .Where(o => o.Id == categoryId)
               .FirstOrDefaultAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Category,
                $"Attribute \"{attribute.Name}\" removed from \"{category.Name}\" category ");
        }
    }
}
