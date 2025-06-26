using Microsoft.AspNetCore.SignalR;
using GameOfLifeBackend.Services;
using GameOfLifeBackend.Models;
using System.Threading.Tasks;

namespace GameOfLifeBackend.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameOfLifeService _game;

        public GameHub(GameOfLifeService game)
        {
            _game = game;
        }

        public async Task SendMessage(CommandPayload data)
        {
            if (string.IsNullOrWhiteSpace(data.Type))
            {
                Console.WriteLine("⚠ No type provided in message.");
                return;
            }


            switch (data.Type.ToLower())
            {
                case "setgridsize":
                    if (data.Cols.HasValue && data.Rows.HasValue)
                    {
                        Console.WriteLine($"✅ SetGridSize received: {data.Cols}x{data.Rows}");
                        _game.SetGridSize(data.Cols.Value, data.Rows.Value);
                    }
                    break;

                case "seed":
                    if (!string.IsNullOrEmpty(data.Pattern))
                    {
                        Console.WriteLine($"✅ Seed pattern received: {data.Pattern}");
                        _game.Seed(data.Pattern);
                    }
                    break;

                case "start":

                    await Task.Delay(50);
                    Console.WriteLine("▶ Starting game...");
                    _game.Start();
                    break;

                case "stop":
                    Console.WriteLine("⏹ Stopping game");
                    _game.Stop();
                    break;

                case "reset":
                    Console.WriteLine("🔄 Resetting game");
                    _game.Reset();
                    break;
            }
        }

    }
}
