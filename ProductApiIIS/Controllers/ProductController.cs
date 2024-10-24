using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApiIIS.Data;
using ProductApiIIS.DTO;

namespace ProductApiIIS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly TestDbContext _context;

        public ProductsController(TestDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? name)
        {
            if (string.IsNullOrEmpty(name)) return Ok(await _context.Products.ToListAsync());

            var products = await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductCreateDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("Invalid product");
            }
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
