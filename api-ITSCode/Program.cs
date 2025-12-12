using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseLazyLoadingProxies()
          .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<DAOFactory, EFDAOFactory>();

// --- CONFIGURACIÓN DE CORS SIMPLIFICADA (DRY) ---
var corsOrigins = new List<string>();

// Leemos los orígenes desde la configuración, sin importar el entorno
var originsFromConfig = builder.Configuration["AllowedOrigins"];
if (!string.IsNullOrEmpty(originsFromConfig))
{
    corsOrigins.AddRange(originsFromConfig.Split(','));
}

// Registramos la política de CORS con la lista de orígenes que construimos
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(corsOrigins.ToArray())
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// --- ¡CORRECCIÓN AQUÍ! ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Usamos EnsureCreated() porque no tenemos archivos de migración aún
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// app.UseHttpsRedirection(); // Comentado para producción detrás de un proxy

app.UseStaticFiles();

// Usamos la política por defecto que registramos
app.UseCors();

app.UseAuthorization();
app.MapControllers();
app.Run();