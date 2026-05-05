namespace HaveFun.Core;

public sealed class ActiveGameStateService : IActiveGameStateService
{
    private readonly IAvailableGameService availableGames;
    private readonly object syncRoot = new();
    private AvailableGame? activeGame;

    public ActiveGameStateService(IAvailableGameService availableGames)
    {
        this.availableGames = availableGames;
    }

    public AvailableGame? ActiveGame
    {
        get
        {
            lock (syncRoot)
            {
                return activeGame;
            }
        }
    }

    public bool TrySelectGame(string gameId, out AvailableGame? game)
    {
        if (!availableGames.TryGetGame(gameId, out game) || game is null)
        {
            return false;
        }

        lock (syncRoot)
        {
            activeGame = game;
        }

        return true;
    }
}
