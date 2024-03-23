using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothCanvasData.Migrations
{
    /// <inheritdoc />
    public partial class customDetailsURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_CustomDetails_CustomDetailId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CustomDetailId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CustomDetailId",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "CustomDetailURL",
                table: "OrderItems",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomDetails_OrderItemId",
                table: "CustomDetails",
                column: "OrderItemId");

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

            migrationBuilder.DropColumn(
                name: "CustomDetailURL",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "CustomDetailId",
                table: "OrderItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CustomDetailId",
                table: "OrderItems",
                column: "CustomDetailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_CustomDetails_CustomDetailId",
                table: "OrderItems",
                column: "CustomDetailId",
                principalTable: "CustomDetails",
                principalColumn: "CustomDetailId");
        }
    }
}
