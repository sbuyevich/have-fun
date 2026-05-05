using Microsoft.AspNetCore.SignalR;

namespace HaveFun.Web;

public sealed class GameSelectionHub : Hub
{
    public const string Route = "/game-selection-hub";

    public const string GameSelectedMethod = "GameSelected";
}
