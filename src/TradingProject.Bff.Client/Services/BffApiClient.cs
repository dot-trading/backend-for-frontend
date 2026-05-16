using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TradingProject.Bff.Client.Models.Dtos;

namespace TradingProject.Bff.Client.Services;

/// <summary>
/// Typed HttpClient implementation of <see cref="IBffApiClient"/>.
/// Communicates with the local BFF API over HTTP.
/// </summary>
public class BffApiClient : IBffApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BffApiClient> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="BffApiClient"/>.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> configured by DI.</param>
    /// <param name="logger">Logger instance.</param>
    public BffApiClient(HttpClient httpClient, ILogger<BffApiClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<ServiceStartNotificationDto?> GetServiceStartContextAsync(
        string quoteAsset,
        string serviceName,
        CancellationToken cancellationToken = default)
    {
        var url = $"notifications/service-start-context?quoteAsset={Uri.EscapeDataString(quoteAsset)}&serviceName={Uri.EscapeDataString(serviceName)}";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ServiceStartNotificationDto>(url, cancellationToken);
            return response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch service-start context for {QuoteAsset}/{ServiceName}", quoteAsset, serviceName);
            return null;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Unexpected error fetching service-start context for {QuoteAsset}/{ServiceName}", quoteAsset, serviceName);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<MarketStressNotificationDto?> GetMarketStressContextAsync(
        string quoteAsset,
        int stressValue,
        CancellationToken cancellationToken = default)
    {
        var url = $"notifications/market-stress-context?quoteAsset={Uri.EscapeDataString(quoteAsset)}&stressValue={stressValue}";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<MarketStressNotificationDto>(url, cancellationToken);
            return response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch market-stress context for {QuoteAsset}", quoteAsset);
            return null;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Unexpected error fetching market-stress context for {QuoteAsset}", quoteAsset);
            return null;
        }
    }
}
