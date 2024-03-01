
using Core.Catalog.Domain;

namespace Web.Models
{
    public class CategoryProductsVm
    {
        public IEnumerable<Product> Products { get; set; }
        public Category Category { get; set; }
    }
}
