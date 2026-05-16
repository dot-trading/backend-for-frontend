using Microsoft.AspNetCore.Mvc;
using TradingProject.Persistence.Api.Stubs.Models;
using TradingProject.Persistence.Api.Stubs.V1;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/opportunities")]
public class OpportunitiesController(IOpportunitiesApi opportunitiesApi) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOpportunities(
        [FromQuery] int limit = 50, [FromQuery] int page = 1,
        [FromQuery] string? symbol = null, [FromQuery] bool? isApproved = null,
        CancellationToken ct = default)
        => Ok(await opportunitiesApi.GetOpportunitiesAsync(limit, page, symbol, isApproved, ct));

    [HttpPost]
    public async Task<IActionResult> CreateOpportunity([FromBody] CreateOpportunityRequest request, CancellationToken ct)
    {
        var opportunity = await opportunitiesApi.CreateOpportunityAsync(request, ct);
        return CreatedAtAction(nameof(GetOpportunities), opportunity);
    }
}
