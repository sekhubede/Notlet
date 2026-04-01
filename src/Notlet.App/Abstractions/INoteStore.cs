using Notlet.App.Domain;

namespace Notlet.App.Abstractions;

public interface INoteStore
{
    Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken ct = default);

    Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Note note, CancellationToken ct = default);

    Task UpdateAsync(Note note, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
