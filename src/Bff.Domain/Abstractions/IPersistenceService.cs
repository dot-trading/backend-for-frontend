using Bff.Domain.Models.Persistence;

namespace Bff.Domain.Abstractions;

public interface IPersistenceService
{
    Task<List<TradeResponse>> GetTradesAsync(int limit = 50, int page = 1, string? status = null, string? symbol = null, CancellationToken ct = default);
    Task<TradeResponse> CreateTradeAsync(CreateTradeRequest request, CancellationToken ct = default);
    Task<TradeResponse?> UpdateTradeAsync(int id, UpdateTradeRequest request, CancellationToken ct = default);

    Task<List<OpportunityResponse>> GetOpportunitiesAsync(int limit = 50, int page = 1, string? symbol = null, bool? isApproved = null, CancellationToken ct = default);
    Task<OpportunityResponse> CreateOpportunityAsync(CreateOpportunityRequest request, CancellationToken ct = default);

    Task<List<PortfolioSnapshotResponse>> GetPortfolioSnapshotsAsync(int limit = 50, int page = 1, CancellationToken ct = default);
    Task<PortfolioSnapshotResponse> CreatePortfolioSnapshotAsync(CreatePortfolioSnapshotRequest request, CancellationToken ct = default);

    Task<PnlSummaryResponse> GetPnlSummaryAsync(string? quoteAsset = null, CancellationToken ct = default);
}
