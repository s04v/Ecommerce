using Common.Data;
using Common.Exceptions;
using Core.Catalog.Domain;
using Core.Catalog.Interfaces;
using Core.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core
{
    public class CategoryTests
    {
        private readonly IServiceProvider _provider;
        private readonly IApplicationDbContext _dbContext;
        private readonly ICategoryService _categoryService;

        public CategoryTests()
        {
            _provider = IoC.GetServiceProvider();
            _dbContext = _provider.GetService<IApplicationDbContext>();
            _categoryService = _provider.GetService<ICategoryService>();
        }

        [Fact]
        public async Task CreateCategory()
        {
            var categoryName = "PC";

            await _categoryService.CreateCategory(categoryName);

            var category = await _dbContext.Category
                .Where(o => o.Name == categoryName)
                .FirstOrDefaultAsync();

            Assert.NotNull(category);

            await Assert.ThrowsAsync<DomainException>(async () => await _categoryService.CreateCategory(categoryName));
        }

        [Fact]
        public async Task UpdateCategory()
        {
            var categoryName = "PC";

            await _categoryService.CreateCategory(categoryName);

            var category = await _dbContext.Category
                .Where(o => o.Name == categoryName)
                .FirstOrDefaultAsync();

            categoryName = "Laptop";

            await _categoryService.UpdateCategory(category.Id, categoryName);

            category = await _dbContext.Category
                .Where(o => o.Name == categoryName)
                .FirstOrDefaultAsync();

            Assert.Equal(categoryName, category.Name);
        }

        [Fact]
        public async Task AddAttribute()
        {
            var category = new Category
            {
                Name = "Phone",
            };

            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var attributeName = "Memory";

            await _categoryService.AddAttribute(category.Id, attributeName);

            Assert.NotNull(
                await _dbContext.ProductAttribute
                .Where(o => o.Name == attributeName)
                .FirstOrDefaultAsync()
            );

            await Assert.ThrowsAsync<DomainException>(async () => await _categoryService.AddAttribute(category.Id, attributeName));
        }

        [Fact]
        public async Task RemoveAttribute()
        {
            var category = new Category
            {
                Name = "Phone",
            };

            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var attributeName = "Memory";

            var attribute = await _categoryService.AddAttribute(category.Id, attributeName);
            
            await _categoryService.RemoveAttribute(category.Id, attribute.Id);

            Assert.Null(
                await _dbContext.ProductAttribute
                .Where(o => o.Name == attributeName)
                .FirstOrDefaultAsync()
            );
        }
    }
}
