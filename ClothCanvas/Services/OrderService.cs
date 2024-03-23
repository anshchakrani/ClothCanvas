using ClothCanvas.DataAccess;
using ClothCanvas.Entity;
using ClothCanvas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClothCanvas.Services
{
    public interface IOrderService
    {
        public void PlaceOrder(string userId);

        public List<Order> GetOrders(string userId);
    }
    public class OrderService : IOrderService
    {
        public readonly AppDbContext _context;
        public readonly ICookieService _cookieService;

        public OrderService(AppDbContext context, ICookieService cookieService)
        {
            _context = context;
            _cookieService = cookieService;
        }

        List<Order> IOrderService.GetOrders(string userId)
        {
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();
            return orders;
        }

        public void PlaceOrder(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var cartItems = _cookieService.GetFromCookie<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (cartItems == null)
            {
                throw new Exception("Cart not found");
            }

            var order = new Order
            {
                UserId = user.Id,
                Status = "Pending",
                CreatedAt = DateTime.Now.ToUniversalTime(),
                TotalPrice = cartItems.Sum(ci => ci.Price * ci.Quantity)
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    CustomDetailURL = cartItem.customProductURL
                };
                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();
        }

    }
}
