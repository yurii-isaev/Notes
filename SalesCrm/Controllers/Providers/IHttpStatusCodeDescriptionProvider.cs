namespace SalesCrm.Controllers.Providers;

public interface IHttpStatusCodeDescriptionProvider
{
    string? GetStatusDescription(int statusCode);
}

public class HttpStatusCodeDescriptionProvider : IHttpStatusCodeDescriptionProvider
{
    static readonly Dictionary<int, string?> HttpStatusCodes = new()
    {
        {100, "Continue"},
        {101, "Switching Protocols"},
        {400, "Bad Request"},
        {500, "Internal Server Error"},
        {501, "Not Implemented"},
        {502, "Bad Gateway"},
        {503, "Service Unavailable"},
        {504, "Gateway Timeout"},
        {505, "HTTP Version Not Supported"}
    };

    public string? GetStatusDescription(int statusCode)
    {
        return HttpStatusCodes.TryGetValue(statusCode, out string? statusDescription)
            ? statusDescription
            : "Unknown error";
    }
}
