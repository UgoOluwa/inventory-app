using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        [BindProperty]
        public ProductViewModel Product { get; set; }

        [BindProperty]
        public UpdateProductDto UpdateProductDto { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }
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
            Image = new FormFile(new MemoryStream(Product.Image), 0,Product.Image.Length, Product.Name, Product.Name);

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProduct()
        {
            if (Image != null)
            {
                await using var memoryStream = new MemoryStream();
                await Image.CopyToAsync(memoryStream);
                if (memoryStream.Length < 2097152)
                {
                    UpdateProductDto.Image = UpdateProductDto.Image != memoryStream.ToArray() ? memoryStream.ToArray() : UpdateProductDto.Image;
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }
            var product = await _inventoryApi.UpdateProduct(UpdateProductDto);

            if (product.IsSuccessful && product.Data != null)
            {
                Product = new ProductViewModel()
                {
                    Description = product.Data.Description,
                    Image = product.Data.Image,
                    Name = product.Data.Name,
                    Price = product.Data.Price,
                    Id = product.Data.Id
                };
                return Page();
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteProduct()
        {
            var response = await _inventoryApi.DeleteProduct(UpdateProductDto.Id);

            if (response.IsSuccessful)
            {
                return RedirectToPage("Product");
            }

            return Page();
        }
    }
}