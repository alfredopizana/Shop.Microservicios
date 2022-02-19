using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController:ControllerBase
    {
        private readonly IProductRepository productRepository;

        public CatalogController( IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await productRepository.CreateProduct(product);
            return product;
        }
        [HttpGet]
        public async Task<ActionResult<Product>> GetProducts()
            => Ok(await productRepository.GetProducts());

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var product = await productRepository.GetProducts(id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        } 
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateProduct([FromBody] Product product)
            => Ok(await productRepository.UpdateProduct(product));

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteProduct(string id)
            => Ok(await productRepository.DeleteProduct(id));
    }
}
