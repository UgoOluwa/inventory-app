using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserInterface.ApiCollection.Interfaces;
using UserInterface.Models;

namespace UserInterface.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryApi _inventoryApi;

        public IndexModel(IInventoryApi inventoryApi)
        {
            _inventoryApi = inventoryApi ?? throw new ArgumentNullException(nameof(inventoryApi));
        }

        public IEnumerable<ProductViewModel> ProductList { get; set; } = new List<ProductViewModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _inventoryApi.GetProducts();
            if (response != null && response.IsSuccessful)
            {
                ProductList = response.Data;
            }

            return Page();
        }
    }
}
