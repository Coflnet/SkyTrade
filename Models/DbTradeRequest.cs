using Coflnet.Sky.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyTrade.Models
{
    public class DbTradeRequest
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string PlayerUuid { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string BuyerUuid { get; set; }

        public DbItem Item { get; set; }

        public List<WantedItem> WantedItems { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
