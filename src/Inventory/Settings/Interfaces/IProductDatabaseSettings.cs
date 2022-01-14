namespace Inventory.API.Settings.Interfaces
{
    public interface IProductDatabaseSettings
    {
        string ProductCollectionName { get; set; }
        string ImageCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
