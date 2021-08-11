﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Plutus.Infrastructure;

namespace Plutus.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "6.0.0-preview.7.21378.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Plutus.Domain.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("TransactionType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name", "TransactionType")
                        .IsUnique();

                    b.ToTable("Category", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("5952fff4-c241-4c87-8eab-47625893e08a"),
                            Name = "Food & Drinks",
                            TransactionType = 0
                        },
                        new
                        {
                            Id = new Guid("1bb370fa-5f84-4919-95bd-f6b422d04e53"),
                            Name = "Online Shopping",
                            TransactionType = 0
                        },
                        new
                        {
                            Id = new Guid("01322ab5-be70-4691-a48c-d614d173ce4a"),
                            Name = "Travel",
                            TransactionType = 0
                        },
                        new
                        {
                            Id = new Guid("f3407e98-9a7c-4576-8926-308d309f11f9"),
                            Name = "Transfer",
                            TransactionType = 0
                        },
                        new
                        {
                            Id = new Guid("03577014-5d5a-4736-8c28-c2966532c9f5"),
                            Name = "Bills",
                            TransactionType = 0
                        },
                        new
                        {
                            Id = new Guid("3bd27765-d5b2-4534-ba73-daa8bbfceb8a"),
                            Name = "Salary",
                            TransactionType = 1
                        },
                        new
                        {
                            Id = new Guid("3163b824-cae4-4953-988b-27c98f3ab585"),
                            Name = "Transfer",
                            TransactionType = 1
                        },
                        new
                        {
                            Id = new Guid("7bbb3fe2-4948-4b60-a02e-5ecc75cd5fa6"),
                            Name = "Gifts",
                            TransactionType = 1
                        });
                });

            modelBuilder.Entity("Plutus.Domain.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOnUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2021, 8, 11, 18, 23, 27, 89, DateTimeKind.Utc).AddTicks(5838));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<bool>("InActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("TransactionType")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("character varying(16)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Username");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("Plutus.Domain.User", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2021, 8, 11, 18, 23, 27, 92, DateTimeKind.Utc).AddTicks(9031));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<bool>("InActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("Password");

                    b.HasKey("Username");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User", (string)null);

                    b.HasData(
                        new
                        {
                            Username = "sanjay",
                            CreatedOnUtc = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "sanjay11@outlook.com",
                            FirstName = "Sanjay",
                            InActive = false,
                            LastModifiedUtc = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Idpuganti",
                            Password = "�f��G\n��.���@v�6�A��1�x��"
                        });
                });

            modelBuilder.Entity("Plutus.Domain.Transaction", b =>
                {
                    b.HasOne("Plutus.Domain.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plutus.Domain.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Plutus.Domain.Category", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Plutus.Domain.User", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
