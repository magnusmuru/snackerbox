using SnackerBox.Dto;

namespace SnackerBox.Services.Interfaces;

public interface ISnackableParsingService
{
    Task<bool> FindFinishedFile(string guid);
    Task<MetadataDto> FindMetadata(string guid);
}