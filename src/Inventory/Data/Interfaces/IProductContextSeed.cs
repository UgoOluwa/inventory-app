using System.Threading.Tasks;
using Inventory.API.Entities;
using MongoDB.Driver;

namespace Inventory.API.Data.Interfaces
{
    public interface IProductContextSeed
    {
        Task SeedData(IMongoCollection<Product> productCollection, IMongoCollection<Image> imageCollection,
            IMongoDatabase database);
    }
}