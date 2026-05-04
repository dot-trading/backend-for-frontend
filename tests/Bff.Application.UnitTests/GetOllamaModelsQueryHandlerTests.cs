using Bff.Application.Features.Ollama.Queries;
using Bff.Domain.Abstractions;
using Moq;
using Xunit;

namespace Bff.Application.UnitTests;

public class GetOllamaModelsQueryHandlerTests
{
    private readonly Mock<IClusterService> _clusterMock = new();

    [Fact]
    public async Task Handle_ShouldReturnModels()
    {
        var model = new OllamaModel("llama3", 4000000000, "2024-05-01");
        _clusterMock.Setup(x => x.GetOllamaModelsAsync()).ReturnsAsync(new List<OllamaModel> { model });
        var handler = new GetOllamaModelsQueryHandler(_clusterMock.Object);

        var result = await handler.Handle(new GetOllamaModelsQuery(), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("llama3", result[0].Name);
    }
}
