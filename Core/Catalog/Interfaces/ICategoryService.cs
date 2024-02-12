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
        Task<IEnumerable<Category>> GetAllCategories();

        Task UpdateCategory(int id, string name);
    }
}
