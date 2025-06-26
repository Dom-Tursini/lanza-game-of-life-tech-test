using Xunit;

// Used this test to validate rules for the output of the console version of the game of life, using a known input and expected output using
// 3 cells of living in a row that would flip it state from a horizontal line to a vertical line.
// I added a blank border around the grid to get around the modulus method used in the main code to make the grid "seamless"

namespace ConsoleVersion.Tests
{
    public class GameOfLifeTests
    {
        [Fact]
        public void Blinker_Oscillates_Horizontally()
        {
            int[][] input = new int[][]
            {
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 1, 0, 0},
                new[] {0, 0, 1, 0, 0},
                new[] {0, 0, 1, 0, 0},
                new[] {0, 0, 0, 0, 0}
            };

            int[][] expected = new int[][]
            {
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0},
                new[] {0, 1, 1, 1, 0},
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0}
            };

            int[][] output = GameLogic.CreateGrid(5, 5);
            GameLogic.Step(input, output);

            Assert.True(GridsEqual(expected, output));
        }

        private bool GridsEqual(int[][] a, int[][] b)
        {
            for (int y = 0; y < a.Length; y++)
                for (int x = 0; x < a[0].Length; x++)
                    if (a[y][x] != b[y][x])
                        return false;
            return true;
        }
    }
}
