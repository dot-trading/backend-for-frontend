using MediatR;
using Bff.Domain.Abstractions;

namespace Bff.Application.Features.Positions.Queries;

public record GetOpenPositionsQuery : IRequest<List<OpenPositionDto>>;

public record OpenPositionDto(
    string Symbol, string Side,
    double Entry, double Quantity, double UsdtValue,
    double? StopLoss, double? TakeProfit, int? AiScore,
    DateTime CreatedAt, double? CurrentPrice, double? PnlPct, double? PnlUsdt
);

public class GetOpenPositionsQueryHandler(
    IBinanceService binance,
    IDatabaseService db) : IRequestHandler<GetOpenPositionsQuery, List<OpenPositionDto>>
{
    public async Task<List<OpenPositionDto>> Handle(GetOpenPositionsQuery request, CancellationToken cancellationToken)
    {
        var positions = await db.GetOpenPositionsAsync();
        var dtos = new List<OpenPositionDto>();

        foreach (var p in positions)
        {
            double? curPrice = null;
            double? pnlPct = null;
            double? pnlUsdt = null;

            try
            {
                curPrice = await binance.GetCurrentPriceAsync(p.Symbol);
                pnlPct = (curPrice - p.Entry) / p.Entry * 100;
                pnlUsdt = (curPrice - p.Entry) * p.Quantity;
            }
            catch { /* Ignore price fetch errors for now */ }

            dtos.Add(new OpenPositionDto(
                p.Symbol, p.Side, p.Entry, p.Quantity, p.UsdtValue,
                p.StopLoss, p.TakeProfit, p.AiScore, p.CreatedAt,
                curPrice, pnlPct, pnlUsdt));
        }

        return dtos;
    }
}
