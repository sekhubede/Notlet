namespace Notlet.App.Abstractions;

public enum AppExitCode
{
    Success = 0,
    UnexpectedError = 1,
    ValidationOrUsageError = 2,
    Cancelled = 3
}