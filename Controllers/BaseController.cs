using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coflnet.Sky.Trade.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using Coflnet.Sky.Trade.Services;

namespace Coflnet.Sky.Trade.Controllers;

/// <summary>
/// Main Controller handling tracking
/// </summary>
[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly BaseService service;

    /// <summary>
    /// Creates a new instance of <see cref="TradeController"/>
    /// </summary>
    /// <param name="service"></param>
    public TradeController(BaseService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Tracks a flip
    /// </summary>
    /// <param name="flip"></param>
    /// <param name="AuctionId"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("flip/{AuctionId}")]
    public async Task<TradeRequest> TrackFlip([FromBody] TradeRequest flip, string AuctionId)
    {
        await service.AddFlip(flip);
        return flip;
    }
}
