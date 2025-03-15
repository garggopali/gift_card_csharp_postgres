using System;
using System.ComponentModel.DataAnnotations;

namespace gift_card_csharp_postgres.Models
{
    public class GiftCard
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(64)]
        public string? CardNumber { get; set; }

        [Required, StringLength(64)]
        public string? RedemptionCodeHash { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required, StringLength(3)]
        public string? Currency { get; set; }

        [Required]
        public string Status { get; set; } = "ACTIVE";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}