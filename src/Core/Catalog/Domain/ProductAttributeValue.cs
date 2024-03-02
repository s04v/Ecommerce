using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Domain
{
    public class ProductAttributeValue
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public decimal AdditionalPrice { get; set; }

        public int AttributeId { get; set; }

        public Guid ProductUuid { get; set; }
    }
}
