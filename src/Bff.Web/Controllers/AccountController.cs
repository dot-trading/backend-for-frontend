using Microsoft.AspNetCore.Mvc;
using TradingProject.ThirdParty.Client.Services;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(IThirdPartyApiClient thirdParty) : ControllerBase
{
    [HttpGet("balances")]
    public async Task<IActionResult> GetBalances(CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetBalancesAsync(cancellationToken));

    [HttpGet("pnl")]
    public IActionResult TestPnl() => Ok(new { Daily = 1.0, Weekly = 2.0, Monthly = 3.0, Total = 4.0 });
}
