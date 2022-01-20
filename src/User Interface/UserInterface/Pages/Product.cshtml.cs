using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserInterface.ApiCollection.Interfaces;
using UserInterface.Models;

namespace UserInterface.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IInventoryApi _inventoryApi;
        
        public ProductModel(IInventoryApi inventoryApi)
        {
            _inventoryApi = inventoryApi ?? throw new ArgumentNullException(nameof(inventoryApi));
        }

        public IEnumerable<ProductViewModel> ProductList { get; set; } = new List<ProductViewModel>();

        [BindProperty] 
        public CreateProductDto CreateProductDto { get; set; }
        
        [BindProperty]
        public IFormFile Image { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        public decimal TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            CurrentPage = PageNumber;

            var productList = await _inventoryApi.GetProducts(new GetProductsPaginatedDto()
            {
                Page = PageNumber,
                PageSize = PageSize
            });

            if (productList != null && productList.IsSuccessful)
            {
                ProductList = productList.Data;
                TotalRecords = productList.TotalPages * PageSize;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreate()
        {
            await using (var memoryStream = new MemoryStream())
            {
                await Image.CopyToAsync(memoryStream);
                if (memoryStream.Length < 2097152)
                {
                    CreateProductDto.Image = memoryStream.ToArray();
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            var product = await _inventoryApi.CreateProduct(CreateProductDto);

            if (product.IsSuccessful && product.Data != null)
            {
                return RedirectToAction("OnGetAsync");
            }
            
            ModelState.AddModelError("File", $"{product.Message}");

            return Page();
        }
    }
}