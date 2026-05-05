using HaveFun.Core;
using HaveFun.Web;
using Microsoft.Extensions.Options;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

builder.Services.AddOptions<GameOptions>()
    .Bind(builder.Configuration.GetSection(GameOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.MasterName), "Game:MasterName is required.")
    .ValidateOnStart();

builder.Services.AddSingleton<ISentenceLibraryService>(_ =>
{
    var sentenceFilePath = Path.Combine(builder.Environment.ContentRootPath, "assets", "sentences.json");
    var sentences = SentenceFileLoaderService.Load(sentenceFilePath);

    return new InMemorySentenceLibraryService(sentences);
});
builder.Services.AddSingleton<IPlayerRegistryService, PlayerRegistryService>();
builder.Services.AddSingleton<IGameStateService, GameStateService>();
builder.Services.AddSingleton<IAvailableGameService, AvailableGameService>();
builder.Services.AddSingleton<IActiveGameStateService, ActiveGameStateService>();
builder.Services.AddSingleton<IJoinUrlProviderService, JoinUrlProviderService>();
builder.Services.AddSingleton<IAvatarLibraryService>(_ =>
{
    var avatarFolderPath = Path.Combine(builder.Environment.ContentRootPath, "assets", "avatars");

    return new AvatarLibraryService(avatarFolderPath);
});
builder.Services.AddSingleton<IQrCodeService, QrCodeService>();
builder.Services.AddScoped<IUserSessionStorageService, UserSessionStorageService>();
builder.Services.AddScoped<ICurrentUserStateService, CurrentUserStateService>();

var app = builder.Build();

_ = app.Services.GetRequiredService<IOptions<GameOptions>>().Value;
_ = app.Services.GetRequiredService<ISentenceLibraryService>();
_ = app.Services.GetRequiredService<IJoinUrlProviderService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapGet("/assets/avatars/{fileName}", (string fileName, IAvatarLibraryService avatarLibrary) =>
{
    if (!avatarLibrary.TryGetAvatarFilePath(fileName, out var filePath) || filePath is null)
    {
        return Results.NotFound();
    }

    var contentType = Path.GetExtension(filePath).ToLowerInvariant() switch
    {
        ".svg" => "image/svg+xml",
        ".png" => "image/png",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".webp" => "image/webp",
        _ => "application/octet-stream"
    };

    return Results.File(filePath, contentType);
});
app.MapHub<GameSelectionHub>(GameSelectionHub.Route);
app.MapRazorComponents<HaveFun.Web.App>()
    .AddInteractiveServerRenderMode();

app.Run();
