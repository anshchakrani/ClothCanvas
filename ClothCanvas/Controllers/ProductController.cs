using ClothCanvas.DataAccess;
using ClothCanvas.Entity;
using ClothCanvas.Models;
using ClothCanvas.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Stripe.Climate;
using System.Security.Claims;


namespace ClothCanvas.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ICookieService _cookieService;
        private readonly UserManager<User> _userManager;
        private readonly IOrderService _orderService;
        private readonly IOpenAIService _openAIService;

        private readonly AppDbContext _context;

        public ProductController(
            ILogger<ProductController> logger, 
            AppDbContext context, 
            ICookieService cookieService, 
            UserManager<User> userManager,
            IOrderService orderService,
            IOpenAIService openAIService)
            
        {
            _logger = logger;
            _context = context;
            _cookieService = cookieService;
            _userManager = userManager;
            _orderService = orderService;
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        [Route("Product/{id}")]
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            // check if user is business?

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Register", "Login");
            }


            var productViewModel = new ProductViewModel(product);

            if (user.isBusiness && product.MinimumQuantity > 0)
            {
                productViewModel.Quantity = (int)product.MinimumQuantity;
            }else
            {
                productViewModel.Quantity = 1;
            }

            return View(productViewModel);
        }


        [HttpPost]
        [Route("Product/AddToCart")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return RedirectToAction("Details", new { id = productId });
            }
            
            // Retrieve the current cart from the cookie
            var cart = _cookieService.GetFromCookie<List<CartItem>>("Cart");
            
            // get product
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            if (product.IsCustom)
            {
                return RedirectToAction("Create", "CustomProduct", new { productId = productId , quantity = quantity});
            }


            // Check if the product already exists in the cart
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                // Update the quantity if the item is already in the cart
                cartItem.Quantity += quantity;
            }
            else
            {

                var serId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.FirstOrDefault(u => u.Id == serId);

                if(user.isBusiness && quantity < product.MinimumQuantity)
                {
                    quantity = (int)product.MinimumQuantity;
                }

                // Add the item to the cart if it's not there
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price
                });
            }

            // Save the updated cart back to the cookie
            _cookieService.SetCookie("Cart", cart, 720); // Expires in 12 hours

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Cart")]
        public IActionResult Cart()
        {
            var cartItems = _cookieService.GetFromCookie<List<CartItem>?>("Cart") ?? new List<CartItem>();
            var cartViewModel = new CartViewModel();

            foreach (var item in cartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    var cartItemViewModel = new CartItemViewModel
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        ImageUrl = product.ImageUrl,
                        CustomProductURL = item.customProductURL
                    };
                    cartViewModel.Items.Add(cartItemViewModel);
                }
            }

            cartViewModel.TotalPrice = cartViewModel.Items.Sum(i => i.Total);
            return View(cartViewModel);
        }




        [HttpPost]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cartItems = _cookieService.GetFromCookie<List<CartItem>>("Cart") ?? new List<CartItem>();
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in cartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    var discountedPrice = (float) product.Price;

                    // Check if there's a discount and apply it
                    if (product.Discount > 0)
                    {
                        discountedPrice = (float)product.Price * (1 - product.Discount / 100);
                    }

                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(discountedPrice * 100), // Adjusted for discount
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Name,
                                Images = new List<string> { product.ImageUrl },
                                Description = product.Description
                            },
                        },
                        Quantity = item.Quantity,
                        // Assuming you're applying tax rates as well
                        TaxRates = new List<string> { "txr_1OoNeBHKIrB24w755qaA89qh" }
                    });
                }
            }


            var successUrl = "https://localhost:7115/Success";
            var cancelUrl = "https://localhost:7115/Cancel";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card", },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                TaxIdCollection = new SessionTaxIdCollectionOptions { Enabled = true },
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            // Redirect to the Stripe Checkout page
            return Redirect(session.Url);
        }

        [HttpGet]
        [Route("Success")]
        public async Task<IActionResult> Success()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handle case when user is not found
                return Unauthorized();
            }

            _orderService.PlaceOrder(user.Id);

            // Redirect or return a response as needed
            return RedirectToAction("Orders");
        }

        [HttpGet]
        [Route("Cancel")]
        public IActionResult Cancel()
        {
            return RedirectToAction("Cart");
        }

        [HttpGet]
        [Route("OrderSuccess")]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var orders = _orderService.GetOrders(user.Id);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cart = _cookieService.GetFromCookie<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }
            _cookieService.SetCookie("Cart", cart, 720);
            return RedirectToAction("Cart");
        }
    }
    
    
}