using Bff.Domain.Models.ThirdParty;

namespace Bff.Domain.Abstractions;

public interface IThirdPartyService
{
    Task<Dictionary<string, double>> GetBalancesAsync(CancellationToken ct = default);
    Task<double> GetPriceAsync(string symbol, CancellationToken ct = default);
    Task<double> GetMinNotionalAsync(string symbol, CancellationToken ct = default);
    Task<List<Kline>> GetKlinesAsync(string symbol, string interval = "1h", int limit = 24, CancellationToken ct = default);
    Task<Ticker24h?> GetTicker24hAsync(string symbol, CancellationToken ct = default);
    Task<FearAndGreedIndex> GetFearAndGreedAsync(CancellationToken ct = default);
    Task<double> GetCoinGeckoPriceAsync(string coinId, string vsCurrency = "usd", CancellationToken ct = default);
    Task<OrderResult> PlaceMarketBuyAsync(PlaceMarketBuyRequest request, CancellationToken ct = default);
    Task<OrderResult> PlaceMarketSellAsync(PlaceMarketSellRequest request, CancellationToken ct = default);
}
