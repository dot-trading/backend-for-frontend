using Bff.Domain.Entities;

namespace Bff.Domain.Abstractions;

public interface IDatabaseService
{
    Task<int> GetOpenPositionsCountAsync();
    Task<double> GetDailyPnlAsync(string? quoteAsset = null);
    Task<double> GetTotalPnlAsync(string? quoteAsset = null);
    Task<Stats> GetStatsAsync(string? quoteAsset = null);
    Task<PnlSummary> GetPnlSummaryAsync(string? quoteAsset = null);
    Task<List<OpenPosition>> GetOpenPositionsAsync();
    Task<List<ClosedTrade>> GetLastTradesAsync(int limit = 5);
}

public record Stats(
    double PnlDay, double PnlWeek, double PnlMonth, double PnlTotal,
    int CountDay, int CountWeek, int CountMonth, int CountTotal,
    int Wins, double WinRate);

public record PnlSummary(double Daily, double Weekly, double Monthly, double Total);

public record OpenPosition(
    string Symbol, string Side,
    double Entry, double Quantity, double UsdtValue,
    double? StopLoss, double? TakeProfit, int? AiScore,
    DateTime CreatedAt);

public record ClosedTrade(
    string Symbol, string Side,
    double Entry, double ClosePrice,
    double Pnl, double PnlPct,
    int? AiScore, DateTime OpenedAt, DateTime ClosedAt);
