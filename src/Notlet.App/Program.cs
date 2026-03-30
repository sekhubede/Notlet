using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notlet.App.Abstractions;
using Notlet.App.CompositionRoot;

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
var exitCode = await app.RunAsync(args);

return exitCode;