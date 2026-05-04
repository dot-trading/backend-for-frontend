using Bff.Domain.Abstractions;
using Bff.Domain.Models.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/opportunities")]
public class OpportunitiesController(IPersistenceService persistence) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOpportunities(
        [FromQuery] int limit = 50, [FromQuery] int page = 1,
        [FromQuery] string? symbol = null, [FromQuery] bool? isApproved = null,
        CancellationToken ct = default)
        => Ok(await persistence.GetOpportunitiesAsync(limit, page, symbol, isApproved, ct));

    [HttpPost]
    public async Task<IActionResult> CreateOpportunity([FromBody] CreateOpportunityRequest request, CancellationToken ct)
    {
        var opportunity = await persistence.CreateOpportunityAsync(request, ct);
        return CreatedAtAction(nameof(GetOpportunities), opportunity);
    }
}
