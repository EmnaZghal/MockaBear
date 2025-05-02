using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockaBear.Migrations
{
    /// <inheritdoc />
    public partial class renameRr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarItems_Carts_CartId",
                table: "CarItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CarItems_Products_ProductId",
                table: "CarItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarItems",
                table: "CarItems");

            migrationBuilder.RenameTable(
                name: "CarItems",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_CarItems_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CarItems_CartId",
                table: "CartItems",
                newName: "IX_CartItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CarItems");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                table: "CarItems",
                newName: "IX_CarItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId",
                table: "CarItems",
                newName: "IX_CarItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarItems",
                table: "CarItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarItems_Carts_CartId",
                table: "CarItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarItems_Products_ProductId",
                table: "CarItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
