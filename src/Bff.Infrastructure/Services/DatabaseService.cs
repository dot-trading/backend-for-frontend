using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Bff.Domain.Abstractions;
using Bff.Infrastructure.Options;

namespace Bff.Infrastructure.Services;

public class DatabaseService(HttpClient httpClient, IOptions<PersistenceOptions> options) : IDatabaseService
{
    private readonly string _baseUrl = options.Value.BaseUrl.TrimEnd('/');

    public async Task<int> GetOpenPositionsCountAsync()
    {
        return await httpClient.GetFromJsonAsync<int>($"{_baseUrl}/api/TradingData/positions/open/count");
    }

    public async Task<double> GetDailyPnlAsync(string? quoteAsset = null)
    {
        var url = $"{_baseUrl}/api/TradingData/pnl/daily";
        if (!string.IsNullOrEmpty(quoteAsset)) url += $"?quoteAsset={quoteAsset}";
        return await httpClient.GetFromJsonAsync<double>(url);
    }

    public async Task<double> GetTotalPnlAsync(string? quoteAsset = null)
    {
        var url = $"{_baseUrl}/api/TradingData/pnl/total";
        if (!string.IsNullOrEmpty(quoteAsset)) url += $"?quoteAsset={quoteAsset}";
        return await httpClient.GetFromJsonAsync<double>(url);
    }

    public async Task<Stats> GetStatsAsync(string? quoteAsset = null)
    {
        var url = $"{_baseUrl}/api/TradingData/stats";
        if (!string.IsNullOrEmpty(quoteAsset)) url += $"?quoteAsset={quoteAsset}";
        return await httpClient.GetFromJsonAsync<Stats>(url) ?? throw new Exception("Failed to fetch stats");
    }

    public async Task<PnlSummary> GetPnlSummaryAsync(string? quoteAsset = null)
    {
        var url = $"{_baseUrl}/api/TradingData/pnl/summary";
        if (!string.IsNullOrEmpty(quoteAsset)) url += $"?quoteAsset={quoteAsset}";
        return await httpClient.GetFromJsonAsync<PnlSummary>(url) ?? throw new Exception("Failed to fetch PnL summary");
    }

    public async Task<List<OpenPosition>> GetOpenPositionsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<OpenPosition>>($"{_baseUrl}/api/TradingData/positions/open") ?? [];
    }

    public async Task<List<ClosedTrade>> GetLastTradesAsync(int limit = 5)
    {
        return await httpClient.GetFromJsonAsync<List<ClosedTrade>>($"{_baseUrl}/api/TradingData/trades/last?limit={limit}") ?? [];
    }
}
