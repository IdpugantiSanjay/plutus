using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Plutus.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
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
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2021, 8, 11, 18, 23, 27, 92, DateTimeKind.Utc).AddTicks(9031)),
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
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2021, 8, 11, 18, 23, 27, 89, DateTimeKind.Utc).AddTicks(5838)),
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
                    { new Guid("5952fff4-c241-4c87-8eab-47625893e08a"), "Food & Drinks", 0 },
                    { new Guid("1bb370fa-5f84-4919-95bd-f6b422d04e53"), "Online Shopping", 0 },
                    { new Guid("01322ab5-be70-4691-a48c-d614d173ce4a"), "Travel", 0 },
                    { new Guid("f3407e98-9a7c-4576-8926-308d309f11f9"), "Transfer", 0 },
                    { new Guid("03577014-5d5a-4736-8c28-c2966532c9f5"), "Bills", 0 },
                    { new Guid("3bd27765-d5b2-4534-ba73-daa8bbfceb8a"), "Salary", 1 },
                    { new Guid("3163b824-cae4-4953-988b-27c98f3ab585"), "Transfer", 1 },
                    { new Guid("7bbb3fe2-4948-4b60-a02e-5ecc75cd5fa6"), "Gifts", 1 }
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
