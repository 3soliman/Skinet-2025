using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Specification;

namespace API.Controllers;

[Route("api/[controller]")]
public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<Product> _genericRepository;

    public ProductsController(IGenericRepository<Product> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<object>> GetProducts([FromQuery] ProductSpecParams specParams, [FromQuery(Name = "brands")] string? brandsCsv, [FromQuery(Name = "types")] string? typesCsv)
    {
        if (!string.IsNullOrWhiteSpace(brandsCsv))
        {
            specParams.Brands = brandsCsv
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
        if (!string.IsNullOrWhiteSpace(typesCsv))
        {
            specParams.Types = typesCsv
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
        var spec = new ProductSpecification(specParams);
        var totalSpec = new ProductSpecification(new ProductSpecParams
        {
            Brands = specParams.Brands,
            Types = specParams.Types,
            Search = specParams.Search
        });
        var totalItems = await _genericRepository.CountAsync(totalSpec);
        var products = await _genericRepository.ListAsync(spec);
        return CreatePagedResponse(products, specParams.PageIndex, specParams.PageSize, totalItems);
    }

    // GET: api/products/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var brands = await _genericRepository.ListAsync<string>(new BrandListSpecification());
        var result = brands.Distinct().OrderBy(b => b).ToList();
        return Ok(result);
    }

    // GET: api/products/types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var types = await _genericRepository.ListAsync<string>(new TypeListSpecification());
        var result = types.Distinct().OrderBy(t => t).ToList();
        return Ok(result);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var spec = new ProductSpecification(id);
        var product = await _genericRepository.GetEntityWithSpecAsync(spec);

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
