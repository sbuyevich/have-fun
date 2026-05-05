using Microsoft.AspNetCore.Components;

namespace HaveFun.Web;

public partial class HomePlayer
{
    [Parameter]
    public string? CurrentUserName { get; set; }

    [Parameter, EditorRequired]
    public IReadOnlyList<AvatarOption> Avatars { get; set; } = [];

    [Parameter, EditorRequired]
    public string SelectedAvatarFileName { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> SelectedAvatarFileNameChanged { get; set; }

    [Inject]
    private IAvatarLibraryService AvatarLibrary { get; set; } = default!;
}
