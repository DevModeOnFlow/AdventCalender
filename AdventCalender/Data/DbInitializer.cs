using AdventCalender.Models;
using Microsoft.AspNetCore.Identity;

namespace AdventCalender.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            context.Database.EnsureCreated();

            string[] roleNames = { "Buyer", "Seller" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (context.Users.Any(u => u.StoreName != null)) return;

            var seller = new ApplicationUser
            {
                UserName = "seller@test.com",
                Email = "seller@test.com",
                StoreName = "Магазин Чудес",
                EmailConfirmed = true,
                CalendarDaysCount = 24
            };
            if ((await userManager.CreateAsync(seller, "Pass123!")).Succeeded)
            {
                await userManager.AddToRoleAsync(seller, "Seller"); 
            }
        }
    }
}