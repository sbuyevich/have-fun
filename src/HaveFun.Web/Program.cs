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

builder.Services.AddCoreServices();

var app = builder.Build();

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
app.MapRazorComponents<HaveFun.Web.App>()
    .AddInteractiveServerRenderMode();

app.Run();
