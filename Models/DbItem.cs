using Coflnet.Sky.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyTrade.Models
{
    public class DbItem : IDbItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ItemName { get; set; } = null!;

        [Column(TypeName = "varchar(30)")]
        public string Tag { get; set; } = null!;

        public Dictionary<string, object>? ExtraAttributes { get; set; }

        public List<Enchantment> Enchantments { get; set; } = new();

        public int? Color { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string? Description { get; set; }

        public long Count { get; set; }

        public IEnumerable<NBTLookup> NBTLookup { get; set; }
    }
}
