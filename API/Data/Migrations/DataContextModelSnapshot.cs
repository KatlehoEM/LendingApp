﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("API.Entities.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BorrowerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DurationInYears")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("TEXT");

                    b.Property<int>("LoanOfferId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("MonthlyRepayment")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrincipalAmount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("RemainingBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalRepayment")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId");

                    b.HasIndex("LoanOfferId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("API.Entities.LoanApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AcceptanceDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("BorrowerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("LoanOfferId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("MonthlyRepayment")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalRepayment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId");

                    b.HasIndex("LoanOfferId");

                    b.ToTable("LoanApplications");
                });

            modelBuilder.Entity("API.Entities.LoanOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("DurationInYears")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasApplied")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LenderId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("MonthlyRepayment")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrincipalAmount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalRepayment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LenderId");

                    b.HasIndex("UserId");

                    b.ToTable("LoanOffers");
                });

            modelBuilder.Entity("API.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<int>("LoanApplicationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LoanId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LoanApplicationId");

                    b.HasIndex("LoanId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BorrowerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("LenderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LoanApplicationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId");

                    b.HasIndex("LenderId");

                    b.HasIndex("LoanApplicationId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AnnualIncome")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreditScore")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmploymentStatus")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StreetName")
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("WalletAddress")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.Loan", b =>
                {
                    b.HasOne("API.Entities.User", "Borrower")
                        .WithMany()
                        .HasForeignKey("BorrowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.LoanOffer", "LoanOffer")
                        .WithMany()
                        .HasForeignKey("LoanOfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Borrower");

                    b.Navigation("LoanOffer");
                });

            modelBuilder.Entity("API.Entities.LoanApplication", b =>
                {
                    b.HasOne("API.Entities.User", "Borrower")
                        .WithMany()
                        .HasForeignKey("BorrowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entities.LoanOffer", "LoanOffer")
                        .WithMany()
                        .HasForeignKey("LoanOfferId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Borrower");

                    b.Navigation("LoanOffer");
                });

            modelBuilder.Entity("API.Entities.LoanOffer", b =>
                {
                    b.HasOne("API.Entities.User", "Lender")
                        .WithMany()
                        .HasForeignKey("LenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entities.User", null)
                        .WithMany("LoanOffers")
                        .HasForeignKey("UserId");

                    b.Navigation("Lender");
                });

            modelBuilder.Entity("API.Entities.Payment", b =>
                {
                    b.HasOne("API.Entities.LoanApplication", null)
                        .WithMany("Payments")
                        .HasForeignKey("LoanApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Loan", "Loan")
                        .WithMany("Payments")
                        .HasForeignKey("LoanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Loan");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.HasOne("API.Entities.User", "Borrower")
                        .WithMany()
                        .HasForeignKey("BorrowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "Lender")
                        .WithMany()
                        .HasForeignKey("LenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entities.LoanApplication", "LoanApplication")
                        .WithMany()
                        .HasForeignKey("LoanApplicationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entities.User", null)
                        .WithMany("Ratings")
                        .HasForeignKey("UserId");

                    b.Navigation("Borrower");

                    b.Navigation("Lender");

                    b.Navigation("LoanApplication");
                });

            modelBuilder.Entity("API.Entities.Loan", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("API.Entities.LoanApplication", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Navigation("LoanOffers");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
