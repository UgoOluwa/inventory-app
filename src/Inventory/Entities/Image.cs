using Inventory.Data;
using MongoDB.Bson;


namespace Inventory.API.Entities
{
    [BsonCollection("Images")]
    public class Image : Document
    {
        public ObjectId DocId { get; set; }
    }
}