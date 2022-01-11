namespace Inventory.API.Settings.Implementations
{
    public class ProductDatabaseSettings : IProductDatabaseSettings
    {
        public string ProductCollectionName { get; set; }
        public string ImageCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
