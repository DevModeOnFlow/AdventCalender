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
            int pageSize = 8; 

            var sellersQuery = _context.Users.Where(u => !string.IsNullOrEmpty(u.StoreName));

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                sellersQuery = sellersQuery.Where(s =>
                    s.StoreName.ToLower().Contains(searchString) ||
                    (s.Description != null && s.Description.ToLower().Contains(searchString)));
            }

            ViewData["CurrentFilter"] = searchString;

            int totalItems = await sellersQuery.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var sellers = await sellersQuery.Take(pageSize).ToListAsync();

            return View(sellers);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMore(string searchString, int page)
        {
            int pageSize = 8;

            var sellersQuery = _context.Users.Where(u => !string.IsNullOrEmpty(u.StoreName));

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                sellersQuery = sellersQuery.Where(s =>
                    s.StoreName.ToLower().Contains(searchString) ||
                    (s.Description != null && s.Description.ToLower().Contains(searchString)));
            }

            var sellers = await sellersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return PartialView("SellerCardList", sellers);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}