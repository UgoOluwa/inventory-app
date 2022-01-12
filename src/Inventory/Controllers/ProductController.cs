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
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpPost]
        [ProducesResponseType(typeof(SingleProductViewModel), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductViewModel product)
        {
            await _productService.CreateProduct(product);
        
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }
        
        
        [HttpPut]
        [ProducesResponseType(typeof(SingleProductViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductViewModel value)
        {
            await _productService.UpdateProduct(value);
            return Ok();
        }


        [HttpGet]
        [ProducesResponseType(typeof(MultipleProductViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
        {
            var products = await _productService.GetProducts();
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
        
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            await _productService.DeleteProduct(id);
            return Ok();
        }
        
        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProducts()
        {
            await _productService.DeleteProducts();
            return Ok();
        }
    }
}
