using System.Text.Json;
using CaptionMaker;
using CaptionMaker.Data;
using CaptionMaker.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables("CAPTION_MAKER_");

builder
    .Services
        .Configure<CaptionMakerOptions>(builder.Configuration)
        .AddDbContext<CaptionContext>((services, dbContextOptions) =>
        {
            var appOptions = services.GetRequiredService<IOptions<CaptionMakerOptions>>();

            dbContextOptions.UseMySQL(
                appOptions.Value.DbConnectionString,
                mySqlOptions => mySqlOptions.MigrationsAssembly("CaptionMaker.Data")
                );
        })
        .AddScoped<IUserRepository, UserRepository>()
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
