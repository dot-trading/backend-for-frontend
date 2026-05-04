using System.Net.Http.Json;
using Bff.Domain.Abstractions;
using Bff.Domain.Models.Persistence;
using Bff.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Bff.Infrastructure.Services;

public class PersistenceService(HttpClient httpClient, IOptions<PersistenceOptions> options) : IPersistenceService
{
    private readonly string _base = options.Value.BaseUrl.TrimEnd('/');

    public async Task<List<TradeResponse>> GetTradesAsync(int limit = 50, int page = 1, string? status = null, string? symbol = null, CancellationToken ct = default)
    {
        var url = $"{_base}/api/trades?limit={limit}&page={page}";
        if (status is not null) url += $"&status={status}";
        if (symbol is not null) url += $"&symbol={symbol}";
        return await httpClient.GetFromJsonAsync<List<TradeResponse>>(url, ct) ?? [];
    }

    public async Task<TradeResponse> CreateTradeAsync(CreateTradeRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync($"{_base}/api/trades", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TradeResponse>(ct)
               ?? throw new InvalidOperationException("Failed to create trade");
    }

    public async Task<TradeResponse?> UpdateTradeAsync(int id, UpdateTradeRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync($"{_base}/api/trades/{id}", request, ct);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TradeResponse>(ct);
    }

    public async Task<List<OpportunityResponse>> GetOpportunitiesAsync(int limit = 50, int page = 1, string? symbol = null, bool? isApproved = null, CancellationToken ct = default)
    {
        var url = $"{_base}/api/opportunities?limit={limit}&page={page}";
        if (symbol is not null) url += $"&symbol={symbol}";
        if (isApproved.HasValue) url += $"&isApproved={isApproved.Value.ToString().ToLower()}";
        return await httpClient.GetFromJsonAsync<List<OpportunityResponse>>(url, ct) ?? [];
    }

    public async Task<OpportunityResponse> CreateOpportunityAsync(CreateOpportunityRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync($"{_base}/api/opportunities", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OpportunityResponse>(ct)
               ?? throw new InvalidOperationException("Failed to create opportunity");
    }

    public async Task<List<PortfolioSnapshotResponse>> GetPortfolioSnapshotsAsync(int limit = 50, int page = 1, CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<List<PortfolioSnapshotResponse>>($"{_base}/api/portfolio-snapshots?limit={limit}&page={page}", ct) ?? [];

    public async Task<PortfolioSnapshotResponse> CreatePortfolioSnapshotAsync(CreatePortfolioSnapshotRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync($"{_base}/api/portfolio-snapshots", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PortfolioSnapshotResponse>(ct)
               ?? throw new InvalidOperationException("Failed to create portfolio snapshot");
    }

    public async Task<PnlSummaryResponse> GetPnlSummaryAsync(string? quoteAsset = null, CancellationToken ct = default)
    {
        var url = $"{_base}/api/TradingData/pnl/summary";
        if (quoteAsset is not null) url += $"?quoteAsset={quoteAsset}";
        return await httpClient.GetFromJsonAsync<PnlSummaryResponse>(url, ct) ?? new();
    }
}
