using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Dtos
{
    public class ProductDto
    {
        public Guid Uuid { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Manufactuter is required")]
        public int ManufacturerId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        public Stream Thumbnail { get; set; }

        public string ThumbnailPath { get; set; }

        public string ThumbnailMimeType { get; set; }

        public bool IsPublished { get; set; }
    }
}
