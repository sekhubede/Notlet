using Microsoft.Extensions.Logging.Abstractions;
using Notlet.App.CompositionRoot;
using Notlet.App.Abstractions;

namespace Notlet.App.Tests;

public class NotletApplicationTests
{
    [Fact]
    public async Task RunAsync_ReturnsSuccess_WhenStartupCompletes()
    {
        // Arrange
        var notletApplication = new NotletApplication(NullLogger<NotletApplication>.Instance);

        // Act
        var result = await notletApplication.RunAsync([]);

        // Assert
        Assert.Equal((int)AppExitCode.Success, result);
    }

    [Fact]
    public async Task RunAsync_ReturnsValidationError_WhenArgsContainEmptyValue()
    {
        // Arrange
        var notletApplication = new NotletApplication(NullLogger<NotletApplication>.Instance);

        // Act
        var result = await notletApplication.RunAsync([""]);

        // Assert
        Assert.Equal((int)AppExitCode.ValidationOrUsageError, result);
    }

    [Fact]
    public async Task RunAsync_ReturnsValidationError_WhenArgsHaveInvalidFormat()
    {
        // Arrange
        var notletApplication = new NotletApplication(NullLogger<NotletApplication>.Instance);

        // Act
        var result = await notletApplication.RunAsync(["--bad-format"]);

        // Assert
        Assert.Equal((int)AppExitCode.ValidationOrUsageError, result);
    }
}