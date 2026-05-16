using Microsoft.AspNetCore.Mvc;
using TradingProject.Persistence.Api.Stubs.V1;
using TradingProject.ThirdParty.Client.Services;

namespace Bff.Web.Controllers;

/// <summary>
/// Fournit des données enrichies destinées aux notifications du telegram-bot.
/// Agrège des informations provenant de plusieurs services (persistence, third-party)
/// afin que l'orchestrateur reste léger (il n'envoie que les paramètres bruts).
/// </summary>
[ApiController]
[Route("api/notifications")]
public class NotificationsController(
    ITradesApi tradesApi,
    IThirdPartyApiClient thirdParty) : ControllerBase
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
        var fearAndGreedTask = thirdParty.GetFearAndGreedAsync(ct);
        var tradesTask = tradesApi.GetTradesAsync(limit: 200, page: 1, status: "open", cancellationToken: ct);

        await Task.WhenAll(fearAndGreedTask, tradesTask);

        var fearAndGreed = await fearAndGreedTask;
        var paging = await tradesTask;
        var trades = paging.Payload;

        // Calcul du PnL à partir des trades
        double totalP = 0;
        double dailyP = 0;
        foreach (var t in trades)
        {
            totalP += t.Pnl.GetValueOrDefault();
            if (t.CloseAt.HasValue && t.CloseAt.Value.Date == DateTime.UtcNow.Date)
                dailyP += t.Pnl.GetValueOrDefault();
        }

        int openCount = 0;
        foreach (var _ in trades) openCount++;

        return Ok(new
        {
            FearAndGreedIndex = fearAndGreed!.Value,
            FearAndGreedLabel = fearAndGreed!.Classification,
            OpenPositionsCount = openCount,
            PnlDaily = dailyP,
            PnlTotal = totalP,
        });
    }
}
