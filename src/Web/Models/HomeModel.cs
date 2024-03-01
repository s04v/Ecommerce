using Core.Catalog.Domain;

namespace Web.Models
{
    public class HomeModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
