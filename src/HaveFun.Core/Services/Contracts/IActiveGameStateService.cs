namespace HaveFun.Core;

public interface IActiveGameStateService
{
    AvailableGame? ActiveGame { get; }

    bool TrySelectGame(string gameId, out AvailableGame? game);
}
