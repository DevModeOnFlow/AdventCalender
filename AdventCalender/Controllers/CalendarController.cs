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
            var day = await _context.AdventDays
                .Include(d => d.Seller)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (day == null) return NotFound();

            var seller = day.Seller;
            DateTime now = DateTime.Now;

            DateTime targetDate = seller.CalendarStartDate.Date.AddDays(day.DayNumber - 1);

            if (now.Date < targetDate)
            {
                TempData["Message"] = $"Ещё рано! Этот подарок откроется только {targetDate:dd.MM.yyyy}";
                return RedirectToAction("Index", new { sellerId = seller.Id });
            }

            if (now.Date == targetDate)
            {
                if (now.TimeOfDay < day.StartTime)
                {
                    TempData["Message"] = $"Слишком рано! Заходите в {day.StartTime:hh\\:mm}";
                    return RedirectToAction("Index", new { sellerId = seller.Id });
                }
                if (now.TimeOfDay > day.EndTime)
                {
                    TempData["Message"] = "Увы, время этого предложения на сегодня истекло!";
                    return RedirectToAction("Index", new { sellerId = seller.Id });
                }
            }

            if (now.Date > targetDate)
            {
                TempData["Message"] = "Акция для этого дня уже завершена!";
                return RedirectToAction("Index", new { sellerId = seller.Id });
            }

            return View(day);
        }

    }
}