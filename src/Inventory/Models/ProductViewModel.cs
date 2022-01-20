using System;
using System.Drawing;
using MongoDB.Bson;

namespace Inventory.API.Models
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        DateTime CreatedAt { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string ImageId { get; set; }
        public decimal Price { get; set; }
    }
}
