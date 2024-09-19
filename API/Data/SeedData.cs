namespace API.Data;
// Data/SeedData.cs
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void SeedUsers(IServiceProvider serviceProvider)
    {
        using (var context = new DataContext(
            serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
        {
            
            if (!context.Users.Any())
            {
                // Seed Users
                context.Users.AddRange(
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PasswordHash = HashPassword("Pa$$w0rd"), // You should hash actual passwords in production
                    WalletAddress = "0xabc123walletaddress1",
                    CreditScore = 750,
                    IdNumber = "1234567890",
                    Address = "123 Main St, City, Country",
                    PhoneNumber = "+1234567890",
                    EmploymentStatus = "Employed",
                    AnnualIncome = 50000,
                    Role = Role.Borrower, // Assuming Role is an enum
                    CreatedAt = DateTime.Now.AddMonths(-6),
                    UpdatedAt = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PasswordHash = HashPassword("Pa$$w0rd"),
                    WalletAddress = "0xabc123walletaddress2",
                    CreditScore = 680,
                    IdNumber = "0987654321",
                    Address = "456 Oak St, City, Country",
                    PhoneNumber = "+9876543210",
                    EmploymentStatus = "Self-employed",
                    AnnualIncome = 75000,
                    Role = Role.Lender, // Assuming Role is an enum
                    CreatedAt = DateTime.Now.AddYears(-1),
                    UpdatedAt = DateTime.Now
                },
                new User
                {
                    Id = 3,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    PasswordHash = HashPassword("Pa$$w0rd"),
                    WalletAddress = "0xabc123walletaddress3",
                    CreditScore = 620,
                    IdNumber = "1122334455",
                    Address = "789 Pine St, City, Country",
                    PhoneNumber = "+1122334455",
                    EmploymentStatus = "Unemployed",
                    AnnualIncome = 0,
                    Role = Role.Borrower,
                    CreatedAt = DateTime.Now.AddMonths(-3),
                    UpdatedAt = DateTime.Now
                },
                new User
                {
                    Id = 4,
                    FirstName = "Thabo",
                    LastName = "Mokhothu",
                    Email = "thabo.mokhothu@example.com",
                    PasswordHash = HashPassword("Pa$$w0rd"), 
                    WalletAddress = "0xjkl456walletaddress4",
                    CreditScore = 800,
                    IdNumber = "3344556677",
                    Address = "101 Freedom St, Maseru",
                    PhoneNumber = "064-987-6543",
                    EmploymentStatus = "Employed",
                    AnnualIncome = 120000.00m,
                    Role = Role.Lender,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new User
                {
                    Id = 5,
                    FirstName = "Lerato",
                    LastName = "Mokoena",
                    Email = "lerato.mokoena@example.com",
                    PasswordHash = HashPassword("Pa$$w0rd"), 
                    WalletAddress = "0xxyz789walletaddress5",
                    CreditScore = 670,
                    IdNumber = "5566778899",
                    Address = "99 Hope St, Bloemfontein",
                    PhoneNumber = "082-123-4567",
                    EmploymentStatus = "Self-Employed",
                    AnnualIncome = 95000.00m,
                    Role = Role.Borrower,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
                );
                context.SaveChanges();
            }

            

            // if (!context.LoanOffers.Any())
            // {
            //     // Seed LoanOffers
            //    context.LoanOffers.AddRange(
            //    new LoanOffer
            //     {
            //         Id = 1,
            //         LenderId = context.Users.Single(u => u.FirstName == "Jane").Id,
            //         Lender = context.Users.Single(u => u.FirstName == "Jane"),
            //         PrincipalAmount = 10000.00m,
            //         InterestRate = 5.5m,
            //         DurationInMonths = 12,
            //         IsActive = true,
            //         CreatedAt = DateTime.UtcNow,
            //         UpdatedAt = DateTime.UtcNow
            //     },
            //     new LoanOffer
            //     {
            //         Id = 2,
            //         LenderId = context.Users.Single(u => u.FirstName == "Thabo").Id,
            //         Lender = context.Users.Single(u => u.FirstName == "Thabo"),
            //         PrincipalAmount = 15000.00m,
            //         InterestRate = 6.0m,
            //         DurationInMonths = 24,
            //         IsActive = true,
            //         CreatedAt = DateTime.UtcNow,
            //         UpdatedAt = DateTime.UtcNow
            //     },
            //     new LoanOffer
            //     {
            //         Id = 3,
            //         LenderId = context.Users.Single(u => u.FirstName == "Thabo").Id,
            //         Lender = context.Users.Single(u => u.FirstName == "Thabo"),
            //         PrincipalAmount = 5000.00m,
            //         InterestRate = 4.0m,
            //         DurationInMonths = 6,
            //         IsActive = true,
            //         CreatedAt = DateTime.UtcNow,
            //         UpdatedAt = DateTime.UtcNow
            //     },
            //     new LoanOffer
            //     {
            //         Id = 4,
            //         LenderId = context.Users.Single(u => u.FirstName == "Jane").Id,
            //         Lender = context.Users.Single(u => u.FirstName == "Jane"),
            //         PrincipalAmount = 25000.00m,
            //         InterestRate = 7.0m,
            //         DurationInMonths = 36,
            //         IsActive = true,
            //         CreatedAt = DateTime.UtcNow,
            //         UpdatedAt = DateTime.UtcNow
            //     },
            //     new LoanOffer
            //     {
            //         Id = 5,
            //         LenderId = context.Users.Single(u => u.FirstName == "Jane").Id,
            //         Lender = context.Users.Single(u => u.FirstName == "Jane"),
            //         PrincipalAmount = 2000.00m,
            //         InterestRate = 3.5m,
            //         DurationInMonths = 3,
            //         IsActive = false, // This one is no longer active
            //         CreatedAt = DateTime.UtcNow,
            //         UpdatedAt = DateTime.UtcNow
            //     }
            //         );
            //     context.SaveChanges();
            // }

            // if (!context.LoanApplications.Any())
            // {
            //     // Seed LoanApplications
            //     context.LoanApplications.AddRange(
            //     new LoanApplication
            // {
            //     Id = 1,
            //     LoanOfferId = 1,
            //     BorrowerId = context.Users.Single(u => u.FirstName == "Lerato").Id,
            //     Status = LoanApplicationStatus.Pending,
            //     CreatedAt = DateTime.UtcNow.AddDays(-5),
            //     UpdatedAt = DateTime.UtcNow.AddDays(-5)
            // },
            // new LoanApplication
            // {
            //     Id = 2,
            //     LoanOfferId = 2,
            //     BorrowerId = context.Users.Single(u => u.FirstName == "Alice").Id,
            //     Status = LoanApplicationStatus.Approved,
            //     CreatedAt = DateTime.UtcNow.AddDays(-10),
            //     UpdatedAt = DateTime.UtcNow.AddDays(-8)
            // },
            // new LoanApplication
            // {
            //     Id = 3,
            //     LoanOfferId = 3,
            //     BorrowerId = context.Users.Single(u => u.FirstName == "Lerato").Id,
            //     Status = LoanApplicationStatus.Rejected,
            //     CreatedAt = DateTime.UtcNow.AddDays(-15),
            //     UpdatedAt = DateTime.UtcNow.AddDays(-14)
            // },
            // new LoanApplication
            // {
            //     Id = 4,
            //     LoanOfferId = 4,
            //     BorrowerId = context.Users.Single(u => u.FirstName == "Alice").Id,
            //     Status = LoanApplicationStatus.Withdrawn,
            //     CreatedAt = DateTime.UtcNow.AddDays(-20),
            //     UpdatedAt = DateTime.UtcNow.AddDays(-19)
            // },
            // new LoanApplication
            // {
            //     Id = 5,
            //     LoanOfferId = 5,
            //     BorrowerId = context.Users.Single(u => u.FirstName == "John").Id,
            //     Status = LoanApplicationStatus.Pending,
            //     CreatedAt = DateTime.UtcNow.AddDays(-2),
            //     UpdatedAt = DateTime.UtcNow.AddDays(-2)
            // }
                    
            //     );
            //     context.SaveChanges();
            // }


            // if (!context.Ratings.Any())
            // {
            //     // Seed Ratings
            //     context.Ratings.AddRange(
            //     new Rating
            //     {
            //         Id = 1,
            //         LenderId = context.Users.Single(u => u.FirstName == "Jane").Id,
            //         BorrowerId = context.Users.Single(u => u.FirstName == "Lerato").Id,
            //         LoanApplicationId = 1,
            //         Score = 5,
            //         CreatedAt = DateTime.UtcNow.AddDays(-30),
            //         UpdatedAt = DateTime.UtcNow.AddDays(-30)
            //     },
            //     new Rating
            //     {
            //         Id = 2,
            //         LenderId = context.Users.Single(u => u.FirstName == "Thabo").Id,
            //         BorrowerId = context.Users.Single(u => u.FirstName == "Alice").Id,
            //         LoanApplicationId = 2,
            //         Score = 4,
            //         CreatedAt = DateTime.UtcNow.AddDays(-25),
            //         UpdatedAt = DateTime.UtcNow.AddDays(-25)
            //     },
            //     new Rating
            //     {
            //         Id = 3,
            //         LenderId = context.Users.Single(u => u.FirstName == "Thabo").Id,
            //         BorrowerId = context.Users.Single(u => u.FirstName == "Lerato").Id,
            //         LoanApplicationId = 3,
            //         Score = 2,
            //         CreatedAt = DateTime.UtcNow.AddDays(-20),
            //         UpdatedAt = DateTime.UtcNow.AddDays(-20)
            //     },
            //     new Rating
            //     {
            //         Id = 4,
            //         LenderId = context.Users.Single(u => u.FirstName == "Jane").Id,
            //         BorrowerId = context.Users.Single(u => u.FirstName == "Alice").Id,
            //         LoanApplicationId = 4,
            //         Score = 3,
            //         CreatedAt = DateTime.UtcNow.AddDays(-15),
            //         UpdatedAt = DateTime.UtcNow.AddDays(-15)
            //      }
                    
            //     );
            //     context.SaveChanges();
            // }

        }
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
