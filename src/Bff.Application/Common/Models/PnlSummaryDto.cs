namespace Bff.Application.Common.Models;

public record PnlSummaryDto(
    double FreeCapital,
    double DailyPnl,
    double WeeklyPnl,
    double MonthlyPnl,
    double TotalPnl,
    DateTime Timestamp
);
