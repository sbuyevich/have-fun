namespace HaveFun.Web;

public interface ICurrentUserStateService
{
    event Action? Changed;

    StoredUserSession? CurrentUser { get; }

    ValueTask InitializeAsync();

    ValueTask SetCurrentUserAsync(StoredUserSession userSession);

    ValueTask ClearAsync();
}
