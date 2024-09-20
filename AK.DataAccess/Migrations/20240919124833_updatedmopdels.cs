using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AK.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedmopdels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gallerys_Categories_CategoryId",
                table: "Gallerys");

            migrationBuilder.DropIndex(
                name: "IX_Gallerys_CategoryId",
                table: "Gallerys");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Gallerys");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Gallerys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Gallerys");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Gallerys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gallerys_CategoryId",
                table: "Gallerys",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gallerys_Categories_CategoryId",
                table: "Gallerys",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
