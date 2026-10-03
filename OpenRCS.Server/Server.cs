namespace OpenRCS.Server;

public class Server
{
    private readonly WebApplication app;

    public Server()
    {
        app = buildServer();
    }
    
    private WebApplication buildServer()
    {
        var builder = WebApplication.CreateBuilder();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseHttpsRedirection();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        (
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");    
        return app;
    }
    

    public void StartServerAsync()
    {
        app.RunAsync();
    }

    public void StopServerAsync()
    {
        app.StopAsync();
    }

    internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
