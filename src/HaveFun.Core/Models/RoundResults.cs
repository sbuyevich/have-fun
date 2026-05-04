namespace HaveFun.Core;

public sealed record RoundResults
{
    public required Guid RoundId { get; init; }

    public required string CorrectSentence { get; init; }

    public required IReadOnlyList<PlayerResult> Results { get; init; }
}
