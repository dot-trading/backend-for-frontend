using TradingProject.Bff.Client.Models.Dtos;
using TradingProject.Persistence.Api.Stubs.Models;
using TradingProject.Persistence.Api.Stubs.V1;
using TradingProject.ThirdParty.Client.Models.Responses;
using TradingProject.ThirdParty.Client.Services;

namespace Bff.Web.Services;

/// <summary>
/// Aggregates data from ThirdParty (Fear &amp; Greed) and Persistence (trades/positions)
/// to build enriched notification DTOs consumed by the telegram-bot via the BFF API.
/// </summary>
public class NotificationAggregationService : INotificationAggregationService
{
    private readonly ITradesApi _tradesApi;
    private readonly IThirdPartyApiClient _thirdParty;

    public NotificationAggregationService(ITradesApi tradesApi, IThirdPartyApiClient thirdParty)
    {
        _tradesApi = tradesApi;
        _thirdParty = thirdParty;
    }

    /// <summary>
    /// Builds a <see cref="ServiceStartNotificationDto"/> by aggregating data from
    /// Persistence (open positions count, P&amp;L) and ThirdParty (Fear &amp; Greed).
    /// </summary>
    public async Task<ServiceStartNotificationDto> GetServiceStartContextAsync(
        string quoteAsset,
        string serviceName,
        CancellationToken ct = default)
    {
        // Parallel calls to minimise latency
        var fearAndGreedTask = GetFearAndGreedSafeAsync(ct);
        var tradesPagingTask = GetOpenTradesSafeAsync(ct);

        await Task.WhenAll(fearAndGreedTask, tradesPagingTask);

        var fearAndGreed = await fearAndGreedTask;
        var tradesPaging = await tradesPagingTask;

        // Compute P&L and position count from trades
        double totalPnl = 0;
        double dailyPnl = 0;
        int openCount = 0;

        if (tradesPaging?.Payload is { } trades)
        {
            openCount = trades.Length;
            foreach (var t in trades)
            {
                totalPnl += t.Pnl.GetValueOrDefault();
                if (t.CloseAt.HasValue && t.CloseAt.Value.Date == DateTime.UtcNow.Date)
                    dailyPnl += t.Pnl.GetValueOrDefault();
            }
        }

        return new ServiceStartNotificationDto
        {
            QuoteAsset = quoteAsset,
            ServiceName = serviceName,
            Timestamp = DateTime.UtcNow,
            OpenPositionsCount = openCount,
            DailyPnl = dailyPnl,
            TotalPnl = totalPnl,
            FearAndGreedIndex = fearAndGreed?.Value ?? 0,
            FearAndGreedLabel = fearAndGreed?.Classification ?? "Unknown",
        };
    }

    /// <summary>
    /// Builds a <see cref="MarketStressNotificationDto"/> by aggregating data from
    /// ThirdParty and Persistence, reusing the existing market-stress logic.
    /// </summary>
    public async Task<MarketStressNotificationDto> GetMarketStressContextAsync(
        string quoteAsset,
        int stressValue,
        CancellationToken ct = default)
    {
        var fearAndGreedTask = GetFearAndGreedSafeAsync(ct);
        var tradesPagingTask = GetOpenTradesSafeAsync(ct);

        await Task.WhenAll(fearAndGreedTask, tradesPagingTask);

        var fearAndGreed = await fearAndGreedTask;
        var tradesPaging = await tradesPagingTask;

        double totalPnl = 0;
        double dailyPnl = 0;
        int openCount = 0;

        if (tradesPaging?.Payload is { } trades)
        {
            openCount = trades.Length;
            foreach (var t in trades)
            {
                totalPnl += t.Pnl.GetValueOrDefault();
                if (t.CloseAt.HasValue && t.CloseAt.Value.Date == DateTime.UtcNow.Date)
                    dailyPnl += t.Pnl.GetValueOrDefault();
            }
        }

        return new MarketStressNotificationDto
        {
            QuoteAsset = quoteAsset,
            StressValue = stressValue,
            Timestamp = DateTime.UtcNow,
            OpenPositionsCount = openCount,
            DailyPnl = dailyPnl,
            TotalPnl = totalPnl,
            FearAndGreedIndex = fearAndGreed?.Value ?? 0,
            FearAndGreedLabel = fearAndGreed?.Classification ?? "Unknown",
        };
    }

    /// <summary>
    /// Retrieves Fear &amp; Greed with safe fallback (returns null on failure).
    /// </summary>
    private async Task<FearAndGreedResponse?> GetFearAndGreedSafeAsync(CancellationToken ct)
    {
        try
        {
            return await _thirdParty.GetFearAndGreedAsync(ct);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Retrieves open trades with safe fallback (returns null on failure).
    /// </summary>
    private async Task<PagingList<TradeResponse>?> GetOpenTradesSafeAsync(CancellationToken ct)
    {
        try
        {
            return await _tradesApi.GetTradesAsync(limit: 200, page: 1, status: "open", cancellationToken: ct);
        }
        catch
        {
            return null;
        }
    }
}
