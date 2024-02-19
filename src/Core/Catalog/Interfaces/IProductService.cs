﻿using Core.Catalog.Domain;
using Core.Catalog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Interfaces
{
    public interface IProductService
    {
        Task CreateProduct(ProductDto productDto);

        Task<Product> GetProduct(Guid uuid);

        Task<IEnumerable<Product>> GetAllProducts();

        Task UpdateProduct(ProductDto productDto);

        /*Task RemoveProduct(int productId);*/
    }
}
