namespace Bff.Domain.Models.ThirdParty;

public record PlaceMarketBuyRequest(string Symbol, double QuoteOrderQty);
public record PlaceMarketSellRequest(string Symbol, double Quantity);
