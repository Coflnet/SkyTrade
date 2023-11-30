using AutoMapper;
using Coflnet.Sky.Core;
using Coflnet.Sky.Filter;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SkyTrade.Models;

namespace SkyTrade.Services;
public interface IDBService
{
    Task DeleteTrade(string userId, long id);
    Task<IEnumerable<DbTradeRequest>> GetDbItems(int pageSize, int page);
    Task<IEnumerable<DbTradeRequest>> GetDbItemsByFilters(Dictionary<string, string> filters, int max, int page);
    Task<IEnumerable<DbTradeRequest>> GetDbItemsByUser(string userId, int max, int page);
    Task<int> InsertDbItem(TradeRequestDTO tradeRequestDTO);
    Task InsertDbItems(TradeRequestDTO[] tradeRequestDTOs);
}

public class DBService : IDBService
{
    private readonly TradeRequestDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly FilterEngine _filterEngine;
    private readonly ILogger<DBService> logger;

    public DBService(TradeRequestDBContext dbContext, IMapper mapper, FilterEngine filterEngine, ILogger<DBService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _filterEngine = filterEngine;
        this.logger = logger;
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

        var baseSelect = _dbContext.TradeRequests
                .Where(t => selection.Contains(t.Item.Id))
                .OrderByDescending(t => t.Id)
           ;
        return await ExecuteSelect(pageSize, page, baseSelect);
    }

    private static async Task<IEnumerable<DbTradeRequest>> ExecuteSelect(int pageSize, int page, IQueryable<DbTradeRequest> baseSelect)
    {
        return await baseSelect.Include(t => t.Item)
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
        ValidateFilters(dbTradeRequest);
        if (dbTradeRequest.Item.Id == 0)
            dbTradeRequest.Item.Id = null;

        foreach (var item in dbTradeRequest.WantedItems)
        {
            if (item.Id == 0)
                item.Id = null;
        }
        dbTradeRequest.BuyerUuid = string.Empty;
        Console.WriteLine(JsonConvert.SerializeObject(dbTradeRequest, Formatting.Indented));
        _dbContext.Add(dbTradeRequest);
        return await _dbContext.SaveChangesAsync();
    }

    private void ValidateFilters(DbTradeRequest dbTradeRequest)
    {
        foreach (var group in dbTradeRequest.WantedItems)
        {
            var unsupported = new List<string>();
            var args = new FilterArgs(group.Filters, true, _filterEngine);
            foreach (var filter in group.Filters)
            {
                try
                {
                    var expression = _filterEngine.GetExpressions(new Dictionary<string, string>(){
                            {filter.Key,filter.Value}
                        }).First();
                    expression.Compile().Invoke(dbTradeRequest.Item);
                }
                catch (System.Exception e)
                {
                    logger.LogError(e, $"Error while executing filter {filter.Key} with value {filter.Value}");
                    unsupported.Add(filter.Key);
                }
            }
            if (unsupported.Count > 1)
                throw new CoflnetException("invalid_filter", $"The filters {string.Join(',', unsupported)} are not supported for trades right now");
            if (unsupported.Count == 1)
                throw new CoflnetException("invalid_filter", $"The filter {unsupported[0]} is not supported for trades right now");

        }
    }

    public async Task<IEnumerable<DbTradeRequest>> GetDbItemsByUser(string userId, int max, int page)
    {
        var baseSelect = _dbContext.TradeRequests.Where(t => t.UserId == userId);
        return await ExecuteSelect(max, page, baseSelect);
    }


    public async Task DeleteTrade(string userId, long id)
    {
        DbTradeRequest? dbTradeRequest = await _dbContext.TradeRequests.Where(t => t.Id == id).Include(t => t.WantedItems).Include(t => t.Item).FirstOrDefaultAsync();
        if (dbTradeRequest == null)
            return;
        if (dbTradeRequest.UserId != userId && !string.IsNullOrEmpty(dbTradeRequest.UserId))
            throw new CoflnetException("not_allowed", "You are not allowed to delete this trade because you didn't create it");
        _dbContext.TradeRequests.Remove(dbTradeRequest);
        await _dbContext.SaveChangesAsync();
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
