namespace Bff.Domain.Models.Persistence;

public record TradeResponse(
    int Id, string Symbol, string Side, string Status,
    double Price, double Quantity, double Value,
    double? StopLoss, double? TakeProfit, int? AiScore,
    double? ClosePrice, double? Pnl, double? PnlPct,
    DateTime CreatedAt, DateTime? CloseAt);

public record CreateTradeRequest(
    string Symbol, string Side,
    double Price, double Quantity, double Value,
    double? StopLoss = null, double? TakeProfit = null, int? AiScore = null);

public record UpdateTradeRequest(
    string? Status = null, double? ClosePrice = null, double? Pnl = null, double? PnlPct = null,
    double? TakeProfit = null, double? StopLoss = null);
