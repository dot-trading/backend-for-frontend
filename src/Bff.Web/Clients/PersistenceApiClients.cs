using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using TradingProject.Persistence.Api.Stubs.Models;
using TradingProject.Persistence.Api.Stubs.V1;

namespace Bff.Web.Clients;

internal class PagingListDto<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public T[] Payload { get; set; } = Array.Empty<T>();
    public Dictionary<string, object?>? Metadata { get; set; }
}

internal record TradeResponseDto(
    int Id,
    string Symbol,
    string QuoteAsset,
    string Side,
    string Status,
    double Price,
    double Quantity,
    [property: JsonPropertyName("usdtValue")] double Value,
    double? StopLoss,
    double? TakeProfit,
    int? AiScore,
    string? BinanceOrderId,
    double? ClosePrice,
    [property: JsonPropertyName("pnlUsdt")] double? Pnl,
    double? PnlPct,
    DateTime CreatedAt,
    DateTime? CloseAt);

public class TradesHttpClient(HttpClient httpClient) : ITradesApi
{
    public async Task<PagingList<TradeResponse>> GetTradesAsync(
        int limit = 50,
        int page = 1,
        string? status = null,
        string? symbol = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["limit"] = limit.ToString(),
            ["page"] = page.ToString()
        };
        if (!string.IsNullOrEmpty(status)) queryParams["status"] = status;
        if (!string.IsNullOrEmpty(symbol)) queryParams["symbol"] = symbol;

        var url = QueryHelpers.AddQueryString("api/v1/trades", queryParams);
        var result = await httpClient.GetFromJsonAsync<PagingListDto<TradeResponseDto>>(url, cancellationToken);
        if (result == null)
        {
            return new PagingList<TradeResponse>(Array.Empty<TradeResponse>(), page, limit, 0);
        }

        var mappedPayload = result.Payload.Select(dto => new TradeResponse(
            dto.Id, dto.Symbol, dto.QuoteAsset, dto.Side, dto.Status,
            dto.Price, dto.Quantity, dto.Value, dto.StopLoss, dto.TakeProfit,
            dto.AiScore, dto.BinanceOrderId, dto.ClosePrice, dto.Pnl, dto.PnlPct,
            dto.CreatedAt, dto.CloseAt
        )).ToArray();

        return new PagingList<TradeResponse>(
            mappedPayload,
            result.PageNumber,
            result.PageSize,
            result.TotalCount,
            result.Metadata
        );
    }

    public async Task<TradeResponse> CreateTradeAsync(
        CreateTradeRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/trades", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var dto = (await response.Content.ReadFromJsonAsync<TradeResponseDto>(cancellationToken: cancellationToken))!;
        return new TradeResponse(
            dto.Id, dto.Symbol, dto.QuoteAsset, dto.Side, dto.Status,
            dto.Price, dto.Quantity, dto.Value, dto.StopLoss, dto.TakeProfit,
            dto.AiScore, dto.BinanceOrderId, dto.ClosePrice, dto.Pnl, dto.PnlPct,
            dto.CreatedAt, dto.CloseAt
        );
    }

    public async Task<TradeResponse?> UpdateTradeAsync(
        int id,
        UpdateTradeRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"api/v1/trades/{id}", request, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        var dto = await response.Content.ReadFromJsonAsync<TradeResponseDto>(cancellationToken: cancellationToken);
        if (dto == null) return null;
        return new TradeResponse(
            dto.Id, dto.Symbol, dto.QuoteAsset, dto.Side, dto.Status,
            dto.Price, dto.Quantity, dto.Value, dto.StopLoss, dto.TakeProfit,
            dto.AiScore, dto.BinanceOrderId, dto.ClosePrice, dto.Pnl, dto.PnlPct,
            dto.CreatedAt, dto.CloseAt
        );
    }
}

public class OpportunitiesHttpClient(HttpClient httpClient) : IOpportunitiesApi
{
    public async Task<PagingList<OpportunityResponse>> GetOpportunitiesAsync(
        int limit = 50,
        int page = 1,
        string? symbol = null,
        bool? isApproved = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["limit"] = limit.ToString(),
            ["page"] = page.ToString()
        };
        if (!string.IsNullOrEmpty(symbol)) queryParams["symbol"] = symbol;
        if (isApproved.HasValue) queryParams["isApproved"] = isApproved.Value.ToString().ToLower();

        var url = QueryHelpers.AddQueryString("api/v1/opportunities", queryParams);
        var result = await httpClient.GetFromJsonAsync<PagingListDto<OpportunityResponse>>(url, cancellationToken);
        return result != null
            ? new PagingList<OpportunityResponse>(result.Payload, result.PageNumber, result.PageSize, result.TotalCount, result.Metadata)
            : new PagingList<OpportunityResponse>(Array.Empty<OpportunityResponse>(), page, limit, 0);
    }

    public async Task<OpportunityResponse> CreateOpportunityAsync(
        CreateOpportunityRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/opportunities", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<OpportunityResponse>(cancellationToken: cancellationToken))!;
    }
}

public class PortfolioSnapshotsHttpClient(HttpClient httpClient) : IPortfolioSnapshotsApi
{
    public async Task<PagingList<PortfolioSnapshotResponse>> GetPortfolioSnapshotsAsync(
        int limit = 50,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["limit"] = limit.ToString(),
            ["page"] = page.ToString()
        };

        var url = QueryHelpers.AddQueryString("api/v1/portfolio-snapshots", queryParams);
        var result = await httpClient.GetFromJsonAsync<PagingListDto<PortfolioSnapshotResponse>>(url, cancellationToken);
        return result != null
            ? new PagingList<PortfolioSnapshotResponse>(result.Payload, result.PageNumber, result.PageSize, result.TotalCount, result.Metadata)
            : new PagingList<PortfolioSnapshotResponse>(Array.Empty<PortfolioSnapshotResponse>(), page, limit, 0);
    }

    public async Task<PortfolioSnapshotResponse> CreatePortfolioSnapshotAsync(
        CreatePortfolioSnapshotRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/portfolio-snapshots", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PortfolioSnapshotResponse>(cancellationToken: cancellationToken))!;
    }
}
