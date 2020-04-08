using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProject.EFDataAccess.Migrations
{
    public partial class AddBankAccountMetaData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccountMetaDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    BankAccountId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountMetaDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountMetaDatas_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountMetaDatas_BankAccountId",
                table: "BankAccountMetaDatas",
                column: "BankAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccountMetaDatas");
        }
    }
}
