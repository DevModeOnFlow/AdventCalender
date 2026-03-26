using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventCalender.Data;

namespace AdventCalender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var sellersQuery = _context.Users
                .Where(u => !string.IsNullOrEmpty(u.StoreName));

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                sellersQuery = sellersQuery.Where(s =>
                    s.StoreName.ToLower().Contains(searchString) ||
                    (s.Description != null && s.Description.ToLower().Contains(searchString)));
            }

            ViewData["CurrentFilter"] = searchString;

            var sellers = await sellersQuery.ToListAsync();
            return View(sellers);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}