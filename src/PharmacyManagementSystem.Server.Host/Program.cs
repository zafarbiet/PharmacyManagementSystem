using Serilog;
using PharmacyManagementSystem.Server.Host;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting PharmacyManagementSystem Host.");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
                     .ReadFrom.Services(services)
                     .Enrich.FromLogContext());

    builder.Services.AddPharmacyManagementServices(builder.Configuration);
    builder.Services.AddOpenApi();

    var app = builder.Build();

    app.MapOpenApi();

    app.UseSerilogRequestLogging();
    app.AddMinimalApis();

    app.RunMigrations();

    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Fatal(ex, "PharmacyManagementSystem Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
