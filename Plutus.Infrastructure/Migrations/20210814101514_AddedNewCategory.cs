using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Plutus.Infrastructure.Migrations
{
    public partial class AddedNewCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("01322ab5-be70-4691-a48c-d614d173ce4a"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("03577014-5d5a-4736-8c28-c2966532c9f5"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("1bb370fa-5f84-4919-95bd-f6b422d04e53"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("3163b824-cae4-4953-988b-27c98f3ab585"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("3bd27765-d5b2-4534-ba73-daa8bbfceb8a"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("5952fff4-c241-4c87-8eab-47625893e08a"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("7bbb3fe2-4948-4b60-a02e-5ecc75cd5fa6"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("f3407e98-9a7c-4576-8926-308d309f11f9"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 14, 10, 15, 14, 62, DateTimeKind.Utc).AddTicks(6298),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2021, 8, 11, 18, 23, 27, 92, DateTimeKind.Utc).AddTicks(9031));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Transaction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 14, 10, 15, 14, 59, DateTimeKind.Utc).AddTicks(2325),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2021, 8, 11, 18, 23, 27, 89, DateTimeKind.Utc).AddTicks(5838));

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name", "TransactionType" },
                values: new object[,]
                {
                    { new Guid("7ae15088-adbe-4666-b8a2-c808006999eb"), "Food & Drinks", 0 },
                    { new Guid("33cc6ca7-6fd7-4582-8661-445db50da886"), "Online Shopping", 0 },
                    { new Guid("e777300d-40c4-4b9d-a43e-54126a404671"), "Travel", 0 },
                    { new Guid("adb40027-8122-481c-ba6b-b3c3df9377c3"), "Transfer", 0 },
                    { new Guid("439dc4a2-f13c-486c-aa80-eade8c518253"), "Bills", 0 },
                    { new Guid("c9e14393-73ef-4fca-9bc1-a42f136c9a96"), "Salary", 1 },
                    { new Guid("e39652e2-b8cf-4f1c-b31a-20b0a564b31e"), "Transfer", 1 },
                    { new Guid("47cba66c-6eff-4e5f-8f4e-861b9a726597"), "Gift", 1 },
                    { new Guid("c323143a-c288-442f-8b7a-58065588e6e1"), "Subscriptions", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("33cc6ca7-6fd7-4582-8661-445db50da886"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("439dc4a2-f13c-486c-aa80-eade8c518253"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("47cba66c-6eff-4e5f-8f4e-861b9a726597"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("7ae15088-adbe-4666-b8a2-c808006999eb"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("adb40027-8122-481c-ba6b-b3c3df9377c3"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("c323143a-c288-442f-8b7a-58065588e6e1"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("c9e14393-73ef-4fca-9bc1-a42f136c9a96"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("e39652e2-b8cf-4f1c-b31a-20b0a564b31e"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("e777300d-40c4-4b9d-a43e-54126a404671"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 11, 18, 23, 27, 92, DateTimeKind.Utc).AddTicks(9031),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2021, 8, 14, 10, 15, 14, 62, DateTimeKind.Utc).AddTicks(6298));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Transaction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 11, 18, 23, 27, 89, DateTimeKind.Utc).AddTicks(5838),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2021, 8, 14, 10, 15, 14, 59, DateTimeKind.Utc).AddTicks(2325));

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
        }
    }
}
