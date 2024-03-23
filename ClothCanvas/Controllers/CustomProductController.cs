using ClothCanvas.DataAccess;
using ClothCanvas.Entity;
using ClothCanvas.Models;
using ClothCanvas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClothCanvas.Controllers;

public class CustomProductController : Controller
{
    private readonly IOpenAIService _openAiService;
    private readonly AppDbContext _context;
    private readonly ICookieService _cookieService;

    public CustomProductController(IOpenAIService openAiService, AppDbContext context, ICookieService cookieService)
    {
        _openAiService = openAiService;
        _context = context;
        _cookieService = cookieService;
    }

    [HttpGet]
    [Route("CustomProduct/{productId}/{quantity}")]
    public IActionResult Create(int productId, int quantity)
    {
        var product = _context.Products.Find(productId);
        if (product == null) throw new Exception("Product not found");

        var model = new CustomProductModel()
        {
            productId = productId,
            product = product
        };

        return View(model);
    }

    [HttpPost]
    [Route("CustomProduct/{productId}/{quantity}")]
    public async Task<IActionResult> Create(int productId, int quantity, CustomProductModel model)
    {
        model.product = await _context.Products.FindAsync(productId) ?? throw new Exception("Product not found");

        if (!ModelState.IsValid) return View(model);


        var request = new OpenAIRequest()
        {
            prompt = model.prompt + ". Create this image on " + model.product.Name
        };

        try
        {
            var response = await _openAiService.GenerateImage(request);

            if (response.data[0]?.url != null)
            {
                model.ImageUrl = response.data[0].url;
                return View(model);
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError("prompt", e.Message);
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    [Route("AddToCart")]
    public IActionResult AddToCart(CustomProductModel model)
    {
        if (model.quantity <= 0)
        {
            return RedirectToAction("Details", "Product", new { id = model.productId });
        }
        
        var cart = _cookieService.GetFromCookie<List<CartItemViewModel>?>("Cart") ?? new List<CartItemViewModel>();

        var product = _context.Products.Find(model.productId);
        if (product == null)
        {
            return RedirectToAction("Create", "CustomProduct", new { productId = model.productId, quantity = model.quantity });
        }

        if (product.IsCustom)
        {
            var cartItem = new CartItemViewModel()
            {
                ProductId = model.productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = model.quantity,
                ImageUrl = model.ImageUrl,
                CustomProductURL = model.ImageUrl
            };
                cart.Add(cartItem);
        }
        else
        {
            var cartItem = new CartItemViewModel()
            {
                ProductId = model.productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = model.quantity,
                ImageUrl = product.ImageUrl
            };
            cart.Add(cartItem);
        }

        _cookieService.SetCookie("Cart", cart, 2700);

        return RedirectToAction("Index", "Home");
    }
}