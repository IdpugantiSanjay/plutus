using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Plutus.Infrastructure.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2021, 5, 9, 10, 11, 21, 136, DateTimeKind.Utc).AddTicks(3189)),
                    InActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Username = table.Column<string>(type: "character varying(16)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2021, 5, 9, 10, 11, 21, 84, DateTimeKind.Utc).AddTicks(1561)),
                    InActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_User_Username",
                        column: x => x.Username,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name", "TransactionType" },
                values: new object[,]
                {
                    { new Guid("e1476226-a820-41b1-b34b-ef10858dcdf2"), "Food & Drinks", 0 },
                    { new Guid("2b4a499d-f099-4609-b217-dab164ae1759"), "Travel", 0 },
                    { new Guid("780546af-ee40-4513-b2b8-b9f43f2b7aec"), "Transfer", 0 },
                    { new Guid("50ad9a4e-8eb0-4e34-9e4f-21433e671558"), "Bills", 0 },
                    { new Guid("8b4d2ee1-9cdf-4760-b0f9-7e5a543a3f97"), "Salary", 1 },
                    { new Guid("6193fbce-a045-4ad6-8cf0-d11a0ea8cb7d"), "Transfer", 1 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Username", "Email", "FirstName", "InActive", "LastModifiedUtc", "LastName", "Password" },
                values: new object[] { "sanjay", "sanjay11@outlook.com", "Sanjay", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Idpuganti", "�f��G\n��.���@v�6�A��1�x��" });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name_TransactionType",
                table: "Category",
                columns: new[] { "Name", "TransactionType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CategoryId",
                table: "Transaction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Username",
                table: "Transaction",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
