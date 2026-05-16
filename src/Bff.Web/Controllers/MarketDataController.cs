using Microsoft.AspNetCore.Mvc;
using TradingProject.ThirdParty.Client.Models.Responses;
using TradingProject.ThirdParty.Client.Services;

namespace Bff.Web.Controllers;

[ApiController]
[Route("api/market-data")]
public class MarketDataController(IThirdPartyApiClient thirdParty) : ControllerBase
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
        => Ok(await thirdParty.GetTicker24hAsync(symbol, cancellationToken));

    [HttpGet("tickers")]
    public async Task<IActionResult> Get24hTickers(CancellationToken cancellationToken = default)
        => Ok(await thirdParty.Get24hTickersAsync(cancellationToken));

    [HttpGet("sentiment/fear-and-greed")]
    public async Task<IActionResult> GetFearAndGreed(
        CancellationToken cancellationToken = default)
        => Ok(await thirdParty.GetFearAndGreedAsync(cancellationToken));
}
