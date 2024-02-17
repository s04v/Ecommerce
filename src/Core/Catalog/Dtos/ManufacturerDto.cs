using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Dtos
{
    public class ManufacturerDto
    {
        public int Id { get; set; }

        public string Name { get; set; } 
        
        public Stream Picture { get; set; }
        
        public string MimeType { get; set; }
    }
}
