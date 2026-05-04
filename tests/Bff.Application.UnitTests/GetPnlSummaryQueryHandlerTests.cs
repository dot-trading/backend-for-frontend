using Bff.Application.Features.Pnl.Queries;
using Bff.Domain.Abstractions;
using Moq;
using Xunit;

namespace Bff.Application.UnitTests;

public class GetPnlSummaryQueryHandlerTests
{
    private readonly Mock<IBinanceService> _binanceMock = new();
    private readonly Mock<IDatabaseService> _dbMock = new();

    [Fact]
    public async Task Handle_ShouldReturnPnlSummaryDto()
    {
        _binanceMock.Setup(x => x.GetBalancesAsync()).ReturnsAsync(new Dictionary<string, double> { { "EUR", 50 }, { "USDC", 50 } });
        _dbMock.Setup(x => x.GetPnlSummaryAsync()).ReturnsAsync(new PnlSummary(10, 50, 200, 1000));
        var handler = new GetPnlSummaryQueryHandler(_binanceMock.Object, _dbMock.Object);

        var result = await handler.Handle(new GetPnlSummaryQuery(), CancellationToken.None);

        Assert.Equal(100, result.FreeCapital);
        Assert.Equal(10, result.DailyPnl);
    }
}
