namespace HaveFun.Core;

public sealed class GameStateService : IGameStateService
{
    private readonly object syncRoot = new();
    private readonly Dictionary<PlayerRoundKey, PlayerRoundState> playerRoundStates = [];
    private CurrentRound? currentRound;

    public event Action<CurrentRound>? CurrentRoundChanged;

    public event Action<PlayerRoundState>? PlayerRoundStateChanged;

    public CurrentRound? CurrentRound
    {
        get
        {
            lock (syncRoot)
            {
                return currentRound;
            }
        }
    }

    public CurrentRound StartRound(SentenceDefinition sentence)
    {
        if (string.IsNullOrWhiteSpace(sentence.Text))
        {
            throw new ArgumentException("Sentence text is required.", nameof(sentence));
        }

        if (sentence.TimeLimitInSeconds <= 0)
        {
            throw new ArgumentException("Sentence time limit must be greater than zero.", nameof(sentence));
        }

        var originalWords = SplitWords(sentence.Text);
        var shuffledWords = ShuffleWords(originalWords);
        var round = new CurrentRound
        {
            Id = Guid.NewGuid(),
            SentenceText = sentence.Text,
            TimeLimitInSeconds = sentence.TimeLimitInSeconds,
            OriginalWords = originalWords,
            ShuffledWords = shuffledWords,
            Status = RoundStatus.Started,
            StartedAt = DateTimeOffset.UtcNow
        };

        lock (syncRoot)
        {
            currentRound = round;
            playerRoundStates.Clear();
        }

        CurrentRoundChanged?.Invoke(round);

        return round;
    }

    public PlayerRoundState? GetPlayerRoundState(string playerName)
    {
        var normalizedName = NormalizePlayerName(playerName);

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return null;
        }

        lock (syncRoot)
        {
            return currentRound is null
                ? null
                : GetPlayerRoundStateUnsafe(currentRound.Id, normalizedName);
        }
    }

    public PlayerRoundState? GetOrCreatePlayerRoundState(string playerName)
    {
        var normalizedName = NormalizePlayerName(playerName);

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return null;
        }

        lock (syncRoot)
        {
            if (currentRound is null)
            {
                return null;
            }

            var key = new PlayerRoundKey(currentRound.Id, normalizedName);

            if (playerRoundStates.TryGetValue(key, out var playerRoundState))
            {
                return playerRoundState;
            }

            playerRoundState = new PlayerRoundState
            {
                PlayerName = normalizedName,
                RoundId = currentRound.Id,
                AvailableWords = currentRound.ShuffledWords
                    .Select(word => new WordTile
                    {
                        Id = Guid.NewGuid(),
                        Text = word
                    })
                    .ToArray(),
                CollectedWords = []
            };

            playerRoundStates.Add(key, playerRoundState);

            return playerRoundState;
        }
    }

    public PlayerRoundState? SelectWord(string playerName, Guid wordId)
    {
        var normalizedName = NormalizePlayerName(playerName);
        PlayerRoundState? updatedState;

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return null;
        }

        lock (syncRoot)
        {
            if (currentRound is null)
            {
                return null;
            }

            var playerRoundState = GetOrCreatePlayerRoundStateUnsafe(currentRound, normalizedName);

            if (playerRoundState.IsSubmitted)
            {
                return playerRoundState;
            }

            var selectedWord = playerRoundState.AvailableWords.FirstOrDefault(word => word.Id == wordId);

            if (selectedWord is null)
            {
                return playerRoundState;
            }

            updatedState = playerRoundState with
            {
                AvailableWords = playerRoundState.AvailableWords
                    .Where(word => word.Id != wordId)
                    .ToArray(),
                CollectedWords = playerRoundState.CollectedWords
                    .Append(selectedWord)
                    .ToArray()
            };

            playerRoundStates[new PlayerRoundKey(currentRound.Id, normalizedName)] = updatedState;
        }

        PlayerRoundStateChanged?.Invoke(updatedState);

        return updatedState;
    }

    public PlayerRoundState? SubmitPlayerRound(string playerName)
    {
        var normalizedName = NormalizePlayerName(playerName);
        PlayerRoundState? updatedState;

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return null;
        }

        lock (syncRoot)
        {
            if (currentRound?.StartedAt is null)
            {
                return null;
            }

            var playerRoundState = GetOrCreatePlayerRoundStateUnsafe(currentRound, normalizedName);

            if (playerRoundState.IsSubmitted)
            {
                return playerRoundState;
            }

            if (!playerRoundState.CanSubmit)
            {
                return playerRoundState;
            }

            var submittedAt = DateTimeOffset.UtcNow;
            updatedState = playerRoundState with
            {
                IsSubmitted = true,
                SubmittedSentence = playerRoundState.CollectedSentence,
                SubmittedAt = submittedAt,
                SpentTime = submittedAt - currentRound.StartedAt.Value
            };

            playerRoundStates[new PlayerRoundKey(currentRound.Id, normalizedName)] = updatedState;
        }

        PlayerRoundStateChanged?.Invoke(updatedState);

        return updatedState;
    }

    public IReadOnlyList<PlayerRoundState> GetSubmittedPlayerRoundStates()
    {
        lock (syncRoot)
        {
            if (currentRound is null)
            {
                return [];
            }

            return playerRoundStates.Values
                .Where(playerRoundState => playerRoundState.RoundId == currentRound.Id && playerRoundState.IsSubmitted)
                .OrderBy(playerRoundState => playerRoundState.SubmittedAt)
                .ToArray();
        }
    }

    private static IReadOnlyList<string> SplitWords(string sentenceText)
    {
        return sentenceText
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();
    }

    private static IReadOnlyList<string> ShuffleWords(IReadOnlyList<string> words)
    {
        var shuffledWords = words.ToArray();

        for (var index = shuffledWords.Length - 1; index > 0; index--)
        {
            var swapIndex = Random.Shared.Next(index + 1);
            (shuffledWords[index], shuffledWords[swapIndex]) = (shuffledWords[swapIndex], shuffledWords[index]);
        }

        return shuffledWords;
    }

    private PlayerRoundState? GetPlayerRoundStateUnsafe(Guid roundId, string playerName)
    {
        return playerRoundStates.TryGetValue(new PlayerRoundKey(roundId, playerName), out var playerRoundState)
            ? playerRoundState
            : null;
    }

    private PlayerRoundState GetOrCreatePlayerRoundStateUnsafe(CurrentRound round, string playerName)
    {
        var key = new PlayerRoundKey(round.Id, playerName);

        if (playerRoundStates.TryGetValue(key, out var playerRoundState))
        {
            return playerRoundState;
        }

        playerRoundState = new PlayerRoundState
        {
            PlayerName = playerName,
            RoundId = round.Id,
            AvailableWords = round.ShuffledWords
                .Select(word => new WordTile
                {
                    Id = Guid.NewGuid(),
                    Text = word
                })
                .ToArray(),
            CollectedWords = []
        };

        playerRoundStates.Add(key, playerRoundState);

        return playerRoundState;
    }

    private static string NormalizePlayerName(string playerName)
    {
        return playerName.Trim();
    }

    private sealed record PlayerRoundKey(Guid RoundId, string PlayerName);
}
