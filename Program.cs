using Microsoft.AspNetCore.Identity;
using preparacion_pt_bdv.Data;
using preparacion_pt_bdv.models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Usamos un scope para obtener los servicios necesarios para la inicialización
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var usuarioManager = services.GetRequiredService<UserManager<Usuario>>();
        var configuration = services.GetRequiredService<IConfiguration>();
        
        // Llamamos al método de inicialización pasando la configuración
        await LoadDatabase.InsertarData(context, usuarioManager, configuration);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al insertar datos iniciales en la base de datos.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilito la autenticación
app.UseAuthentication();

//Comento esta línea para no forzar HTTPS en desarrollo
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
