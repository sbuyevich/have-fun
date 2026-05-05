namespace HaveFun.Web;

public sealed record AvatarOption
{
    public required string FileName { get; init; }

    public required string Url { get; init; }

    public required string DisplayName { get; init; }
}
