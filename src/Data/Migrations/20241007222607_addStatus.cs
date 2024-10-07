using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attachment",
                table: "Verifications",
                newName: "Attachments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 22, 26, 6, 502, DateTimeKind.Utc).AddTicks(2002));

            migrationBuilder.InsertData(
                table: "VerificationStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Pendente Revisão" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameColumn(
                name: "Attachments",
                table: "Verifications",
                newName: "Attachment");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 11, 41, 12, 863, DateTimeKind.Utc).AddTicks(1737));
        }
    }
}
