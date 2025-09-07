using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? Brand,string? Type, string? sort)
    {
        var query =_context.Products.AsQueryable();
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
        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await _context.Products
            .Select(p => p.Brand)
            .Where(b => b != null && b != "")
            .Distinct()
            .OrderBy(b => b)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await _context.Products
            .Select(p => p.Type)
            .Where(t => t != null && t != "")
            .Distinct()
            .OrderBy(t => t)
            .ToListAsync();
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
    }

    public bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
