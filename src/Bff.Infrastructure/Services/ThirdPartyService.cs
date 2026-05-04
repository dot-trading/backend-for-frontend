using System.Net.Http.Json;
using Bff.Domain.Abstractions;
using Bff.Domain.Models.ThirdParty;
using Bff.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Bff.Infrastructure.Services;

public class ThirdPartyService(HttpClient httpClient, IOptions<ThirdPartyOptions> options) : IThirdPartyService
{
    private readonly string _base = options.Value.BaseUrl.TrimEnd('/');

    public async Task<Dictionary<string, double>> GetBalancesAsync(CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<Dictionary<string, double>>($"{_base}/api/account/balances", ct) ?? [];

    public async Task<double> GetPriceAsync(string symbol, CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<double>($"{_base}/api/market-data/price/{symbol}", ct);

    public async Task<double> GetMinNotionalAsync(string symbol, CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<double>($"{_base}/api/market-data/notional/{symbol}", ct);

    public async Task<List<Kline>> GetKlinesAsync(string symbol, string interval = "1h", int limit = 24, CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<List<Kline>>($"{_base}/api/market-data/klines/{symbol}?interval={interval}&limit={limit}", ct) ?? [];

    public async Task<Ticker24h?> GetTicker24hAsync(string symbol, CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<Ticker24h>($"{_base}/api/market-data/ticker/{symbol}", ct);

    public async Task<FearAndGreedIndex> GetFearAndGreedAsync(CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<FearAndGreedIndex>($"{_base}/api/market-data/sentiment/fear-and-greed", ct)
           ?? throw new InvalidOperationException("Failed to fetch Fear & Greed index");

    public async Task<double> GetCoinGeckoPriceAsync(string coinId, string vsCurrency = "usd", CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<double>($"{_base}/api/market-data/price/coingecko/{coinId}?vsCurrency={vsCurrency}", ct);

    public async Task<OrderResult> PlaceMarketBuyAsync(PlaceMarketBuyRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync($"{_base}/api/trading/order/buy", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OrderResult>(ct)
               ?? throw new InvalidOperationException("Failed to place buy order");
    }

    public async Task<OrderResult> PlaceMarketSellAsync(PlaceMarketSellRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync($"{_base}/api/trading/order/sell", request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Binance order failed: {error}");
        }
        return await response.Content.ReadFromJsonAsync<OrderResult>(ct)
               ?? throw new InvalidOperationException("Failed to place sell order");
    }
}
