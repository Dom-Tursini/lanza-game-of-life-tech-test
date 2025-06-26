namespace GameOfLifeBackend.Models;

public class GridMessage
{
    public string Type { get; set; } = "updateGrid";
    public List<List<Cell>> Grid { get; set; } = new();
}
