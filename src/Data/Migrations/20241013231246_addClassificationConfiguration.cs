using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addClassificationConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 13, 23, 12, 46, 38, DateTimeKind.Utc).AddTicks(2198));

            migrationBuilder.InsertData(
                table: "VerificationClassification",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fake News" },
                    { 2, "Verídico" },
                    { 3, "Enganoso" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VerificationClassification",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VerificationClassification",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VerificationClassification",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 13, 20, 28, 14, 580, DateTimeKind.Utc).AddTicks(927));
        }
    }
}
