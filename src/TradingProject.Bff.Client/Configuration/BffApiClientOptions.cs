using System.ComponentModel.DataAnnotations;

namespace TradingProject.Bff.Client.Configuration;

/// <summary>
/// Configuration options for the <c>TradingProject.Bff.Client</c>.
/// Bind from <c>IConfiguration</c> section <c>"BffApi"</c>.
/// </summary>
public class BffApiClientOptions
{
    /// <summary>Configuration section name in appsettings.json / environment variables.</summary>
    public const string SectionName = "BffApi";

    /// <summary>
    /// Base URL of the BFF API (e.g. <c>http://trading-bff-api/api</c>).
    /// </summary>
    [Required]
    [Url]
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Optional API key sent as the <c>X-Api-Key</c> header.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Request timeout in seconds. Defaults to 30.
    /// </summary>
    [Range(1, 300)]
    public int TimeoutSeconds { get; set; } = 30;
}
