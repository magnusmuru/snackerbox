using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using SnackerBox.Dto;
using SnackerBox.Services.Interfaces;

namespace SnackerBox.Services;

public class SnackableParsingService : ISnackableParsingService
{
    private readonly IHttpQueryService _httpQueryService;

    public SnackableParsingService(IHttpQueryService httpQueryService)
    {
        _httpQueryService = httpQueryService;
    }

    public async Task<bool> FindFinishedFile(string guid)
    {
        var current = 0;
        var files = new List<AllFilesDto>();
        var response = await FindFiles(current);

        while (response.Count > 0)
        {
            files.AddRange(response);
            current += 5;
            response = await FindFiles(current);
        }

        return files.Any(x => x.FileId == guid && x.ProcessingStatus == "FINISHED");
    }

    public async Task<MetadataDto> FindMetadata(string guid)
    {

        var fileEndpoint = $"http://interview-api.snackable.ai/api/file/details/{guid}";
        var segmentEndpoint = $"http://interview-api.snackable.ai/api/file/segments/{guid}";

        var metadata = new MetadataDto();

        var fileResponse = await _httpQueryService.HttpQueryAsync(fileEndpoint, HttpMethod.Get);
        if (fileResponse.IsSuccessStatusCode)
        {
            await using var contentStream = await fileResponse.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var fileResponseContent = await JsonSerializer.DeserializeAsync<MetadataDto>(contentStream, options);
            if (fileResponseContent != null)
            {
                metadata = fileResponseContent;
            }
        }

        var segmentResponse = await _httpQueryService.HttpQueryAsync(segmentEndpoint, HttpMethod.Get);
        if (segmentResponse.IsSuccessStatusCode)
        {
            await using var contentStream = await segmentResponse.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var segmentResponseContent =
                await JsonSerializer.DeserializeAsync<List<SegmentDto>>(contentStream, options);
            if (segmentResponseContent != null)
            {
                metadata.Segments = segmentResponseContent;
            }
        }

        return metadata;
    }

    private async Task<List<AllFilesDto>> FindFiles(int offset)
    {
        var endpoint = $"http://interview-api.snackable.ai/api/file/all?offset={offset}";

        var response = await _httpQueryService.HttpQueryAsync(endpoint, HttpMethod.Get);
        if (response.IsSuccessStatusCode)
        {
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseContent = await JsonSerializer.DeserializeAsync<List<AllFilesDto>>(contentStream, options);
            if (responseContent != null)
            {
                return responseContent;
            }
        }

        return new List<AllFilesDto>();
    }
}