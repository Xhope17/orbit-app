using Scalar.AspNetCore;
using Serilog;
using XClone.Infrastructure.Workers;
using XClone.WebApi.Extensions;
using XClone.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde .env (busca desde el directorio actual hacia arriba)
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (!File.Exists(envPath))
{
    envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
}
if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

builder.Services.AddHostedService<TimerNotifyWorker>(); //registro del worker para que se ejecute en segundo plano

builder.Host.UseSerilog();

await builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Theme = ScalarTheme.Mars;
        options.WithTitle("X-Clone API");
    });
    app.MapOpenApi();
}

app.UseMiddleware<ErrorHandlerMiddleware>(); //siempre va antes de cualquier otro middleware para capturar errores

app.UseHttpsRedirection();

app.UseAuthentication(); //va antes de authorization para validar el token antes de verificar los roles o permisos
app.UseAuthorization();

app.MapControllers();
app.MapHub<XClone.WebApi.Hubs.ChatHub>("/hubs/chat");
app.MapHub<XClone.WebApi.Hubs.NotificationHub>("/hubs/notifications");

app.Run();
