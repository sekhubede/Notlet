using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notlet.App.Abstractions;
using Notlet.App.Domain;

namespace Notlet.App.Infrastructure.Storage;

public sealed class JsonNoteStore : INoteStore
{
    private readonly string _filePath;
    private readonly ILogger<JsonNoteStore> _logger;

    public JsonNoteStore(IOptions<NotletStorageOptions> options, ILogger<JsonNoteStore> logger)
    {
        _logger = logger;

        var configuredPath = options.Value.FilePath;

        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            throw new ArgumentException("Storage file path is not configured.", nameof(options));
        }

        _filePath = Path.GetFullPath(configuredPath);
    }

    public async Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken ct = default)
    {
        return await ReadAllAsync(ct);
    }

    public async Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var notes = await ReadAllAsync(ct);
        return notes.FirstOrDefault(n => n.Id == id);
    }

    public async Task AddAsync(Note note, CancellationToken ct = default)
    {
        ValidateNote(note);

        var notes = await ReadAllAsync(ct);

        if (notes.Any(n => n.Id == note.Id))
        {
            _logger.LogWarning("Note with id {Id} already exists. No add performed.", note.Id);
            throw new InvalidOperationException($"Note with id {note.Id} already exists.");
        }

        notes.Add(note);
        await WriteAllAsync(notes, ct);
    }

    public async Task UpdateAsync(Note note, CancellationToken ct = default)
    {
        ValidateNote(note);

        var notes = await ReadAllAsync(ct);

        if (notes.RemoveAll(n => n.Id == note.Id) == 0)
        {
            _logger.LogWarning("Note with id {Id} not found. No update performed.", note.Id);
            throw new KeyNotFoundException($"Note with id {note.Id} not found.");
        }
        notes.Add(note);
        await WriteAllAsync(notes, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var notes = await ReadAllAsync(ct);
        if (notes.RemoveAll(n => n.Id == id) == 0)
        {
            _logger.LogWarning("Note with id {Id} not found. No delete performed.", id);
            return;
        }
        await WriteAllAsync(notes, ct);
        _logger.LogInformation("Note with id {Id} deleted.", id);
    }

    private void ValidateNote(Note note)
    {
        if (note == null)
        {
            _logger.LogWarning("Note is null. Validation failed.");
            throw new ArgumentNullException(nameof(note));
        }

        if (string.IsNullOrWhiteSpace(note.Title))
        {
            _logger.LogWarning("Note title is null or empty. Validation failed.");
            throw new ArgumentException("Note title is null or empty.", nameof(note));
        }

        if (string.IsNullOrWhiteSpace(note.Content))
        {
            _logger.LogWarning("Note content is null or empty. Validation failed.");
            throw new ArgumentException("Note content is null or empty.", nameof(note));
        }
    }

    private async Task<List<Note>> ReadAllAsync(CancellationToken ct)
    {
        if (!File.Exists(_filePath))
        {
            _logger.LogWarning("Storage file not found at {FilePath}. Returning empty list.", _filePath);
            return [];
        }

        try
        {
            var fileContent = await File.ReadAllTextAsync(_filePath, ct);

            if (string.IsNullOrWhiteSpace(fileContent))
            {
                _logger.LogWarning("Storage file at {FilePath} is empty. Returning empty list.", _filePath);
                return [];
            }

            return JsonSerializer.Deserialize<List<Note>>(fileContent) ?? [];
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error reading storage file at {FilePath}.", _filePath);
            throw;
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Error reading storage file at {FilePath}.", _filePath);
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation cancelled while reading storage file at {FilePath}.", _filePath);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error reading storage file at {FilePath}.", _filePath);
            throw;
        }
    }

    private async Task WriteAllAsync(List<Note> notes, CancellationToken ct)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            _logger.LogWarning("Storage directory not found at {Directory}. Creating it.", directory);
            Directory.CreateDirectory(directory);
        }

        try
        {
            var fileContent = JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, fileContent, ct);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error serializing storage file at {FilePath}.", _filePath);
            throw;
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Error writing storage file at {FilePath}.", _filePath);
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation cancelled while writing storage file at {FilePath}.", _filePath);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error writing storage file at {FilePath}.", _filePath);
            throw;
        }
    }
}