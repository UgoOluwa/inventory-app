﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory.API.Models;

namespace Inventory.Services.Interfaces
{
    public interface IProductService
    {
        Task<SingleProductViewModel> CreateProduct(CreateProductDto product);
        Task<SingleProductViewModel> UpdateProduct(UpdateProductDto product);
        Task<MultipleProductViewModel> GetProducts();
        Task<SingleProductViewModel> GetProduct(string id);
        Task DeleteProduct(string id);
        Task DeleteProducts();
    }
}