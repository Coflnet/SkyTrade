using Coflnet.Sky.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
//using System.Text.Json;
using static Grpc.Core.Metadata;

namespace SkyTrade.Models
{
    public class TradeRequestDBContext : DbContext
    {
        public TradeRequestDBContext(DbContextOptions<TradeRequestDBContext> options)
            : base(options)
        {
        }

        public DbSet<DbTradeRequest> TradeRequests { get; set; }
        public DbSet<DbItem> DBItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<NBTLookup>(entity =>
            {
                entity.HasKey(e => new { e.AuctionId, e.KeyId });
                entity.HasIndex(e => new { e.KeyId, e.Value });
            });

            modelBuilder.Entity<DbItem>(entity =>
            {
                entity.HasMany(e => e.NBTLookup).WithOne().HasForeignKey("AuctionId");
                entity.Property(e => e.ExtraAttributes).HasJsonConversionJSoft();
            });

            modelBuilder.Entity<WantedItem>()
                .Property(e => e.Filters)
                .HasJsonConversionJSoft();

            modelBuilder.Entity<Enchantment>(entity =>
            {
                entity.Property(e => e.Type).HasColumnType("SMALLINT").HasMaxLength(3);
                entity.Property(e => e.Level).HasColumnType("SMALLINT").HasMaxLength(3);
                entity.Property(e => e.ItemType).HasColumnType("INT").HasMaxLength(9);
                entity.HasIndex(e => new { e.ItemType, e.Type, e.Level });
            });
        }
    }
    public static class ValueConversionExtensions
    {
        /*public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?) null),
                v => JsonSerializer.Deserialize<T>(v, (JsonSerializerOptions?) null) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(r, (JsonSerializerOptions?) null),
                v => v == null ? 0 : JsonSerializer.Serialize(v, (JsonSerializerOptions?) null).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, (JsonSerializerOptions?) null), (JsonSerializerOptions?) null)
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }*/

        public static PropertyBuilder<T> HasJsonConversionJSoft<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
                v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
                v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }
    }
}

