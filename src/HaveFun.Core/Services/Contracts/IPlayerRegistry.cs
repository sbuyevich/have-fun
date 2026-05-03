namespace HaveFun.Core;

public interface IPlayerRegistry
{
    JoinResult RegisterPlayer(string submittedName);

    bool IsPlayerNameTaken(string submittedName);

    bool TryGetPlayer(Guid playerId, out PlayerSession? player);

    IReadOnlyList<PlayerSession> GetPlayers();
}
