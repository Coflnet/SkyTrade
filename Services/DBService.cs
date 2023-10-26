﻿using AutoMapper;
using Coflnet.Sky.Core;
using Coflnet.Sky.Filter;
using Microsoft.EntityFrameworkCore;
using SkyTrade.Models;

namespace SkyTrade.Services;
public interface IDBService
{
    Task<IEnumerable<DbTradeRequest>> GetDbItems(int pageSize, int page);
    Task<IEnumerable<DbTradeRequest>> GetDbItemsByFilters(Dictionary<string, string> filters, int max, int page);
    Task<int> InsertDbItem(TradeRequestDTO tradeRequestDTO);
    Task InsertDbItems(TradeRequestDTO[] tradeRequestDTOs);
}

public class DBService : IDBService
{
    private readonly TradeRequestDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly FilterEngine _filterEngine;

    public DBService(TradeRequestDBContext dbContext, IMapper mapper, FilterEngine filterEngine)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _filterEngine = filterEngine;
    }

    public async Task<IEnumerable<DbTradeRequest>> GetDbItems(int pageSize, int page)
    {
        return await _dbContext.TradeRequests.OrderBy(t => t.Timestamp)
            .Include(t => t.Item)
            .Include(t => t.Item.Enchantments)
            .Include(t => t.Item.NBTLookup)
            .Include(t => t.WantedItems)
            .Paged(page, pageSize).ToListAsync();
    }

    public async Task<IEnumerable<DbTradeRequest>> GetDbItemsByFilters(Dictionary<string, string> filters, int pageSize, int page)
    {
        IQueryable<IDbItem> mainSelect = _dbContext.DBItems.Include(t => t.NBTLookup).AsQueryable();

        HashSet<int?> selection = _filterEngine.AddInterfaceFilters(mainSelect, filters).Cast<DbItem>().Select(item => item.Id).ToHashSet();

        return await _dbContext.TradeRequests
            .Include(t => t.Item)
                .Where(t => selection.Contains(t.Item.Id))
            .Include(t => t.Item.NBTLookup)
            .Include(t => t.Item.Enchantments)
            .Include(t => t.WantedItems)
            .Paged(page, pageSize)
            .ToListAsync();
    }

    public async Task InsertDbItems(TradeRequestDTO[] tradeRequestDTOs)
    {
        foreach (var item in tradeRequestDTOs)
        {
            await InsertDbItem(item);
        }
    }

    public async Task<int> InsertDbItem(TradeRequestDTO tradeRequestDTO)
    {
        DbTradeRequest dbTradeRequest = _mapper.Map<DbTradeRequest>(tradeRequestDTO);
        _dbContext.Add(dbTradeRequest);
        return await _dbContext.SaveChangesAsync();
    }
}
public static class QueryableExtensions
{
    public static IQueryable<T> Paged<T>(this IQueryable<T> source, int page,
                                                                    int pageSize)
    {
        return source
          .Skip(page * pageSize)
          .Take(pageSize);
    }
}
