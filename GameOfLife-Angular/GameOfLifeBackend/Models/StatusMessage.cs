namespace GameOfLifeBackend.Models;

public class StatusMessage
{
    public string Type { get; set; } = "status";
    public string State { get; set; } = "stopped"; // "playing", "stopped", "reset"
}
