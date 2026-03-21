using Microsoft.AspNetCore.Identity;

namespace AdventCalender.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? StoreName { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        // Связь с товарами календаря
        public virtual ICollection<AdventDay> AdventDays { get; set; } = new List<AdventDay>();
    }
}
