using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventCalender.Models
{
    public class AdventDay
    {
        public int Id { get; set; }

        [Required]
        public int DayNumber { get; set; }

        [Required(ErrorMessage = "Введите название подарка")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsPaid { get; set; } = false;

        public TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan EndTime { get; set; } = new TimeSpan(21, 0, 0);

        public string SellerId { get; set; } = string.Empty;

        public virtual ApplicationUser? Seller { get; set; }
    }
}