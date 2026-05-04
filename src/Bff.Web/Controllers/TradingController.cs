using Bff.Domain.Abstractions;
using Bff.Domain.Models.ThirdParty;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/trading")]
public class TradingController(IThirdPartyService thirdParty) : ControllerBase
{
    [HttpPost("order/buy")]
    public async Task<IActionResult> PlaceMarketBuy([FromBody] PlaceMarketBuyRequest request, CancellationToken ct)
        => Ok(await thirdParty.PlaceMarketBuyAsync(request, ct));

    [HttpPost("order/sell")]
    public async Task<IActionResult> PlaceMarketSell([FromBody] PlaceMarketSellRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await thirdParty.PlaceMarketSellAsync(request, ct));
        }
        catch (HttpRequestException ex) when (ex.Message.StartsWith("Binance order failed"))
        {
            return BadRequest(ex.Message);
        }
    }
}
