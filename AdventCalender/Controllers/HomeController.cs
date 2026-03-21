using System.Diagnostics;
using AdventCalender.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            // Берем всех пользователей, у которых есть StoreName (продавцы)
            var sellers = await _context.Users
                .Where(u => !string.IsNullOrEmpty(u.StoreName))
                .ToListAsync();
            return View(sellers);
        }
    }
}
