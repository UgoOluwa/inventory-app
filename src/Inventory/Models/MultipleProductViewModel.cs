using System.Collections.Generic;

namespace Inventory.API.Models
{
    public class MultipleProductViewModel : BaseResponse
    {
        public IEnumerable<ProductViewModel> Data { get; set; }
        public decimal TotalPages { get; set; }
    }
}