using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Domain
{
    public class Product
    {
        public Guid Uuid { get; set; }
        
        public string Name { get; set; }

        public string Slug { get; set; }

        public int ManufacturerId { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Thumbnail { get; set; }

        public bool IsPublished { get; set; }

        public DateTime CreatedDate { get; set; }

        public Manufacturer Manufacturer { get; set; }
        public Category Category { get; set; }
        public IEnumerable<ProductImage> Images { get; set; }
    }
}
