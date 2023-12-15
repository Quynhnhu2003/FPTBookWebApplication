using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace FPTBookWeb.Controllers
{
    public class BooksController : Controller
    {
        private readonly DbFptbookContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BooksController(DbFptbookContext context, UserManager<User> userManager,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var dbFptbookContext = _context.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Include(b => b.User).Where(s => s.UserId == userId);
            return View(await dbFptbookContext.ToListAsync());
        }

        // GET: Books/Details/5
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

        // GET: Books/Create
        public IActionResult Create()
        {

            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName");
            /*ViewData["UserId"] = user;*/
            return View();
        }




        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookTitle,BookPrice,BookDescription,BookImage1,BookImage2,BookImage3,PublishedYear,Quantity,BookPages,PublisherId,CategoryId,AuthorId")] Book book, ClaimsPrincipal user,IFormFile file)
        {

            /*var id = user.Id;*/
            if (book != null && !BookNameExists(book.BookTitle))
            {
                string uniqueFileName = null;  //to contain the filename
                
                if (file != null )  //handle iformfile
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    uniqueFileName = file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
               
                var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;

                book.UserId = userId;
                /*var test = file.Name;*/
                book.BookImage1 = "images/"+uniqueFileName;
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", book.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", book.PublisherId);
            /*ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", book.UserId);*/
            return View(book);
        }

        // GET: Books/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", book.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", book.PublisherId);
            /*ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", book.UserId);*/
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookTitle,BookPrice,BookDescription,BookImage1,BookImage2,BookImage3,PublishedYear,Quantity,BookPages,PublisherId,CategoryId,AuthorId")] Book book, ClaimsPrincipal user, IFormFile file)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (book != null)
            {
                try
                {
                    string uniqueFileName = null;  //to contain the filename

                    if (file != null)  //handle iformfile
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                        uniqueFileName = file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                    book.BookImage1 = "images/" + uniqueFileName;
                    }
                    var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;

                    book.UserId = userId;
                    _context.Books.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", book.CategoryId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", book.PublisherId);

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'DbFptbookContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
        private bool BookNameExists(string title)
        {
            return (_context.Books?.Any(e => e.BookTitle == title)).GetValueOrDefault();
        }
    }
}
