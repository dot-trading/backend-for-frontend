using Bff.Application.Features.Ollama.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

public class ClusterController : ApiControllerBase
{
    [HttpGet("models")]
    public async Task<IActionResult> GetModels()
    {
        return Ok(await Mediator.Send(new GetOllamaModelsQuery()));
    }
}
