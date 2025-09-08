using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _genericRepository;
    private readonly StoreContext _context;

    public ProductsController(IGenericRepository<Product> genericRepository, StoreContext context)
    {
        _genericRepository = genericRepository;
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? Brand,string? Type, string? sort)
    {
        var query = _context.Products.AsQueryable();
        if(!string.IsNullOrEmpty(Brand))
        {
            query = query.Where(p => p.Brand == Brand);
        }
        if(!string.IsNullOrEmpty(Type))
        {
            query = query.Where(p => p.Type == Type);
        }
        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = sort.ToLower() switch
            {
                "priceasc" => query.OrderBy(p => p.Price),
                "pricedesc" => query.OrderByDescending(p => p.Price),
                "name" => query.OrderBy(p => p.Name),
                _ => query
            };
        }
        var products = await query.ToListAsync();
        return Ok(products);
    }

    // GET: api/products/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var brands = await _context.Products
            .Select(p => p.Brand)
            .Where(b => b != null && b != "")
            .Distinct()
            .OrderBy(b => b)
            .ToListAsync();
        return Ok(brands);
    }

    // GET: api/products/types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var types = await _context.Products
            .Select(p => p.Type)
            .Where(t => t != null && t != "")
            .Distinct()
            .OrderBy(t => t)
            .ToListAsync();
        return Ok(types);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _genericRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _genericRepository.AddAsync(product);
        var saved = await _genericRepository.SaveAllAsync();
        if (!saved) return StatusCode(500, "Failed to save product");

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !await _genericRepository.ExistsAsync(id))
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _genericRepository.Update(product);
        var saved = await _genericRepository.SaveAllAsync();
        if (!saved) return StatusCode(500, "Failed to save product");

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _genericRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _genericRepository.Remove(product);
        var saved = await _genericRepository.SaveAllAsync();
        if (!saved) return StatusCode(500, "Failed to delete product");

        return NoContent();
    }
}
