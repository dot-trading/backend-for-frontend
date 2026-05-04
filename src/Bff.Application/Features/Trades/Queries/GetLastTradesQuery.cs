using MediatR;
using Bff.Domain.Abstractions;

namespace Bff.Application.Features.Trades.Queries;

public record GetLastTradesQuery(int Limit = 5) : IRequest<List<ClosedTradeDto>>;

public record ClosedTradeDto(
    string Symbol, string Side,
    double Entry, double ClosePrice,
    double Pnl, double PnlPct,
    int? AiScore, DateTime OpenedAt, DateTime ClosedAt
);

public class GetLastTradesQueryHandler(IDatabaseService db) : IRequestHandler<GetLastTradesQuery, List<ClosedTradeDto>>
{
    public async Task<List<ClosedTradeDto>> Handle(GetLastTradesQuery request, CancellationToken cancellationToken)
    {
        var trades = await db.GetLastTradesAsync(request.Limit);
        return trades.Select(t => new ClosedTradeDto(
            t.Symbol, t.Side, t.Entry, t.ClosePrice, t.Pnl, t.PnlPct, t.AiScore, t.OpenedAt, t.ClosedAt)).ToList();
    }
}
