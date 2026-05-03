namespace HaveFun.Web;

public interface IPlayerSessionStorage
{
    ValueTask<StoredPlayerSession?> GetCurrentPlayerAsync();

    ValueTask SaveCurrentPlayerAsync(StoredPlayerSession playerSession);

    ValueTask ClearCurrentPlayerAsync();
}
