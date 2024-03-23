using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothCanvasData.Migrations
{
    /// <inheritdoc />
    public partial class CustomDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomDetails_Products_ProductId",
                table: "CustomDetails");

            migrationBuilder.DropIndex(
                name: "IX_CustomDetails_ProductId",
                table: "CustomDetails");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CustomDetails",
                newName: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomDetails_OrderItemId",
                table: "CustomDetails",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomDetails_OrderItems_OrderItemId",
                table: "CustomDetails",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "OrderItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomDetails_OrderItems_OrderItemId",
                table: "CustomDetails");

            migrationBuilder.DropIndex(
                name: "IX_CustomDetails_OrderItemId",
                table: "CustomDetails");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "CustomDetails",
                newName: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomDetails_ProductId",
                table: "CustomDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomDetails_Products_ProductId",
                table: "CustomDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
