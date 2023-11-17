using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyTrade.Migrations
{
    /// <inheritdoc />
    public partial class AddItemTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "WantedItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TradeRequests",
                type: "varchar(32)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "WantedItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TradeRequests");
        }
    }
}
