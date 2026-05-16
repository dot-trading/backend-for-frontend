using TradingProject.Persistence.Api.Stubs.Stubs;
using TradingProject.Persistence.Api.Stubs.V1;
using TradingProject.ThirdParty.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddThirdPartyApiClient(builder.Configuration);

// Register Persistence API stubs (in-memory implementations)
builder.Services.AddSingleton<ITradesApi, TradesApiStub>();
builder.Services.AddSingleton<IOpportunitiesApi, OpportunitiesApiStub>();
builder.Services.AddSingleton<IPortfolioSnapshotsApi, PortfolioSnapshotsApiStub>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapControllers();
app.MapGet("/api/Status", () => Results.Ok("Healthy"));

app.Run();
