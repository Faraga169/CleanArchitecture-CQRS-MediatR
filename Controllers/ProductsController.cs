using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Application.DTOS;
using Products.Application.Products.Command;
using Products.Application.Products.Query;

namespace Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ISender _sender) : ControllerBase
    {
        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var products = await _sender.Send( new GetAllProductsQuery(),cancellationToken);

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetById( int id,CancellationToken cancellationToken)
        {
            var product = await _sender.Send(new GetProductByIdQuery { Id = id },cancellationToken);


            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command,CancellationToken cancellationToken)
        {
            var product = await _sender.Send(command, cancellationToken);

            return Ok(product);
        }

        // PUT: api/products/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,UpdateProductCommand command,CancellationToken cancellationToken)
        {
            command.Id = id;
            var updated = await _sender.Send(command, cancellationToken);


            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var deleted = await _sender.Send(new DeleteProductCommand { Id = id }, cancellationToken);


            return NoContent();
        }
    }
}
