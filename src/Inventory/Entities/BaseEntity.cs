using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.API.Entities
{
    public class BaseEntity
    {
        
    }

    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }

    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
        
        [BsonElement("Name")]
        public string Name { get; set; }
        DateTime CreatedAt { get; }
    }
}