using System.Text.Json;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public static class StoreContextSead
{
    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.Products.Any())
            {
                var jsonPath = ResolveProductsJsonPath();
                if (jsonPath is null)
                {
                    var logger = loggerFactory.CreateLogger("StoreContextSead");
                    logger.LogWarning("products.json not found in expected locations. Skipping seed.");
                    return;
                }

                var json = await File.ReadAllTextAsync(jsonPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var products = JsonSerializer.Deserialize<List<Product>>(json, options) ?? new List<Product>();

                if (products.Count > 0)
                {
                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger("StoreContextSead");
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }

    private static string? ResolveProductsJsonPath()
    {
        var current = Directory.GetCurrentDirectory();
        var solutionRoot = Directory.GetParent(current)?.FullName ?? current;

        string MakePath(params string[] parts) => Path.GetFullPath(Path.Combine(parts));

        var candidates = new[]
        {
            // When current dir is API
            MakePath(current, "..", "Infrastructure", "Data", "seed data", "products.json"),
            MakePath(current, "..", "Infrastructure", "Data", "SeedData", "products.json"),
            // When current dir is solution root
            MakePath(solutionRoot, "Infrastructure", "Data", "seed data", "products.json"),
            MakePath(solutionRoot, "Infrastructure", "Data", "SeedData", "products.json"),
            // Fallbacks
            MakePath(current, "Infrastructure", "Data", "seed data", "products.json"),
            MakePath(AppContext.BaseDirectory, "SeedData", "products.json")
        };

        foreach (var path in candidates)
        {
            if (File.Exists(path)) return path;
        }

        return null;
    }
}

