using System.Text.Json.Serialization;

namespace GameOfLifeBackend.Models;

public class CommandPayload
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("pattern")]
    public string? Pattern { get; set; }

    [JsonPropertyName("cols")]
    public int? Cols { get; set; }

    [JsonPropertyName("rows")]
    public int? Rows { get; set; }
}
