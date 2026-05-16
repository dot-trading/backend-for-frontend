using Microsoft.AspNetCore.Mvc;
using TradingProject.Persistence.Api.Stubs.Models;
using TradingProject.Persistence.Api.Stubs.V1;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/pnl")]
public class PnlController(ITradesApi tradesApi) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] string? quoteAsset, CancellationToken ct)
    {
        var paging = await tradesApi.GetTradesAsync(limit: 500, page: 1, cancellationToken: ct);
        var trades = paging.Payload;

        double totalPnl = 0;
        double dailyPnl = 0;
        foreach (var t in trades)
        {
            totalPnl += t.Pnl.GetValueOrDefault();
            if (t.CloseAt.HasValue && t.CloseAt.Value.Date == DateTime.UtcNow.Date)
                dailyPnl += t.Pnl.GetValueOrDefault();
        }

        return Ok(new
        {
            Today = new { Value = dailyPnl },
            Total = new { Value = totalPnl },
        });
    }
}
