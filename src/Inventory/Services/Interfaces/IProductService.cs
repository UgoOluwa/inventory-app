using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory.API.Models;

namespace Inventory.Services.Interfaces
{
    public interface IProductService
    {
        Task<SingleProductViewModel> CreateProduct(CreateProductDto product);
        Task<SingleProductViewModel> UpdateProduct(UpdateProductDto product);
        Task<MultipleProductViewModel> GetProducts(GetProductsPaginatedDto request);
        Task<SingleProductViewModel> GetProduct(string id);
        Task<BaseResponse> DeleteProduct(string id);
        Task<BaseResponse> DeleteProducts();
    }
}