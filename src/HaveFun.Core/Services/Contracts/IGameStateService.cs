namespace HaveFun.Core;

public interface IGameStateService
{
    event Action<CurrentRound>? CurrentRoundChanged;

    event Action<PlayerRoundState>? PlayerRoundStateChanged;

    CurrentRound? CurrentRound { get; }

    CurrentRound StartRound(SentenceDefinition sentence);

    PlayerRoundState? GetPlayerRoundState(string playerName);

    PlayerRoundState? GetOrCreatePlayerRoundState(string playerName);

    PlayerRoundState? SelectWord(string playerName, Guid wordId);

    PlayerRoundState? SubmitPlayerRound(string playerName);

    IReadOnlyList<PlayerRoundState> GetSubmittedPlayerRoundStates();
}
