namespace HaveFun.Web;

public sealed record GameSelectedMessage
{
    public required string GameId { get; init; }

    public required string DisplayName { get; init; }

    public required string PlayerRoute { get; init; }
}
