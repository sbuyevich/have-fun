namespace HaveFun.Core;

public interface ISessionStorageService
{
    ValueTask<SessionStorageModel?> GetCurrentUserAsync();

    ValueTask SaveCurrentUserAsync(SessionStorageModel userSession);

    ValueTask ClearCurrentUserAsync();
}
