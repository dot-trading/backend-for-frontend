using Bff.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(IThirdPartyService thirdParty) : ControllerBase
{
    [HttpGet("balances")]
    public async Task<IActionResult> GetBalances(CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetBalancesAsync(cancellationToken));
}
