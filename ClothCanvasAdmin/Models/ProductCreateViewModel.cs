using ClothCanvas.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ClothCanvasAdmin.Models
{
    public class ProductCreateViewModel
    {
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        public bool IsCustom { get; set; }

        public string ImageUrl { get; set; }

        public int MinimumOrderQuantity { get; set; }

        [ValidateNever]
        public List<Supplier> Suppliers { get; set; }

        [ValidateNever]
        public List<Category> Categories { get; set; }
        
        [Display(Name = "Discount (%)")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        public float Discount { get; set; }

    }

}
