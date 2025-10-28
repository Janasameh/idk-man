using System.Text;
using hamada.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // built-in OpenAPI generator

// Register application services
builder.Services.AddScoped<hamada.Repo.IUserRepository, hamada.Repo.UserRepository>();
builder.Services.AddScoped<hamada.Services.IPasswordService, hamada.Services.PasswordService>();
// Register concrete CachingService since controllers inject the concrete type
builder.Services.AddScoped<hamada.Services.CachingService>();

var jwtSettings = builder.Configuration.GetSection("jwt");
var jwtKey = jwtSettings["key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT key is missing in configuration.");
}
// Expect jwt:key to be a base64-encoded 32+ byte secret. Decode here.
byte[] key;
try
{
    key = Convert.FromBase64String(jwtKey);
}
catch (FormatException)
{
    // Fall back to UTF8 bytes if the key is not base64 (but prefer base64 in production)
    key = Encoding.UTF8.GetBytes(jwtKey);
}

builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["jwt:issuer"],
        ValidAudience = builder.Configuration["jwt:audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)

    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hamada API",
        Version = "v1",
        Description = "API for managing user tasks"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hamada API v1");
        c.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();