using Bff.Application.Features.Positions.Queries;
using Bff.Domain.Abstractions;
using Moq;
using Xunit;

namespace Bff.Application.UnitTests;

public class GetOpenPositionsQueryHandlerTests
{
    private readonly Mock<IBinanceService> _binanceMock = new();
    private readonly Mock<IDatabaseService> _dbMock = new();

    [Fact]
    public async Task Handle_ShouldReturnOpenPositions()
    {
        var pos = new OpenPosition("BTCUSDT", "BUY", 50000, 0.1, 5000, null, null, 80, DateTime.UtcNow);
        _dbMock.Setup(x => x.GetOpenPositionsAsync()).ReturnsAsync(new List<OpenPosition> { pos });
        _binanceMock.Setup(x => x.GetCurrentPriceAsync("BTCUSDT")).ReturnsAsync(55000);
        
        var handler = new GetOpenPositionsQueryHandler(_binanceMock.Object, _dbMock.Object);

        var result = await handler.Handle(new GetOpenPositionsQuery(), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(55000, result[0].CurrentPrice);
        Assert.Equal(10, result[0].PnlPct);
    }
}
