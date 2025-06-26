namespace GameOfLifeBackend.Models;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public string State { get; set; } = "dead"; // "alive", "dying", "dead"
}
