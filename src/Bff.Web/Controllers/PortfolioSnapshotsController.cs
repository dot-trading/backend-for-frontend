using Microsoft.AspNetCore.Mvc;
using TradingProject.Persistence.Api.Stubs.Models;
using TradingProject.Persistence.Api.Stubs.V1;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/portfolio-snapshots")]
public class PortfolioSnapshotsController(IPortfolioSnapshotsApi snapshotsApi) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPortfolioSnapshots(
        [FromQuery] int limit = 50, [FromQuery] int page = 1,
        CancellationToken ct = default)
        => Ok(await snapshotsApi.GetPortfolioSnapshotsAsync(limit, page, ct));

    [HttpPost]
    public async Task<IActionResult> CreatePortfolioSnapshot([FromBody] CreatePortfolioSnapshotRequest request, CancellationToken ct)
    {
        var snapshot = await snapshotsApi.CreatePortfolioSnapshotAsync(request, ct);
        return CreatedAtAction(nameof(GetPortfolioSnapshots), snapshot);
    }
}
