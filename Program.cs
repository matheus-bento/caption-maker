using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .Services
        .AddControllers()
        .AddJsonOptions(options => {
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
