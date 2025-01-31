﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SkyTrade.Models;

#nullable disable

namespace SkyTrade.Migrations
{
    [DbContext(typeof(TradeRequestDBContext))]
    [Migration("20250131174218_Dotnet8")]
    partial class Dotnet8
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Coflnet.Sky.Core.Enchantment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("DbItemId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemType")
                        .HasMaxLength(9)
                        .HasColumnType("INT");

                    b.Property<byte>("Level")
                        .HasMaxLength(3)
                        .HasColumnType("SMALLINT");

                    b.Property<int>("SaveAuctionId")
                        .HasColumnType("integer");

                    b.Property<short>("Type")
                        .HasMaxLength(3)
                        .HasColumnType("SMALLINT");

                    b.HasKey("Id");

                    b.HasIndex("DbItemId");

                    b.HasIndex("ItemType", "Type", "Level");

                    b.ToTable("Enchantment");
                });

            modelBuilder.Entity("Coflnet.Sky.Core.NBTLookup", b =>
                {
                    b.Property<int>("AuctionId")
                        .HasColumnType("integer");

                    b.Property<short>("KeyId")
                        .HasColumnType("smallint");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("AuctionId", "KeyId");

                    b.HasIndex("KeyId", "Value");

                    b.ToTable("NBTLookup");
                });

            modelBuilder.Entity("SkyTrade.Models.DbItem", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("Color")
                        .HasColumnType("integer");

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("ExtraAttributes")
                        .HasColumnType("jsonb");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(44)");

                    b.HasKey("Id");

                    b.ToTable("DBItems");
                });

            modelBuilder.Entity("SkyTrade.Models.DbTradeRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("BuyerUuid")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("PlayerUuid")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("TradeRequests");
                });

            modelBuilder.Entity("SkyTrade.Models.WantedItem", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long?>("Id"));

                    b.Property<long?>("DbTradeRequestId")
                        .HasColumnType("bigint");

                    b.Property<string>("Filters")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DbTradeRequestId");

                    b.ToTable("WantedItem");
                });

            modelBuilder.Entity("Coflnet.Sky.Core.Enchantment", b =>
                {
                    b.HasOne("SkyTrade.Models.DbItem", null)
                        .WithMany("Enchantments")
                        .HasForeignKey("DbItemId");
                });

            modelBuilder.Entity("Coflnet.Sky.Core.NBTLookup", b =>
                {
                    b.HasOne("SkyTrade.Models.DbItem", null)
                        .WithMany("NBTLookup")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SkyTrade.Models.DbTradeRequest", b =>
                {
                    b.HasOne("SkyTrade.Models.DbItem", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("SkyTrade.Models.WantedItem", b =>
                {
                    b.HasOne("SkyTrade.Models.DbTradeRequest", null)
                        .WithMany("WantedItems")
                        .HasForeignKey("DbTradeRequestId");
                });

            modelBuilder.Entity("SkyTrade.Models.DbItem", b =>
                {
                    b.Navigation("Enchantments");

                    b.Navigation("NBTLookup");
                });

            modelBuilder.Entity("SkyTrade.Models.DbTradeRequest", b =>
                {
                    b.Navigation("WantedItems");
                });
#pragma warning restore 612, 618
        }
    }
}
