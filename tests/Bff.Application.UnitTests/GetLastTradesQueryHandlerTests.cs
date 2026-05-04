using Bff.Application.Features.Trades.Queries;
using Bff.Domain.Abstractions;
using Moq;
using Xunit;

namespace Bff.Application.UnitTests;

public class GetLastTradesQueryHandlerTests
{
    private readonly Mock<IDatabaseService> _dbMock = new();

    [Fact]
    public async Task Handle_ShouldReturnLastTrades()
    {
        var trade = new ClosedTrade("ETHUSDT", "BUY", 3000, 3100, 100, 3.33, 75, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
        _dbMock.Setup(x => x.GetLastTradesAsync(5)).ReturnsAsync(new List<ClosedTrade> { trade });
        var handler = new GetLastTradesQueryHandler(_dbMock.Object);

        var result = await handler.Handle(new GetLastTradesQuery(5), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(100, result[0].Pnl);
    }
}
