using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Settings
{
    public interface IProductDatabaseSettings
    {
        string ProductCollectionName { get; set; }
        string ImageCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
