namespace HaveFun.Web;

public interface IQrCodeService
{
    string CreateSvgDataUri(string text);
}
