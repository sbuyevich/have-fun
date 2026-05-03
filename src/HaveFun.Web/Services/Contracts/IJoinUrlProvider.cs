namespace HaveFun.Web;

public interface IJoinUrlProvider
{
    JoinUrls GetJoinUrls(Uri currentUri);
}
