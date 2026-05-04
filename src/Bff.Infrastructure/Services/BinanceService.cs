using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Bff.Domain.Abstractions;
using Bff.Infrastructure.Options;

namespace Bff.Infrastructure.Services;

public class BinanceService(HttpClient httpClient, IOptions<ThirdPartyOptions> options) : IBinanceService
{
    private readonly string _baseUrl = options.Value.BaseUrl.TrimEnd('/');

    public async Task<Dictionary<string, double>> GetBalancesAsync()
    {
        return await httpClient.GetFromJsonAsync<Dictionary<string, double>>($"{_baseUrl}/api/Binance/balances") ?? [];
    }

    public async Task<double> GetCurrentPriceAsync(string symbol)
    {
        return await httpClient.GetFromJsonAsync<double>($"{_baseUrl}/api/Binance/price/{symbol}");
    }
}
