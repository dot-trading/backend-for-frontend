using Bff.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/pnl")]
public class PnlController(IPersistenceService persistence) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] string? quoteAsset, CancellationToken ct)
        => Ok(await persistence.GetPnlSummaryAsync(quoteAsset, ct));
}
