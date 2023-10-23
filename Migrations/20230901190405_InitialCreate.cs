using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkyTrade.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemName = table.Column<string>(type: "varchar(200)", nullable: false),
                    Tag = table.Column<string>(type: "varchar(30)", nullable: false),
                    ExtraAttributes = table.Column<string>(type: "jsonb", nullable: true),
                    Color = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enchantment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<short>(type: "SMALLINT", maxLength: 3, nullable: false),
                    Level = table.Column<byte>(type: "SMALLINT", maxLength: 3, nullable: false),
                    ItemType = table.Column<int>(type: "INT", maxLength: 9, nullable: false),
                    SaveAuctionId = table.Column<int>(type: "integer", nullable: false),
                    DbItemId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enchantment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enchantment_DBItems_DbItemId",
                        column: x => x.DbItemId,
                        principalTable: "DBItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NBTLookup",
                columns: table => new
                {
                    AuctionId = table.Column<int>(type: "integer", nullable: false),
                    KeyId = table.Column<short>(type: "smallint", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NBTLookup", x => new { x.AuctionId, x.KeyId });
                    table.ForeignKey(
                        name: "FK_NBTLookup_DBItems_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "DBItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerUuid = table.Column<string>(type: "varchar(200)", nullable: false),
                    BuyerUuid = table.Column<string>(type: "varchar(200)", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeRequests_DBItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "DBItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WantedItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Filters = table.Column<string>(type: "jsonb", nullable: false),
                    DbTradeRequestId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WantedItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WantedItem_TradeRequests_DbTradeRequestId",
                        column: x => x.DbTradeRequestId,
                        principalTable: "TradeRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enchantment_DbItemId",
                table: "Enchantment",
                column: "DbItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Enchantment_ItemType_Type_Level",
                table: "Enchantment",
                columns: new[] { "ItemType", "Type", "Level" });

            migrationBuilder.CreateIndex(
                name: "IX_NBTLookup_KeyId_Value",
                table: "NBTLookup",
                columns: new[] { "KeyId", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeRequests_ItemId",
                table: "TradeRequests",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WantedItem_DbTradeRequestId",
                table: "WantedItem",
                column: "DbTradeRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enchantment");

            migrationBuilder.DropTable(
                name: "NBTLookup");

            migrationBuilder.DropTable(
                name: "WantedItem");

            migrationBuilder.DropTable(
                name: "TradeRequests");

            migrationBuilder.DropTable(
                name: "DBItems");
        }
    }
}
