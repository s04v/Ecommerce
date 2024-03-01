using Core.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategory(string name);

        Task<Category> GetCategory(int id);

        Task<Category> GetBySlug(string slug);

        Task<IEnumerable<Category>> GetAllCategories();

        Task UpdateCategory(int id, string name);

        Task<ProductAttribute> AddAttribute(int categoryId, string attributeName);

        Task RemoveAttribute(int categoryId, int attributeId);

    }
}
