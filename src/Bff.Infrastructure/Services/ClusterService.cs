using System.Text.Json;
using Microsoft.Extensions.Options;
using Bff.Domain.Abstractions;
using Bff.Domain.Enums;
using Bff.Infrastructure.Options;
using k8s;

namespace Bff.Infrastructure.Services;

public class ClusterService(IOptions<ClusterOptions> options) : IClusterService
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(5) };
    private readonly IKubernetes _client = new Kubernetes(KubernetesClientConfiguration.BuildDefaultConfig());

    public async Task<ServiceStatus> GetOrchestratorStatusAsync()
    {
        try
        {
            var pods = await _client.CoreV1.ListNamespacedPodAsync("trading-ai", labelSelector: "app=orchestrator");
            if (pods.Items.Count == 0) return ServiceStatus.Offline;
            
            return MapPhaseToStatus(pods.Items[0].Status.Phase);
        }
        catch
        {
            return ServiceStatus.Error;
        }
    }

    public async Task<ServiceStatus> GetOllamaStatusAsync()
    {
        try
        {
            var response = await Http.GetAsync($"{options.Value.OllamaUrl}/api/tags");
            return response.IsSuccessStatusCode ? ServiceStatus.Online : ServiceStatus.Offline;
        }
        catch
        {
            return ServiceStatus.Error;
        }
    }

    public async Task<ServiceStatus> GetPersistenceStatusAsync()
    {
        try
        {
            var response = await Http.GetAsync($"{options.Value.PersistenceServiceUrl}/health");
            return response.IsSuccessStatusCode ? ServiceStatus.Online : ServiceStatus.Offline;
        }
        catch
        {
            return ServiceStatus.Error;
        }
    }

    public async Task<List<OllamaModel>> GetOllamaModelsAsync()
    {
        try
        {
            var response = await Http.GetAsync($"{options.Value.OllamaUrl}/api/tags");
            response.EnsureSuccessStatusCode();
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return doc.RootElement.GetProperty("models")
                .EnumerateArray()
                .Select(m => new OllamaModel(
                    m.GetProperty("name").GetString() ?? "",
                    m.TryGetProperty("size", out var s) ? s.GetInt64() : 0,
                    m.TryGetProperty("modified_at", out var d) ? d.GetString() ?? "" : ""))
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    public async Task<List<PodStatus>> GetK8sPodsStatusAsync()
    {
        try
        {
            var pods = await _client.CoreV1.ListNamespacedPodAsync("trading-ai");
            return pods.Items.Select(p => new PodStatus(
                p.Metadata.Name,
                MapPhaseToStatus(p.Status.Phase),
                p.Spec.NodeName
            )).ToList();
        }
        catch
        {
            return [];
        }
    }

    private static ServiceStatus MapPhaseToStatus(string phase) => phase switch
    {
        "Running" => ServiceStatus.Online,
        "Pending" => ServiceStatus.Pending,
        "Failed" => ServiceStatus.Offline,
        _ => ServiceStatus.Unknown
    };
}
