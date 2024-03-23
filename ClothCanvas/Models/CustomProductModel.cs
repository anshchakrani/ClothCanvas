using ClothCanvas.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClothCanvas.Models;

public class CustomProductModel
{
    public string prompt { get; set; }
    
    [ValidateNever]
    public Product product { get; set; }
    
    
    [ValidateNever]
    public string userId { get; set; }
    public int productId { get; set; }
    
    public int quantity { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string? CustomProductURL { get; set; }
}