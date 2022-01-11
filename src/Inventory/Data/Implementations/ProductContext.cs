using Inventory.API.Data.Interfaces;
using Inventory.API.Entities;
using Inventory.API.Settings;
using MongoDB.Driver;

namespace Inventory.API.Data.Implementations
{
    public class ProductContext : IProductContext
    {
        public ProductContext(IProductDatabaseSettings settings, IProductContextSeed productContextSeed)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("InventoryDb");

            Products = database.GetCollection<Product>("Products");
            Images = database.GetCollection<Image>("Images");
            productContextSeed.SeedData(Products, Images, database).Wait();
        }

        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Image> Images { get; }
    }
}
