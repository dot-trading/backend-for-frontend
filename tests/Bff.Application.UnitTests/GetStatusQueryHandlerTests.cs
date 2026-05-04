using Bff.Application.Features.Status.Queries;
using Bff.Domain.Abstractions;
using Bff.Domain.Enums;
using Moq;
using Xunit;

namespace Bff.Application.UnitTests;

public class GetStatusQueryHandlerTests
{
    private readonly Mock<IBinanceService> _binanceMock = new();
    private readonly Mock<IDatabaseService> _dbMock = new();
    private readonly Mock<IClusterService> _clusterMock = new();

    [Fact]
    public async Task Handle_ShouldReturnStatusDto()
    {
        // Arrange
        _binanceMock.Setup(x => x.GetBalancesAsync()).ReturnsAsync(new Dictionary<string, double> { { "USDC", 100 } });
        _clusterMock.Setup(x => x.GetOrchestratorStatusAsync()).ReturnsAsync(ServiceStatus.Online);
        _clusterMock.Setup(x => x.GetOllamaStatusAsync()).ReturnsAsync(ServiceStatus.Online);
        _clusterMock.Setup(x => x.GetPersistenceStatusAsync()).ReturnsAsync(ServiceStatus.Online);
        _dbMock.Setup(x => x.GetOpenPositionsCountAsync()).ReturnsAsync(5);
        _dbMock.Setup(x => x.GetDailyPnlAsync()).ReturnsAsync(10.5);
        _dbMock.Setup(x => x.GetTotalPnlAsync()).ReturnsAsync(100.0);

        var handler = new GetStatusQueryHandler(_binanceMock.Object, _dbMock.Object, _clusterMock.Object);

        // Act
        var result = await handler.Handle(new GetStatusQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ServiceStatus.Online, result.OrchestratorStatus);
        Assert.Equal(100, result.TotalCapital);
    }
}
