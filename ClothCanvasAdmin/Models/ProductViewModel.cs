namespace ClothCanvasAdmin.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public bool ProductIsCustom { get; set; }

        public string ProductImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string SupplierName { get; set; }

    }

}
