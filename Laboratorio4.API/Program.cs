using Laboratorio4.API.Context;
using Laboratorio4.API.Services.Interface;
using Laboratorio4.API.Services.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Laboratorio4.API.Security;
using Npgsql;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Render proporciona el puerto mediante la variable PORT.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Obtener la cadena de conexión del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("NeonConnection");
if (string.IsNullOrWhiteSpace(connectionString) && builder.Environment.IsDevelopment())
{
    var neonEnvPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", ".env.local"));
    if (File.Exists(neonEnvPath))
    {
        var line = File.ReadLines(neonEnvPath)
            .FirstOrDefault(value => value.StartsWith("DATABASE_URL=", StringComparison.Ordinal));
        var value = line?["DATABASE_URL=".Length..].Trim().Trim('"', '\'');
        if (!string.IsNullOrWhiteSpace(value))
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
                (uri.Scheme != "postgres" && uri.Scheme != "postgresql"))
            {
                throw new InvalidOperationException("La URL de Neon en .env.local no tiene un formato PostgreSQL válido.");
            }

            var credentials = uri.UserInfo.Split(':', 2);
            if (credentials.Length != 2 || string.IsNullOrEmpty(uri.Host) || uri.AbsolutePath == "/")
            {
                throw new InvalidOperationException("La URL de Neon en .env.local está incompleta.");
            }

            connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = uri.Host,
                Port = uri.IsDefaultPort || uri.Port < 0 ? 5432 : uri.Port,
                Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
                Username = Uri.UnescapeDataString(credentials[0]),
                Password = Uri.UnescapeDataString(credentials[1]),
                SslMode = SslMode.Require
            }.ConnectionString;
        }
    }
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure ConnectionStrings:NeonConnection mediante secretos de usuario o la variable " +
        "ConnectionStrings__NeonConnection. En desarrollo también puede utilizar .env.local de Neon.");
}

// Límites globales por instancia. No confiamos en cabeceras IP que se pueden falsificar.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
        RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 300, Window = TimeSpan.FromMinutes(1), QueueLimit = 0
        }));
    options.AddFixedWindowLimiter("ranking-escritura", limiter =>
    {
        limiter.PermitLimit = 30;
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.QueueLimit = 0;
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "X-API-Key",
        Description = "Escribe la clave de administración configurada para esta API."
    });
    options.DocumentFilter<SwaggerClaveAdministracionFilter>();
});

// Registrar servicios e inyección de dependencias
builder.Services.AddScoped<ILaboratorioService, LaboratorioService>();

// Configurar PostgreSQL.


builder.Services.AddDbContext<AplicacionDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Creación automática de la base de datos y tablas al iniciar (EnsureCreated)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AplicacionDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al crear la base de datos.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.Run();
