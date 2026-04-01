using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Notlet.App.Domain;
using Notlet.App.Infrastructure.Storage;

namespace Notlet.App.Tests;

public sealed class JsonNoteStoreTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _tempFilePath;

    public JsonNoteStoreTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "NotletTests", Guid.NewGuid().ToString("N"));
        _tempFilePath = Path.Combine(_tempDirectory, "notes.json");
    }

    [Fact]
    public async Task AddAsync_PersistsNote_WhenValid()
    {
        var noteStore = CreateStore();
        var note = NewNote(title: "Test", content: "Body");

        await noteStore.AddAsync(note, TestContext.Current.CancellationToken);

        var savedNote = await noteStore.GetByIdAsync(note.Id, TestContext.Current.CancellationToken);
        Assert.Equal(note, savedNote);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMissing()
    {
        var noteStore = CreateStore();

        var result = await noteStore.GetByIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFound_WhenMissing()
    {
        var noteStore = CreateStore();
        var note = NewNote("Missing", "Nope");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => noteStore.UpdateAsync(note, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task DeleteAsync_DoesNotThrow_WhenMissing()
    {
        var noteStore = CreateStore();

        await noteStore.DeleteAsync(Guid.NewGuid(), TestContext.Current.CancellationToken); // should be no-op
    }

    [Fact]
    public async Task AddAsync_ThrowsArgumentException_WhenTitleEmpty()
    {
        var noteStore = CreateStore();
        var note = NewNote(title: "", content: "Body");

        await Assert.ThrowsAsync<ArgumentException>(() => noteStore.AddAsync(note, TestContext.Current.CancellationToken));
    }

    private JsonNoteStore CreateStore()
    {
        var options = Options.Create(new NotletStorageOptions { FilePath = _tempFilePath });
        return new JsonNoteStore(options, NullLogger<JsonNoteStore>.Instance);
    }

    private static Note NewNote(string title, string content) =>
        new(
            Guid.NewGuid(),
            title,
            content,
            DateTime.UtcNow,
            DateTime.UtcNow,
            false,
            null,
            null);

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }
}