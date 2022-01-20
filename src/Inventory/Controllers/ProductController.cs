using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Inventory.API.Entities;
using Inventory.API.Models;
using Inventory.API.Repositories.Interfaces;
using Inventory.Services.Interfaces;

namespace Inventory.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }


        [HttpPost]
        [ProducesResponseType(typeof(SingleProductViewModel), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto product)
        {
            var response = await _productService.CreateProduct(product);
            return Ok(response);
        }
        
        
        [HttpPut]
        [ProducesResponseType(typeof(SingleProductViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto value)
        {
            var response = await _productService.UpdateProduct(value);
            return Ok(response);
        }


        [HttpPost("GetAll")]
        [ProducesResponseType(typeof(MultipleProductViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts(GetProductsPaginatedDto request)
        {
            var products = await _productService.GetProducts(request);
            return Ok(products);
        }

        
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SingleProductViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _productService.GetProduct(id);
            return Ok(product);
        }
        
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            var response = await _productService.DeleteProduct(id);
            return Ok(response);
        }
        
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProducts()
        {
            var response = await _productService.DeleteProducts();
            return Ok(response);
        }
    }
}
