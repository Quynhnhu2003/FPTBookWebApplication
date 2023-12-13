using FPTBookWeb.Models;

using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly DbFptbookContext _context;
        private readonly Cart _cart;

        public OrderController(DbFptbookContext context, Cart cart)
        {
            _cart = cart;
            _context = context;
        }
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;

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
        public void CreateOrder(Order order)
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
                };

                order.OrderDetails.Add(orderItem);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
}
