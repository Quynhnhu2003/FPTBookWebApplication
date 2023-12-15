using FPTBookWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookWeb.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly Cart _cart;
        private readonly DbFptbookContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(Cart cart, DbFptbookContext context, UserManager<User> userManager)
        {
            _cart = cart;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var items = _cart.GetAllCartItems();
            
            _cart.CartItems = items;

            return View(_cart);
        }

        public IActionResult AddToCart(int id)
        {
            var selectedBook = GetBookById(id);
            /*var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;*/

            if (selectedBook != null)
            {
                _cart.AddToCart(selectedBook, 1);
                
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var selectedBook = GetBookById(id);

            if (selectedBook != null)
            {
                _cart.RemoveFromCart(selectedBook);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ReduceQuantity(int id)
        {
            var selectedBook = GetBookById(id);

            if (selectedBook != null)
            {
                _cart.ReduceQuantity(selectedBook);
            }

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int id)
        {
            var selectedBook = GetBookById(id);

            if (selectedBook != null)
            {
                _cart.IncreaseQuantity(selectedBook);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            _cart.ClearCart();

            return RedirectToAction("Index");
        }

        public Book GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.BookId == id);
        }
    }
}
