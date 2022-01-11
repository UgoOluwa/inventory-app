using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.API.Entities
{
    public class Image
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public ObjectId DocId { get; set; }
    }
}