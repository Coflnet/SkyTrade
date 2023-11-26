using Coflnet.Sky.Core;

namespace SkyTrade.Models
{
    public class TradeRequestDTO
    {
        public long? Id { get; set; }
        public string UserId { get; set; }
        public string PlayerUuid { get; set; }
        public string? BuyerUuid { get; set; }

        public Item Item { get; set; }
        public long? Coins { get; set; }

        public List<WantedItem> WantedItems { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class WantedItem
    {
        public long? Id { get; set; }
        public string Tag { get; set; }
        public Dictionary<string, string> Filters { get; set; }
    }
}
