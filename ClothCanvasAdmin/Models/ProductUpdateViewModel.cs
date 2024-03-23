using System.ComponentModel.DataAnnotations;
using ClothCanvas.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClothCanvasAdmin.Models
{
    public class ProductUpdateViewModel
    {
        public int ProductId { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        public int MinimumOrderQuantity { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public bool ProductIsCustom { get; set; }

        public string ProductImageUrl { get; set; }

        [ValidateNever]
        public List<Supplier> Suppliers { get; set; }

        [ValidateNever]
        public List<Category> Categories { get; set; }
        
        [Display(Name = "Discount (%)")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        public float Discount { get; set; }
        
    }

}
