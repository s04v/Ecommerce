﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Domain
{
    public class ProductImage
    {
        public int Id { get; set; }
        public Guid ProductUuid { get; set; }
        public string Name { get; set; }
    }
}
