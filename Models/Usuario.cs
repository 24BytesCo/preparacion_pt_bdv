using Microsoft.AspNetCore.Identity;

namespace preparacion_pt_bdv.models
{
    // Extiende la clase base de Identity para añadir propiedades personalizadas.
    public class Usuario : IdentityUser
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }

        // Propiedad calculada que retorna el nombre y apellido concatenados.
        public string NombreCompleto
        {
            get
            {
                return $"{Nombre} {Apellido}";
            }
        }
    }
}