namespace Notlet.App.Domain;

public record Note(Guid Id, string Title, string Content, DateTime CreatedAtUtc, DateTime UpdatedAtUtc, bool IsPinned, DateTime? FinalizedAtUtc, DateTime? ExpiresAtUtc);