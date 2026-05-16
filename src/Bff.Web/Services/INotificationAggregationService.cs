using TradingProject.Bff.Client.Models.Dtos;

namespace Bff.Web.Services;

/// <summary>
/// Service interface for aggregating notification context data from ThirdParty and Persistence.
/// </summary>
public interface INotificationAggregationService
{
    /// <summary>
    /// Builds enriched context for a service-start notification.
    /// </summary>
    Task<ServiceStartNotificationDto> GetServiceStartContextAsync(
        string quoteAsset,
        string serviceName,
        CancellationToken ct = default);

    /// <summary>
    /// Builds enriched context for a market-stress notification.
    /// </summary>
    Task<MarketStressNotificationDto> GetMarketStressContextAsync(
        string quoteAsset,
        int stressValue,
        CancellationToken ct = default);
}
