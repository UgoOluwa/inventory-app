using Inventory.API.Entities;
using MongoDB.Driver;

namespace Inventory.API.Data.Interfaces
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
