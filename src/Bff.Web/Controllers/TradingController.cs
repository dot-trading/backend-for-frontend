using Bff.Application.Features.Pnl.Queries;
using Bff.Application.Features.Positions.Queries;
using Bff.Application.Features.Trades.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

public class TradingController : ApiControllerBase
{
    [HttpGet("pnl")]
    public async Task<IActionResult> GetPnl([FromQuery] string? quoteAsset)
    {
        return Ok(await Mediator.Send(new GetPnlSummaryQuery(quoteAsset)));
    }

    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions()
    {
        return Ok(await Mediator.Send(new GetOpenPositionsQuery()));
    }

    [HttpGet("trades")]
    public async Task<IActionResult> GetTrades([FromQuery] int limit = 5)
    {
        return Ok(await Mediator.Send(new GetLastTradesQuery(limit)));
    }
}
