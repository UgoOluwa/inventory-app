using Inventory.Data;
using MongoDB.Bson;


namespace Inventory.API.Entities
{
    [BsonCollection("Products")]
    public class Product : Document
    {
        public string Description { get; set; }
        public ObjectId ImageId { get; set; }
        public decimal Price { get; set; }
    }

}
