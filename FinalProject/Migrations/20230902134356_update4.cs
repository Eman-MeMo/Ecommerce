using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_carts_CartId",
                table: "products");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_products_carts_CartId",
                table: "products",
                column: "CartId",
                principalTable: "carts",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_carts_CartId",
                table: "products");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_products_carts_CartId",
                table: "products",
                column: "CartId",
                principalTable: "carts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
