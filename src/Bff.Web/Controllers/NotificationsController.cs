using Bff.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Web.Controllers;

/// <summary>
/// Fournit des données enrichies destinées aux notifications du telegram-bot.
/// Agrège des informations provenant de plusieurs services (persistence, third-party)
/// afin que l'orchestrateur reste léger (il n'envoie que les paramètres bruts).
/// </summary>
[ApiController]
[Route("api/notifications")]
public class NotificationsController(
    IPersistenceService persistence,
    IThirdPartyService thirdParty) : ControllerBase
{
    /// <summary>
    /// Retourne le contexte enrichi pour une alerte de stress de marché :
    /// Fear &amp; Greed, nombre de positions ouvertes, et P&amp;L du quoteAsset concerné.
    /// </summary>
    [HttpGet("market-stress-context")]
    public async Task<IActionResult> GetMarketStressContext(
        [FromQuery] string quoteAsset,
        CancellationToken ct = default)
    {
        // Appels parallèles pour minimiser la latence
        var fearAndGreedTask  = thirdParty.GetFearAndGreedAsync(ct);
        var tradesTask        = persistence.GetTradesAsync(limit: 200, page: 1, status: "open", ct: ct);
        var pnlTask           = persistence.GetPnlSummaryAsync(quoteAsset, ct);

        await Task.WhenAll(fearAndGreedTask, tradesTask, pnlTask);

        var fearAndGreed = await fearAndGreedTask;
        var trades       = await tradesTask;
        var pnl          = await pnlTask;

        var result = new
        {
            FearAndGreedIndex  = fearAndGreed.Value,
            FearAndGreedLabel  = fearAndGreed.Classification,
            OpenPositionsCount = trades.Count,
            DailyPnl           = pnl.Today.Value,
            TotalPnl           = pnl.Total.Value,
        };

        return Ok(result);
    }
}
