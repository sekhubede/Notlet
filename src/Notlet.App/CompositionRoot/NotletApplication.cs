using Microsoft.Extensions.Logging;
using Notlet.App.Abstractions;

namespace Notlet.App.CompositionRoot;

public sealed class NotletApplication(ILogger<NotletApplication> logger) : INotletApplication
{
    public Task<int> RunAsync(string[] args)
    {
        logger.LogInformation("Notlet started");
        Console.WriteLine("Notlet initialized");
        return Task.FromResult(0);
    }
}