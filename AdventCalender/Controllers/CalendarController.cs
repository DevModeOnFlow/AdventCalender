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
            var seller = await _context.Users
                .Include(u => u.AdventDays)
                .FirstOrDefaultAsync(u => u.Id == sellerId);

            if (seller == null) return NotFound();

            return View(seller);
        }

        public async Task<IActionResult> DayDetails(int id)
        {
            var day = await _context.AdventDays
                .Include(d => d.Seller)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (day == null) return NotFound();

            if (IsDayLocked(day.DayNumber))
            {
                TempData["Message"] = $"Этот подарок откроется только {day.DayNumber} декабря!";
                return RedirectToAction("Index", new { sellerId = day.SellerId });
            }

            return View(day);
        }

        private bool IsDayLocked(int dayNumber)
        {
            var now = DateTime.Now;

            var fakeNow = new DateTime(2024, 12, 5);
            return dayNumber > fakeNow.Day;

            /* 
            // РЕАЛЬНАЯ ЛОГИКА (раскомментировать в декабре):
            if (now.Year > 2024) return false; // Прошлые года открыты
            if (now.Month < 12) return true;   // До декабря всё закрыто
            if (now.Month > 12) return false;  // После декабря всё открыто
            return dayNumber > now.Day;        // В декабре закрыто всё, что позже текущего дня
            */
        }
    }
}