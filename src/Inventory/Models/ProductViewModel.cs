using System;
using System.Drawing;
using MongoDB.Bson;

namespace Inventory.API.Models
{
    public class ProductViewModel
    {
        public ObjectId Id { get; set; }
        DateTime CreatedAt { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public ObjectId ImageId { get; set; }
        public decimal Price { get; set; }
    }
}
