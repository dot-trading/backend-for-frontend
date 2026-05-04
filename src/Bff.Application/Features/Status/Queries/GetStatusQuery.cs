using MediatR;
using Bff.Application.Common.Models;
using Bff.Domain.Abstractions;

namespace Bff.Application.Features.Status.Queries;

public record GetStatusQuery : IRequest<SystemStatusDto>;

public class GetStatusQueryHandler(
    IBinanceService binance,
    IDatabaseService db,
    IClusterService cluster) : IRequestHandler<GetStatusQuery, SystemStatusDto>
{
    public async Task<SystemStatusDto> Handle(GetStatusQuery request, CancellationToken cancellationToken)
    {
        var balances = await binance.GetBalancesAsync();
        var totalCapital = balances.GetValueOrDefault("EUR") + balances.GetValueOrDefault("USDC") + balances.GetValueOrDefault("USDT");
        
        return new SystemStatusDto(
            await cluster.GetOrchestratorStatusAsync(),
            await cluster.GetOllamaStatusAsync(),
            await cluster.GetPersistenceStatusAsync(),
            true,
            balances,
            totalCapital,
            await db.GetOpenPositionsCountAsync(),
            await db.GetDailyPnlAsync(),
            await db.GetTotalPnlAsync(),
            DateTime.UtcNow
        );
    }
}
