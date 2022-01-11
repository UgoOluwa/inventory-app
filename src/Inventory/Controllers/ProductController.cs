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


        // [HttpPost]
        // [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        // public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        // {
        //     await _repository.Create(product);
        //
        //     return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        // }
        //
        //
        // [HttpPut]
        // [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        // public async Task<IActionResult> UpdateProduct([FromBody] Product value)
        // {
        //     return Ok(await _repository.Update(value));
        // }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        //
        // [HttpGet("{id:length(24)}", Name = "GetProduct")]
        // [ProducesResponseType((int)HttpStatusCode.NotFound)]
        // [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        // public async Task<ActionResult<Product>> GetProduct(string id)
        // {
        //     var product = await _repository.GetById(id);
        //
        //     if (product == null)
        //     {
        //         _logger.LogError($"Product with id: {id}, hasn't been found in database.");
        //         return NotFound();
        //     }
        //
        //     return Ok(product);
        // }
        //
        // [HttpDelete("{id:length(24)}")]
        // [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        // public async Task<IActionResult> DeleteProductById(string id)
        // {
        //     return Ok(await _repository.Delete(id));
        // }
    }
}
