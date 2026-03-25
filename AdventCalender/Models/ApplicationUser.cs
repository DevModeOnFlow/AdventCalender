using Microsoft.AspNetCore.Identity;

namespace AdventCalender.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nickname { get; set; }
        public string? StoreName { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public virtual ICollection<AdventDay> AdventDays { get; set; } = new List<AdventDay>();
    }
}
