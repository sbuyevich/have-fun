namespace HaveFun.Core;

public sealed class PlayerRegistry : IPlayerRegistry
{
    private readonly object syncRoot = new();
    private readonly Dictionary<Guid, PlayerSession> playersById = [];
    private readonly HashSet<string> normalizedPlayerNames = new(StringComparer.OrdinalIgnoreCase);

    public JoinResult RegisterPlayer(string submittedName)
    {
        var displayName = submittedName.Trim();

        if (string.IsNullOrWhiteSpace(displayName))
        {
            return JoinResult.Failed("Name is required.");
        }

        lock (syncRoot)
        {
            if (normalizedPlayerNames.Contains(displayName))
            {
                return JoinResult.Failed("That player name is already in use.", displayName);
            }

            var player = new PlayerSession
            {
                Id = Guid.NewGuid(),
                DisplayName = displayName,
                JoinedAt = DateTimeOffset.UtcNow
            };

            playersById.Add(player.Id, player);
            normalizedPlayerNames.Add(displayName);

            return JoinResult.PlayerJoined(player);
        }
    }

    public bool IsPlayerNameTaken(string submittedName)
    {
        var displayName = submittedName.Trim();

        if (string.IsNullOrWhiteSpace(displayName))
        {
            return false;
        }

        lock (syncRoot)
        {
            return normalizedPlayerNames.Contains(displayName);
        }
    }

    public bool TryGetPlayer(Guid playerId, out PlayerSession? player)
    {
        lock (syncRoot)
        {
            return playersById.TryGetValue(playerId, out player);
        }
    }

    public IReadOnlyList<PlayerSession> GetPlayers()
    {
        lock (syncRoot)
        {
            return playersById.Values
                .OrderBy(player => player.JoinedAt)
                .ToArray();
        }
    }
}
