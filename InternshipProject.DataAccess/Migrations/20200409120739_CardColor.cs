using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternshipProject.EFDataAccess.Migrations
{
    public partial class CardColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardColors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<int>(nullable: false),
                    CardId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardColors_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardColors_CardId",
                table: "CardColors",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardColors");
        }
    }
}
