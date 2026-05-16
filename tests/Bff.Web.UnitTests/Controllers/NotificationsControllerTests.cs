using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Bff.Web.Controllers;
using Bff.Web.Services;
using TradingProject.Bff.Client.Models.Dtos;

namespace Bff.Web.UnitTests.Controllers;

public class NotificationsControllerTests
{
    private readonly Mock<INotificationAggregationService> _aggregationMock;
    private readonly NotificationsController _controller;

    public NotificationsControllerTests()
    {
        _aggregationMock = new Mock<INotificationAggregationService>();
        _controller = new NotificationsController(_aggregationMock.Object);
    }

    [Fact]
    public async Task GetServiceStartContext_ShouldReturnOkWithDto()
    {
        // Arrange
        var expected = new ServiceStartNotificationDto
        {
            QuoteAsset = "USDC",
            ServiceName = "OrchestratorServiceWorker",
            Timestamp = DateTime.UtcNow,
            OpenPositionsCount = 3,
            DailyPnl = 42.5,
            TotalPnl = 1500.0,
            FearAndGreedIndex = 55,
            FearAndGreedLabel = "Neutral",
        };

        _aggregationMock
            .Setup(x => x.GetServiceStartContextAsync("USDC", "OrchestratorServiceWorker", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetServiceStartContext("USDC", "OrchestratorServiceWorker");

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var dto = okResult.Value.Should().BeOfType<ServiceStartNotificationDto>().Subject;
        dto.QuoteAsset.Should().Be("USDC");
        dto.ServiceName.Should().Be("OrchestratorServiceWorker");
        dto.FearAndGreedIndex.Should().Be(55);
        dto.OpenPositionsCount.Should().Be(3);
    }

    [Fact]
    public async Task GetMarketStressContext_ShouldReturnOkWithDto()
    {
        // Arrange
        var expected = new MarketStressNotificationDto
        {
            QuoteAsset = "USDC",
            StressValue = 80,
            Timestamp = DateTime.UtcNow,
            OpenPositionsCount = 5,
            DailyPnl = -120.0,
            TotalPnl = 3000.0,
            FearAndGreedIndex = 25,
            FearAndGreedLabel = "Extreme Fear",
        };

        _aggregationMock
            .Setup(x => x.GetMarketStressContextAsync("USDC", 80, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetMarketStressContext("USDC", 80);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var dto = okResult.Value.Should().BeOfType<MarketStressNotificationDto>().Subject;
        dto.QuoteAsset.Should().Be("USDC");
        dto.StressValue.Should().Be(80);
        dto.FearAndGreedIndex.Should().Be(25);
        dto.FearAndGreedLabel.Should().Be("Extreme Fear");
        dto.DailyPnl.Should().Be(-120.0);
    }

    [Fact]
    public async Task GetServiceStartContext_ShouldReturnOk_WhenAggregationReturnsDefaults()
    {
        // Arrange
        _aggregationMock
            .Setup(x => x.GetServiceStartContextAsync("EUR", "FastExitWorker", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceStartNotificationDto
            {
                QuoteAsset = "EUR",
                ServiceName = "FastExitWorker",
                Timestamp = DateTime.UtcNow,
            });

        // Act
        var result = await _controller.GetServiceStartContext("EUR", "FastExitWorker");

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var dto = okResult.Value.Should().BeOfType<ServiceStartNotificationDto>().Subject;
        dto.QuoteAsset.Should().Be("EUR");
        dto.ServiceName.Should().Be("FastExitWorker");
        dto.OpenPositionsCount.Should().Be(0);
        dto.FearAndGreedIndex.Should().Be(0);
    }
}
