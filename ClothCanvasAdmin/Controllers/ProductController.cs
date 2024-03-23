using ClothCanvas.DataAccess;
using ClothCanvas.Entity;
using ClothCanvasAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothCanvasAdmin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private AppDbContext _dbContext;

        public ProductController(ILogger<ProductController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(int? selectedSupplierId)
        {
            AllProductsViewModel viewModel = new AllProductsViewModel
            {
                Suppliers = _dbContext.Suppliers.ToList()
            };

            if (selectedSupplierId.HasValue)
            {
                viewModel.Products = _dbContext.Products
                                                .Where(p => p.SupplierId == selectedSupplierId.Value)
                                                .ToList();
            }
            else
            {
                viewModel.Products = _dbContext.Products.ToList();
            }

            viewModel.SelectedSupplierId = selectedSupplierId; // Ensure this property exists in your ViewModel

            return View(viewModel);
        }


        public IActionResult Create()
        {
            ProductCreateViewModel viewModel = new ProductCreateViewModel();
            viewModel.Suppliers = _dbContext.Suppliers.ToList();
            viewModel.Categories = _dbContext.Categories.ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    CategoryId = model.CategoryId,
                    SupplierId = model.SupplierId,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    IsCustom = model.IsCustom,
                    ImageUrl = model.ImageUrl,
                    MinimumQuantity = model.MinimumOrderQuantity,
                    Discount = model.Discount
                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            model.Suppliers = _dbContext.Suppliers.ToList();
            model.Categories = _dbContext.Categories.ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Product product = _dbContext.Products.Find(id);

            List<Supplier> suppliers = _dbContext.Suppliers.ToList();

            ProductUpdateViewModel viewModel = new ProductUpdateViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPrice = product.Price,
                ProductIsCustom = product.IsCustom,
                ProductImageUrl = product.ImageUrl,
                MinimumOrderQuantity = (int)product.MinimumQuantity,
                Suppliers = suppliers,
                Categories = _dbContext.Categories.ToList(),
                Discount = product.Discount
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(ProductUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = _dbContext.Products.Find(model.ProductId);

                product.Name = model.ProductName;
                product.Description = model.ProductDescription;
                product.Price = model.ProductPrice;
                product.IsCustom = model.ProductIsCustom;
                product.ImageUrl = model.ProductImageUrl;
                product.MinimumQuantity = model.MinimumOrderQuantity;
                product.Discount = model.Discount;

                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            model.Suppliers = _dbContext.Suppliers.ToList();
            model.Categories = _dbContext.Categories.ToList();

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            Product product = _dbContext.Products.Find(id);

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
