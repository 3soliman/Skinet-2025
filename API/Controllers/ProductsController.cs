using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? Brand,string? Type, string? sort)
    {
        var products = await _repository.GetProductsAsync(Brand,Type, sort);
        return Ok(products);
    }

    // GET: api/products/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var brands = await _repository.GetBrandsAsync();
        return Ok(brands);
    }

    // GET: api/products/types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var types = await _repository.GetTypesAsync();
        return Ok(types);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);

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

        _repository.AddProduct(product);
        var saved = await _repository.SaveChangesAsync();
        if (!saved) return StatusCode(500, "Failed to save product");

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _repository.UpdateProduct(product);
        var saved = await _repository.SaveChangesAsync();
        if (!saved) return StatusCode(500, "Failed to save product");

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _repository.DeleteProduct(product);
        var saved = await _repository.SaveChangesAsync();
        if (!saved) return StatusCode(500, "Failed to delete product");

        return NoContent();
    }

    private bool ProductExists(int id) => _repository.ProductExists(id);
}
