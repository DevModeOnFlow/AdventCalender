using AdventCalender.Models;
using Microsoft.AspNetCore.Identity;

namespace AdventCalender.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();

            // Проверяем, есть ли уже продавцы
            if (context.Users.Any(u => u.StoreName != null))
            {
                return; // База уже заполнена
            }

            // 1. Создаем тестового продавца
            var seller = new ApplicationUser
            {
                UserName = "test@seller.com",
                Email = "test@seller.com",
                StoreName = "Новогодняя Лавка 🎄",
                Description = "Лучшие подарки и сладости для вашего праздника!",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(seller, "Password123!");

            // 2. Добавляем несколько дней в календарь для этого продавца
            var days = new List<AdventDay>
            {
                new AdventDay {
                    DayNumber = 1,
                    Title = "Имбирный пряник",
                    Description = "Вкусный домашний пряник с корицей",
                    Price = 150.00m,
                    SellerId = seller.Id
                },
                new AdventDay {
                    DayNumber = 2,
                    Title = "Набор свечей",
                    Description = "Ароматические свечи с запахом хвои",
                    Price = 450.00m,
                    SellerId = seller.Id
                },
                new AdventDay {
                    DayNumber = 3,
                    Title = "Горячий шоколад",
                    Description = "Порция густого шоколада с маршмэллоу",
                    Price = 200.00m,
                    SellerId = seller.Id
                }
            };

            context.AdventDays.AddRange(days);
            await context.SaveChangesAsync();
        }
    }
}