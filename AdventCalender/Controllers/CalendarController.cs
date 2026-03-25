using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventCalender.Data;

namespace AdventCalender.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId)) return RedirectToAction("Index", "Home");

            var seller = await _context.Users
                .Include(u => u.AdventDays)
                .FirstOrDefaultAsync(u => u.Id == sellerId);

            if (seller == null) return NotFound();

            return View(seller);
        }

        public async Task<IActionResult> DayDetails(int id)
        {
            var day = await _context.AdventDays.Include(d => d.Seller).FirstOrDefaultAsync(d => d.Id == id);
            if (day == null) return NotFound();

            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;

            bool isTooEarly = currentTime < day.StartTime;
            bool isTooLate = currentTime > day.EndTime;


            if (isTooEarly || isTooLate)
            {
                TempData["Message"] = "Извините, это предложение сейчас недоступно!";
                return RedirectToAction("Index", new { sellerId = day.SellerId });
            }

            return View(day);
        }

    }
}