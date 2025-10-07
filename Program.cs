using Microsoft.AspNetCore.Identity;
using preparacion_pt_bdv.Data;
using preparacion_pt_bdv.Middleware;
using preparacion_pt_bdv.models;

var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios al contenedor de inyección de dependencias.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Bloque para inicializar datos en la base de datos al arrancar.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var usuarioManager = services.GetRequiredService<UserManager<Usuario>>();
        var configuration = services.GetRequiredService<IConfiguration>();
        
        // Ejecuta la siembra de datos.
        await LoadDatabase.InsertarData(context, usuarioManager, configuration);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al insertar datos iniciales en la base de datos.");
    }
}

// Configura el pipeline de peticiones HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Registra el middleware para manejo centralizado de excepciones.
app.UseMiddleware<ManagerMiddleware>();

// Habilita la autenticación en la aplicación.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();