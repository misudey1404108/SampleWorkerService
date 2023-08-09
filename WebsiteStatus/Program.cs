using WebsiteStatus;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
.Enrich.FromLogContext()
.WriteTo.File(@"C:\temp\LogFile.txt")
.CreateLogger();
try
{
    Log.Information("Starting service");
    IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

    await host.RunAsync();
}
catch(Exception ex)
{
    Log.Fatal(ex, "there was an error");
}
finally
{
    Log.CloseAndFlush();
}

