using Microsoft.AspNetCore.Mvc;
using TradingProject.Bff.Client.Models.Dtos;
using Bff.Web.Services;

namespace Bff.Web.Controllers;

/// <summary>
/// Provides enriched data for telegram-bot notifications.
/// Aggregates information from Persistence and ThirdParty services
/// so the orchestrator stays lightweight (it only sends raw parameters).
///
/// The telegram-bot is the sole consumer: it calls these endpoints to obtain
/// typed DTOs and builds the final human-readable messages.
/// </summary>
[ApiController]
[Route("api/notifications")]
public class NotificationsController(
    INotificationAggregationService aggregation) : ControllerBase
{
    /// <summary>
    /// Returns enriched context for a service-start notification.
    /// Aggregates Fear &amp; Greed (ThirdParty) and open positions / P&amp;L (Persistence)
    /// for the given quote asset.
    /// </summary>
    /// <param name="quoteAsset">The quote asset (e.g. "USDC", "EUR").</param>
    /// <param name="serviceName">The name of the started service.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("service-start-context")]
    public async Task<ActionResult<ServiceStartNotificationDto>> GetServiceStartContext(
        [FromQuery] string quoteAsset,
        [FromQuery] string serviceName,
        CancellationToken ct = default)
    {
        var context = await aggregation.GetServiceStartContextAsync(quoteAsset, serviceName, ct);
        return Ok(context);
    }

    /// <summary>
    /// Returns enriched context for a market-stress alert.
    /// Aggregates Fear &amp; Greed (ThirdParty) and open positions / P&amp;L (Persistence)
    /// for the given quote asset.
    /// </summary>
    /// <param name="quoteAsset">The quote asset (e.g. "USDC", "EUR").</param>
    /// <param name="stressValue">The stress level reported by the orchestrator.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("market-stress-context")]
    public async Task<ActionResult<MarketStressNotificationDto>> GetMarketStressContext(
        [FromQuery] string quoteAsset,
        [FromQuery] int stressValue = 0,
        CancellationToken ct = default)
    {
        var context = await aggregation.GetMarketStressContextAsync(quoteAsset, stressValue, ct);
        return Ok(context);
    }
}
