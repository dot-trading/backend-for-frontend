using Bff.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/market-data")]
public class MarketDataController(IThirdPartyService thirdParty) : ControllerBase
{
    [HttpGet("price/{symbol}")]
    public async Task<IActionResult> GetPrice(
        string symbol,
        CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetPriceAsync(symbol, cancellationToken));

    [HttpGet("notional/{symbol}")]
    public async Task<IActionResult> GetMinNotional(
        string symbol,
        CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetMinNotionalAsync(symbol, cancellationToken));

    [HttpGet("klines/{symbol}")]
    public async Task<IActionResult> GetKlines(
        string symbol,
        [FromQuery] string interval = "1h",
        [FromQuery] int limit = 24,
        CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetKlinesAsync(symbol, interval, limit, cancellationToken));

    [HttpGet("ticker/{symbol}")]
    public async Task<IActionResult> GetTicker24h(
        string symbol,
        CancellationToken cancellationToken = default)
    {
        var result = await thirdParty.GetTicker24hAsync(symbol, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("sentiment/fear-and-greed")]
    public async Task<IActionResult> GetFearAndGreed(
        CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetFearAndGreedAsync(cancellationToken));

    [HttpGet("price/coingecko/{coinId}")]
    public async Task<IActionResult> GetCoinGeckoPrice(
        string coinId,
        [FromQuery] string vsCurrency = "usd",
        CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetCoinGeckoPriceAsync(coinId, vsCurrency, cancellationToken));
}
