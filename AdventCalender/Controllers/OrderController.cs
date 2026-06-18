using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventCalender.Data;
using AdventCalender.Models;

namespace AdventCalender.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(int adventDayId)
        {
            var buyerId = _userManager.GetUserId(User);

            var day = await _context.AdventDays.FirstOrDefaultAsync(d => d.Id == adventDayId);
            if (day == null) return NotFound();

            if (day.IsPaid)
            {
                TempData["Message"] = "Товар уже оплачен.";
                return RedirectToAction("DayDetails", "Calendar", new { id = adventDayId });
            }

            var order = await _context.Orders.FirstOrDefaultAsync(o =>
                o.AdventDayId == adventDayId && o.BuyerId == buyerId && o.Status == OrderStatus.Pending);

            if (order == null)
            {
                order = new Order
                {
                    BuyerId = buyerId!,
                    AdventDayId = adventDayId,
                    Amount = day.Price,
                    Status = OrderStatus.Pending
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Checkout), new { orderId = order.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(int orderId)
        {
            var buyerId = _userManager.GetUserId(User);

            var order = await _context.Orders
                .Include(o => o.AdventDay)
                .ThenInclude(d => d!.Seller)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.BuyerId == buyerId);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int orderId)
        {
            var buyerId = _userManager.GetUserId(User);

            var order = await _context.Orders
                .Include(o => o.AdventDay)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.BuyerId == buyerId);

            if (order == null) return NotFound();

            if (order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Paid;
                order.PaidAt = DateTime.UtcNow;

                if (order.AdventDay != null)
                {
                    order.AdventDay.IsPaid = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Checkout), new { orderId = order.Id });
        }
    }
}