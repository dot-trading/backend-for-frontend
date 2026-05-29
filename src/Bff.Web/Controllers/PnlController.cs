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
        var paging = await tradesApi.GetTradesAsync(limit: 10000, status: "closed", page: 1, cancellationToken: ct);
        var trades = paging.Payload;

        var today = DateTime.UtcNow.Date;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var startOfWeek = today.AddDays(-diff);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);

        double totalPnl = 0;
        double dailyPnl = 0;
        double weeklyPnl = 0;
        double monthlyPnl = 0;

        foreach (var t in trades)
        {
            if (!string.IsNullOrEmpty(quoteAsset) && !string.Equals(t.QuoteAsset, quoteAsset, StringComparison.OrdinalIgnoreCase))
                continue;

            var pnl = t.Pnl.GetValueOrDefault();
            totalPnl += pnl;

            if (t.CloseAt.HasValue)
            {
                var closeDate = t.CloseAt.Value.Date;
                if (closeDate == today)
                    dailyPnl += pnl;
                if (closeDate >= startOfWeek)
                    weeklyPnl += pnl;
                if (closeDate >= startOfMonth)
                    monthlyPnl += pnl;
            }
        }

        return Ok(new
        {
            Today = new { Value = dailyPnl },
            ThisWeek = new { Value = weeklyPnl },
            ThisMonth = new { Value = monthlyPnl },
            Total = new { Value = totalPnl }
        });
    }
}
