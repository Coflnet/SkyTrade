using System.Collections.Concurrent;
using System.Reflection;
using AutoMapper;
using Coflnet.Sky.Core;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace SkyTrade.Models.Mappers
{
    public class TradeRequestProfileTests
    {
        private const short HotPotatoKeyId = 7;
        private IMapper mapper = null!;
        private AutoMapper.IConfigurationProvider configuration = null!;

        [SetUp]
        public void Setup()
        {
            // NBT resolves attribute keys through a static cache that is filled from the database on first
            // use. Seeding it (it is private, hence reflection) keeps the test free of any infrastructure.
            var cache = (ConcurrentDictionary<string, short>)typeof(NBT)
                .GetField("Cache", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;
            cache["hot_potato_count"] = HotPotatoKeyId;

            // the smallest NBT the shared library allows: ItemDetails without an items client, which
            // is only consulted for attributes that reference other items
            var services = new ServiceCollection();
            services.AddSingleton(new NBT(new ItemDetails(null)));
            services.AddAutoMapper(typeof(TradeRequestProfile));
            var provider = services.BuildServiceProvider();
            mapper = provider.GetRequiredService<IMapper>();
            configuration = provider.GetRequiredService<AutoMapper.IConfigurationProvider>();
        }

        [Test]
        public void ItemExtraAttributesMapToNbtLookup()
        {
            var item = new Item
            {
                Tag = "HYPERION",
                ItemName = "Hyperion",
                Count = 1,
                ExtraAttributes = new Dictionary<string, object> { { "hot_potato_count", 5 } }
            };

            var request = mapper.Map<DbTradeRequest>(new TradeRequestDTO { PlayerUuid = "p", Item = item, Timestamp = DateTime.UtcNow });

            var lookup = request.Item!.NBTLookup.ToList();
            Assert.That(lookup, Has.Count.EqualTo(1));
            Assert.That(lookup[0].KeyId, Is.EqualTo(HotPotatoKeyId));
            Assert.That(lookup[0].Value, Is.EqualTo(5));
        }

        [Test]
        public void ConfigurationIsValid()
        {
            Assert.DoesNotThrow(() => configuration.AssertConfigurationIsValid());
        }
    }
}
