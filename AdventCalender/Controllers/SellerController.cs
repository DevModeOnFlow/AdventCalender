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

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStore(string storeName, string description)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.StoreName = storeName;
                user.Description = description;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Данные магазина успешно обновлены!";
                }
            }
            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> EditDay(int? id, int dayNumber)
        {
            var userId = _userManager.GetUserId(User);

            if (id.HasValue)
            {
                var day = await _context.AdventDays
                    .FirstOrDefaultAsync(d => d.Id == id && d.SellerId == userId);

                if (day == null) return Forbid(); 

                return View(day);
            }
            else
            {
                var newDay = new AdventDay
                {
                    DayNumber = dayNumber,
                    SellerId = userId!
                };
                return View(newDay);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDay(AdventDay model)
        {
            var userId = _userManager.GetUserId(User);
            model.SellerId = userId!;

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var existingDay = await _context.AdventDays
                        .AnyAsync(d => d.DayNumber == model.DayNumber && d.SellerId == userId);

                    if (existingDay)
                    {
                        ModelState.AddModelError("", "Этот день в календаре уже заполнен.");
                        return View("EditDay", model);
                    }

                    _context.AdventDays.Add(model);
                }
                else
                {
                    var dbDay = await _context.AdventDays
                        .AsNoTracking()
                        .FirstOrDefaultAsync(d => d.Id == model.Id && d.SellerId == userId);

                    if (dbDay == null) return Forbid();

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
            var day = await _context.AdventDays
                .FirstOrDefaultAsync(d => d.Id == id && d.SellerId == userId);

            if (day != null)
            {
                _context.AdventDays.Remove(day);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Запись удалена.";
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}