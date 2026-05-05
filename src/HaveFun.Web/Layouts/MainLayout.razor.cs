using HaveFun.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace HaveFun.Web;

public partial class MainLayout : IAsyncDisposable
{
    private HubConnection? gameSelectionConnection;
    private bool isNavigationOpen = true;

    private string CurrentUserName => CurrentUserState.CurrentUser?.Name ?? string.Empty;

    private string CurrentUserAvatarUrl => AvatarLibrary.GetAvatarUrl(CurrentUserState.CurrentUser?.AvatarFileName);

    [Inject]
    private ICurrentUserStateService CurrentUserState { get; set; } = default!;

    [Inject]
    private IAvatarLibraryService AvatarLibrary { get; set; } = default!;

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
        await StartGameSelectionConnectionAsync();
    }

    private void ToggleNavigation()
    {
        isNavigationOpen = !isNavigationOpen;
    }

    private void HandleCurrentUserChanged()
    {
        _ = InvokeAsync(StateHasChanged);
    }

    private async Task StartGameSelectionConnectionAsync()
    {
        if (gameSelectionConnection is not null)
        {
            return;
        }

        gameSelectionConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri(GameSelectionHub.Route))
            .WithAutomaticReconnect()
            .Build();

        gameSelectionConnection.On<GameSelectedMessage>(
            GameSelectionHub.GameSelectedMethod,
            HandleGameSelected);

        await gameSelectionConnection.StartAsync();
    }

    private void HandleGameSelected(GameSelectedMessage message)
    {
        _ = InvokeAsync(() =>
        {
            if (CurrentUserState.CurrentUser?.Role == JoinRole.Player)
            {
                NavigationManager.NavigateTo(message.PlayerRoute);
            }
        });
    }

    public async ValueTask DisposeAsync()
    {
        CurrentUserState.Changed -= HandleCurrentUserChanged;

        if (gameSelectionConnection is not null)
        {
            await gameSelectionConnection.DisposeAsync();
        }
    }
}
