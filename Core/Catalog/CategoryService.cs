using Common.Data;
using Common.Exceptions;
using Core.Catalog.Domain;
using Core.Catalog.Interfaces;
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

        public CategoryService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

            if (category == null)
            {
                throw new DomainException("Category not found");
            }

            category.Name = name;

            await _dbContext.SaveChangesAsync(); 
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _dbContext.Category
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
            
            return category;
        }
    }
}
