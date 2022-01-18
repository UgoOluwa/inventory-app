using System.Collections.Generic;
using System.Threading.Tasks;
using UserInterface.Models;

namespace UserInterface.ApiCollection.Interfaces
{
    public interface IInventoryApi
    {
        Task<MultipleProductViewModel> GetProducts();
        Task<SingleProductViewModel> GetProduct(string id);
        Task<SingleProductViewModel> CreateProduct(CreateProductDto model);
        Task<SingleProductViewModel> UpdateProduct(UpdateProductDto productModel);
        Task<BaseResponse> DeleteProduct(string productId);
    }
}
