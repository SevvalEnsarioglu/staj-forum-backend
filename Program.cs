using Microsoft.EntityFrameworkCore;
using staj_forum_backend.Data;
using staj_forum_backend.Data.Repositories;
using staj_forum_backend.Mappings;
using staj_forum_backend.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repository Pattern - Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// Service Layer - Register Services
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            if (string.IsNullOrEmpty(origin))
                return false;

            try
            {
                var uri = new Uri(origin);
                return uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                       uri.Host.Equals("127.0.0.1");
            }
            catch
            {
                return false;
            }
        })
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// CORS middleware - UseCors must be called before UseAuthorization and MapControllers
app.UseCors("AllowFrontend");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
