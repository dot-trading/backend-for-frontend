namespace TradingProject.Bff.Client.Models.Dtos;

/// <summary>
/// Enriched context for a "service started" notification.
/// The BFF aggregates data from Persistence (positions, P&amp;L) and ThirdParty (Fear &amp; Greed)
/// so the telegram-bot can build a rich message without multiple backend calls.
/// </summary>
public class ServiceStartNotificationDto
{
    /// <summary>The quote asset (e.g. "USDC", "EUR").</summary>
    public string QuoteAsset { get; set; } = string.Empty;

    /// <summary>Name of the service that started (e.g. "OrchestratorServiceWorker").</summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>UTC timestamp of the notification.</summary>
    public DateTime Timestamp { get; set; }

    // ── Aggregated from Persistence ──────────────────────────────────

    /// <summary>Number of currently open positions.</summary>
    public int OpenPositionsCount { get; set; }

    /// <summary>Daily P&amp;L for the quote asset.</summary>
    public double DailyPnl { get; set; }

    /// <summary>Total P&amp;L for the quote asset.</summary>
    public double TotalPnl { get; set; }

    // ── Aggregated from ThirdParty ───────────────────────────────────

    /// <summary>Fear &amp; Greed Index (0-100).</summary>
    public int FearAndGreedIndex { get; set; }

    /// <summary>Fear &amp; Greed classification label (e.g. "Extreme Fear", "Greed").</summary>
    public string FearAndGreedLabel { get; set; } = string.Empty;
}
