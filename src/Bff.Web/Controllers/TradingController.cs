using Microsoft.AspNetCore.Mvc;
using TradingProject.ThirdParty.Client.Models.Responses;
using TradingProject.ThirdParty.Client.Services;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/trading")]
public class TradingController(IThirdPartyApiClient thirdParty) : ControllerBase
{
    [HttpPost("order/buy")]
    public async Task<IActionResult> PlaceMarketBuy([FromBody] PlaceMarketBuyRequest request, CancellationToken ct)
        => Ok(await thirdParty.PlaceMarketBuyAsync(request, ct));

    [HttpPost("order/sell")]
    public async Task<IActionResult> PlaceMarketSell([FromBody] PlaceMarketSellRequest request, CancellationToken ct)
        => Ok(await thirdParty.PlaceMarketSellAsync(request, ct));

    [HttpGet("pnl")]
    public IActionResult TestPnl() => Ok(new { Daily = 1.0, Weekly = 2.0, Monthly = 3.0, Total = 4.0 });
}
