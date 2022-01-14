using Inventory.API.Entities;
using MongoDB.Driver;

namespace Inventory.API.Data.Interfaces
{
    public interface IContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Image> Images { get; }
    }
}
