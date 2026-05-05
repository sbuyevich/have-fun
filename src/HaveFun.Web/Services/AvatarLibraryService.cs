namespace HaveFun.Web;

public sealed class AvatarLibraryService : IAvatarLibraryService
{
    private static readonly string[] SupportedExtensions = [".svg", ".png", ".jpg", ".jpeg", ".webp"];
    private readonly Dictionary<string, string> avatarFilePaths;

    public AvatarLibraryService(string avatarFolderPath)
    {
        if (!Directory.Exists(avatarFolderPath))
        {
            throw new DirectoryNotFoundException($"Avatar folder was not found: {avatarFolderPath}");
        }

        avatarFilePaths = Directory
            .EnumerateFiles(avatarFolderPath)
            .Where(filePath => SupportedExtensions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase))
            .ToDictionary(filePath => Path.GetFileName(filePath), StringComparer.OrdinalIgnoreCase);

        if (avatarFilePaths.Count == 0)
        {
            throw new InvalidOperationException($"Avatar folder must contain at least one avatar image: {avatarFolderPath}");
        }

        DefaultPlayerAvatarFileName = avatarFilePaths.ContainsKey("player-1.svg")
            ? "player-1.svg"
            : avatarFilePaths.Keys.Order(StringComparer.OrdinalIgnoreCase).First();

        Avatars = avatarFilePaths.Keys
            .Order(StringComparer.OrdinalIgnoreCase)
            .Select(fileName => new AvatarOption
            {
                FileName = fileName,
                Url = GetAvatarUrl(fileName),
                DisplayName = Path.GetFileNameWithoutExtension(fileName).Replace('-', ' ')
            })
            .ToArray();
    }

    public string DefaultPlayerAvatarFileName { get; }

    public IReadOnlyList<AvatarOption> Avatars { get; }

    public string GetAvatarUrl(string? avatarFileName)
    {
        var resolvedAvatarFileName = IsKnownAvatar(avatarFileName)
            ? avatarFileName!.Trim()
            : DefaultPlayerAvatarFileName;

        return $"/assets/avatars/{Uri.EscapeDataString(resolvedAvatarFileName)}";
    }

    public bool IsKnownAvatar(string? avatarFileName)
    {
        return !string.IsNullOrWhiteSpace(avatarFileName) &&
            avatarFilePaths.ContainsKey(avatarFileName.Trim());
    }

    public bool TryGetAvatarFilePath(string avatarFileName, out string? filePath)
    {
        if (string.IsNullOrWhiteSpace(avatarFileName))
        {
            filePath = null;
            return false;
        }

        return avatarFilePaths.TryGetValue(avatarFileName.Trim(), out filePath);
    }
}
