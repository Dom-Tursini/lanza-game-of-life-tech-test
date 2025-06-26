using GameOfLifeBackend.Hubs;
using GameOfLifeBackend.Services;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddSingleton<GameOfLifeService>();

builder.Services.AddCors();

var app = builder.Build();

app.UseRouting();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());


app.MapHub<GameHub>("/gamehub");

app.Run();
