using GameOfLifeBackend.Models;
using Microsoft.AspNetCore.SignalR;
using GameOfLifeBackend.Hubs;
using System.Timers;

namespace GameOfLifeBackend.Services;

public class GameOfLifeService
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly object _lock = new();
    private List<List<Cell>> _grid = new();
    private int _cols = 64;
    private int _rows = 36;
    private string _status = "stopped";
    private System.Timers.Timer? _timer;
    private string _pendingSeedPattern = "random";
    private bool _gridReady = false;

    public GameOfLifeService(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void Start()
    {
        if (_status == "playing") return;

        lock (_lock)
        {
            if (!_gridReady)
            {
                _grid = CreateEmptyGrid(_cols, _rows);

                if (_pendingSeedPattern.ToLower() == "random")
                {
                    var rand = new Random();
                    foreach (var row in _grid)
                    {
                        foreach (var cell in row)
                        {
                            cell.State = rand.NextDouble() < 0.2 ? "alive" : "dead";
                        }
                    }
                }
                else
                {
                    ApplyPattern(_pendingSeedPattern);
                }

                _gridReady = true;
                SendGrid();
            }
        }

        _status = "playing";
        _timer = new System.Timers.Timer(250);
        _timer.Elapsed += (s, e) => Tick();
        _timer.Start();
        SendStatus();
    }

    public void Stop()
    {
        _status = "stopped";
        _timer?.Stop();
        SendStatus();
    }

    public void Reset()
    {
        lock (_lock)
        {
            _grid = CreateEmptyGrid(_cols, _rows);
            _gridReady = false;
            _status = "reset";
            SendStatus();
            SendGrid();
        }
    }

    public void SetGridSize(int cols, int rows)
    {
        lock (_lock)
        {
            if (_cols != cols || _rows != rows)
            {
                _cols = cols;
                _rows = rows;
                _gridReady = false;
            }
        }
    }




    public void Seed(string pattern)
    {
        lock (_lock)
        {
            _pendingSeedPattern = pattern;
            _gridReady = false;
        }
    }

    private void ApplyPattern(string pattern)
    {
        int centerX = _cols / 2;
        int centerY = _rows / 2;

        switch (pattern.ToLower())
        {
            case "glider":
                SetAlive(centerX - 1, centerY - 2);
                SetAlive(centerX, centerY - 1);
                SetAlive(centerX - 2, centerY);
                SetAlive(centerX - 1, centerY);
                SetAlive(centerX, centerY);
                break;




            case "pulsar":
                for (int dx = -6; dx <= 6; dx++)
                {
                    for (int dy = -6; dy <= 6; dy++)
                    {
                        if (Math.Abs(dx) == 4 || Math.Abs(dx) == 0 || Math.Abs(dx) > 5) continue;
                        if (Math.Abs(dy) == 2 || Math.Abs(dy) == 0 || Math.Abs(dy) > 5)
                            SetAlive(centerX + dx, centerY + dy);
                    }
                }
                break;

            case "angel":
                // Row -2
                SetAlive(centerX, centerY - 2);

                // Row -1
                SetAlive(centerX - 2, centerY - 1);
                SetAlive(centerX - 1, centerY - 1);
                SetAlive(centerX + 1, centerY - 1);
                SetAlive(centerX + 2, centerY - 1);

                // Row 0
                SetAlive(centerX - 1, centerY);
                SetAlive(centerX + 1, centerY);

                // Row +1
                SetAlive(centerX - 2, centerY + 1);
                SetAlive(centerX + 2, centerY + 1);
                break;



            default:
                Console.WriteLine($"⚠ Unknown pattern: {pattern}, defaulting to random.");
                var rand = new Random();
                foreach (var row in _grid)
                {
                    foreach (var cell in row)
                    {
                        cell.State = rand.NextDouble() < 0.2 ? "alive" : "dead";
                    }
                }
                break;
        }
    }

    private void SetAlive(int x, int y)
    {
        if (x >= 0 && x < _cols && y >= 0 && y < _rows)
        {
            _grid[y][x].State = "alive";
        }
    }

    public void Tick()
    {
        lock (_lock)
        {

            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    if (_grid[y][x].State == "dying")
                    {
                        _grid[y][x].State = "dead";
                    }
                }
            }

            var nextGrid = CreateEmptyGrid(_cols, _rows);


            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    int aliveNeighbors = CountAliveNeighbors(x, y);
                    var currentState = _grid[y][x].State;

                    if (currentState == "alive")
                    {
                        if (aliveNeighbors < 2 || aliveNeighbors > 3)
                            nextGrid[y][x].State = "dying";
                        else
                            nextGrid[y][x].State = "alive";
                    }
                    else if (currentState == "dead")
                    {
                        if (aliveNeighbors == 3)
                            nextGrid[y][x].State = "alive";
                        else
                            nextGrid[y][x].State = "dead";
                    }
                }
            }

            _grid = nextGrid;
            SendGrid();
        }
    }





    private int CountAliveNeighbors(int x, int y)
    {
        int count = 0;

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = (x + dx + _cols) % _cols;
                int ny = (y + dy + _rows) % _rows;

                if (_grid[ny][nx].State == "alive")
                    count++;
            }
        }

        return count;
    }

    private List<List<Cell>> CreateEmptyGrid(int cols, int rows)
    {
        return Enumerable.Range(0, rows)
            .Select(y => Enumerable.Range(0, cols)
                .Select(x => new Cell { X = x, Y = y, State = "dead" }).ToList())
            .ToList();
    }

    private void SendGrid()
    {
        Console.WriteLine($"Broadcasting grid: {_cols}x{_rows}");
        _hubContext.Clients.All.SendAsync("updateGrid", new { grid = _grid });
    }

    private void SendStatus()
    {
        _hubContext.Clients.All.SendAsync("status", new { state = _status });
    }
}
