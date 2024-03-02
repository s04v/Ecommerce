using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Dtos
{
    public class ProductAttributeValueDto
    {
        public string Value { get; set; }

        public decimal AdditionalPrice { get; set; }

        public int AttributeId { get; set; }

        public Guid ProductUuid { get; set; }
    }
}
