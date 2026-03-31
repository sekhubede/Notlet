using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notlet.App.Abstractions;
using Notlet.App.CompositionRoot;

using var loggerFactory = LoggerFactory.Create(b =>
{
    b.AddConsole();
});
var programLogger = loggerFactory.CreateLogger("Notlet.Program");

try
{
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        })
        .ConfigureServices(services =>
        {
            services.AddSingleton<INotletApplication, NotletApplication>();
        })
        .Build();

    var app = host.Services.GetRequiredService<INotletApplication>();
    return await app.RunAsync(args);
}
catch (OperationCanceledException)
{
    programLogger.LogWarning("Execution cancelled.");
    Console.WriteLine("Operation cancelled.");
    return (int)AppExitCode.Cancelled;
}
catch (Exception ex)
{
    programLogger.LogError(ex, "Unhandled exception.");
    Console.WriteLine("Something went wrong. Please try again.");
    return (int)AppExitCode.UnexpectedError;
}