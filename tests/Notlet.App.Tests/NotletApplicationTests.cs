using Microsoft.Extensions.Logging.Abstractions;
using Notlet.App.CompositionRoot;
using Notlet.App.Abstractions;

namespace Notlet.App.Tests;

public class NotletApplicationTests
{
    [Fact]
    public async Task RunAsync_ReturnsSuccess_WhenStartupCompletes()
    {
        var notletApplication = new NotletApplication(NullLogger<NotletApplication>.Instance);

        var result = await notletApplication.RunAsync([]);

        Assert.Equal((int)AppExitCode.Success, result);
    }

    [Fact]
    public async Task RunAsync_ReturnsValidationError_WhenArgsContainEmptyValue()
    {
        var notletApplication = new NotletApplication(NullLogger<NotletApplication>.Instance);

        var result = await notletApplication.RunAsync([""]);

        Assert.Equal((int)AppExitCode.ValidationOrUsageError, result);
    }

    [Fact]
    public async Task RunAsync_ReturnsValidationError_WhenArgsHaveInvalidFormat()
    {
        var notletApplication = new NotletApplication(NullLogger<NotletApplication>.Instance);

        var result = await notletApplication.RunAsync(["--bad-format"]);

        Assert.Equal((int)AppExitCode.ValidationOrUsageError, result);
    }
}