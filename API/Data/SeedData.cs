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
                    StreetNumber = "789",
                    StreetName = "Pine St",
                    City = "City",
                    PostalCode = "324",
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
                    StreetNumber = "789",
                    StreetName = "Pine St",
                    City = "City",
                    PostalCode = "324",
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
                    StreetNumber = "789",
                    StreetName = "Pine St",
                    City = "City",
                    PostalCode = "324",
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
                    StreetNumber = "789",
                    StreetName = "Pine St",
                    City = "City",
                    PostalCode = "324",
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
                    StreetNumber = "789",
                    StreetName = "Pine St",
                    City = "City",
                    PostalCode = "324",
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
