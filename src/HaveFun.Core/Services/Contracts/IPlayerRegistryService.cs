namespace HaveFun.Core;

public interface IPlayerRegistryService
{
    JoinResult RegisterPlayer(string submittedName);

    bool IsPlayerNameTaken(string submittedName);

    bool TryGetPlayer(Guid playerId, out PlayerSession? player);

    bool TryGetPlayerByName(string submittedName, out PlayerSession? player);

    bool UpdatePlayerAvatar(string submittedName, string avatarFileName);

    IReadOnlyList<PlayerSession> GetPlayers();
}
