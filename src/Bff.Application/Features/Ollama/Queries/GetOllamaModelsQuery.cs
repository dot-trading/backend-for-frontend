using MediatR;
using Bff.Domain.Abstractions;

namespace Bff.Application.Features.Ollama.Queries;

public record GetOllamaModelsQuery : IRequest<List<OllamaModelDto>>;

public record OllamaModelDto(string Name, long Size, string ModifiedAt);

public class GetOllamaModelsQueryHandler(IClusterService cluster) : IRequestHandler<GetOllamaModelsQuery, List<OllamaModelDto>>
{
    public async Task<List<OllamaModelDto>> Handle(GetOllamaModelsQuery request, CancellationToken cancellationToken)
    {
        var models = await cluster.GetOllamaModelsAsync();
        return models.Select(m => new OllamaModelDto(m.Name, m.Size, m.ModifiedAt)).ToList();
    }
}
