namespace HaveFun.Core;

public sealed class AvailableGameService : IAvailableGameService
{
    public const string WordScrambleGameId = "word-scramble";

    public AvailableGameService()
    {
        Games =
        [
            new AvailableGame
            {
                Id = WordScrambleGameId,
                DisplayName = "Word Scramble",
                MasterRoute = "/dashboard",
                PlayerRoute = "/word-scramble"
            }
        ];
    }

    public IReadOnlyList<AvailableGame> Games { get; }

    public bool TryGetGame(string gameId, out AvailableGame? game)
    {
        game = Games.FirstOrDefault(candidate =>
            candidate.Id.Equals(gameId, StringComparison.OrdinalIgnoreCase));

        return game is not null;
    }
}
