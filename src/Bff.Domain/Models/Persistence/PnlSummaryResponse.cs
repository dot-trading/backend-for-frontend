using System.Text.Json.Serialization;

namespace Bff.Domain.Models.Persistence;

public class PnlSummaryResponse
{
    [JsonPropertyName("today")]
    public PnlSummaryItemResponse Today { get; set; } = new();
    
    [JsonPropertyName("yesterday")]
    public PnlSummaryItemResponse Yesterday { get; set; } = new();
    
    [JsonPropertyName("thisWeek")]
    public PnlSummaryItemResponse ThisWeek { get; set; } = new();
    
    [JsonPropertyName("thisMonth")]
    public PnlSummaryItemResponse ThisMonth { get; set; } = new();
    
    [JsonPropertyName("thisYear")]
    public PnlSummaryItemResponse ThisYear { get; set; } = new();
    
    [JsonPropertyName("total")]
    public PnlSummaryItemResponse Total { get; set; } = new();
}

public class PnlSummaryItemResponse
{
    [JsonPropertyName("value")]
    public double Value { get; set; }
}
