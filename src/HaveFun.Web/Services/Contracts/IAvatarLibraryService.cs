namespace HaveFun.Web;

public interface IAvatarLibraryService
{
    string DefaultPlayerAvatarFileName { get; }

    IReadOnlyList<AvatarOption> Avatars { get; }

    string GetAvatarUrl(string? avatarFileName);

    bool IsKnownAvatar(string? avatarFileName);

    bool TryGetAvatarFilePath(string avatarFileName, out string? filePath);
}
