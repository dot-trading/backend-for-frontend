using FluentAssertions;
using Moq;
using TradingProject.Bff.Client.Models.Dtos;
using TradingProject.Persistence.Api.Stubs.Models;
using Bff.Web.Services;
using TradingProject.Persistence.Api.Stubs.V1;
using TradingProject.ThirdParty.Client.Models.Responses;
using TradingProject.ThirdParty.Client.Services;

namespace Bff.Web.UnitTests.Services;

public class NotificationAggregationServiceTests
{
    private readonly Mock<ITradesApi> _tradesApiMock;
    private readonly Mock<IThirdPartyApiClient> _thirdPartyMock;
    private readonly NotificationAggregationService _service;

    public NotificationAggregationServiceTests()
    {
        _tradesApiMock = new Mock<ITradesApi>();
        _thirdPartyMock = new Mock<IThirdPartyApiClient>();
        _service = new NotificationAggregationService(
            _tradesApiMock.Object,
            _thirdPartyMock.Object);
    }

    [Fact]
    public async Task GetServiceStartContextAsync_ShouldReturnPopulatedDto_WhenAllServicesRespond()
    {
        // Arrange
        var quoteAsset = "USDC";
        var serviceName = "OrchestratorServiceWorker";

        var fearAndGreed = new FearAndGreedResponse(42, "Fear", 1234567890);

        var trades = new[]
                {
                    // Trade closed today: contributes to daily P&amp;L
                    new TradeResponse(1, "BTCUSDC", quoteAsset, "BUY", "closed", 50000, 0.1, 5000, null, null, null, null, null, 150.0, 0.03, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddMinutes(-30)),
                    // Trade still open: does NOT contribute to daily P&amp;L (CloseAt is null)
                    new TradeResponse(2, "ETHUSDC", quoteAsset, "SELL", "open", 3000, 2.0, 6000, null, null, null, null, null, -75.0, -0.025, DateTime.UtcNow.AddHours(-2), null),
                };

        _thirdPartyMock
            .Setup(x => x.GetFearAndGreedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fearAndGreed);

        _tradesApiMock
            .Setup(x => x.GetTradesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagingList<TradeResponse>(trades, 1, 200, trades.Length));

        // Act
        var result = await _service.GetServiceStartContextAsync(quoteAsset, serviceName);

        // Assert
        result.Should().NotBeNull();
        result.QuoteAsset.Should().Be(quoteAsset);
        result.ServiceName.Should().Be(serviceName);
        result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.FearAndGreedIndex.Should().Be(42);
        result.FearAndGreedLabel.Should().Be("Fear");
        result.OpenPositionsCount.Should().Be(2);
        result.TotalPnl.Should().Be(75.0);  // 150 + (-75)
        result.DailyPnl.Should().Be(150.0); // Only the trade closed today (BTCUSDC)
    }

    [Fact]
    public async Task GetServiceStartContextAsync_ShouldHandleDownstreamFailures_Gracefully()
    {
        // Arrange
        _thirdPartyMock
            .Setup(x => x.GetFearAndGreedAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("ThirdParty unavailable"));

        _tradesApiMock
            .Setup(x => x.GetTradesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Persistence unavailable"));

        // Act
        var result = await _service.GetServiceStartContextAsync("USDC", "TestService");

        // Assert
        result.Should().NotBeNull();
        result.FearAndGreedIndex.Should().Be(0);
        result.FearAndGreedLabel.Should().Be("Unknown");
        result.OpenPositionsCount.Should().Be(0);
        result.DailyPnl.Should().Be(0);
        result.TotalPnl.Should().Be(0);
        result.QuoteAsset.Should().Be("USDC");
        result.ServiceName.Should().Be("TestService");
    }

    [Fact]
    public async Task GetServiceStartContextAsync_ShouldHandleEmptyTrades_Gracefully()
    {
        // Arrange
        var fearAndGreed = new FearAndGreedResponse(75, "Greed", 1234567890);

        _thirdPartyMock
            .Setup(x => x.GetFearAndGreedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fearAndGreed);

        _tradesApiMock
            .Setup(x => x.GetTradesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagingList<TradeResponse>(Array.Empty<TradeResponse>(), 1, 200, 0));

        // Act
        var result = await _service.GetServiceStartContextAsync("EUR", "Worker");

        // Assert
        result.Should().NotBeNull();
        result.FearAndGreedIndex.Should().Be(75);
        result.FearAndGreedLabel.Should().Be("Greed");
        result.OpenPositionsCount.Should().Be(0);
        result.DailyPnl.Should().Be(0);
        result.TotalPnl.Should().Be(0);
    }

    [Fact]
    public async Task GetMarketStressContextAsync_ShouldReturnPopulatedDto()
    {
        // Arrange
        var tradeToday = new TradeResponse(1, "BTCUSDC", "USDC", "BUY", "open", 50000, 0.1, 5000, null, null, null, null, null, 150.0, 0.03, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
        var tradeOld = new TradeResponse(2, "ETHUSDC", "USDC", "SELL", "open", 3000, 2.0, 6000, null, null, null, null, null, -75.0, -0.025, DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(-3));

        _thirdPartyMock
            .Setup(x => x.GetFearAndGreedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FearAndGreedResponse(25, "Extreme Fear", 1234567890));

        _tradesApiMock
            .Setup(x => x.GetTradesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagingList<TradeResponse>(new[] { tradeToday, tradeOld }, 1, 200, 2));

        // Act
        var result = await _service.GetMarketStressContextAsync("USDC", 85);

        // Assert
        result.Should().NotBeNull();
        result.QuoteAsset.Should().Be("USDC");
        result.StressValue.Should().Be(85);
        result.FearAndGreedIndex.Should().Be(25);
        result.FearAndGreedLabel.Should().Be("Extreme Fear");
        result.OpenPositionsCount.Should().Be(2);
        result.DailyPnl.Should().Be(150.0); // tradeToday closed today
        result.TotalPnl.Should().Be(75.0);  // 150 + (-75)
    }
}
