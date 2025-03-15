using System;
using System.ComponentModel.DataAnnotations;

namespace gift_card_csharp_postgres.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public string Type { get; set; } // ENUM: BLOCK, PAYMENT, CAPTURE, CANCEL

        [Required]
        public decimal Amount { get; set; }

        [Required, StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public string Status { get; set; } // ENUM: BLOCKED, COMPLETED, CANCELLED

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
