using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyTrade.Migrations
{
    /// <inheritdoc />
    public partial class Dotnet8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeRequests_DBItems_ItemId",
                table: "TradeRequests");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "TradeRequests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "DBItems",
                type: "varchar(44)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)");

            migrationBuilder.AlterColumn<long>(
                name: "Count",
                table: "DBItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeRequests_DBItems_ItemId",
                table: "TradeRequests",
                column: "ItemId",
                principalTable: "DBItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeRequests_DBItems_ItemId",
                table: "TradeRequests");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "TradeRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "DBItems",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(44)");

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "DBItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeRequests_DBItems_ItemId",
                table: "TradeRequests",
                column: "ItemId",
                principalTable: "DBItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
