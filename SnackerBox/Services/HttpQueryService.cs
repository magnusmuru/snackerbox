using SnackerBox.Services.Interfaces;

namespace SnackerBox.Services;

public class HttpQueryService : IHttpQueryService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpQueryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> HttpQueryAsync(string endpoint, HttpMethod method)
    {
        var request = new HttpRequestMessage(method, endpoint);

        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.SendAsync(request);

        return response;
    }
}