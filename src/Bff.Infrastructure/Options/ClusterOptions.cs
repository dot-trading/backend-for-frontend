namespace Bff.Infrastructure.Options;

public class ClusterOptions
{
    public string OllamaUrl { get; set; } = "http://localhost:11434";
    public string PersistenceServiceUrl { get; set; } = "http://localhost:5000";
}
