using System;
using System.Threading;

class Program
{
    const int Delay = 80;

    static void Main()
    {
        (int width, int height) = AskGridSize();
        char liveIcon = AskLiveIcon();
        string seed = AskSeedPattern();

        int[][] current = CreateGrid(height, width);
        int[][] next = CreateGrid(height, width);

        SeedPattern(current, seed);

        while (true)
        {
            Console.Clear();
            PrintGrid(current, liveIcon);
            Step(current, next);
            var temp = current;
            current = next;
            next = temp;
            Thread.Sleep(Delay);
        }
    }

    static (int, int) AskGridSize()
    {
        Console.WriteLine("Select grid size:");
        Console.WriteLine("1. 100 x 30");
        Console.WriteLine("2. 50 x 15");
        Console.WriteLine("3. 20 x 8");
        Console.Write("Select option [1-3]: ");
        string choice = Console.ReadLine() ?? "";
        return choice switch
        {
            "1" => (100, 30),
            "2" => (50, 15),
            _ => (20, 8)
        };
    }

    static char AskLiveIcon()
    {
        Console.WriteLine("\nSelect live cell icon:");
        Console.WriteLine("1. ☻");
        Console.WriteLine("2. ♥");
        Console.WriteLine("3. █");
        Console.Write("Select option [1-3]: ");
        string choice = Console.ReadLine() ?? "";
        return choice switch
        {
            "1" => '☻',
            "2" => '♥',
            _ => '█'
        };
    }

    static string AskSeedPattern()
    {
        Console.WriteLine("\nSelect seed pattern:");
        Console.WriteLine("1. Random noise");
        Console.WriteLine("2. Angel");
        Console.Write("Select option [1-2]: ");
        string choice = Console.ReadLine() ?? "";
        return choice switch
        {
            "2" => "angel",
            _ => "random"
        };
    }

    static int[][] CreateGrid(int height, int width)
    {
        int[][] grid = new int[height][];
        for (int y = 0; y < height; y++)
            grid[y] = new int[width];
        return grid;
    }

    static void SeedPattern(int[][] grid, string pattern)
    {
        if (pattern == "random")
        {
            Random rand = new Random();
            for (int y = 0; y < grid.Length; y++)
                for (int x = 0; x < grid[0].Length; x++)
                    grid[y][x] = rand.NextDouble() < 0.3 ? 1 : 0;
        }
        else
        {
            LoadPresetPattern(grid, pattern);
        }
    }

    static void LoadPresetPattern(int[][] grid, string pattern)
    {
        int offsetY = grid.Length / 2;
        int offsetX = grid[0].Length / 2;

        int[][] shape = pattern switch
        {
            "angel" => new int[][]
            {
                new[] {0,1,0},
                new[] {1,1,1},
                new[] {1,0,1}
            },
            _ => new int[0][]
        };

        int startY = offsetY - shape.Length / 2;
        int startX = offsetX - shape[0].Length / 2;

        for (int y = 0; y < shape.Length; y++)
        {
            for (int x = 0; x < shape[y].Length; x++)
            {
                grid[startY + y][startX + x] = shape[y][x];
            }
        }
    }

    static void PrintGrid(int[][] grid, char liveIcon)
    {
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[0].Length; x++)
            {
                Console.Write(grid[y][x] == 1 ? liveIcon : ' ');
            }
            Console.WriteLine();
        }
    }

    static void Step(int[][] current, int[][] next)
    {
        int height = current.Length;
        int width = current[0].Length;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int neighbors = CountLiveNeighbors(current, x, y);
                int state = current[y][x];
                if (state == 1 && (neighbors < 2 || neighbors > 3))
                    next[y][x] = 0; // Dies
                else if (state == 0 && neighbors == 3)
                    next[y][x] = 1; // Born
                else
                    next[y][x] = state; // Stays the same
            }
        }
    }

    static int CountLiveNeighbors(int[][] grid, int x, int y)
    {
        int count = 0;
        int height = grid.Length;
        int width = grid[0].Length;

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = (x + dx + width) % width;
                int ny = (y + dy + height) % height;

                count += grid[ny][nx];
            }
        }
        return count;
    }
}

public static class GameLogic
{
    public static int[][] CreateGrid(int height, int width)
    {
        int[][] grid = new int[height][];
        for (int y = 0; y < height; y++)
            grid[y] = new int[width];
        return grid;
    }

    public static void Step(int[][] current, int[][] next)
    {
        int height = current.Length;
        int width = current[0].Length;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int neighbors = CountLiveNeighbors(current, x, y);
                int state = current[y][x];
                if (state == 1 && (neighbors < 2 || neighbors > 3))
                    next[y][x] = 0;
                else if (state == 0 && neighbors == 3)
                    next[y][x] = 1;
                else
                    next[y][x] = state;
            }
        }
    }

    private static int CountLiveNeighbors(int[][] grid, int x, int y)
    {
        int count = 0;
        int height = grid.Length;
        int width = grid[0].Length;

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = (x + dx + width) % width;
                int ny = (y + dy + height) % height;

                count += grid[ny][nx];
            }
        }
        return count;
    }
}
