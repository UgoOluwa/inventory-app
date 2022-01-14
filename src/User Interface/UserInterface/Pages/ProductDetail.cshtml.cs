using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserInterface.ApiCollection.Interfaces;
using UserInterface.Models;

namespace UserInterface.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly IInventoryApi _inventoryApi;

        public ProductDetailModel(IInventoryApi inventoryApi)
        {
            _inventoryApi = inventoryApi ?? throw new ArgumentNullException(nameof(inventoryApi));
        }

        public ProductViewModel Product { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            var product = await _inventoryApi.GetProduct(productId);
            if (product == null || !product.IsSuccessful)
            {
                return NotFound();
            }

            Product = product.Data;

            return Page();
        }
    }
}