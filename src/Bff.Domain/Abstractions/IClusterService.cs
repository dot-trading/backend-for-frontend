using Bff.Domain.Enums;

namespace Bff.Domain.Abstractions;

public interface IClusterService
{
    Task<ServiceStatus> GetOrchestratorStatusAsync();
    Task<ServiceStatus> GetOllamaStatusAsync();
    Task<ServiceStatus> GetPersistenceStatusAsync();
    Task<List<OllamaModel>> GetOllamaModelsAsync();
    Task<List<PodStatus>> GetK8sPodsStatusAsync();
}

public record OllamaModel(string Name, long Size, string ModifiedAt);
public record PodStatus(string Name, ServiceStatus Status, string Node);
