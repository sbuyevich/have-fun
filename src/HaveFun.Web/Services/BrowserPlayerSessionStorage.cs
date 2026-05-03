using System.Text.Json;
using Microsoft.JSInterop;

namespace HaveFun.Web;

public sealed class BrowserPlayerSessionStorage : IPlayerSessionStorage
{
    private const string StorageKey = "havefun.currentPlayer";
    private readonly IJSRuntime jsRuntime;

    public BrowserPlayerSessionStorage(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async ValueTask<StoredPlayerSession?> GetCurrentPlayerAsync()
    {
        var json = await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", StorageKey);

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<StoredPlayerSession>(json);
        }
        catch (JsonException)
        {
            await ClearCurrentPlayerAsync();
            return null;
        }
    }

    public async ValueTask SaveCurrentPlayerAsync(StoredPlayerSession playerSession)
    {
        var json = JsonSerializer.Serialize(playerSession);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", StorageKey, json);
    }

    public ValueTask ClearCurrentPlayerAsync()
    {
        return jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", StorageKey);
    }
}
