namespace SnackerBox.Dto;

public class MetadataDto
{
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string MP3Path { get; set; }
    public string OriginalFilepath { get; set; }
    public string SeriesTitle { get; set; }
    public string User { get; set; } = "garyvee";
    public List<SegmentDto> Segments { get; set; }
}