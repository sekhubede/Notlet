namespace Notlet.App.Abstractions;

public interface INotletApplication
{
    Task<int> RunAsync(string[] args);
}