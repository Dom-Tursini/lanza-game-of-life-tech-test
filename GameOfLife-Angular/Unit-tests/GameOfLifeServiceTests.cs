using Xunit;
using GameOfLifeBackend.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using GameOfLifeBackend.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;


public class GameOfLifeServiceTests
{
    [Fact]
    public void FullGameFlow_Start_Tick_Stop_Reset_WorksCorrectly()
    {
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        mockClientProxy
            .Setup(proxy => proxy.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
            .Returns(Task.CompletedTask);

        mockClients
            .Setup(clients => clients.All)
            .Returns(mockClientProxy.Object);

        var mockHubContext = new Mock<IHubContext<GameHub>>();
        mockHubContext
            .Setup(context => context.Clients)
            .Returns(mockClients.Object);

        var service = new GameOfLifeService(mockHubContext.Object);

        service.SetGridSize(10, 10);
        service.Seed("glider");

        var initialGrid = GetGridState(service);

        service.Start();
        service.Tick();
        var evolvedGrid = GetGridState(service);

        Assert.NotEqual(initialGrid, evolvedGrid);

        service.Stop();
        service.Reset();
        var resetGrid = GetGridState(service);

        Assert.All(resetGrid, row =>
            Assert.All(row, cell =>
                Assert.Equal("dead", cell.State)
            )
        );
    }



    private List<List<GameOfLifeBackend.Models.Cell>> GetGridState(GameOfLifeService service)
    {
        var gridField = typeof(GameOfLifeService)
            .GetField("_grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        return gridField?.GetValue(service) as List<List<GameOfLifeBackend.Models.Cell>>
               ?? new List<List<GameOfLifeBackend.Models.Cell>>();
    }
}
