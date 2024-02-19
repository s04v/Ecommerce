using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Dtos
{
    public class ProductDto
    {
        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public int ManufacturerId { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Stream Thumbnail { get; set; }

        public string ThumbnailPath { get; set; }

        public string ThumbnailMimeType { get; set; }

        public bool IsPublished { get; set; }
    }
}
