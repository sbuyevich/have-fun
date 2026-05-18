namespace HaveFun.Core;

public sealed record SessionStorageModel
{
    public required string Name { get; init; }

    public required JoinRole Role { get; init; }
}
