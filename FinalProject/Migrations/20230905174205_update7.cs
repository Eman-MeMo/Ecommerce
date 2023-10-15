using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_carts_CartId",
                table: "CartProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_products_ProductId",
                table: "CartProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartProduct",
                table: "CartProduct");

            migrationBuilder.RenameTable(
                name: "CartProduct",
                newName: "cartProducts");

            migrationBuilder.RenameIndex(
                name: "IX_CartProduct_CartId",
                table: "cartProducts",
                newName: "IX_cartProducts_CartId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "categories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cartProducts",
                table: "cartProducts",
                columns: new[] { "ProductId", "CartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_cartProducts_carts_CartId",
                table: "cartProducts",
                column: "CartId",
                principalTable: "carts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cartProducts_products_ProductId",
                table: "cartProducts",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cartProducts_carts_CartId",
                table: "cartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_cartProducts_products_ProductId",
                table: "cartProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cartProducts",
                table: "cartProducts");

            migrationBuilder.RenameTable(
                name: "cartProducts",
                newName: "CartProduct");

            migrationBuilder.RenameIndex(
                name: "IX_cartProducts_CartId",
                table: "CartProduct",
                newName: "IX_CartProduct_CartId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartProduct",
                table: "CartProduct",
                columns: new[] { "ProductId", "CartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_carts_CartId",
                table: "CartProduct",
                column: "CartId",
                principalTable: "carts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_products_ProductId",
                table: "CartProduct",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
