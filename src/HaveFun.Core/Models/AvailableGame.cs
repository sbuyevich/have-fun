namespace HaveFun.Core;

public sealed record AvailableGame
{
    public required string Id { get; init; }

    public required string DisplayName { get; init; }

    public required string MasterRoute { get; init; }

    public required string PlayerRoute { get; init; }
}
