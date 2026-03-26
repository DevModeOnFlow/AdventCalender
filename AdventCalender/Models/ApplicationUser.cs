using Microsoft.AspNetCore.Identity;

namespace AdventCalender.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nickname { get; set; }
        public string? StoreName { get; set; }
        public string? Description { get; set; }

        public int CalendarDaysCount { get; set; } = 24;
        public DateTime CalendarStartDate { get; set; } = DateTime.UtcNow.Date;
        public DateTime CalendarEndDate { get; set; } = DateTime.UtcNow.Date.AddDays(24);

        public virtual ICollection<AdventDay> AdventDays { get; set; } = new List<AdventDay>();
    }
}
