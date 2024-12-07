using ImageDetector.Middlewares;
using ImageDetector.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<ApiKeyService>();
builder.Services.AddScoped<ImageDetectionService>();
var app = builder.Build();
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();
app.Run();
