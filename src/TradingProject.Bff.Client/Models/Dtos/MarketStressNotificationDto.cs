namespace TradingProject.Bff.Client.Models.Dtos;

/// <summary>
/// Enriched context for a market stress alert notification.
/// The BFF aggregates Fear &amp; Greed, open positions and P&amp;L data so the telegram-bot
/// can build the alert message without multiple backend calls.
/// </summary>
public class MarketStressNotificationDto
{
    /// <summary>The quote asset under stress (e.g. "USDC", "EUR").</summary>
    public string QuoteAsset { get; set; } = string.Empty;

    /// <summary>The stress level value reported by the orchestrator.</summary>
    public int StressValue { get; set; }

    /// <summary>UTC timestamp of the notification.</summary>
    public DateTime Timestamp { get; set; }

    // ── Aggregated from ThirdParty ───────────────────────────────────

    /// <summary>Fear &amp; Greed Index (0-100).</summary>
    public int FearAndGreedIndex { get; set; }

    /// <summary>Fear &amp; Greed classification label.</summary>
    public string FearAndGreedLabel { get; set; } = string.Empty;

    // ── Aggregated from Persistence ──────────────────────────────────

    /// <summary>Number of currently open positions.</summary>
    public int OpenPositionsCount { get; set; }

    /// <summary>Daily P&amp;L for the quote asset.</summary>
    public double DailyPnl { get; set; }

    /// <summary>Total P&amp;L for the quote asset.</summary>
    public double TotalPnl { get; set; }
}
