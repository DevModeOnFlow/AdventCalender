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

        // Страница календаря (сетка дней)
        public async Task<IActionResult> Index(string sellerId)
        {
            var seller = await _context.Users
                .Include(u => u.AdventDays)
                .FirstOrDefaultAsync(u => u.Id == sellerId);

            if (seller == null) return NotFound();

            return View(seller);
        }

        // Детали конкретного дня/товара
        public async Task<IActionResult> DayDetails(int id)
        {
            var day = await _context.AdventDays
                .Include(d => d.Seller)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (day == null) return NotFound();

            return View(day);
        }

        [HttpPost]
        public async Task<IActionResult> BuyStub(int id)
        {
            var day = await _context.AdventDays.FindAsync(id);
            if (day != null)
            {
                day.IsPaid = true; // Заглушка оплаты
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DayDetails", new { id = id });
        }
    }
}
