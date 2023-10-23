using Microsoft.AspNetCore.Mvc;
using SkyTrade.Models;
using SkyTrade.Services;
using AutoMapper;
using Coflnet.Sky.Filter;

namespace SkyTrade.Controllers
{
    [ApiController]
    [Route("api/Trades")]
    public class TradeController
    {
        private readonly IDBService _dbService;
        private readonly IMapper _mapper;

        public TradeController(IDBService dbService, IMapper mapper)
        {
            _dbService = dbService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetTrades")]
        public async Task<IEnumerable<TradeRequestDTO>> GetMax(int max = 20, int page = 0)
        {
            //TODO Validate parameters
            IEnumerable<DbTradeRequest> dbTradeRequests = await _dbService.GetDbItems(max, page);
            return _mapper.Map<IEnumerable<TradeRequestDTO>>(dbTradeRequests);
        }

        [HttpGet]
        [Route("GetTradesByFilters")]
        public async Task<IEnumerable<TradeRequestDTO>> GetByFilters([FromQuery] Dictionary<string, string> filters, int max = 20, int page = 0)
        {
            if (filters.ContainsKey("max"))
                filters.Remove("max");

            if(filters.ContainsKey("page"))
                filters.Remove("page");
            //TODO Validate parameters
            IEnumerable<DbTradeRequest> dbTradeRequests = await _dbService.GetDbItemsByFilters(filters, max, page);
            return _mapper.Map<IEnumerable<TradeRequestDTO>>(dbTradeRequests);
        }

        [HttpPost]
        [Route("InsertTrades")]
        public async Task Insert(TradeRequestDTO[] tradeRequestDTO)
        {
            //TODO Validate parameters
            await _dbService.InsertDbItems(tradeRequestDTO);
        }
    }
}
