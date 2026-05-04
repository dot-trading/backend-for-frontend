using Bff.Application.Features.Stats.Queries;
using Bff.Domain.Abstractions;
using Moq;
using Xunit;

namespace Bff.Application.UnitTests;

public class GetStatsQueryHandlerTests
{
    private readonly Mock<IDatabaseService> _dbMock = new();

    [Fact]
    public async Task Handle_ShouldReturnStatsDto()
    {
        var stats = new Stats(10, 70, 300, 1000, 5, 20, 80, 200, 120, 60.0);
        _dbMock.Setup(x => x.GetStatsAsync()).ReturnsAsync(stats);
        var handler = new GetStatsQueryHandler(_dbMock.Object);

        var result = await handler.Handle(new GetStatsQuery(), CancellationToken.None);

        Assert.Equal(10, result.PnlDay);
        Assert.Equal(60.0, result.WinRate);
    }
}
