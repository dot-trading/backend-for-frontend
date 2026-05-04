using Bff.Domain.Enums;

namespace Bff.Application.Common.Models;

public record SystemStatusDto(
    ServiceStatus OrchestratorStatus,
    ServiceStatus OllamaStatus,
    ServiceStatus PersistenceStatus,
    bool DatabaseConnected,
    Dictionary<string, double> Balances,
    double TotalCapital,
    int OpenPositionsCount,
    double DailyPnl,
    double TotalPnl,
    DateTime Timestamp
);
