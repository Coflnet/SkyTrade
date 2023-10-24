using Coflnet.Sky.Core;
using Confluent.Kafka;
using NUnit.Framework;
using SkyTrade.Models;
using System.Text.Json;

namespace SkyTrade.Services
{
    public class DBServiceTest
    {
        [Test]
        public void DbShouldPersistItem()
        {
            Item item = GetMockItem("sword.bin");
            var trade = new TradeRequestDTO()
            {
                Id = 1,
                PlayerUuid = "osiddsfsf",
                Item = item,
                WantedItems = new() { new WantedItem { Filters = new() { { "HotPotatoCountFilter", "1-4" } } } },
                Timestamp = DateTime.UtcNow
            };
            var res = JsonSerializer.Serialize(trade);
            Console.WriteLine(res);
        }
        
        Item GetMockItem(string filename)
        {
            string MockObjects = "./MockObjects/";

            return MessagePack.MessagePackSerializer.Deserialize<Item>(File.ReadAllBytes(Path.Join(MockObjects, filename)));
        }
    }
}
