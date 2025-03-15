using Microsoft.EntityFrameworkCore;
using gift_card_csharp_postgres.Models; // Ensure this is correct

namespace gift_card_csharp_postgres.Data // Ensure this matches the project name
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GiftCard>()
                .HasIndex(gc => gc.CardNumber)
                .IsUnique();
        }
    }
}