using FPTBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly DbFptbookContext _context;
        private readonly Cart _cart;
        private readonly UserManager<User> _userManager;

        public OrderController(DbFptbookContext context, Cart cart, UserManager<User> userManager)
        {
            _cart = cart;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            order.CustomerId = userId;
            
            if (_cart.CartItems.Count == 0)
            {
                ModelState.AddModelError("", "Cart is empty, please choose one of all book add to cart!");
            }

            if (ModelState.IsValid)
            {
                CreateOrder(order);
                _cart.ClearCart();
                return View("CheckoutComplete", order);
            }

            return View(order);
        }

        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }
        public  void CreateOrder(Order order)
        {
            order.OrderDate = DateTime.Now;

            var cartItems = _cart.CartItems;

            foreach (var item in cartItems)
            {
                var orderItem = new OrderDetail()
                {
                    Quantity = item.Quantity,
                    BookId = item.Book.BookId,
                    OrderId = order.OrderId,
                    UserId = order.CustomerId
                };
                
                order.OrderDetails.Add(orderItem);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
}
