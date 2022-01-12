using System;
using System.Collections.Generic;
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
        public Image Image { get; set; }
        public ObjectId ImageId { get; set; }
        public decimal Price { get; set; }
    }

    public class MultipleProductViewModel : BaseResponse
    {
        public IEnumerable<ProductViewModel> Data { get; set; }
    }
    
    public class SingleProductViewModel : BaseResponse
    {
        public ProductViewModel Data { get; set; }
    }

    public class BaseResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
