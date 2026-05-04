namespace Bff.Application.Common.Models;

public record StatsDto(
    double PnlDay, int CountDay,
    double PnlWeek, int CountWeek,
    double PnlMonth, int CountMonth,
    double PnlTotal, int CountTotal,
    int Wins, double WinRate,
    DateTime Timestamp
);
