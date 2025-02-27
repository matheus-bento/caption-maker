using System.Text;
using System.Text.Json;
using CaptionMaker;
using CaptionMaker.Data;
using CaptionMaker.Data.Repository;
using CaptionMaker.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// TODO: Put everything into a class for better organization

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables("CAPTION_MAKER_");

builder.Services.Configure<CaptionMakerOptions>(builder.Configuration);

builder.Services.AddDbContext<CaptionContext>((services, dbContextOptions) =>
{
    var appOptions = services.GetRequiredService<IOptions<CaptionMakerOptions>>();

    dbContextOptions.UseMySQL(
        appOptions.Value.DbConnectionString,
        mySqlOptions => mySqlOptions.MigrationsAssembly("CaptionMaker.Data")
    );
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();

builder.Services
    .AddAuthentication(authOptions =>
    {
        authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer((jwtOptions) =>
    {
        string jwtSecret = builder.Configuration.GetValue<string>("JWT_SECRET");

        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            
            ValidateIssuer = false,
            ValidateAudience = false
        };

        jwtOptions.RequireHttpsMetadata = false;
        jwtOptions.SaveToken = true;
    });

builder.Services
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
