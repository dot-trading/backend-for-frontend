using Bff.Web.Services;
using TradingProject.Persistence.Api.Stubs.Stubs;
using TradingProject.Persistence.Api.Stubs.V1;
using TradingProject.ThirdParty.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddControllers();
builder.Services.AddThirdPartyApiClient(builder.Configuration);

// Register Persistence API stubs (in-memory implementations)
builder.Services.AddScoped<ITradesApi, TradesApiStub>();
builder.Services.AddScoped<IOpportunitiesApi, OpportunitiesApiStub>();
builder.Services.AddScoped<IPortfolioSnapshotsApi, PortfolioSnapshotsApiStub>();

// Register the notification aggregation service
builder.Services.AddScoped<INotificationAggregationService, NotificationAggregationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapControllers();
app.MapGet("/api/Status", () => Results.Ok("Healthy"));

app.Run();
