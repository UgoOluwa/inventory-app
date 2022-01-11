using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory.API.Models;

namespace Inventory.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetProducts();
    }
}