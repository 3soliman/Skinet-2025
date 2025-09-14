namespace API.DTOs;
using System.ComponentModel.DataAnnotations;

public class CreateProductDto
{
    

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "PictureUrl is required")]
    public string PictureUrl { get; set; } = string.Empty;


    [Required]
    [StringLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Type { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be greater than 0")]
    public int QuantityInStock { get; set; }
}