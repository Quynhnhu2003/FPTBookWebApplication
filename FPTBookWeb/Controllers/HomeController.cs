using FPTBookWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPTBookWeb.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly DbFptbookContext _context;

		public HomeController(ILogger<HomeController> logger, DbFptbookContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
        {
			var bookquantity = _context.Books
	        .OrderByDescending(b => b.Quantity)
	        .Take(3)
	        .ToList();
			return View(bookquantity);
		}

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}