using HaveFun.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace HaveFun.Web;

public partial class NavMenu : IDisposable
{
    [Inject]
    private ICurrentUserStateService CurrentUserState { get; set; } = default!;

    [Inject]
    private IAvailableGameService AvailableGames { get; set; } = default!;

    [Inject]
    private IActiveGameStateService ActiveGameState { get; set; } = default!;

    [Inject]
    private IHubContext<GameSelectionHub> GameSelectionHubContext { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        CurrentUserState.Changed += HandleCurrentUserChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        await CurrentUserState.InitializeAsync();
    }

    private void HandleCurrentUserChanged()
    {
        _ = InvokeAsync(StateHasChanged);
    }

    private static string? GetGameHref(StoredUserSession currentUser, AvailableGame game)
    {
        return currentUser.Role == JoinRole.Master ? game.MasterRoute : null;
    }

    private async Task SelectGameAsync(StoredUserSession currentUser, AvailableGame game)
    {
        if (currentUser.Role != JoinRole.Master)
        {
            return;
        }

        if (!ActiveGameState.TrySelectGame(game.Id, out var selectedGame) || selectedGame is null)
        {
            return;
        }

        var message = new GameSelectedMessage
        {
            GameId = selectedGame.Id,
            DisplayName = selectedGame.DisplayName,
            PlayerRoute = selectedGame.PlayerRoute
        };

        await GameSelectionHubContext.Clients.All.SendAsync(GameSelectionHub.GameSelectedMethod, message);
        NavigationManager.NavigateTo(selectedGame.MasterRoute);
    }

    public void Dispose()
    {
        CurrentUserState.Changed -= HandleCurrentUserChanged;
    }
}
