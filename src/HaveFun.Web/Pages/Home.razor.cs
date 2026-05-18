using HaveFun.Core;
using Microsoft.AspNetCore.Components;

namespace HaveFun.Web;

public partial class Home : ComponentBase
{
    private string LocalhostUrl { get; set; } = string.Empty;

    private string? LanUrl { get; set; }

    private int SentenceCount { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IUrlService UrlService { get; set; } = default!;

    [Inject]
    private ISentenceLibraryService SentenceLibrary { get; set; } = default!;

    [Inject]
    private ISessionStorageService UserSessionStorageService { get; set; } = default!;

    protected override void OnInitialized()
    {
        var urls = UrlService.GetLanBaseUrl(NavigationManager.BaseUri);

        LocalhostUrl = urls ?? NavigationManager.BaseUri;
        LanUrl = urls ?? NavigationManager.BaseUri;
        SentenceCount = SentenceLibrary.Sentences.Count;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        var currentUser = await UserSessionStorageService.GetCurrentUserAsync();

        if (currentUser is null)
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
