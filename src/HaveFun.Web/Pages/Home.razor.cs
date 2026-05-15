using HaveFun.Core;
using Microsoft.AspNetCore.Components;

namespace HaveFun.Web;

public partial class Home : ComponentBase
{
    private bool IsSessionChecked { get; set; }

    private StoredUserSession? CurrentUser { get; set; }

    private bool IsMaster => CurrentUser?.Role == JoinRole.Master;

    private bool IsPlayer => CurrentUser?.Role == JoinRole.Player;

    private string SharedJoinUrl { get; set; } = string.Empty;

    private string QrCodeDataUri { get; set; } = string.Empty;

    private IReadOnlyList<PlayerSession> Players { get; set; } = [];

    private IReadOnlyList<AvatarOption> Avatars { get; set; } = [];

    private string SelectedAvatarFileName { get; set; } = string.Empty;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJoinUrlProviderService JoinUrlProvider { get; set; } = default!;

    [Inject]
    private IPlayerRegistryService PlayerRegistry { get; set; } = default!;

    [Inject]
    private ICurrentUserStateService CurrentUserState { get; set; } = default!;

    [Inject]
    private IAvatarLibraryService AvatarLibrary { get; set; } = default!;

    [Inject]
    private IQrCodeService QrCode { get; set; } = default!;

    protected override void OnInitialized()
    {
        var baseUri = new Uri(NavigationManager.BaseUri);

        SharedJoinUrl = JoinUrlProvider.GetJoinUrl(baseUri) ?? baseUri.GetLeftPart(UriPartial.Authority);
        QrCodeDataUri = QrCode.CreateSvgDataUri(SharedJoinUrl);
        Avatars = AvatarLibrary.Avatars;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        await CurrentUserState.InitializeAsync();
        var currentUser = CurrentUserState.CurrentUser;

        if (currentUser is null)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        CurrentUser = currentUser;

        if (currentUser.Role == JoinRole.Player)
        {
            if (!PlayerRegistry.TryGetPlayerByName(currentUser.Name, out var player) || player is null)
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            var avatarFileName = AvatarLibrary.IsKnownAvatar(currentUser.AvatarFileName)
                ? currentUser.AvatarFileName!
                : player.AvatarFileName;

            if (!AvatarLibrary.IsKnownAvatar(avatarFileName))
            {
                avatarFileName = AvatarLibrary.DefaultPlayerAvatarFileName;
            }

            SelectedAvatarFileName = avatarFileName;
            PlayerRegistry.UpdatePlayerAvatar(currentUser.Name, avatarFileName);

            if (currentUser.AvatarFileName != avatarFileName)
            {
                CurrentUser = currentUser with
                {
                    AvatarFileName = avatarFileName
                };
                await CurrentUserState.SetCurrentUserAsync(CurrentUser);
            }
        }

        RefreshPlayers();
        IsSessionChecked = true;
        StateHasChanged();
    }

    private async Task SelectAvatarAsync(string avatarFileName)
    {
        if (!IsPlayer || CurrentUser is null || !AvatarLibrary.IsKnownAvatar(avatarFileName))
        {
            return;
        }

        SelectedAvatarFileName = avatarFileName;
        CurrentUser = CurrentUser with
        {
            AvatarFileName = avatarFileName
        };

        PlayerRegistry.UpdatePlayerAvatar(CurrentUser.Name, avatarFileName);
        await CurrentUserState.SetCurrentUserAsync(CurrentUser);
    }

    private void RefreshPlayers()
    {
        Players = PlayerRegistry.GetPlayers();
    }
}
