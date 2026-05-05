namespace HaveFun.Core;

public interface IAvailableGameService
{
    IReadOnlyList<AvailableGame> Games { get; }

    bool TryGetGame(string gameId, out AvailableGame? game);
}
