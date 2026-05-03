namespace HaveFun.Core;

public sealed record PlayerSession
{
    public required Guid Id { get; init; }

    public required string DisplayName { get; init; }

    public required DateTimeOffset JoinedAt { get; init; }
}
