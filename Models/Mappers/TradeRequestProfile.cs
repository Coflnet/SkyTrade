﻿using AutoMapper;
using Coflnet.Sky.Core;

namespace SkyTrade.Models.Mappers
{
    public class TradeRequestProfile : Profile
    {
        public TradeRequestProfile()
        {
            _ = CreateMap<Item, DbItem>()
                .ForMember(dest => dest.Enchantments, opt => opt.MapFrom(src => MapDictToEnchantment(src.Enchantments)))
                .ForMember(dest => dest.NBTLookup, opt => opt.MapFrom(src => NBT.CreateLookup(src.Tag, src.ExtraAttributes, null)))
                .ReverseMap()
                .ForMember(dest => dest.Enchantments, opt => opt.MapFrom(src => MapEnchantmentToDict(src.Enchantments)))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count > 64 ? 1 : src.Count));

            CreateMap<TradeRequestDTO, DbTradeRequest>()
                //.ForMember(dest => dest.Item.Count, opt => opt.MapFrom(src => src.Coins == 0 ? src.Item.Count : src.Coins))
                .AfterMap((src, dest) =>
                {
                    if (dest.Item != null)
                        dest.Item.Count = (src.Coins ?? 0) == 0 ? src.Item.Count : src.Coins ?? 0;
                })
                .ReverseMap()
                .ForMember(dest => dest.Coins, opt => opt.MapFrom(src => src.Item.Count));
        }

        private List<Enchantment> MapDictToEnchantment(Dictionary<string, byte>? enchantment)
        {
            return enchantment == null
                ? new List<Enchantment>()
                : enchantment!.Select(e => new Enchantment()
                {
                    Type = Enum.Parse<Enchantment.EnchantmentType>(e.Key),
                    Level = (byte)e.Value
                }).ToList();
        }

        private Dictionary<string, byte>? MapEnchantmentToDict(List<Enchantment> enchantment)
        {
            return enchantment.ToDictionary(e => e.Type.ToString(), e => e.Level);
        }
    }
}
