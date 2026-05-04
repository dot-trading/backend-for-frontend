using Bff.Application.Features.Stats.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

public class StatsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStats([FromQuery] string? quoteAsset)
    {
        return Ok(await Mediator.Send(new GetStatsQuery(quoteAsset)));
    }
}
