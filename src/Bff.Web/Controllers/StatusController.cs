using Bff.Application.Features.Status.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

public class StatusController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStatus()
    {
        return Ok(await Mediator.Send(new GetStatusQuery()));
    }
}
