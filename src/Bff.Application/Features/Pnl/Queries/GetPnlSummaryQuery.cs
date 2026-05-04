using MediatR;
using Bff.Application.Common.Models;
using Bff.Domain.Abstractions;

namespace Bff.Application.Features.Pnl.Queries;

public record GetPnlSummaryQuery(string? QuoteAsset = null) : IRequest<PnlSummaryDto>;

public class GetPnlSummaryQueryHandler(
    IBinanceService binance,
    IDatabaseService db) : IRequestHandler<GetPnlSummaryQuery, PnlSummaryDto>
{
    public async Task<PnlSummaryDto> Handle(GetPnlSummaryQuery request, CancellationToken cancellationToken)
    {
        var balances = await binance.GetBalancesAsync();
        
        // If QuoteAsset is specified, only consider that balance for free capital
        double freeCapital;
        if (!string.IsNullOrEmpty(request.QuoteAsset))
        {
            freeCapital = balances.GetValueOrDefault(request.QuoteAsset);
        }
        else
        {
            freeCapital = balances.GetValueOrDefault("EUR") + balances.GetValueOrDefault("USDC") + balances.GetValueOrDefault("USDT");
        }

        var s = await db.GetPnlSummaryAsync(request.QuoteAsset);

        return new PnlSummaryDto(
            freeCapital,
            s.Daily,
            s.Weekly,
            s.Monthly,
            s.Total,
            DateTime.UtcNow
        );
    }
}
