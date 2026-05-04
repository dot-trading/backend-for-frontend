using Bff.Domain.Abstractions;
using Bff.Domain.Models.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/portfolio-snapshots")]
public class PortfolioSnapshotsController(IPersistenceService persistence) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPortfolioSnapshots(
        [FromQuery] int limit = 50, [FromQuery] int page = 1,
        CancellationToken ct = default)
        => Ok(await persistence.GetPortfolioSnapshotsAsync(limit, page, ct));

    [HttpPost]
    public async Task<IActionResult> CreatePortfolioSnapshot([FromBody] CreatePortfolioSnapshotRequest request, CancellationToken ct)
    {
        var snapshot = await persistence.CreatePortfolioSnapshotAsync(request, ct);
        return CreatedAtAction(nameof(GetPortfolioSnapshots), snapshot);
    }
}
