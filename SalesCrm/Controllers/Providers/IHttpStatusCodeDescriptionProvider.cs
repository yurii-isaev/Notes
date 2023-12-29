namespace SalesCrm.Controllers.Providers;

public interface IHttpStatusCodeDescriptionProvider
{
    string? GetStatusDescription(int statusCode);
}

public class HttpStatusCodeDescriptionProvider : IHttpStatusCodeDescriptionProvider
{
    private static readonly Dictionary<int, string?> HttpStatusCodes = new()
    {
        {100, "Continue"},
        {101, "Switching Protocols"},
        {500, "Internal Server Error"},
        {501, "Not Implemented"},
        {502, "Bad Gateway"},
        {503, "Service Unavailable"},
        {504, "Gateway Timeout"},
        {505, "HTTP Version Not Supported"}
    };

    public string? GetStatusDescription(int statusCode)
    {
        if (HttpStatusCodes.TryGetValue(statusCode, out string? statusDescription))
        {
            return statusDescription;
        }

        return "Unknown error";
    }
}
