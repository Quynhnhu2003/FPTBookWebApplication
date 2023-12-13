using FPTBookWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FPTWeb.Controllers
{
    public class ShopController : Controller
    {
        private readonly DbFptbookContext _context;

        public ShopController(DbFptbookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            var books = _context.Books.Select(b => b);

            //Filter by Title, Author, Category and Publisher
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.BookTitle.Contains(searchString) ||
                b.Author.AuthorName.Contains(searchString) ||
                b.Category.CategoryName.Contains(searchString) ||
                b.Publisher.PublisherName.Contains(searchString));
			}

			var bookList = await books.ToListAsync();

			if (bookList.Count == 0)
			{
				TempData["NoResultMessage"] = "No Books is here!!!";
			}

			//Sorting by Title and Price
			switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.BookTitle);
                    break;
                case "price":
                    books = books.OrderBy(b => b.BookPrice);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.BookPrice);
                    break;
                default:
                    // Default sorting by title
                    books = books.OrderBy(b => b.BookTitle);
                    break;
            }

            return View(await books.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}
