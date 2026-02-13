using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;
var builder = WebApplication.CreateBuilder(args);

// Serilog: logs estruturados (ótimo pra Fluentd/Loki depois)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("App", "SchedPay.Api")
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.GrafanaLoki(
    "http://loki:3100",
    labels: new List<LokiLabel>
    {
        new LokiLabel { Key = "job", Value = "schedpay" },
        new LokiLabel { Key = "app", Value = "schedpay-api" }
    })
    .CreateLogger();
Log.Information("SchedPay API starting up...");

builder.Host.UseSerilog();

// Controllers + OpenAPI 
builder.Services.AddControllers();
builder.Services.AddOpenApi();


builder.Services.AddTransient<SchedPay.Api.Middlewares.CorrelationIdMiddleware>();
var app = builder.Build();

app.UseMiddleware<SchedPay.Api.Middlewares.CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();


// Log por request (status + tempo)
app.UseSerilogRequestLogging(opts =>
{
    opts.EnrichDiagnosticContext = (ctx, http) =>
    {
        ctx.Set("TraceId", http.TraceIdentifier);
        ctx.Set("RemoteIp", http.Connection.RemoteIpAddress?.ToString());
        ctx.Set("UserAgent", http.Request.Headers.UserAgent.ToString());
    };
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
