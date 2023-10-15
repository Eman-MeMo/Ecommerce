using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalPrice",
                table: "carts");

            migrationBuilder.AddColumn<string>(
                name: "imag",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imag",
                table: "products");

            migrationBuilder.AddColumn<int>(
                name: "totalPrice",
                table: "carts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
