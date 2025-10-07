using Microsoft.AspNetCore.Identity;
using preparacion_pt_bdv.models;
using preparacion_pt_bdv.Models;

namespace preparacion_pt_bdv.Data
{
    /**
     * @class LoadDatabase
     * @brief Contiene métodos estáticos para inicializar la base de datos con datos de prueba.
     */
    public class LoadDatabase
    {
        
        /**
         * @brief Inserta datos iniciales en la base de datos, incluyendo un usuario por defecto.
         * Los datos del usuario se leen desde la configuración de la aplicación (appsettings y User Secrets).
         * @param context El contexto de la base de datos (AppDbContext).
         * @param usuarioManager El servicio UserManager para la gestión de identidades.
         * @param configuration La configuración de la aplicación para acceder a los secretos y ajustes.
         * @returns Una tarea que representa la operación asíncrona.
         * @throws InvalidOperationException si los datos del usuario inicial no se encuentran en la configuración.
         */
        public static async Task InsertarData(AppDbContext context, UserManager<Usuario> usuarioManager, IConfiguration configuration)
        {
            // Se ejecuta solo si no hay ningún usuario en la base de datos.
            if (!context.Users.Any())
            {
                // Crea un objeto para almacenar la configuración del usuario inicial.
                var initialUserConfig = new InitialUserData();

                // Lee la sección "InitialUserData" de appsettings.json y user-secrets, y mapea los valores a las propiedades del objeto.
                configuration.GetSection("InitialUserData").Bind(initialUserConfig);

                // Valida que los datos esenciales se hayan cargado.
                if (string.IsNullOrEmpty(initialUserConfig.UserName) || string.IsNullOrEmpty(initialUserConfig.Password))
                {
                    throw new InvalidOperationException("Los datos para el usuario inicial no se encontraron en la configuración. Asegúrate de configurar 'InitialUserData' en appsettings.json y los User Secrets.");
                }

                // Crea la nueva instancia del usuario con los datos cargados.
                var usuario = new Usuario
                {
                    UserName = initialUserConfig.UserName,
                    Nombre = initialUserConfig.Nombre,
                    Apellido = initialUserConfig.Apellido,
                    Email = initialUserConfig.Email,
                    Telefono = initialUserConfig.Telefono
                };

                // Usa el UserManager para crear el usuario en la base de datos con la contraseña obtenida de forma segura.
                await usuarioManager.CreateAsync(usuario, initialUserConfig.Password);

                context.SaveChanges();
            }
        }
    }
}
