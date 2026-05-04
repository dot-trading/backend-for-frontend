namespace Bff.Domain.Models.Persistence;

public record PortfolioSnapshotResponse(
    int Id, double Total, double Free,
    int PositionsCount, double DailyPnl, double TotalPnl, DateTime CreatedAt);

public record CreatePortfolioSnapshotRequest(
    double Total, double Free, int PositionsCount,
    double DailyPnl = 0, double TotalPnl = 0);
