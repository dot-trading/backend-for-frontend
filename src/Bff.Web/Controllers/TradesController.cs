using Microsoft.AspNetCore.Mvc;
using TradingProject.Persistence.Api.Stubs.Models;
using TradingProject.Persistence.Api.Stubs.V1;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/trades")]
public class TradesController(ITradesApi tradesApi) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTrades(
        [FromQuery] int limit = 50, [FromQuery] int page = 1,
        [FromQuery] string? status = null, [FromQuery] string? symbol = null,
        CancellationToken ct = default)
        => Ok(await tradesApi.GetTradesAsync(limit, page, status, symbol, ct));

    [HttpPost]
    public async Task<IActionResult> CreateTrade([FromBody] CreateTradeRequest request, CancellationToken ct)
    {
        var trade = await tradesApi.CreateTradeAsync(request, ct);
        return CreatedAtAction(nameof(GetTrades), trade);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTrade(int id, [FromBody] UpdateTradeRequest request, CancellationToken ct)
    {
        var trade = await tradesApi.UpdateTradeAsync(id, request, ct);
        return trade is null ? NotFound() : Ok(trade);
    }
}
