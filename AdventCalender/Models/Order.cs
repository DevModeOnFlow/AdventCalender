using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventCalender.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string BuyerId { get; set; } = string.Empty;
        public virtual ApplicationUser? Buyer { get; set; }

        [Required]
        public int AdventDayId { get; set; }
        public virtual AdventDay? AdventDay { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}