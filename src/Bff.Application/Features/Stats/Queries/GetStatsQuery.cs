using MediatR;
using Bff.Application.Common.Models;
using Bff.Domain.Abstractions;

namespace Bff.Application.Features.Stats.Queries;

public record GetStatsQuery(string? QuoteAsset = null) : IRequest<StatsDto>;

public class GetStatsQueryHandler(IDatabaseService db) : IRequestHandler<GetStatsQuery, StatsDto>
{
    public async Task<StatsDto> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var s = await db.GetStatsAsync(request.QuoteAsset);
        return new StatsDto(
            s.PnlDay, s.CountDay,
            s.PnlWeek, s.CountWeek,
            s.PnlMonth, s.CountMonth,
            s.PnlTotal, s.CountTotal,
            s.Wins, s.WinRate,
            DateTime.UtcNow
        );
    }
}
