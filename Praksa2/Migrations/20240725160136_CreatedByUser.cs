using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Praksa2.Migrations
{
    /// <inheritdoc />
    public partial class CreatedByUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_CreatedByUserId",
                table: "Products");

           /* migrationBuilder.DropIndex(
                name: "IX_Products_CreatedByUserId",
                table: "Products");*/

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUser",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUser",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedByUserId",
                table: "Products",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_CreatedByUserId",
                table: "Products",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
