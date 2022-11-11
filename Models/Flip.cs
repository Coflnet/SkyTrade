
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Coflnet.Sky.Trade.Models;
[DataContract]
public class TradeRequest
{
    [IgnoreDataMember]
    [JsonIgnore]
    public int Id { get; set; }
    /// <summary>
    /// All list of things the user would like for the item
    /// </summary>
    public List<Filter> Want { get; set; }

    public DateTime Timestamp { get; set; }
}

public class Filter
{
    /// <summary>
    /// Filters narrowing down the item
    /// </summary>
    public Dictionary<string, string> Elements { get; set; }
    /// <summary>
    /// How important this requirement is
    /// </summary>
    public int Importance;
}