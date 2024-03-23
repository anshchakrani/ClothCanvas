using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ClothCanvas.Entity;

public class Category
{
    public int CategoryId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [MaxLength(300)]
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }

    [ValidateNever]
    public List<Product> Products { get; set; }
}