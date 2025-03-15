using gift_card_csharp_postgres.Data;
using gift_card_csharp_postgres.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace gift_card_csharp_postgres.Controllers
{
// testing changes
    [Route("api/giftcards")]
    [ApiController]
    public class GiftCardController : ControllerBase
    {
    private readonly AppDbContext _context;

    public GiftCardController(AppDbContext context)
    {
        _context = context;
    }
        // 1. Create Gift Card
        [HttpPost]
        public async Task<IActionResult> CreateGiftCard([FromBody] CreateGiftCardRequest request)
        {
            var cardNumber = GenerateCardNumber();
            var redemptionCode = GenerateRedemptionCode();
            var redemptionCodeHash = HashRedemptionCode(redemptionCode);

            var giftCard = new GiftCard
            {
                CardNumber = cardNumber,
                RedemptionCodeHash = redemptionCodeHash,
                Balance = request.InitialBalance,
                Currency = request.Currency
            };

            _context.GiftCards.Add(giftCard);
            await _context.SaveChangesAsync();

            return Ok(new { cardNumber, redemptionCode, giftCard.Status, giftCard.Balance, giftCard.Currency });
        }

        [HttpGet]
        public async Task<IActionResult> GetGiftCardDetails([FromQuery] string? redemptionHash)
        {
            if (string.IsNullOrEmpty(redemptionHash))
                return BadRequest("Redemption hash is required.");

            var giftCard = await _context.GiftCards
                .FirstOrDefaultAsync(gc => gc.RedemptionCodeHash == redemptionHash);

            if (giftCard == null)
                return NotFound();

            return Ok(new { giftCard.CardNumber, giftCard.Balance, giftCard.Currency, giftCard.Status });
        }


        // 3. Check Balance
        [HttpGet("{redemptionHash}/balance")]

        public async Task<IActionResult> CheckBalanceByHash(string redemptionHash)
        {
            if (string.IsNullOrEmpty(redemptionHash))
                return BadRequest("Redemption hash is required.");

            var giftCard = await _context.GiftCards.FirstOrDefaultAsync(gc => gc.RedemptionCodeHash == redemptionHash);
            if (giftCard == null)
                return NotFound("Gift card not found.");

            return Ok(new { giftCard.Balance, giftCard.Currency });
        }

        private static string GenerateCardNumber() => new Random().Next(100000000, 999999999).ToString() + new Random().Next(100000000, 999999999).ToString();
        private static string GenerateRedemptionCode() => Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();

        private static string HashRedemptionCode(string code)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code));
            return Convert.ToBase64String(hashBytes)
                .Replace("+", "-")  // Make URL-safe
                .Replace("/", "_")  // Make URL-safe
                .TrimEnd('=');      // Remove padding
        }



    }
}

public class CreateGiftCardRequest
{
    [Required]
    public decimal InitialBalance { get; set; }

    [Required, StringLength(3)]
    public string Currency { get; set; }
}