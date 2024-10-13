using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addClassification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddColumn<int>(
                name: "VerificationClassificationId",
                table: "Verifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VerificationClassification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationClassification", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 13, 20, 28, 14, 580, DateTimeKind.Utc).AddTicks(927));

            migrationBuilder.UpdateData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Activo no Portal");

            migrationBuilder.UpdateData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Verificado");

            migrationBuilder.UpdateData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Aguardando Verificação");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_VerificationClassificationId",
                table: "Verifications",
                column: "VerificationClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_VerificationClassification_VerificationClassificationId",
                table: "Verifications",
                column: "VerificationClassificationId",
                principalTable: "VerificationClassification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_VerificationClassification_VerificationClassificationId",
                table: "Verifications");

            migrationBuilder.DropTable(
                name: "VerificationClassification");

            migrationBuilder.DropIndex(
                name: "IX_Verifications_VerificationClassificationId",
                table: "Verifications");

            migrationBuilder.DropColumn(
                name: "VerificationClassificationId",
                table: "Verifications");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 12, 22, 0, 32, 851, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Verdadeira");

            migrationBuilder.UpdateData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Falsa");

            migrationBuilder.UpdateData(
                table: "VerificationStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Em Revisao");

            migrationBuilder.InsertData(
                table: "VerificationStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Pendente Revisão" });
        }
    }
}
