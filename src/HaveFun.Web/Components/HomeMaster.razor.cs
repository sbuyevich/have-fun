using HaveFun.Core;
using Microsoft.AspNetCore.Components;

namespace HaveFun.Web;

public partial class HomeMaster
{
    [Parameter]
    public string? CurrentUserName { get; set; }

    [Parameter, EditorRequired]
    public string SharedJoinUrl { get; set; } = string.Empty;

    [Parameter, EditorRequired]
    public string QrCodeDataUri { get; set; } = string.Empty;

    [Parameter, EditorRequired]
    public IReadOnlyList<PlayerSession> Players { get; set; } = [];

    [Parameter]
    public EventCallback OnRefreshPlayers { get; set; }

    [Inject]
    private IAvatarLibraryService AvatarLibrary { get; set; } = default!;
}
