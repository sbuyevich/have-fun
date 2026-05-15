namespace HaveFun.Web;

public interface IJoinUrlProviderService
{
    string? GetJoinUrl(Uri currentUri);
}
