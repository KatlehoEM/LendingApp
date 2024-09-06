using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<LoanOffer> LoanOffers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // LoanOffer configuration
            modelBuilder.Entity<LoanOffer>()
                .HasOne(lo => lo.Lender)
                .WithMany()
                .HasForeignKey(lo => lo.LenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // LoanApplication configuration
            modelBuilder.Entity<LoanApplication>()
                .HasOne(la => la.LoanOffer)
                .WithMany()
                .HasForeignKey(la => la.LoanOfferId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanApplication>()
                .HasOne(la => la.Borrower)
                .WithMany()
                .HasForeignKey(la => la.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // BorrowerRating configuration
            modelBuilder.Entity<Rating>()
                .HasOne(br => br.Lender)
                .WithMany()
                .HasForeignKey(br => br.LenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(br => br.Borrower)
                .WithMany()
                .HasForeignKey(br => br.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(br => br.LoanApplication)
                .WithMany()
                .HasForeignKey(br => br.LoanApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
