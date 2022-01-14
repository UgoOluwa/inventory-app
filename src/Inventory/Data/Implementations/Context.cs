using Inventory.API.Data.Interfaces;
using Inventory.API.Entities;
using Inventory.API.Settings.Interfaces;
using MongoDB.Driver;

namespace Inventory.API.Data.Implementations
{
    public class Context : IContext
    {
        public Context(IProductDatabaseSettings settings, IContextSeed productContextSeed)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Products = database.GetCollection<Product>(settings.ProductCollectionName);
            Images = database.GetCollection<Image>(settings.ImageCollectionName);
            productContextSeed.SeedData(Products, Images, database).Wait();
        }

        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Image> Images { get; }
    }
}
