namespace HaveFun.Web;

public sealed record StoredPlayerSession
{
    public required Guid PlayerId { get; init; }

    public required string DisplayName { get; init; }
}
