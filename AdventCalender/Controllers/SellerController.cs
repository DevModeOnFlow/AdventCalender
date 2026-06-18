using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventCalender.Data;
using AdventCalender.Models;

namespace AdventCalender.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users
                .Include(u => u.AdventDays)
                .FirstOrDefaultAsync(u => u.Id == userId);


            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStore(string storeName, string description, DateTime calendarStartDate, DateTime calendarEndDate)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (calendarEndDate < calendarStartDate)
                {
                    TempData["Error"] = "Дата окончания не может быть раньше даты начала!";
                    return RedirectToAction(nameof(Dashboard));
                }

                user.StoreName = storeName;
                user.Description = description;

                user.CalendarStartDate = DateTime.SpecifyKind(calendarStartDate.Date, DateTimeKind.Utc);
                user.CalendarEndDate = DateTime.SpecifyKind(calendarEndDate.Date, DateTimeKind.Utc);
                user.CalendarDaysCount = (user.CalendarEndDate.Date - user.CalendarStartDate.Date).Days + 1;

                await _userManager.UpdateAsync(user);
                TempData["Success"] = "Настройки магазина обновлены!";
            }
            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> EditDay(int? id, int dayNumber)
        {
            var userId = _userManager.GetUserId(User);
            if (id.HasValue)
            {
                var day = await _context.AdventDays.FirstOrDefaultAsync(d => d.Id == id && d.SellerId == userId);
                if (day == null) return NotFound();
                return View(day);
            }
            return View(new AdventDay { DayNumber = dayNumber, SellerId = userId! });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDay(AdventDay model)
        {
            var userId = _userManager.GetUserId(User);
            model.SellerId = userId!;

            ModelState.Remove("Seller");
            ModelState.Remove("SellerId");

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    _context.AdventDays.Add(model);
                }
                else
                {
                    _context.AdventDays.Update(model);
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = $"День {model.DayNumber} успешно сохранен!";
                return RedirectToAction(nameof(Dashboard));
            }
            return View("EditDay", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDay(int id)
        {
            var userId = _userManager.GetUserId(User);
            var day = await _context.AdventDays.FirstOrDefaultAsync(d => d.Id == id && d.SellerId == userId);
            if (day != null)
            {
                _context.AdventDays.Remove(day);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }
    }
}