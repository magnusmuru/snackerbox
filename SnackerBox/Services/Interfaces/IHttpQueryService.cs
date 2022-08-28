namespace SnackerBox.Services.Interfaces;

public interface IHttpQueryService
{
    Task<HttpResponseMessage> HttpQueryAsync(string endpoint, HttpMethod method);
}