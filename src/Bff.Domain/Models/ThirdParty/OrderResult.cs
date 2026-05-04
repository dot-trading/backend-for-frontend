namespace Bff.Domain.Models.ThirdParty;

public record OrderResult(string OrderId, double ExecutedQty, double CumulativeQuoteQty, double Price);
