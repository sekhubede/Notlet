using Microsoft.Extensions.Logging;
using Notlet.App.Abstractions;

namespace Notlet.App.CompositionRoot;

public sealed class NotletApplication(ILogger<NotletApplication> logger) : INotletApplication
{
    public Task<int> RunAsync(string[] args)
    {
        try
        {
            if (args.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Arguments cannot be empty.");
            }

            if (args.Contains("--bad-format", StringComparer.Ordinal))
            {
                throw new FormatException("Invalid argument format.");
            }

            logger.LogInformation("Notlet started");
            Console.WriteLine("Notlet initialized");
            return Task.FromResult((int)AppExitCode.Success);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "User input/argument error.");
            Console.WriteLine("Input was invalid. Please check your values and try again.");
            return Task.FromResult((int)AppExitCode.ValidationOrUsageError);
        }
        catch (FormatException ex)
        {
            logger.LogWarning(ex, "Parsing/format error.");
            Console.WriteLine("Input format was not valid. Please check and try again.");
            return Task.FromResult((int)AppExitCode.ValidationOrUsageError);
        }

    }
}