using ClothCanvas.Entity;

namespace ClothCanvasAdmin.Models
{
    public class AllProductsViewModel
    {
        public List<Product> Products { get; set; }

        //SelectedSupplierId
        public int? SelectedSupplierId { get; set; }

        public List<Supplier> Suppliers { get; set; }

    }

}
