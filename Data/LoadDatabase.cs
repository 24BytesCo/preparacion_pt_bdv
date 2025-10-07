using Microsoft.AspNetCore.Identity;
using preparacion_pt_bdv.models;
using preparacion_pt_bdv.Models;

namespace preparacion_pt_bdv.Data
{
    // Clase para inicializar la base de datos con datos por defecto.
    public class LoadDatabase
    {
        // Inserta un usuario inicial si la base de datos está vacía.
        public static async Task InsertarData(AppDbContext context, UserManager<Usuario> usuarioManager, IConfiguration configuration)
        {
            if (!context.Users.Any())
            {
                var initialUserConfig = new InitialUserData();
                configuration.GetSection("InitialUserData").Bind(initialUserConfig);

                if (string.IsNullOrEmpty(initialUserConfig.UserName) || string.IsNullOrEmpty(initialUserConfig.Password))
                {
                    throw new InvalidOperationException("Los datos para el usuario inicial no se encontraron en la configuración.");
                }

                var usuario = new Usuario
                {
                    UserName = initialUserConfig.UserName,
                    Nombre = initialUserConfig.Nombre,
                    Apellido = initialUserConfig.Apellido,
                    Email = initialUserConfig.Email,
                    Telefono = initialUserConfig.Telefono
                };

                // Crea el usuario con la contraseña especificada en los user-secrets.
                await usuarioManager.CreateAsync(usuario, initialUserConfig.Password);
                context.SaveChanges();
            }
        }
    }
}