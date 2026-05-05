namespace HaveFun.Web;

public sealed class CurrentUserStateService : ICurrentUserStateService
{
    private readonly IUserSessionStorageService userSessionStorage;
    private bool isInitialized;

    public CurrentUserStateService(IUserSessionStorageService userSessionStorage)
    {
        this.userSessionStorage = userSessionStorage;
    }

    public event Action? Changed;

    public StoredUserSession? CurrentUser { get; private set; }

    public async ValueTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        CurrentUser = await userSessionStorage.GetCurrentUserAsync();
        isInitialized = true;
        Changed?.Invoke();
    }

    public async ValueTask SetCurrentUserAsync(StoredUserSession userSession)
    {
        await userSessionStorage.SaveCurrentUserAsync(userSession);
        CurrentUser = userSession;
        isInitialized = true;
        Changed?.Invoke();
    }

    public async ValueTask ClearAsync()
    {
        await userSessionStorage.ClearCurrentUserAsync();
        CurrentUser = null;
        isInitialized = true;
        Changed?.Invoke();
    }
}
