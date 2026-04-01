namespace Notlet.App.Infrastructure.Storage;

public sealed class NotletStorageOptions
{
    public const string SectionName = "Notlet:Storage";

    public string Provider { get; init; } = "Json";
    public string FilePath { get; init; } = "data/notes.json";
}