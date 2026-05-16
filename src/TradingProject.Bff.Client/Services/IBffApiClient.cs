using TradingProject.Bff.Client.Models.Dtos;

namespace TradingProject.Bff.Client.Services;

/// <summary>
/// Typed client for consuming the BFF notification aggregation endpoints.
/// All methods target the <c>/api/notifications</c> route prefix.
/// </summary>
public interface IBffApiClient
{
    /// <summary>
    /// Retrieves the enriched context for a service-start notification.
    /// The BFF aggregates Fear &amp; Greed (ThirdParty) and open positions / P&amp;L (Persistence).
    /// <c>GET /api/notifications/service-start-context?quoteAsset=...</c>
    /// </summary>
    /// <param name="quoteAsset">The quote asset (e.g. "USDC", "EUR").</param>
    /// <param name="serviceName">The name of the started service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<ServiceStartNotificationDto?> GetServiceStartContextAsync(
        string quoteAsset,
        string serviceName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the enriched context for a market-stress notification.
    /// The BFF aggregates Fear &amp; Greed (ThirdParty) and open positions / P&amp;L (Persistence).
    /// <c>GET /api/notifications/market-stress-context?quoteAsset=...&amp;stressValue=...</c>
    /// </summary>
    /// <param name="quoteAsset">The quote asset under stress (e.g. "USDC", "EUR").</param>
    /// <param name="stressValue">The stress level reported by the orchestrator.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<MarketStressNotificationDto?> GetMarketStressContextAsync(
        string quoteAsset,
        int stressValue,
        CancellationToken cancellationToken = default);
}
